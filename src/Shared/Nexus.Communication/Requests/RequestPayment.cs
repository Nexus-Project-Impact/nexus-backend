namespace Nexus.Communication.Requests
{
    public class RequestPayment
    {
        public double AmountPaid { get; set; }
        public string Receipt { get; set; } = string.Empty;
        public int TravelPackageId { get; set; }
        public ICollection<RequestTravelers> Traveler { get; set; } = new List<RequestTravelers>();
    }
}
    