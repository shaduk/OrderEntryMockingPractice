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

        public bool CheckIfProductIsInStock(Order order, IProductRepository productRepository)
        {
            /*
            bool f = order.OrderItems
                 .Select(item => item.Product.Sku) */

            foreach(OrderItem orderItem in order.OrderItems.ToList())
            {
                if(productRepository.IsInStock(orderItem.Product.Sku) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public OrderSummary PlaceOrder(Order order, IProductRepository productRepository)
        {
            if(CheckOrderItemsAreUniqueBySKU(order) == false)
            {
                throw new OrderItemsAreNotUniqueBySKUException();
            }

            if(CheckIfProductIsInStock(order, productRepository) == false)
            {
                throw new OrderItemsAreNotInStockException();
            }


            return new OrderSummary();
        }
    }
}
