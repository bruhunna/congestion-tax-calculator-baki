using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Api.Controllers;
using Api.Services;
using Api.Models;
using Api.Services.Interfaces;

namespace Api.Tests
{
    public class CongestionTaxControllerTests
    {
        private Mock<ICongestionTaxCalculatorService> _mockCongestionTaxCalculatorService;
        private CongestionTaxController _controller;

        public CongestionTaxControllerTests()
        {
            _mockCongestionTaxCalculatorService = new Mock<ICongestionTaxCalculatorService>();
            _controller = new CongestionTaxController(_mockCongestionTaxCalculatorService.Object);
        }

        [Fact]
        public async Task GetCongestionTaxFeeTollFree_ReturnsOkResult()
        {
            var mockDateStrings = new List<string> { "2013-02-08 06:27:00", "2013-02-08 06:20:27", "2013-02-08 14:35:00", "2013-02-08 15:29:00" };
            var request = new GetCongestionTaxRequest { VehicleType = "Emergency", TravelDates = mockDateStrings };
            
            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<Vehicle>(), It.IsAny<DateTime[]>())).ReturnsAsync(0);


            // Act
            var result = await _controller.GetCongestionTaxFee(request);

            // Assert;
            Console.WriteLine($"Returned Data: {string.Join(", ", result)}");
            
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetCongestionTaxFee_ReturnsOkResult()
        {
            var mockDateStrings = new List<string> { "2013-02-08 06:27:00", "2013-02-08 06:20:27", "2013-02-08 14:35:00", "2013-02-08 15:29:00" };
            var request = new GetCongestionTaxRequest { VehicleType = "Car", TravelDates = mockDateStrings };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<Vehicle>(), It.IsAny<DateTime[]>())).ReturnsAsync(13);


            // Act
            var result = await _controller.GetCongestionTaxFee(request);

            // Assert;
            Console.WriteLine($"Returned Data: {string.Join(", ", result)}");

            Assert.Equal(13, result);
        }
    }
}