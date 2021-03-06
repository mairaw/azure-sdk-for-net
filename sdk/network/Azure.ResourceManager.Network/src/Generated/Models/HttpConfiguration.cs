// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.ResourceManager.Network.Models
{
    /// <summary> HTTP configuration of the connectivity check. </summary>
    public partial class HttpConfiguration
    {
        /// <summary> Initializes a new instance of HttpConfiguration. </summary>
        public HttpConfiguration()
        {
            Method = "Get";
        }

        /// <summary> Initializes a new instance of HttpConfiguration. </summary>
        /// <param name="method"> HTTP method. </param>
        /// <param name="headers"> List of HTTP headers. </param>
        /// <param name="validStatusCodes"> Valid status codes. </param>
        internal HttpConfiguration(string method, IList<HttpHeader> headers, IList<int> validStatusCodes)
        {
            Method = method;
            Headers = headers;
            ValidStatusCodes = validStatusCodes;
        }

        /// <summary> HTTP method. </summary>
        public string Method { get; set; }
        /// <summary> List of HTTP headers. </summary>
        public IList<HttpHeader> Headers { get; set; }
        /// <summary> Valid status codes. </summary>
        public IList<int> ValidStatusCodes { get; set; }
    }
}
