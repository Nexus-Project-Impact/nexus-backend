using Microsoft.AspNetCore.Http.HttpResults;
using Nexus.Infrastructure.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Infrastructure.Services
{
    public class Stripe : IStripeService
    {
        private readonly IStripeSettings _settings;

        public Stripe(IStripeSettings settings)
        {
            _settings = settings;
            StripeConfiguration.ApiKey = _settings.SecretKey;
        }

        public Task<string> CreateCheckoutSessionAsync(
            string ProductName,
            string ProductDescription,
            long Amount,
            string Currency,
            string successUrl,
            string cancelUrl)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = ProductName,
                                Description = ProductDescription,
                            },
                            UnitAmount = Amount,
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };
            var service = new SessionService();
            var session = service.Create(options);
            var url = session.Url;
            return Task.FromResult(session.Id);
        }

        public async Task<string> CreatePaymentAsync(double amount, string currency, string description)
        {
            // exemplo de uso da api stripe para criar um PaymentIntent
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe espera o valor em centavos
                Currency = currency,
                Description = description,
            };
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return paymentIntent.ClientSecret;
        }
    }
}
