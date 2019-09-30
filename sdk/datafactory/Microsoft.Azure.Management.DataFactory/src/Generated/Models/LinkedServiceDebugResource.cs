// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.DataFactory.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Linked service debug resource.
    /// </summary>
    public partial class LinkedServiceDebugResource : SubResourceDebugResource
    {
        /// <summary>
        /// Initializes a new instance of the LinkedServiceDebugResource class.
        /// </summary>
        public LinkedServiceDebugResource()
        {
            Properties = new LinkedService();
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LinkedServiceDebugResource class.
        /// </summary>
        /// <param name="properties">Properties of linked service.</param>
        /// <param name="name">The resource name.</param>
        public LinkedServiceDebugResource(LinkedService properties, string name = default(string))
            : base(name)
        {
            Properties = properties;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets properties of linked service.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public LinkedService Properties { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Properties == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Properties");
            }
            if (Properties != null)
            {
                Properties.Validate();
            }
        }
    }
}