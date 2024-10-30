using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Api.Controllers;
using Api.Services;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Tests
{
    public class CongestionTaxControllerUnitTests
    {
        private readonly Mock<ICongestionTaxCalculatorService> _mockCongestionTaxCalculatorService;
        private readonly CongestionTaxController _controller;

        public CongestionTaxControllerUnitTests()
        {
            _mockCongestionTaxCalculatorService = new Mock<ICongestionTaxCalculatorService>();
            _controller = new CongestionTaxController(_mockCongestionTaxCalculatorService.Object);
        }



        [Fact]
        public void GetCongestionTaxFeeMultiEntryInOneHour_ReturnsExpectedAmount()
        {
            // Arrange
            var travelDateStrings = new List<string> { "2013-03-05 07:05:00", "2013-03-05 07:30:01", "2013-03-05 07:45:00" };
            int expectedTollAmount = 18;
            var request = new GetCongestionTaxRequest { VehicleType = "Motorbike", TravelDates = travelDateStrings };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.Is<VehiclesType>(v => v == VehiclesType.Car || v == VehiclesType.Motorbike), It.IsIn(travelDateStrings))).Returns(expectedTollAmount);


            // Act
            var result = _controller.GetCongestionTaxFee(request);

            // Assert
            Assert.IsType<int>(result);                  
            Assert.Equal(expectedTollAmount, result);    

        }


        [Fact]
        public void GetCongestionTaxOnWeekend_ReturnsExpectedAmount()
        {
            //Arrange
            var travelDateStrings = new List<string> { "2013-03-30 08:10:00", "2013-03-30 10:10:00" };
            int expectedTollAmount = 0;
            var request = new GetCongestionTaxRequest { VehicleType = "Car", TravelDates = travelDateStrings };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<VehiclesType>(), It.IsIn(travelDateStrings))).Returns(expectedTollAmount);

            // Act
            var result = _controller.GetCongestionTaxFee(request);

            // Assert;
            Assert.IsType<int>(result);
            Assert.Equal(expectedTollAmount, result);

        }

        [Fact]
        public void GetCongestionTaxOnPublicHoliday_ReturnsExpectedAmount()
        {
            var travelDateStrings = new List<string> { "2013-05-01 09:00:00" };
            int expectedTollAmount = 0;
            var request = new GetCongestionTaxRequest { VehicleType = "Car", TravelDates = travelDateStrings };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<VehiclesType>(), It.IsIn(travelDateStrings))).Returns(expectedTollAmount);

            // Act
            var result = _controller.GetCongestionTaxFee(request);

            // Assert;
            Assert.IsType<int>(result);
            Assert.Equal(expectedTollAmount, result);
        }

        [Fact]
        public void GetCongestionTaxOnJuly_ReturnsExpectedAmount()
        {
            var travelDateStrings = new List<string> { "2013-07-01 09:00:00", "2013-07-01 13:00:00" };
            int expectedTollAmount = 0;
            var request = new GetCongestionTaxRequest { VehicleType = "Car", TravelDates = travelDateStrings };

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsAny<VehiclesType>(), It.IsIn(travelDateStrings))).Returns(expectedTollAmount);

            // Act
            var result = _controller.GetCongestionTaxFee(request);

            // Assert;
            Assert.IsType<int>(result);
            Assert.Equal(expectedTollAmount, result);
        }

        [Fact]
        public void GetCongestionTaxOnExemptedVehicle_ReturnsExpectedAmount()
        {
            var travelDateStrings = new List<string> { "2013-03-02 08:00:00", "2013-03-02 12:00:00", "2013-03-02 17:00:00" };
            int expectedTollAmount = 0;
            var request = new GetCongestionTaxRequest { VehicleType = "Emergency", TravelDates = travelDateStrings };

            DateTime[] travelDates = request.TravelDates
                .Select(date => DateTime.Parse(date))
                .ToArray();

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.IsIn(VehiclesType.Diplomat, VehiclesType.Emergency, VehiclesType.Military), It.IsAny<IList<string>>())).Returns(expectedTollAmount);

            // Act
            var result = _controller.GetCongestionTaxFee(request);
            // Assert;
            Assert.IsType<int>(result);
            Assert.Equal(expectedTollAmount, result);
        }

        [Fact]
        public void GetCongestionTaxFeeHighestTotalInDay_ReturnsExpectedAmount()
        {
            int expectedTollAmount = 60;
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

            _mockCongestionTaxCalculatorService.Setup(service => service.GetTax(It.Is<VehiclesType>(v => v == VehiclesType.Car || v == VehiclesType.Motorbike), It.IsAny<IList<string>>())).Returns(expectedTollAmount);

            // Act
            var result = _controller.GetCongestionTaxFee(request);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(expectedTollAmount, result);
        }

    }
}