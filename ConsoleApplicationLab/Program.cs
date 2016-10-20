using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;

namespace ConsoleApplicationLab
{
    class Program
    {
        static void Main(string[] args)
        {
            string publishingFolder = Path.Combine(ConfigurationBag.DirectoryDeployment(), ConfigurationBag.DirectoryNamePublishing);

            var regTriggers = new Regex(ConfigurationBag.DeployExtensionLookFor);
            var deployFiles =
                Directory.GetFiles(publishingFolder, "*.*", SearchOption.AllDirectories)
                  .Where(path => Path.GetExtension(path) == ".trigger" || Path.GetExtension(path) == ".event" || Path.GetExtension(path) == ".component");

            foreach (var file in deployFiles)
            {
                string projectName = Path.GetFileNameWithoutExtension(publishingFolder + file);
                string projectType = Path.GetExtension(publishingFolder + file).Replace(".","");
                GrabCaster.Framework.Deployment.Jit.CompilePublishing(projectType, projectName,"Release","AnyCpu");
            }
            

          
        }
    }
}
