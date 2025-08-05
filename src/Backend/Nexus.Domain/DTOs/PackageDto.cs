using System;

namespace Nexus.Domain.DTOs
{
    public class PackageDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public int Duration { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double Value { get; set; }
        public string ImageUrl { get; set; }
    }
}
