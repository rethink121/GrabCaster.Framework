// Jit.cs
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Log;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

#endregion

namespace GrabCaster.Framework.Deployment
{
    public class Jit
    {
        public static bool CompilePublishing(string projectExtension, string projectName, string configurationBuild,
            string platform)
        {
            try
            {
                string projectType = "";
                switch (projectExtension.ToLower())
                {
                    case "trigger":
                        projectType = "Trigger";
                        break;
                    case "event":
                        projectType = "Event";
                        break;
                    case "component":
                        projectType = "Component";
                        break;
                }
                string projectTtypeName = $"GrabCaster{projectType}";

                string sourcePublishing = Path.Combine(ConfigurationBag.DirectoryDeployment(), "Publishing");
                string sourceTemplate = Path.Combine(ConfigurationBag.DirectoryDeployment(), projectTtypeName);
                string destinationPath = Path.Combine(ConfigurationBag.DirectoryDeployment(), $"Temp\\{Guid.NewGuid()}");

                //Create the new project
                //Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(sourceTemplate, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourceTemplate, destinationPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(sourceTemplate, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(sourceTemplate, destinationPath), true);

                //Copy all the files required from Publishing to the new project Configuration folder 
                //Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(sourcePublishing, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePublishing,
                        destinationPath + $"\\bin\\{configurationBuild}"));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(sourcePublishing, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath,
                        newPath.Replace(sourcePublishing, destinationPath + $"\\bin\\{configurationBuild}"), true);


                string solutionFile = Path.Combine(destinationPath, projectName + ".sln");
                string projectFile = Path.Combine(destinationPath, projectName + ".csproj");
                string classFile = Path.Combine(destinationPath, projectName + ".cs");
                string assemblyInfoFile = Path.Combine(destinationPath, "Properties\\AssemblyInfo.cs");

                //Rename the solution file
                File.Move(Path.Combine(destinationPath, projectTtypeName + ".sln"), solutionFile);
                //Rename the project file
                File.Move(Path.Combine(destinationPath, projectTtypeName + ".csproj"), projectFile);
                //Rename the class file
                File.Move(Path.Combine(destinationPath, projectTtypeName + ".cs"), classFile);

                //Get using, code and dll
                string textBag = File.ReadAllText(Path.Combine(sourcePublishing, projectName + $".{projectExtension}"));

                //Get Dll to add as reference 
                var dllsReference = Directory.GetFiles(destinationPath + $"\\bin\\{configurationBuild}", "*.*",
                        SearchOption.AllDirectories)
                    .Where(
                        path =>
                            Path.GetExtension(path) == ".dll" &&
                            !Path.GetFileName(path).Contains("GrabCaster.Framework"));

                string dllTextReference =
                    "<Reference Include=\"FULLNAME, processorArchitecture=MSIL\">\r" +
                    "<SpecificVersion>False</SpecificVersion>\r" +
                    $"<HintPath>bin\\{configurationBuild}\\DLLNAME </HintPath>\r" +
                    "</Reference>\r";

                StringBuilder sbDlls = new StringBuilder();
                foreach (var dllReference in dllsReference)
                {
                    Assembly assembly = Assembly.LoadFrom(dllReference);
                    sbDlls.AppendLine(dllTextReference.Replace("FULLNAME", assembly.FullName)
                        .Replace("DLLNAME", Path.GetFileName(assembly.Location)));
                }


                //Rename markup in solution file
                string textSln = File.ReadAllText(solutionFile);
                textSln = textSln.Replace(projectTtypeName, projectName);
                File.WriteAllText(solutionFile, textSln);

                //Rename markup in project file
                string textProj = File.ReadAllText(projectFile);
                textProj = textProj.Replace(projectTtypeName, projectName);
                textProj = textProj.Replace("<!--DLL-->", sbDlls.ToString());
                textProj = textProj.Replace("*CONFIGURATION*", configurationBuild);
                textProj = textProj.Replace("*PLATFORM*", platform);
                File.WriteAllText(projectFile, textProj);

                //Rename markup in class file
                string textCs = File.ReadAllText(classFile);
                textCs = textCs.Replace(projectTtypeName, projectName);

                string componentId = Guid.NewGuid().ToString();
                //TriggerID
                textCs = textCs.Replace("*ID*", componentId);

                //ContractID
                textCs = textCs.Replace("*CONTRACTID*", Guid.NewGuid().ToString());

                //Name
                textCs = textCs.Replace("*NAME*", projectName);

                //Description
                textCs = textCs.Replace("*DESCRIPTION*", $"{projectName} {projectType} component");

                //using
                var textUsing =
                    Regex.Matches(textBag, "<USING>(.*?)</USING>", RegexOptions.Multiline | RegexOptions.Singleline)[0]
                        .Value.Replace("<USING>", "").Replace("</USING>", "");
                textCs = textCs.Replace("//<USING>", textUsing);

                //Code
                var textCode =
                    Regex.Matches(textBag, "<MAINCODE>(.*?)</MAINCODE>",
                            RegexOptions.Multiline | RegexOptions.Singleline)[0].Value.Replace("<MAINCODE>", "")
                        .Replace("</MAINCODE>", "");

                //Properties
                var textCodeLines =
                    Regex.Matches(textBag, "<MAINCODE>(.*?)</MAINCODE>",
                            RegexOptions.Multiline | RegexOptions.Singleline)[0].Value.Replace("<MAINCODE>", "")
                        .Replace("</MAINCODE>", "")
                        .Split('\r');

                string functionName = "";
                string innerArgs = "";

                //look the function name
                foreach (var textCodeLine in textCodeLines)
                {
                    var func = Regex.Match(textCodeLine, @"\b[^()]+\((.*)\)$");
                    if (func.Success)
                    {
                        functionName = func.Value;
                        innerArgs = func.Groups[1].Value;
                        break;
                    }
                }

                //remove the func name from the code
                textCode = textCode.Replace(functionName, "");
                //Remove the first and last brachet
                int brachet = textCode.IndexOf("{") + 1;
                textCode = textCode.Substring(brachet, textCode.Length - brachet);
                textCode = textCode.Substring(0, textCode.LastIndexOf("}") - 1);

                textCs = textCs.Replace("//<MAINCODE>", textCode);

                // Get parameters
                var paramTags = Regex.Matches(innerArgs, @"([^,]+\(.+?\))|([^,]+)");
                StringBuilder sbParamsComponent = new StringBuilder();
                StringBuilder sbParamsConfiguration = new StringBuilder();

                sbParamsComponent.AppendLine("");
                foreach (var item in paramTags)
                {
                    string[] param = item.ToString().Split(' ');
                    sbParamsComponent.AppendLine(
                        $"\t\t[{projectType}PropertyContract(\"{param[1]}\", \"{param[1]} property\")]");
                    sbParamsComponent.AppendLine("\t\tpublic " + item + " { get; set; }");

                    sbParamsConfiguration.AppendLine("\t\t{");
                    sbParamsConfiguration.AppendLine($"\t\t\"Name\": \"{param[1]}\",");
                    sbParamsConfiguration.AppendLine("\t\t\"Value\": \"\"");
                    sbParamsConfiguration.AppendLine("\t\t},");
                }


                textCs = textCs.Replace("//<PROPERTIES>", sbParamsComponent.ToString());

                //functions
                string textFunctions =
                    Regex.Matches(textBag, "<FUNCTIONS>(.*?)</FUNCTIONS>",
                            RegexOptions.Multiline | RegexOptions.Singleline)[0].Value.Replace("<FUNCTIONS>", "")
                        .Replace("</FUNCTIONS>", "");
                textCs = textCs.Replace("//<FUNCTIONS>", textFunctions);

                File.WriteAllText(classFile, textCs);

                //Rename markup in project file
                string textInfo = File.ReadAllText(assemblyInfoFile);
                textInfo = textInfo.Replace(projectTtypeName, projectName);
                File.WriteAllText(assemblyInfoFile, textInfo);

                //Create the Configuration file
                var configurationFiles = Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories)
                    .Where(
                        path =>
                            Path.GetExtension(path) == ".off" || Path.GetExtension(path) == ".cmp" ||
                            Path.GetExtension(path) == ".evn");

                foreach (var file in configurationFiles)
                {
                    string configurationFile = file.Replace(projectTtypeName, projectName);
                    File.Move(file, configurationFile.Replace(projectTtypeName, projectName));
                    string textJson = File.ReadAllText(configurationFile);

                    string configurationProperties = sbParamsConfiguration.ToString();
                    configurationProperties = configurationProperties.Substring(0,
                        configurationProperties.LastIndexOf(","));

                    textJson = textJson.Replace("//<PROPERTIES>", configurationProperties);
                    textJson = textJson.Replace("*ID*", componentId);
                    textJson = textJson.Replace("*IDCONFIGURATION*", Guid.NewGuid().ToString());

                    //Name
                    textJson = textJson.Replace("*NAME*", $"{projectName} {projectType} configuration");

                    //Description
                    textJson = textJson.Replace("*DESCRIPTION*", $"{projectName} {projectType} configuration");
                    File.WriteAllText(configurationFile, textJson);
                }


                //Build the project
                bool resultOk = Build(projectFile, sourcePublishing, projectName, configurationBuild, platform);

                //Copy the {Configuration} into the component
                if (resultOk)
                {
                    string folderConfigurationFile = "";
                    string folderdllFile = "";
                    switch (projectExtension.ToLower())
                    {
                        case "trigger":
                            folderdllFile = ConfigurationBag.DirectoryTriggers();
                            folderConfigurationFile = ConfigurationBag.DirectoryBubblingTriggers();
                            break;
                        case "event":
                            folderdllFile = ConfigurationBag.DirectoryEvents();
                            folderConfigurationFile = ConfigurationBag.DirectoryBubblingEvents();
                            break;
                        case "component":
                            folderdllFile = ConfigurationBag.DirectoryComponents();
                            folderConfigurationFile = ConfigurationBag.DirectoryBubblingComponents();
                            break;
                        default:
                            break;
                    }

                    //Copy the dll
                    string source = Path.Combine(destinationPath,
                        $"bin\\{configurationBuild}\\{projectName}.{projectType}.dll");
                    string destinationDll = Path.Combine(folderdllFile,
                        $"{projectName}.{projectType}.dll");
                    File.Copy(source, destinationDll, true);

                    var configurationFilesNew = Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories)
                        .Where(
                            path =>
                                Path.GetExtension(path) == ".off" || Path.GetExtension(path) == ".cmp" ||
                                Path.GetExtension(path) == ".evn");

                    //Copy the configuration files
                    foreach (var configurationFile in configurationFilesNew)
                    {
                        File.Copy(configurationFile,
                            Path.Combine(folderConfigurationFile, Path.GetFileName(configurationFile)), true);
                    }
                }

                return resultOk;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    "Configuration.WebApiEndPoint key empty, internal Web Api interface disable",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelWarning);
                return false;
            }
        }


        public static bool Build(string msbuildFileName, string destinationLogFolder, string fileLogName,
            string Configuration, string Platform)
        {
            try
            {
                var projectCollection = new ProjectCollection();
                var buildParamters = new BuildParameters(projectCollection);
                buildParamters.Loggers = new List<ILogger>
                {
                    new FileLogger {Parameters = $"logfile={destinationLogFolder} \\{fileLogName}Build.log"}
                };

                var globalProperty = new Dictionary<String, String>();
                globalProperty.Add("Configuration", Configuration);
                globalProperty.Add("Platform", Platform);

                BuildManager.DefaultBuildManager.ResetCaches();

                var buildRequest = new BuildRequestData(msbuildFileName, globalProperty, null, new[] {"Clean", "Build"},
                    null);
                var buildResult = BuildManager.DefaultBuildManager.Build(buildParamters, buildRequest);

                return buildResult.OverallResult == BuildResultCode.Success;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    "Configuration.WebApiEndPoint key empty, internal Web Api interface disable",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelWarning);
                return false;
            }
        }
    }
}