// ReceivePipelineWrapper.cs
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
using System.IO;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.Test.BizTalk.PipelineObjects;

#endregion

namespace GrabCaster.BizTalk.Extensibility
{
    /// <summary>
    ///     Wrapper around a receive pipeline you can execute
    /// </summary>
    public class RWPipeline : BWPipeline
    {
        internal RWPipeline(IPipeline pipeline)
            : base(pipeline, true)
        {
            FindStage(PpStage.DecodeStage);
            FindStage(PpStage.DisassembleStage);
            FindStage(PpStage.ValidateStage);
            FindStage(PpStage.ResolvePartyStage);
        }

        /// <summary>
        ///     Executes the receive pipeline
        /// </summary>
        /// <param name="inputMessage">Input message to feed to the pipeline</param>
        /// <returns>A collection of messages that were generated by the pipeline</returns>
        public MessageCollection Execute(IBaseMessage inputMessage)
        {
            if (inputMessage == null)
                throw new ArgumentNullException("inputMessage");

            Pipeline.InputMessages.Add(inputMessage);
            MessageCollection output = new MessageCollection();
            Pipeline.Execute(Context);

            IBaseMessage om = null;
            while ((om = Pipeline.GetNextOutputMessage(Context)) != null)
            {
                output.Add(om);
                // we have to consume the entire stream for the body part.
                // Otherwise, the disassembler might enter an infinite loop.
                // We currently copy the output into a new memory stream
                if (om.BodyPart != null && om.BodyPart.Data != null)
                {
                    om.BodyPart.Data = CopyStream(om.BodyPart.Data);
                }
            }

            return output;
        }

        private Stream CopyStream(Stream source)
        {
            MemoryStream stream = new MemoryStream();

            byte[] buffer = new byte[64*1024];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }
            stream.Position = 0;
            return stream;
        }
    } // class ReceivePipelineWrapper
} // namespace BTSGBizTalkAddins