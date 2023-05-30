namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload
    {
        public abstract void Deserialize();
        public abstract object Serialize();
    }
}