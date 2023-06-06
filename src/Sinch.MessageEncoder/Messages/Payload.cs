namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload
    {
        public static Payload Empty => new EmptyPayload();
    }
}