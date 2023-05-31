namespace Sinch.MessageEncoder.Messages
{
    public abstract class Payload
    {
        protected Payload()
        {
        }

        public virtual object Serialize()
        {
            //object order = new object();
            //var result = order.Select
            //(
            //    property =>
            //    {
            //        byte[] byteArr = property.ToByteArray();
            //        return new[] { byteArr.Length.ToShortByteArray(), byteArr };
            //    }
            //);

            //return result.SelectMany(bytes => bytes);

            return default;
        }
    }
}