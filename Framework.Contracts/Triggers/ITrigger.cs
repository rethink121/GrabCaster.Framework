// ITrigger.cs
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

using GrabCaster.Framework.Contracts.Globals;

#endregion

namespace GrabCaster.Framework.Contracts.Triggers
{
    /// <summary>
    ///     The TriggerType interface.
    /// </summary>
    public interface ITriggerType
    {
        /// <summary>
        ///     Internal Trigger context.
        /// </summary>
        ActionContext Context { get; set; }

        /// <summary>
        ///     internal delegate to use in delegates events
        /// </summary>
        ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        ///     Identify if the trigger request must be syncronous or not
        /// </summary>
        bool Syncronous { get; set; }

        /// <summary>
        ///     Main default data property
        /// </summary>
        byte[] DataContext { get; set; }

        /// <summary>
        ///     Main default method
        /// </summary>
        /// <param name="actionTrigger">
        ///     The set Event Action Trigger.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        byte[] Execute(ActionTrigger actionTrigger, ActionContext context);
    }
}