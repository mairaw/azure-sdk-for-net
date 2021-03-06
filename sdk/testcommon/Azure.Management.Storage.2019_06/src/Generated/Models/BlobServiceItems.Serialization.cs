// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.Management.Storage.Models
{
    public partial class BlobServiceItems
    {
        internal static BlobServiceItems DeserializeBlobServiceItems(JsonElement element)
        {
            IReadOnlyList<BlobServiceProperties> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<BlobServiceProperties> array = new List<BlobServiceProperties>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Null)
                        {
                            array.Add(null);
                        }
                        else
                        {
                            array.Add(BlobServiceProperties.DeserializeBlobServiceProperties(item));
                        }
                    }
                    value = array;
                    continue;
                }
            }
            return new BlobServiceItems(value);
        }
    }
}
