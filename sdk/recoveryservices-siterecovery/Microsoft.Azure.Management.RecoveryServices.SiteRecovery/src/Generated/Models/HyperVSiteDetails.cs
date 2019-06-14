// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.RecoveryServices.SiteRecovery.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// HyperVSite fabric specific details.
    /// </summary>
    [Newtonsoft.Json.JsonObject("HyperVSite")]
    public partial class HyperVSiteDetails : FabricSpecificDetails
    {
        /// <summary>
        /// Initializes a new instance of the HyperVSiteDetails class.
        /// </summary>
        public HyperVSiteDetails()
        {
            CustomInit();
        }


        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

    }
}