namespace Sinch.MessageEncoder.Messages.Enumerations;

public enum MessageHeaderStructure
{
    /// <summary>
    /// Block of <b><see cref="long"/> UUID</b> <b>8-bytes</b>
    /// </summary>
    From = 1,
    /// <summary>
    /// Block of <b><see cref="long"/> UUID</b> <b>8-bytes</b>
    /// </summary>
    To = 2,
    /// <summary>
    /// Block of <b><see cref="uint"/> UNIX Timestamp</b> <b>4-bytes</b>
    /// </summary>
    TimeStamp = 3,
    /// <summary> Block of <b><see cref="byte"/></b>
    /// single-byte
    /// </summary>
    MessageType = 4,
    MessageSize = 5,
    PayloadSize = 6,
    /*
     *...
     *
    */
    CheckSum = 64
}