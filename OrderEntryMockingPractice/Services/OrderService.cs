using OrderEntryMockingPractice.Models;
using System.Linq;

namespace OrderEntryMockingPractice.Services
{
    public class OrderService
    {
        private readonly OrderService _orderService;
        private readonly Customer _customer;
        private readonly OrderConfirmation _orderConfirmation;
        private readonly OrderSummary _orderSummary;
        private readonly Product _product;
        private readonly TaxEntry _taxEntry;
        private readonly Order _order;

        public bool CheckOrderItemsAreUniqueBySKU(Order order)
        {
            int uniqueItems = order.OrderItems
                .Select(item => item.Product.Sku).Distinct().Count();
            int totalItems = order.OrderItems.Count();

            if (totalItems != uniqueItems)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckIfOrderIsValid(Order order)
        {
            return CheckOrderItemsAreUniqueBySKU(order);
        }

        public OrderSummary PlaceOrder(Order order)
        {
            if(CheckIfOrderIsValid(order) == false)
            {
                throw new OrderItemsAreNotUniqueBySKUException();
            }
            return null;
        }
    }
}
