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

            Assert.Equal(result, 0);
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
            Assert.Equal(result, 13);
        }

        [Fact]
        public async Task GetCongestionTaxFee_InvalidVehicleType_ThrowsException()
        {
            var mockDateStrings = new List<string> { "2013-02-08 06:27:00", "2013-02-08 06:20:27", "2013-02-08 14:35:00", "2013-02-08 15:29:00" };
            var request = new GetCongestionTaxRequest { VehicleType = "InvalidVehicleType", TravelDates = mockDateStrings };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<Vehicle>(), It.IsAny<DateTime[]>())).ReturnsAsync(13);

            // Act
            Func<Task> act = async () => await _controller.GetCongestionTaxFee(request);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetCongestionTaxFee_NullTravelDates_ThrowsException()
        {
            var request = new GetCongestionTaxRequest { VehicleType = "Car", TravelDates = null };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<Vehicle>(), It.IsAny<DateTime[]>())).ReturnsAsync(13);

            // Act
            Func<Task> act = async () => await _controller.GetCongestionTaxFee(request);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetCongestionTaxFee_TollFee_NotExceeded()
        {
            var request = new GetCongestionTaxRequest
            {
                VehicleType = "Car",
                TravelDates = new List<string>
                {
                   "2013-02-08 06:27:00",
                   "2013-02-08 06:20:27",
                   "2013-02-08 14:35:00",
                   "2013-02-08 15:29:00",
                   "2013-02-08 15:47:00",
                   "2013-02-08 16:01:00",
                   "2013-02-08 16:48:00",
                   "2013-02-08 17:49:00",
                   "2013-02-08 18:29:00",
                   "2013-02-08 18:35:00",
                }
            };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<Vehicle>(), It.IsAny<DateTime[]>())).ReturnsAsync(60);

            // Act
            var result = await _controller.GetCongestionTaxFee(request);

            // Assert

            Assert.Equal(result, 60);
        }
    }
}