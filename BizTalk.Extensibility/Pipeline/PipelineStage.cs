

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Test.BizTalk.PipelineObjects;
using Microsoft.BizTalk.Component.Interop;


namespace GrabCaster.BizTalk.Extensibility
{
   public sealed class ppStage
   {
      private Guid _id;
      private string _name;
      private ExecuteMethod _executeMethod;
      private bool _isReceiveStage;
      private static IDictionary<Guid, ppStage> _stages = 
         new Dictionary<Guid, ppStage>();


      public Guid StageID
      {
         get { return _id; }
      }

      public string StageName
      {
         get { return _name; }
      }

      public ExecuteMethod ExecuteMethod
      {
         get { return _executeMethod; }
      }

      public bool IsReceiveStage
      {
         get { return _isReceiveStage; }
      }
   

      private ppStage(string id, string name, ExecuteMethod method, bool isReceiveStage)
      {
         _id = new Guid(id);
         _name = name;
         _executeMethod = method;
         _isReceiveStage = isReceiveStage;
         _stages.Add(_id, this);
      }

      internal static ppStage LookupStage(Guid stageId)
      {
         return _stages[stageId];
      }

   
      public static readonly ppStage DecodeStage = new ppStage(CategoryTypes.CATID_Decoder, "Decode", ExecuteMethod.All, true);
   
      public static readonly ppStage DisassembleStage = new ppStage(CategoryTypes.CATID_DisassemblingParser, "Disassemble", ExecuteMethod.FirstMatch, true);

      public static readonly ppStage ValidateStage = new ppStage(CategoryTypes.CATID_Validate, "Validate", ExecuteMethod.All, true);

      public static readonly ppStage ResolvePartyStage = new ppStage(CategoryTypes.CATID_PartyResolver, "ResolveParty", ExecuteMethod.All, true);


      public static readonly ppStage PreAssembleStage = new ppStage(CategoryTypes.CATID_Any, "Pre-Assemble", ExecuteMethod.All, false);

      public static readonly ppStage AssembleStage = new ppStage(CategoryTypes.CATID_AssemblingSerializer, "Assemble", ExecuteMethod.All, false);

      public static readonly ppStage EncodeStage = new ppStage(CategoryTypes.CATID_Encoder, "Encode", ExecuteMethod.All, false);

   } 

} // namespace BTSGBizTalkAddins
