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
       

        [Test]
        public void Order_Items_Are_Unique_By_SKU()
        {
            //Arrange
            OrderItem orderItem1 = new OrderItem();
            Product product1 = new Product();
            product1.Sku = "sku1";
            orderItem1.Product = product1;
            
            OrderItem orderItem2 = new OrderItem();
            Product product2 = new Product();
            product2.Sku = "sku2";
            orderItem2.Product = product2;

            OrderItem orderItem3 = new OrderItem();
            Product product3 = new Product();
            product3.Sku = "sku2";
            orderItem3.Product = product3;

            //Act
            OrderSummary orderSummary = new OrderSummary();
            orderSummary.OrderItems = new System.Collections.Generic.List<OrderItem>() {
                orderItem1,
                orderItem2,
                orderItem3,
            };

            
            int expectedCount = 2;
            int actualCount = orderSummary.OrderItems
                .Select(item => item.Product.Sku).Distinct().Count();
            Assert.AreEqual(expectedCount, actualCount);
            
        }

        [Test]
        public void Check_All_Products_Are_In_Stock()
        {
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.IsInStock(It.IsAny<String>()));

            mockRepository.VerifyAll();
        }

        [Test]
        public void If_Order_Not_Valid_Throw_Exception()
        {

        }

        [Test]
        public void If_Order_Is_Valid_Return_Order_Summary()
        {

        }
        

    }
}
