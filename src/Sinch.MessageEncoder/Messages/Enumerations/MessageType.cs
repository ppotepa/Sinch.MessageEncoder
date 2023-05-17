namespace Sinch.MessageEncoder.Messages.Enumerations
{
    /// <summary>
    /// Each Enumeration here represents single <see cref="byte"/> block of memory
    /// </summary>
    public enum MessageType
    {
        Text = 1,
        Voice = 2,
        Image = 3,
        Video = 4
    }
}
