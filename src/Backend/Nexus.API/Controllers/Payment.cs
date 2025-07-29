using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Communication.Requests;
using Nexus.Infrastructure.Services;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    //[Authorize]
    [ApiController]
    public class Payment : ControllerBase
    {
        // enviar os dados do cartão pro stripe e  usar nosso próprio formulário
        [HttpPost("pay")]
        public async Task<IActionResult> CreateCheckoutSession(
            [FromServices] IStripeService stripeService,
            [FromBody] RequestPayment model)
        {
            var clientSecret = await stripeService.CreatePaymentAsync(model.Amount, model.Currency, model.Description);

            return Ok(new { clientSecret });
        }


        //sistema de checkout para usar interface do stripe
        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromServices] IStripeService stripeService, [FromBody] RequestCheckoutFormModel model)
        { 
            var session = stripeService.CreateCheckoutSessionAsync(
                model.ProductName,
                model.ProductDescription,
                model.Amount,
                model.Currency,
                $"{Request.Scheme}://{Request.Host}/checkout/success",
                $"{Request.Scheme}://{Request.Host}/checkout/cancel").GetAwaiter().GetResult();

            return Ok(new { sessionId = session});
        }
    }
}
