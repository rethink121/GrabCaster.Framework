
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.BizTalk.PipelineOM;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;


using IPipeline = Microsoft.Test.BizTalk.PipelineObjects.IPipeline;
using PipelineHelper = Microsoft.Test.BizTalk.PipelineObjects.PipelineFactory;

namespace GrabCaster.BizTalk.Extensibility
{
    //Main clas to create pipelines
   public static class MainPipelineHelper
   {


      public static RWPipeline RetReceivePipeline(Type type)
      {
         if ( type == null )
           throw new ArgumentNullException("Pipeline type null, deploy Pipeline in BizTalk BizTalkMgmtDb database");

         if ( !type.IsSubclassOf(typeof(ReceivePipeline)) )
            throw new InvalidOperationException("Type must specify a Receive Pipeline");

         PipelineHelper helper = new PipelineHelper();
         IPipeline pipeline = helper.CreatePipelineFromType(type);
         return new RWPipeline(pipeline);
      }

      public static SWPipeline RetSendPipeline(Type type)
      {
         if ( type == null )
            throw new ArgumentNullException("type");

         if ( !type.IsSubclassOf(typeof(SendPipeline)) )
            throw new InvalidOperationException("Type must specify a Send Pipeline");

         PipelineHelper helper = new PipelineHelper();
         IPipeline pipeline = helper.CreatePipelineFromType(type);
         return new SWPipeline(pipeline);
      }


   } 

} 
