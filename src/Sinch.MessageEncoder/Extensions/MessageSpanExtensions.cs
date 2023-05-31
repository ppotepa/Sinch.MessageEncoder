using System;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.Extensions
{
    internal static class MessageSpanExtensions
    {
        private const int LONG_LENGTH = 8;
        private const int MSG_TYPE_INDEX = 24;
        private const int HEADERS_LENGTH_FIRST_BYTE_INDEX = 25;

        public static long GetMessageFrom(this Span<byte> messageSpan)
        {
            return MemoryMarshal.Read<long>(messageSpan[..LONG_LENGTH]);
        }

        public static long GetMessageTo(this Span<byte> messageSpan)
        {
            return MemoryMarshal.Read<long>(messageSpan[LONG_LENGTH..(LONG_LENGTH * 2)]);
        }

        public static long GetMessageTimestamp(this Span<byte> messageSpan)
        {
            return MemoryMarshal.Read<long>(messageSpan[(LONG_LENGTH * 2)..MSG_TYPE_INDEX]);
        }

        public static long GetMessageHeadersLength(this Span<byte> messageSpan)
        {
            return MemoryMarshal.Read<long>(messageSpan[HEADERS_LENGTH_FIRST_BYTE_INDEX..(HEADERS_LENGTH_FIRST_BYTE_INDEX + 8)]);
        }

        public static byte GetMessageType(this Span<byte> messageSpan)
        {
            return messageSpan[MSG_TYPE_INDEX];
        }

        public static Span<byte> GetAllHeaders(this Span<byte> messageSpan, long headersLength)
        {
            return messageSpan[(HEADERS_LENGTH_FIRST_BYTE_INDEX + 8)..((int)headersLength + (HEADERS_LENGTH_FIRST_BYTE_INDEX + 8))];
        }
    }
}
