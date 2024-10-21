using Api.Models;

namespace Api.Services.Interfaces
{
    public interface ICongestionTaxCalculatorService
    {
        Task<int> GetTax(Vehicle vehicle, DateTime[] dates);
    }
}
