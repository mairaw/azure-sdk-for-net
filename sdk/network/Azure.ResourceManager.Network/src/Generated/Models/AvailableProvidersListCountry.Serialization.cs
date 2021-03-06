// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Network.Models
{
    public partial class AvailableProvidersListCountry
    {
        internal static AvailableProvidersListCountry DeserializeAvailableProvidersListCountry(JsonElement element)
        {
            string countryName = default;
            IReadOnlyList<string> providers = default;
            IReadOnlyList<AvailableProvidersListState> states = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("countryName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    countryName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("providers"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<string> array = new List<string>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Null)
                        {
                            array.Add(null);
                        }
                        else
                        {
                            array.Add(item.GetString());
                        }
                    }
                    providers = array;
                    continue;
                }
                if (property.NameEquals("states"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<AvailableProvidersListState> array = new List<AvailableProvidersListState>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Null)
                        {
                            array.Add(null);
                        }
                        else
                        {
                            array.Add(AvailableProvidersListState.DeserializeAvailableProvidersListState(item));
                        }
                    }
                    states = array;
                    continue;
                }
            }
            return new AvailableProvidersListCountry(countryName, providers, states);
        }
    }
}
