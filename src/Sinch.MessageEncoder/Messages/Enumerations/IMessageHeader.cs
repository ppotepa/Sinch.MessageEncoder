namespace Sinch.MessageEncoder.Messages.Enumerations
{
    public enum MessageHeader
    {
        From = 1,           //uuid
        To = 2,             //uuid
        TimeStamp = 3,      
        MessageType = 4,
        MessageSize = 5,
        PayloadSize = 6,
        /*
         *...
         *
         */
        CheckSum = 64
    }

    public enum MessageType
    {
        Text = 1,
        Voice = 2,
        Image = 3,
        Video = 4
    }
}
