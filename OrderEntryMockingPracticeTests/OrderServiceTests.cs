﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OrderEntryMockingPractice.Models;
using OrderEntryMockingPractice.Services;

namespace OrderEntryMockingPracticeTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockTaxRateService = new Mock<ITaxRateService>();
            _mockOrderFulfillmentService = new Mock<IOrderFulfillmentService>();
            _orderConfirmation = CreateOrderConfirmationObject(customer_id_1, order_id_1, order_no_1);

            _taxEntries = new List<TaxEntry>()
            {
                CreateTaxEntry("sample", 23),
                CreateTaxEntry("sample2", 10)
            };

         
            _mockProductRepository
                .Setup(p => p.IsInStock(product_sku_1))
                .Returns(true);

            _mockProductRepository
                .Setup(p => p.IsInStock(product_sku_2))
                .Returns(true);

            _mockOrderFulfillmentService
                .Setup(p => p.Fulfill(It.IsAny<Order>()))
                .Returns((Order order) => CreateOrderConfirmationObject(order.CustomerId, order_id_1, order_no_1));

            _mockTaxRateService
                .Setup(t => t.GetTaxEntries(postal_code, country))
                .Returns(_taxEntries);

            _orderService = new OrderService(_mockProductRepository.Object, _mockTaxRateService.Object, _mockEmailService.Object, _mockOrderFulfillmentService.Object, postal_code, country);
        }

        

        private OrderService _orderService;
        private Order _order;
        private OrderConfirmation _orderConfirmation;
        private IEnumerable<TaxEntry> _taxEntries;

        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private Mock<IEmailService> _mockEmailService;
        private Mock<ITaxRateService> _mockTaxRateService;
        private Mock<IOrderFulfillmentService> _mockOrderFulfillmentService;

        private const string product_sku_1 = "product_sku_1";
        private const string product_sku_2 = "product_sku_2";
        private const string product_sku_3 = "product_sku_3";

        private const int customer_id_1 = 12345;
        private const int customer_id_2 = 23456;

        private const int order_id_1 = 12;
        private const int order_id_2 = 14;

        private const string order_no_1 = "1123";
        private const string order_no_2 = "2332";

        private const string postal_code = "98052";
        private const string country = "USA";

        private const int quantity = 2;
   

        private static OrderItem CreateOrderItemObject(string product_sku, int quantity, decimal price)
        {
            var orderItem = new OrderItem
            {
                Product = new Product(),
                Quantity = quantity
            };
            orderItem.Product.Sku = product_sku;
            orderItem.Product.Price = price;
            return orderItem;
        }

        private Order CreateOrderObject(string product_sku_1, string product_sku_2, int customer_id = customer_id_1)
        {
            _order = new Order();
            OrderItem orderItem1 = CreateOrderItemObject(product_sku_1, 1, 30);
            OrderItem orderItem2 = CreateOrderItemObject(product_sku_2, 2, 10);

            _order.OrderItems = new List<OrderItem>
            {
                orderItem1,
                orderItem2
            };
            _order.CustomerId = customer_id;
            return _order;
        }

        

        private OrderConfirmation CreateOrderConfirmationObject(int customerId, int orderId, string orderNumber)
        {
            var orderConfirmationObject = new OrderConfirmation();
            orderConfirmationObject.CustomerId = customerId;
            orderConfirmationObject.OrderId = orderId;
            orderConfirmationObject.OrderNumber = orderNumber;
            return orderConfirmationObject;
        }

        private TaxEntry CreateTaxEntry(string tax_description, decimal tax_rate)
        {
            TaxEntry taxEntry = new TaxEntry
            {
                Description = tax_description,
                Rate = tax_rate
            };
            return taxEntry;
        }

        [Test]
        public void Test_All_Products_Are_In_Stock_Returns_Correct_OrderSummary()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2, customer_id_2);
           
            OrderSummary orderSummary = _orderService.PlaceOrder(_order);
            Assert.AreEqual(orderSummary.CustomerId, customer_id_2);
            Assert.AreEqual(orderSummary.OrderId, order_id_1);
            Assert.AreEqual(orderSummary.OrderNumber, order_no_1);
        }

        

        [Test]
        public void Test_All_Products_Are_Not_In_Stock_throw_Exception()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_3);
            
            Assert.Throws<OrderItemsAreNotInStockException>
                (() => _orderService.PlaceOrder(_order));
        }

        [Test]
        public void If_Order_Items_Are__Not_Unique_By_SKU_Throw_Exception()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_1);

            //Act and Assert

            Assert.Throws<OrderItemsAreNotUniqueBySKUException>
                (() => _orderService.PlaceOrder(_order));
        }

        [Test]
        public void Test_Email_Service_Has_Been_Called_Once()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2);
            _orderService.PlaceOrder(_order);
            _mockEmailService.Verify(e => e.SendOrderConfirmationEmail(customer_id_1, order_id_1), Times.Once());
        }

        [Test]
        public void Test_OrderFullFilment_Service_Was_Called_Once()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2);
            _orderService.PlaceOrder(_order);
            _mockOrderFulfillmentService.Verify(s => s.Fulfill(_order), Times.Once());
        }

        [Test]
        public void Test_TaxRateService_Was_Called_Once()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2);
            _orderService.PlaceOrder(_order);
            _mockTaxRateService.Verify(s => s.GetTaxEntries(postal_code, country), Times.Once());
        }

        [Test]
        public void Test_TaxRateService_Returns_The_Expected_TaxRate()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2);
            OrderSummary orderSummary = _orderService.PlaceOrder(_order);
            Assert.AreEqual(orderSummary.OrderTotal, 83);
        }

        [Test]
        public void Test_OrderService_Returns_The_Expected_NetTotal()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2);
            OrderSummary orderSummary = _orderService.PlaceOrder(_order);
            Assert.AreEqual(orderSummary.NetTotal, 50);
        }

        [Test]
        public void Test_OrderService_Returns_The_Expected_Tax_Entries()
        {
            _order = CreateOrderObject(product_sku_1, product_sku_2);
            OrderSummary orderSummary = _orderService.PlaceOrder(_order);
            Assert.AreEqual(orderSummary.Taxes, _taxEntries);
        }

        
    }
}