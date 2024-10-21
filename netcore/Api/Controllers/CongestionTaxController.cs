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
        public async Task<int> GetCogestionTaxFee()
        {
            var vehicle = new Vehicle(VehiclesType.Car);
            try
            {
                return await _congestionTaxCalculatorService.GetTax(vehicle, [DateTime.Parse("2013-02-07 15:27:00")]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
