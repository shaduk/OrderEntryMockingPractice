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
        private IProductRepository _productRepository;

        public OrderService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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

        public OrderSummary PlaceOrder(Order order)
        {
            if(order.CheckOrderItemsAreUniqueBySKU() == false)
            {
                throw new OrderItemsAreNotUniqueBySKUException();
            }

            if(CheckIfProductIsInStock(order, _productRepository) == false)
            {
                throw new OrderItemsAreNotInStockException();
            }


            return new OrderSummary();
        }
    }
}
