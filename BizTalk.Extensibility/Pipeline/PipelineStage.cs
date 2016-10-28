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

using Microsoft.BizTalk.Component.Interop;
using Microsoft.Test.BizTalk.PipelineObjects;
using System;
using System.Collections.Generic;


namespace GrabCaster.BizTalk.Extensibility
{
    public sealed class PpStage
    {
        private Guid _id;
        private string _name;
        private ExecuteMethod _executeMethod;
        private bool _isReceiveStage;

        private static IDictionary<Guid, PpStage> _stages =
            new Dictionary<Guid, PpStage>();


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


        private PpStage(string id, string name, ExecuteMethod method, bool isReceiveStage)
        {
            _id = new Guid(id);
            _name = name;
            _executeMethod = method;
            _isReceiveStage = isReceiveStage;
            _stages.Add(_id, this);
        }

        internal static PpStage LookupStage(Guid stageId)
        {
            return _stages[stageId];
        }


        public static readonly PpStage DecodeStage = new PpStage(CategoryTypes.CATID_Decoder, "Decode",
            ExecuteMethod.All, true);

        public static readonly PpStage DisassembleStage = new PpStage(CategoryTypes.CATID_DisassemblingParser,
            "Disassemble", ExecuteMethod.FirstMatch, true);

        public static readonly PpStage ValidateStage = new PpStage(CategoryTypes.CATID_Validate, "Validate",
            ExecuteMethod.All, true);

        public static readonly PpStage ResolvePartyStage = new PpStage(CategoryTypes.CATID_PartyResolver, "ResolveParty",
            ExecuteMethod.All, true);


        public static readonly PpStage PreAssembleStage = new PpStage(CategoryTypes.CATID_Any, "Pre-Assemble",
            ExecuteMethod.All, false);

        public static readonly PpStage AssembleStage = new PpStage(CategoryTypes.CATID_AssemblingSerializer, "Assemble",
            ExecuteMethod.All, false);

        public static readonly PpStage EncodeStage = new PpStage(CategoryTypes.CATID_Encoder, "Encode",
            ExecuteMethod.All, false);
    }
} // namespace BTSGBizTalkAddins