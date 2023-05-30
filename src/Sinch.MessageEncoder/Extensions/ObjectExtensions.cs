using System;

namespace Sinch.MessageEncoder.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteArray(this int @object)
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

        public static byte[] ToByteArray(this short @object)
        {
            byte[] bytes = new[]
            {
                (byte)@object,
                (byte)(@object >> 8)
            };

            return bytes;
        }

        public static byte[] ToByteArray(this byte @object)
        {
            byte[] bytes = new[]
            {
                (byte)@object,
            };

            return bytes;
        }

        public static byte[] ToByteArray(this long @object)
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

        public static byte[] ToByteArray(this string @object)
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
            string @byte => ToByteArray(@byte),
            _ => default
        };


        //public static unsafe byte[] ToByteArrayUnsafe(this object @object)
        //{
        //    if (@object is long @long)
        //    {
        //        byte[] bytes = new byte[8];
        //        fixed (byte* bytesPtr = bytes)
        //        {
        //            *((long*)bytesPtr) = @long;
        //        }
        //        return bytes;
        //    }

        //    if (@object is int @int)
        //    {
        //        byte[] bytes = new byte[4];
        //        fixed (byte* bytesPtr = bytes)
        //        {
        //            *((long*)bytesPtr) = @int;
        //        }
        //        return bytes;
        //    }

        //    return default;
        //}
    }
}
