using System;

namespace Sinch.MessageEncoder.Serialization.Default
{
    public interface ISerializer<in TPayload> : ISerializer
    {
        Span<byte> Serialize(TPayload payload);
    }

    public interface ISerializer { } 
}