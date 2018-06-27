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
            _customer = new Customer();
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            _mockCustomerRepository
                .Setup(c => c.Get(customer_id_1))
                .Returns(_customer);

            _mockCustomerRepository
                .Setup(c => c.Get(customer_id_2))
                .Returns(_customer);
        }

        private Customer _customer;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private const int customer_id_1 = 12345;
        private const int customer_id_2 = 23456;

        [Test]
        public void GetCustomer_Repo_Returns_Customer_If_Present()
        {
            Assert.IsInstanceOf<Customer>(_mockCustomerRepository.Object.Get(customer_id_1));
            Assert.IsInstanceOf<Customer>(_mockCustomerRepository.Object.Get(customer_id_2));
        }

        [Test]
        public void GetCustomer_Repo_Returns_Null_If_Not_Present()
        {
        }
    }
}