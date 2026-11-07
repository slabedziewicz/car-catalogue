using System;

namespace Car_Catalogue_Desktop.Models
{
    public class Car {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? BodyType { get; set; } = string.Empty;
        public int? TopSpeed { get; set; } // km/h
        public int? HorsePower { get; set; } // KM
        public int? Torque { get; set; } // Nm
        public double? EngineCapacity { get; set; } // L
        public double? ZeroToHundred { get; set; } // seconds
        public int? Weight { get; set; } // kg
    }
}
