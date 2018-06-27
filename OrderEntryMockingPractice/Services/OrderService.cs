using System.Linq;
using OrderEntryMockingPractice.Models;

namespace OrderEntryMockingPractice.Services
{
    public class OrderService
    {
        private readonly IProductRepository _productRepository;

        public OrderService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool CheckIfProductIsInStock(Order order, IProductRepository productRepository)
        {
            foreach (var orderItem in order.OrderItems.ToList())
                if (productRepository.IsInStock(orderItem.Product.Sku) == false)
                    return false;
            return true;
        }

        public OrderSummary PlaceOrder(Order order)
        {
            if (order.CheckOrderItemsAreUniqueBySKU() == false) throw new OrderItemsAreNotUniqueBySKUException();

            if (CheckIfProductIsInStock(order, _productRepository) == false)
                throw new OrderItemsAreNotInStockException();

            var orderSummary = new OrderSummary
            {
                OrderId = 12,
                OrderNumber = "randomnumber",
                OrderItems = order.OrderItems,
                CustomerId = 34
            };

            return new OrderSummary();
        }
    }
}