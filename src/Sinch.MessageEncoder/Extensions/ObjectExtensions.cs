using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Extensions
{
    public static class ObjectExtensions
    {
        private static byte[] __toByteArrayByte(this byte @object)
        {
            if (@object <= 0) throw new ArgumentOutOfRangeException(nameof(@object));

            byte[] bytes = new[]
            {
                (byte)@object,
            };

            return bytes;
        }

        private static byte[] __toByteArrayShort(this short @object)
        {
            if (@object <= 0) throw new ArgumentOutOfRangeException(nameof(@object));

            byte[] bytes = new[]
            {
                (byte)@object,
                (byte)(@object >> 8)
            };

            return bytes;
        }

        private static byte[] __toByteArrayInt(this int @object)
        {
            if (@object <= 0) throw new ArgumentOutOfRangeException(nameof(@object));

            byte[] bytes = new[]
            {
                (byte)@object,
                (byte)(@object >> 8),
                (byte)(@object >> 16),
                (byte)(@object >> 24)
            };

            return bytes;
        }

        private static byte[] __toBytearrayLong(this long @object)
        {
            if (@object <= 0) throw new ArgumentOutOfRangeException(nameof(@object));

            byte[] bytes = new[]
            {
                (byte)@object,
                (byte)(@object >> 8),
                (byte)(@object >> 16),
                (byte)(@object >> 24),
                (byte)(@object >> 32),
                (byte)(@object >> 40),
                (byte)(@object >> 48),
                (byte)(@object >> 54)
            };

            return bytes;
        }

        private static byte[] __toByteArrayString(this string @object)
        {
            if (@object == null) throw new ArgumentNullException(nameof(@object));

            byte[] bytes = new byte[@object.Length];
            for (int index = 0; index < @object.Length; bytes[index] = (byte)@object[index++]) { };
            return bytes;
        }

        public static byte[] ToByteArray(this object @object)
        {
            if (@object == null) throw new ArgumentNullException(nameof(@object));

            return @object switch
            {
                long @long => __toBytearrayLong(@long),
                int @int => __toByteArrayInt(@int),
                short @short => __toByteArrayShort(@short),
                byte @byte => __toByteArrayByte(@byte),
                string @string => __toByteArrayString(@string),
                byte[] @bytes => @bytes,
                null => Array.Empty<byte>(),
                _ => throw new ArgumentOutOfRangeException(nameof(@object), @object, "null")
            };
        }

        public static byte[] ToShortByteArray(this int @object)
        {
            if (@object <= 0) throw new ArgumentOutOfRangeException(nameof(@object));

            if (@object == default)
                return Array.Empty<byte>();

            object target = (short)@object;
            return target switch
            {
                short @short => __toByteArrayShort(@short),
            };
        }

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
    }
}
