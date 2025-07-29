using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.Services
{
    public interface IStripeService
    {
        public Task<string> CreatePaymentAsync(double amount, string currency, string description);
        public Task<string> CreateCheckoutSessionAsync(string ProductName, string ProductDescription, long Amount, string Currency, string SucessUrl, string CancelUrl);
    }
}
