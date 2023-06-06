using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<byte> GetBytes(this object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return data switch
            {
                // Out of some reason BitConverter interprets single byte as 2-byte digit ?? 
                byte @byte => BitConverter.GetBytes((short)@byte).Take(1),
                short @short => BitConverter.GetBytes(@short),
                int @int => BitConverter.GetBytes(@int),
                long @long => BitConverter.GetBytes(@long),
                string @string => System.Text.Encoding.ASCII.GetBytes(@string),
                _ => throw new ArgumentException($"Argument was invalid. {data.GetType().Name} is not supported.", $"{nameof(data)}", null)
            };
        }

        public static byte[] ToByteArray(this object @object)
        {
            return @object switch
            {
                long @long => __toBytearrayLong(@long),
                int @int => __toByteArrayInt(@int),
                short @short => __toByteArrayShort(@short),
                byte @byte => __toByteArrayByte(@byte),
                string @string => __toByteArrayString(@string),
                byte[] @bytes => @bytes,
                object[] @objects => @objects.SelectMany(@o => @o.ToByteArray()).ToArray(),
                null => Array.Empty<byte>(),
                _ => throw new ArgumentOutOfRangeException(nameof(@object), @object, null)
            };
        }

        public static short ToInt16(this byte[] @object)
        {
            return (short)(@object[0] | (@object[1] << 8));
        }

        public static short ToInt16(this Span<byte> @object)
        {
            return (short)(@object[0] | (@object[1] << 8));
        }

        public static short ToInt16(this ReadOnlySpan<byte> @object)
        {
            return (short)(@object[0] | (@object[1] << 8));
        }

        public static int ToInt32(this byte[] @object)
        {
            return (int)(@object[0] | @object[1] << 8 | @object[2] << 16 | (@object[3] << 24));
        }

        public static int ToInt32(this Span<byte> @object)
        {
            return (int)(@object[0] | @object[1] << 8 | @object[2] << 16 | (@object[3] << 24));
        }

        public static int ToInt32(this ReadOnlySpan<byte> @object)
        {
            return (int)(@object[0] | @object[1] << 8 | @object[2] << 16 | (@object[3] << 24));
        }

        public static long ToInt64(this Span<byte> @object)
        {
            return BitConverter.ToInt64(@object);
        }

        public static long ToInt64(this ReadOnlySpan<byte> @object)
        {
            return BitConverter.ToInt64(@object);
        }

        public static long ToInt64(this byte[] @object)
        {
            return BitConverter.ToInt64(@object);
        }

        public static byte ToInt8(this byte[] @object)
        {
            return @object[0];
        }

        public static byte ToInt8(this Span<byte> @object)
        {
            return @object[0];
        }

        public static byte ToInt8(this ReadOnlySpan<byte> span)
        {
            return span[0];
        }

        private static byte[] __toByteArrayByte(this byte @object)
        {
            if (@object <= 0) throw new ArgumentOutOfRangeException(nameof(@object));
            return new[] { @object };
        }

        private static byte[] __toByteArrayInt(this int @object)
        {
            return new[]
            {
                (byte)@object,
                (byte)(@object >> 8),
                (byte)(@object >> 16),
                (byte)(@object >> 24)
            };
        }

        private static byte[] __toBytearrayLong(this long @object)
        {
            return new[] {
                (byte)@object,
                (byte)(@object >> 8),
                (byte)(@object >> 16),
                (byte)(@object >> 24),
                (byte)(@object >> 32),
                (byte)(@object >> 40),
                (byte)(@object >> 48),
                (byte)(@object >> 54)
            };
        }

        private static byte[] __toByteArrayShort(this short @object)
        {
            return new[] {
                (byte)@object,
                (byte)(@object >> 8)
            };
        }
        //private static Dictionary<char, byte> _chars 
        //    = Enumerable.Range(0, 255).ToDictionary(c => (char)c, b => (byte)b);

        private static byte[] __toByteArrayString(this string @object)
        {
            var bytes = new byte[@object.Length];
            for (var index = 0; index < @object.Length; bytes[index] = (byte)@object[index++]) ;
            return bytes;
        }
    }
}
