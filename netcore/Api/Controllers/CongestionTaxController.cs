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
        public async Task<int> GetCongestionTaxFee([FromQuery] GetCongestionTaxRequest request)
        {
            if (!Enum.TryParse<VehiclesType>(request.VehicleType, true, out VehiclesType parsedVehiclesType))
            {
                throw new Exception();
            }

            if (request.TravelDates == null )
            {
                throw new Exception("Dates cannot be null or empty.");
            }


            DateTime[] travelDates = request.TravelDates
                .Select(date => DateTime.Parse(date))
                .ToArray();

            try
            {
                return await _congestionTaxCalculatorService.GetTax(new Vehicle(parsedVehiclesType), travelDates);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
