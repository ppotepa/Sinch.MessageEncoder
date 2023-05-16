using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

namespace Sinch.MessageEncoder.PoC
{
    internal class Program
    {
        #region unused_for_now
        //1024x1024
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum MSG_HEADER_TYPE
        {
            CONTENT_HEADER_TYPE = 1,
            MSG_SIZE = 2,
            TIMESTAMP = 3,
            CONTAINS_IMAGES = 4
        }
        #endregion 

        static unsafe void Main(string[] args)
        {
            byte[] headerInfo = {
                0x4d, 0x53, 0x47, 0x5f, 0x4c, 0x45,
                0x4e, 0x3a, 0x3a, 0x31, 0x0a, 0x4d,
                0x53, 0x47, 0x5f, 0x54, 0x59, 0x50,
                0x45, 0x3a, 0x3a, 0x54, 0x45, 0x58,
                0x54
            };

            byte[] payload =
            {
                101, 102, 103, 104, 105, 106, 107, 108,
                101, 102, 103, 104, 105, 106, 107, 108,
                101, 102, 103, 104, 105, 106, 107, 108,
                101, 102, 103, 104, 105, 106, 107, 108
            };

            MyTestMessage testMessage = default;
            fixed (byte* ptrPayload = payload, ptrHeader = headerInfo)
            {
                testMessage = new MyTestMessage
                {
                    hours = 1,
                    minutes = 2
                };

                Marshal.Copy(payload, 0, new IntPtr(testMessage.payload), 24);
                Marshal.Copy(headerInfo, 0, new IntPtr(testMessage.headerInfo), 25);
            }

            byte[] transportBytes = new byte[sizeof(MyTestMessage)];
            
            fixed (byte* p = transportBytes)
            {
                var typed = (MyTestMessage*)p;
                *typed = testMessage;
            }

            File.WriteAllBytes("test_bytes.bin", transportBytes);
            Console.WriteLine(BitConverter.ToString(transportBytes));
        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct MyTestMessage
        {
            [FieldOffset(1)]
            public byte minutes;

            [FieldOffset(2)]
            public byte hours;

            [FieldOffset(3)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 63 * 2)]
            public fixed byte headerInfo[63 * 2];

            [FieldOffset(28)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public fixed byte payload[24];

            public MyTestMessage()
            {
                minutes = (byte)DateTime.Now.Minute;
                hours = (byte)DateTime.Now.Second;
            }
        }
    }
}
