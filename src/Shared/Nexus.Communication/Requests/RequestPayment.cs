namespace Nexus.Communication.Requests
{
    public class RequestPayment
    {
        public int ReservationId { get; set; }
        public double AmountPaid { get; set; }
        public string Receipt { get; set; } = string.Empty;
    }
}
    