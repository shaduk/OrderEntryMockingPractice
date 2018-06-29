using OrderEntryMockingPractice.Services;
using System.Collections.Generic;
using System.Linq;

namespace OrderEntryMockingPractice.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new List<OrderItem>();
        }
        
        public int CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public bool AreOrderItemsUniqueBySKU()
        {
            int uniqueItems = this.OrderItems
                .Select(item => item.Product.Sku).Distinct().Count();
            int totalItems = OrderItems.Count();

            if (totalItems != uniqueItems)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public decimal GetOrderTotal()
        {
            decimal totalPrice = 0;

            foreach (OrderItem item in this.OrderItems)
            {
                totalPrice += item.GetNetPrice();
            }
            return totalPrice;
        }


        
    }
}
