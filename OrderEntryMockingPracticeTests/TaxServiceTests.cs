using NUnit.Framework;
using OrderEntryMockingPractice.Models;
using System;
using Moq;
using System.Linq;
using OrderEntryMockingPractice.Services;
using System.Collections.Generic;

namespace OrderEntryMockingPracticeTests
{
    [TestFixture]
    public class TaxServiceTests
    {
        private IEnumerable<TaxEntry> _taxEntries;
        private Mock<ITaxRateService> _mockTaxRateService;
        private const string postal_code = "98052";
        private const string country = "USA";

        [SetUp]
        public void SetUp()
        {
            _taxEntries = new List<TaxEntry>();
            _mockTaxRateService = new Mock<ITaxRateService>();

            _mockTaxRateService
                .Setup(c => c.GetTaxEntries(postal_code, country))
                .Returns(_taxEntries);
        }

        [Test]
        public void Can_Retrive_Taxes_From_Taxrate_Service()
        {
            Assert.IsInstanceOf<IEnumerable<TaxEntry>>(_mockTaxRateService.Object.GetTaxEntries(postal_code, country));
        }
    }
}
