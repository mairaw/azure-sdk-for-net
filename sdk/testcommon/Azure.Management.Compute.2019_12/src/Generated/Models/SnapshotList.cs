// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure.Management.Compute.Models
{
    /// <summary> The List Snapshots operation response. </summary>
    public partial class SnapshotList
    {
        /// <summary> Initializes a new instance of SnapshotList. </summary>
        /// <param name="value"> A list of snapshots. </param>
        internal SnapshotList(IEnumerable<Snapshot> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value.ToArray();
        }

        /// <summary> Initializes a new instance of SnapshotList. </summary>
        /// <param name="value"> A list of snapshots. </param>
        /// <param name="nextLink"> The uri to fetch the next page of snapshots. Call ListNext() with this to fetch the next page of snapshots. </param>
        internal SnapshotList(IReadOnlyList<Snapshot> value, string nextLink)
        {
            Value = value ?? new List<Snapshot>();
            NextLink = nextLink;
        }

        /// <summary> A list of snapshots. </summary>
        public IReadOnlyList<Snapshot> Value { get; }
        /// <summary> The uri to fetch the next page of snapshots. Call ListNext() with this to fetch the next page of snapshots. </summary>
        public string NextLink { get; }
    }
}
