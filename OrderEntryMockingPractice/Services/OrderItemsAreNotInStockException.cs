using System;
using System.Runtime.Serialization;

namespace OrderEntryMockingPractice.Services
{
    [Serializable]
    public class OrderItemsAreNotInStockException : Exception
    {
        public OrderItemsAreNotInStockException()
        {
        }

        public OrderItemsAreNotInStockException(string message) : base(message)
        {
        }

        public OrderItemsAreNotInStockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderItemsAreNotInStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}