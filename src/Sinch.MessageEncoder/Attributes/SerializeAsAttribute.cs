using System;

namespace Sinch.MessageEncoder.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class SerializeAsAttribute : Attribute
{
    private readonly Type _targetType;

    public SerializeAsAttribute(Type targetType)
    {
        _targetType = targetType;
    }

    public int Order { get; set; }
    public Type Serializer { get; set; }
}