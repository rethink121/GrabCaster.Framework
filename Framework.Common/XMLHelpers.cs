// XMLHelpers.cs
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

using System.Xml;

#endregion

namespace GrabCaster.Framework.Common
{
    /// <summary>
    ///     TODO The xml helpers.
    /// </summary>
    public static class XmlHelpers
    {
        /// <summary>
        ///     The add attribute.
        /// </summary>
        /// <param name="xmlDocument">
        ///     The xml document.
        /// </param>
        /// <param name="xmlElement">
        ///     The xml element.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public static void AddAttribute(XmlDocument xmlDocument, XmlElement xmlElement, string name, string value)
        {
            var xmlAttribute = xmlDocument.CreateAttribute(name);
            xmlAttribute.Value = value;
            xmlElement.Attributes.Append(xmlAttribute);
        }
    }
}