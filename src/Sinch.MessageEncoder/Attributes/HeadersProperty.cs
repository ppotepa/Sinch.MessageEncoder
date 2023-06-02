using System;

namespace Sinch.MessageEncoder.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class HeadersProperty : Attribute
{
    public HeadersProperty(Type targetType, int order, string headerName)
    {
        TargetType = targetType;
        Order = order;
        HeaderName = headerName;
    }

    public string HeaderName { get; set; }
    public int Order { get; set; }
    public Type TargetType { get; set; }
}