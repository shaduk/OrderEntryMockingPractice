using NUnit.Framework;
using OrderEntryMockingPractice.Models;
using System;
using Moq;
using System.Linq;
using OrderEntryMockingPractice.Services;

namespace OrderEntryMockingPracticeTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private Customer _customer;
        private OrderConfirmation _orderConfirmation;
        private OrderSummary _orderSummary;
        private Product _product;
        private TaxEntry _taxEntry;
        private Order _order;

        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private Mock<IEmailService> _mockEmailService;
        private Mock<ITaxRateService> _mockTaxRateService;
        private Mock<IOrderFulfillmentService> _mockOrderFulfillmentService;

        private const string product_sku_1 = "product_sku_1";
        private const string product_sku_2 = "product_sku_2";
        private const string product_sku_3 = "product_sku_3";

        [SetUp]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockTaxRateService = new Mock<ITaxRateService>();
            _mockOrderFulfillmentService = new Mock<IOrderFulfillmentService>();
            _customer = new Customer();

            _mockProductRepository
                .Setup(p => p.IsInStock(product_sku_1))
                .Returns(true);

            _mockProductRepository
                .Setup(p => p.IsInStock(product_sku_2))
                .Returns(true);

            _orderService = new OrderService();
        }

        [Test]
        
        public void If_Order_Items_Are__Not_Unique_By_SKU_Throw_Exception()
        {
            //Arrange
            _order = new Order();
            OrderItem orderItem1 = new OrderItem
            {
                Product = new Product()
            };
            orderItem1.Product.Sku = product_sku_1;

            OrderItem orderItem2 = new OrderItem
            {
                Product = new Product()
            };
            orderItem2.Product.Sku = product_sku_1;

            _order.OrderItems = new System.Collections.Generic.List<OrderItem>()
            {
                orderItem1,
                orderItem2
            };

            //Act and Assert

            Assert.Throws<OrderItemsAreNotUniqueBySKUException>
                (() => _orderService.PlaceOrder(_order, _mockProductRepository.Object));
        }

        [Test]
        public void If_Order_Items_Are_Unique_By_SKU_Return_Order_Summary()
        {
            //Arrange
            _order = new Order();
            OrderItem orderItem1 = new OrderItem
            {
                Product = new Product()
            };
            orderItem1.Product.Sku = product_sku_2;

            OrderItem orderItem2 = new OrderItem
            {
                Product = new Product()
            };
            orderItem2.Product.Sku = product_sku_1;

            _order.OrderItems = new System.Collections.Generic.List<OrderItem>()
            {
                orderItem1,
                orderItem2
            };

            //Act and Assert
            Assert.IsInstanceOf<OrderSummary>(_orderService.PlaceOrder(_order, _mockProductRepository.Object));
        }

        [Test]
        public void Check_All_Products_Are_Not_In_Stock_throw_Exception()
        {
            _order = new Order();
            OrderItem orderItem1 = new OrderItem
            {
                Product = new Product()
            };
            orderItem1.Product.Sku = product_sku_3;

            OrderItem orderItem2 = new OrderItem
            {
                Product = new Product()
            };
            orderItem2.Product.Sku = product_sku_1;

            _order.OrderItems = new System.Collections.Generic.List<OrderItem>()
            {
                orderItem1,
                orderItem2
            };

            Assert.Throws<OrderItemsAreNotInStockException>
                 (() => _orderService.PlaceOrder(_order, _mockProductRepository.Object));
        }

        [Test]
        public void Check_All_Products_Are_In_Stock_Returns_OrderSummary()
        {
            _order = new Order();
            OrderItem orderItem1 = new OrderItem
            {
                Product = new Product()
            };
            orderItem1.Product.Sku = product_sku_2;

            OrderItem orderItem2 = new OrderItem
            {
                Product = new Product()
            };
            orderItem2.Product.Sku = product_sku_1;

            _order.OrderItems = new System.Collections.Generic.List<OrderItem>()
            {
                orderItem1,
                orderItem2
            };

            Assert.IsInstanceOf<OrderSummary>(_orderService.PlaceOrder(_order, _mockProductRepository.Object));
        }
        

    }
}
