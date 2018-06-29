using System.Linq;
using OrderEntryMockingPractice.Models;
using System.Collections.Generic;

namespace OrderEntryMockingPractice.Services
{
    public class OrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderFulfillmentService _orderFulfillmentService;
        private readonly ITaxRateService _taxRateService;
        private readonly IEmailService _emailService;
        private string _postal_code;
        private string _country;

        public OrderService(IProductRepository productRepository,
            ITaxRateService taxRateService,
            IEmailService emailService,
            IOrderFulfillmentService orderFulfillmentService,
            string postal_code, string country)
        {
            _productRepository = productRepository;
            _taxRateService = taxRateService;
            _orderFulfillmentService = orderFulfillmentService;
            _emailService = emailService;
            _postal_code = postal_code;
            _country = country;
        }

        public bool AreProductsInStock(Order order, IProductRepository productRepository)
        {
            foreach (var orderItem in order.OrderItems.ToList())
                if (productRepository.IsInStock(orderItem.Product.Sku) == false)
                    return false;
            return true;
        }

        public decimal GetTotalTax()
        {
            decimal total_tax = 0;

            foreach (TaxEntry entry in _taxRateService.GetTaxEntries(_postal_code, _country))
                total_tax += entry.Rate;

            return total_tax;
        }

        public OrderSummary PlaceOrder(Order order)
        {
            if (order.AreOrderItemsUniqueBySKU() == false) throw new OrderItemsAreNotUniqueBySKUException();

            if (AreProductsInStock(order, _productRepository) == false)
                throw new OrderItemsAreNotInStockException();

            OrderConfirmation orderConfirmation = _orderFulfillmentService.Fulfill(order);
            IEnumerable<TaxEntry> tax_entries = _taxRateService.GetTaxEntries(_postal_code, _country);
            decimal net_total = order.GetOrderTotal();
            decimal order_total = net_total + GetTotalTax();

            OrderSummary orderSummary = new OrderSummary
            {
                OrderId = orderConfirmation.OrderId,
                OrderNumber = orderConfirmation.OrderNumber,
                CustomerId = orderConfirmation.CustomerId,
                OrderItems = order.OrderItems,
                NetTotal = net_total,
                OrderTotal = order_total,
                Taxes = tax_entries,

            };
            
            _emailService.SendOrderConfirmationEmail(orderConfirmation.CustomerId, orderConfirmation.OrderId);

            return orderSummary;
        }
    }
}