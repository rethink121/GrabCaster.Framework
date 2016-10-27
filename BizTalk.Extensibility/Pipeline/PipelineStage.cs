// PipelineStage.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
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
