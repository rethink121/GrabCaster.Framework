// IAction.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System.Collections.Generic;
using System.Reflection;

#endregion

namespace GrabCaster.Framework.Contracts.Bubbling
{
    /// <summary>
    ///     The Lower receive layer, this receive the raw data
    /// </summary>
    internal interface IAction
    {
        /// <summary>
        ///     Unique Action ID
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Method name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Internal Method to invoke
        /// </summary>
        MethodInfo AssemblyMethodInfo { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///     Property Value
        /// </summary>
        object ReturnValue { get; set; }

        /// <summary>
        ///     Bubbling parameters
        /// </summary>
        List<Parameter> Parameters { get; set; }
    }
}