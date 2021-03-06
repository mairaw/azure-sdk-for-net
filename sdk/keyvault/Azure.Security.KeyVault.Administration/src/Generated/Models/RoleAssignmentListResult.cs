// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.Security.KeyVault.Administration.Models
{
    /// <summary> Role assignment list operation result. </summary>
    internal partial class RoleAssignmentListResult
    {
        /// <summary> Initializes a new instance of RoleAssignmentListResult. </summary>
        internal RoleAssignmentListResult()
        {
        }

        /// <summary> Initializes a new instance of RoleAssignmentListResult. </summary>
        /// <param name="value"> Role assignment list. </param>
        /// <param name="nextLink"> The URL to use for getting the next set of results. </param>
        internal RoleAssignmentListResult(IReadOnlyList<RoleAssignment> value, string nextLink)
        {
            Value = value;
            NextLink = nextLink;
        }

        /// <summary> Role assignment list. </summary>
        public IReadOnlyList<RoleAssignment> Value { get; }
        /// <summary> The URL to use for getting the next set of results. </summary>
        public string NextLink { get; }
    }
}
