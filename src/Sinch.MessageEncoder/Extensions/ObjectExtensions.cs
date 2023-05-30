namespace Sinch.MessageEncoder.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteArray(this object @object)
        {
            switch (@object)
            {
                case long @long:
                {
                    byte[] bytes = new[]
                    {
                        (byte)@long,
                        (byte)(@long >> 8),
                        (byte)(@long >> 16),
                        (byte)(@long >> 24),
                        (byte)(@long >> 32),
                        (byte)(@long >> 40),
                        (byte)(@long >> 48),
                        (byte)(@long >> 54)
                    };
                    return bytes;
                }
                case int @int:
                {
                    byte[] bytes = new[]
                    {
                        (byte)@int,
                        (byte)(@int >> 8),
                        (byte)(@int >> 16),
                        (byte)(@int >> 24)
                    };

                    return bytes;
                }
                case short @short:
                {
                    byte[] bytes = new[]
                    {
                        (byte)@short,
                        (byte)(@short >> 8),
                    };
                    return bytes;
                }
                case byte @byte:  return new[]{@byte}; 
                case string @string:
                {
                    byte[] bytes = new byte[@string.Length];
                    for (int index = 0; index < @string.Length; bytes[index] = (byte)@string[index++]) { };
                    return bytes;
                }
                default: return default;
            }
        }

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
