using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class GetCongestionTaxRequest
{
    [Required(AllowEmptyStrings = false)]
    public string VehicleType { get; set; }

    public IList<string>? TravelDates { get; set; }
}