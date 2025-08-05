namespace Nexus.Communication.Responses
{
    public class ResponseReservationAdminJson
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ReservationNumber { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? UserDocument { get; set; }
        public string? TravelPackageName { get; set; }
        public string? TravelPackageDestination { get; set; }

        public string? StatusPayment { get; set; }
        public double TotalValue { get; set; }
        public ICollection<ResponseTravelers> Traveler { get; set; } = new List<ResponseTravelers>();
    }
}
