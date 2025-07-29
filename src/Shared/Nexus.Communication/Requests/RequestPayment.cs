namespace Nexus.Communication.Requests
{
    public class RequestPayment
    {
        public double Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
