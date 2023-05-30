using System.IO;

namespace Sinch.MessageEncoder.DataSource
{
    public abstract class BinaryDataSource
    {
        protected BinaryDataSource(byte[] data)
        {
            this.Data = data;
        }

        protected BinaryDataSource(string filePath)
        {
            this.Data = File.ReadAllBytes(filePath);
        }

        public byte[] Data { get; set; }
    }

    public class CompressedDataSource : BinaryDataSource
    {
        public byte[] Data { get; set; }

        public CompressedDataSource(byte[] data) : base(data)
        {
        }
    }
}
