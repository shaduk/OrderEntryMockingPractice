using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OrderEntryMockingPractice.Models;
using OrderEntryMockingPractice.Services;

namespace OrderEntryMockingPracticeTests
{
    [TestFixture]
    public class TaxServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _taxEntries = new List<TaxEntry>();
            _mockTaxRateService = new Mock<ITaxRateService>();

            _mockTaxRateService
                .Setup(c => c.GetTaxEntries(postal_code, country))
                .Returns(_taxEntries);
        }

        private IEnumerable<TaxEntry> _taxEntries;
        private Mock<ITaxRateService> _mockTaxRateService;
        private const string postal_code = "98052";
        private const string country = "USA";

        [Test]
        public void Can_Retrive_Taxes_From_Taxrate_Service()
        {
            Assert.IsInstanceOf<IEnumerable<TaxEntry>>(_mockTaxRateService.Object.GetTaxEntries(postal_code, country));
        }
    }
}