using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.Messages;

[StructLayout(LayoutKind.Explicit, Size = 66_193_471)]
public unsafe struct MessageHeaderTransport
{
    [FieldOffset(3)]
    public fixed byte HeaderBytes[1024 * 1024 * 64];

    public MessageHeaderTransport()
    {
        fixed (byte* first = HeaderBytes)
        {

        }
    }
}