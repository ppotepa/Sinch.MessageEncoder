using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Extensions
{
    public static class ObjectExtensions
    {
        private static byte[] ToByteArray(this int @object)
        {
            byte[] bytes = new[]
            {
                (byte)@object,
                (byte)(@object >> 8),
                (byte)(@object >> 16),
                (byte)(@object >> 24)
            };

            return bytes;
        }

        private static byte[] ToByteArray(this short @object)
        {
            byte[] bytes = new[]
            {
                (byte)@object,
                (byte)(@object >> 8)
            };

            return bytes;
        }

        private static byte[] ToByteArray(this byte @object)
        {
            byte[] bytes = new[]
            {
                (byte)@object,
            };

            return bytes;
        }

        private static byte[] ToByteArray(this long @object)
        {
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

        private static byte[] ToByteArray(this string @object)
        {
            byte[] bytes = new byte[@object.Length];
            for (int index = 0; index < @object.Length; bytes[index] = (byte)@object[index++]) { };
            return bytes;
        }

        public static byte[] ToByteArray(this object @object) => @object switch
        {
            long @long => ToByteArray(@long),
            int @int => ToByteArray(@int),
            short @short => ToByteArray(@short),
            byte @byte => ToByteArray(@byte),
            string @string => ToByteArray(@string),
            null => Array.Empty<byte>()
        };

        public static byte[] ToShortByteArray(this int @object)
        {
            if (@object == default)
                return Array.Empty<byte>();

            object target = (short)@object;
            return target switch
            {
                short @short => ToByteArray(@short),
            };
        }

        public static IEnumerable<byte> GetBytes(this object data)
        {
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
