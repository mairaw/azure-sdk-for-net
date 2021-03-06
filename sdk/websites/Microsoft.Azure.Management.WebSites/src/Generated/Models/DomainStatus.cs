// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.WebSites.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for DomainStatus.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DomainStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Awaiting")]
        Awaiting,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Confiscated")]
        Confiscated,
        [EnumMember(Value = "Disabled")]
        Disabled,
        [EnumMember(Value = "Excluded")]
        Excluded,
        [EnumMember(Value = "Expired")]
        Expired,
        [EnumMember(Value = "Failed")]
        Failed,
        [EnumMember(Value = "Held")]
        Held,
        [EnumMember(Value = "Locked")]
        Locked,
        [EnumMember(Value = "Parked")]
        Parked,
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Reserved")]
        Reserved,
        [EnumMember(Value = "Reverted")]
        Reverted,
        [EnumMember(Value = "Suspended")]
        Suspended,
        [EnumMember(Value = "Transferred")]
        Transferred,
        [EnumMember(Value = "Unknown")]
        Unknown,
        [EnumMember(Value = "Unlocked")]
        Unlocked,
        [EnumMember(Value = "Unparked")]
        Unparked,
        [EnumMember(Value = "Updated")]
        Updated,
        [EnumMember(Value = "JsonConverterFailed")]
        JsonConverterFailed
    }
    internal static class DomainStatusEnumExtension
    {
        internal static string ToSerializedValue(this DomainStatus? value)
        {
            return value == null ? null : ((DomainStatus)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this DomainStatus value)
        {
            switch( value )
            {
                case DomainStatus.Active:
                    return "Active";
                case DomainStatus.Awaiting:
                    return "Awaiting";
                case DomainStatus.Cancelled:
                    return "Cancelled";
                case DomainStatus.Confiscated:
                    return "Confiscated";
                case DomainStatus.Disabled:
                    return "Disabled";
                case DomainStatus.Excluded:
                    return "Excluded";
                case DomainStatus.Expired:
                    return "Expired";
                case DomainStatus.Failed:
                    return "Failed";
                case DomainStatus.Held:
                    return "Held";
                case DomainStatus.Locked:
                    return "Locked";
                case DomainStatus.Parked:
                    return "Parked";
                case DomainStatus.Pending:
                    return "Pending";
                case DomainStatus.Reserved:
                    return "Reserved";
                case DomainStatus.Reverted:
                    return "Reverted";
                case DomainStatus.Suspended:
                    return "Suspended";
                case DomainStatus.Transferred:
                    return "Transferred";
                case DomainStatus.Unknown:
                    return "Unknown";
                case DomainStatus.Unlocked:
                    return "Unlocked";
                case DomainStatus.Unparked:
                    return "Unparked";
                case DomainStatus.Updated:
                    return "Updated";
                case DomainStatus.JsonConverterFailed:
                    return "JsonConverterFailed";
            }
            return null;
        }

        internal static DomainStatus? ParseDomainStatus(this string value)
        {
            switch( value )
            {
                case "Active":
                    return DomainStatus.Active;
                case "Awaiting":
                    return DomainStatus.Awaiting;
                case "Cancelled":
                    return DomainStatus.Cancelled;
                case "Confiscated":
                    return DomainStatus.Confiscated;
                case "Disabled":
                    return DomainStatus.Disabled;
                case "Excluded":
                    return DomainStatus.Excluded;
                case "Expired":
                    return DomainStatus.Expired;
                case "Failed":
                    return DomainStatus.Failed;
                case "Held":
                    return DomainStatus.Held;
                case "Locked":
                    return DomainStatus.Locked;
                case "Parked":
                    return DomainStatus.Parked;
                case "Pending":
                    return DomainStatus.Pending;
                case "Reserved":
                    return DomainStatus.Reserved;
                case "Reverted":
                    return DomainStatus.Reverted;
                case "Suspended":
                    return DomainStatus.Suspended;
                case "Transferred":
                    return DomainStatus.Transferred;
                case "Unknown":
                    return DomainStatus.Unknown;
                case "Unlocked":
                    return DomainStatus.Unlocked;
                case "Unparked":
                    return DomainStatus.Unparked;
                case "Updated":
                    return DomainStatus.Updated;
                case "JsonConverterFailed":
                    return DomainStatus.JsonConverterFailed;
            }
            return null;
        }
    }
}
