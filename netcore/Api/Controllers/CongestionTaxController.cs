using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CongestionTaxController : ControllerBase
    {
       
        private readonly ICongestionTaxCalculatorService _congestionTaxCalculatorService;

        public CongestionTaxController(ICongestionTaxCalculatorService congestionTaxCalculatorService)
        {
            _congestionTaxCalculatorService = congestionTaxCalculatorService;
        }

        [HttpGet(Name = "GetCongestionTaxFee")]
        public int GetCongestionTaxFee([FromQuery] GetCongestionTaxRequest request) // can think of returning a response entity 
        {
            var isValidVehicleType = Enum.TryParse<VehiclesType>(request.VehicleType, false, out VehiclesType parsedVehiclesType);
            if (!isValidVehicleType)
            {
                throw new Exception();
            }

            if (request.TravelDates == null )
            {
                throw new Exception("Dates cannot be null or empty.");
            }

            try
            {
                Console.WriteLine($"Controller calling GetTax with VehicleType: {parsedVehiclesType} and Dates: {string.Join(", ", request.TravelDates)}");

                //return _congestionTaxCalculatorService.GetTax(new Vehicle(parsedVehiclesType), travelDates);
                return _congestionTaxCalculatorService.GetTax(parsedVehiclesType, request.TravelDates);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
