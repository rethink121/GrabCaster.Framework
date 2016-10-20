using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Components;
using GrabCaster.Framework.Log;

namespace GrabCaster.Framework.BTSPipelineComponent
{
    using BizTalk.Extensibility;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    [ComponentContract("{F60C8A3B-0ABD-4595-BCFD-7A2B6DE46EC6}", "BizTalk Pipeline Executor", "Execute a BizTalk Pipeline")]
    public class BizTalkPipeline : IChainComponentType
    {
        [ComponentPropertyContract("AssemblyFile", "Pipeline assembly file")]
        public string AssemblyFile { get; set; }
        [ComponentPropertyContract("PipelineTypeName", "Pipeline type name")]
        public string PipelineTypeName { get; set; }    
        [ComponentPropertyContract("PipelinePathFile", "Pipeline path file")]
        public string PipelinePathFile { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        [ComponentPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }
        [ComponentActionContract("{0CC64C13-5BCC-440B-B858-1164A76E4CA9}", "Main action", "Main action description")]
        public byte[] Execute()
        {
            try
            {
                string content = EncodingDecoding.EncodingBytes2String(DataContext);
                byte[] contentBack = BizTalkPipelines.ExecutePipeline(AssemblyFile, AssemblyFile, PipelineTypeName, content, PipelinePathFile);
                return contentBack;
            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(ConfigurationBag.EngineName,
                                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                                              Constant.LogLevelError,
                                              Constant.TaskCategoriesError,
                                              ex,
                                              Constant.LogLevelError);

                return null;
            }



        }
    }
}
