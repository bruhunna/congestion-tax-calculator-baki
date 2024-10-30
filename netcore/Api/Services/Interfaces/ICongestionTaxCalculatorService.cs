using Api.Models;

namespace Api.Services.Interfaces
{
    public interface ICongestionTaxCalculatorService
    {
        int GetTax(VehiclesType vehicleType, IList<string> dates);
    }
}
