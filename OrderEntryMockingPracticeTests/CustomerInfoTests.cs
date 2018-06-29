using Moq;
using NUnit.Framework;
using OrderEntryMockingPractice.Models;
using OrderEntryMockingPractice.Services;

namespace OrderEntryMockingPracticeTests
{
    [TestFixture]
    internal class CustomerInfoTests
    {
        [SetUp]
        public void SetUp()
        {
            
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            _mockCustomerRepository
                .Setup(c => c.Get(customer_id_1))
                .Returns((int customer_id_1) => CreateCustomer(customer_id_1));

            _mockCustomerRepository
                .Setup(c => c.Get(customer_id_2))
                .Returns((int customer_id_2) => CreateCustomer(customer_id_2));
        }

       private Customer CreateCustomer(int customer_id)
        {
            _customer = new Customer();
            _customer.CustomerId = customer_id_1;
            return _customer;
        }

        private Customer _customer;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private const int customer_id_1 = 12345;
        private const int customer_id_2 = 23456;
        private const int customer_id_3 = 56566;

        [Test]
        public void GetCustomer_Repo_Returns_Correct_Customer_If_Present()
        {
            Customer _customer = _mockCustomerRepository.Object.Get(customer_id_1);
            Assert.AreEqual(_customer.CustomerId, customer_id_1);
        }

        [Test]
        public void GetCustomer_Repo_Returns_Null_If_Not_Present()
        {
            Customer _customer = _mockCustomerRepository.Object.Get(customer_id_3);
            Assert.AreEqual(_customer, null);
        }
    }
}