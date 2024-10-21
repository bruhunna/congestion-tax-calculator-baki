using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models;
public class Vehicle{
    public VehiclesType VehiclesType { get; }
    public Vehicle(VehiclesType vehiclesType)
    {
        VehiclesType = vehiclesType;
    }
}