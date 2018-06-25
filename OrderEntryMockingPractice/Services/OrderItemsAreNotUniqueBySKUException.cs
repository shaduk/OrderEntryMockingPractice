using System;
using System.Runtime.Serialization;

namespace OrderEntryMockingPractice.Services
{
    [Serializable]
    public class OrderItemsAreNotUniqueBySKUException : Exception
    {
        public OrderItemsAreNotUniqueBySKUException()
        {
        }

        public OrderItemsAreNotUniqueBySKUException(string message) : base(message)
        {
        }

        public OrderItemsAreNotUniqueBySKUException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderItemsAreNotUniqueBySKUException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}