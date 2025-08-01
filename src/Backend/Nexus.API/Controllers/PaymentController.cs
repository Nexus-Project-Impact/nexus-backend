using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Application.Services.Email;
using Nexus.Application.Services.Payments;
using Nexus.Application.UseCases.Payments.Create;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Services;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Nexus.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class Payment : ControllerBase
    {
        // enviar os dados do cartão pro stripe e  usar nosso próprio formulário
        [HttpPost("pay")]
        public async Task<IActionResult> CreateCheckoutSession(
            [FromServices] IStripeService stripeService,
            [FromBody] RequestPayment model)
        {
            var clientSecret = await stripeService.CreatePaymentAsync(
                model.AmountPaid, // valor pago
                "BRL", // ou outro valor fixo ou obtido de outro local
                $"Pagamento via {model.PaymentMethod}"
            );

            return Ok(new { clientSecret });
        }

        //[HttpPost("pay-bank-slip")]
        //public async Task<IActionResult> PayBankSlip([FromServices] ICreatePaymentUseCase useCase, [FromBody] RequestPayment request)
        //{
        //    var result = await useCase.Execute(request);

        //    return Ok(result);
        //}

        [HttpPost("pay-boleto")]
        [ProducesResponseType(typeof(ResponseBoleto), StatusCodes.Status200OK)]
        public async Task<ActionResult> PayBankSlip(
            [FromServices] UserManager<User> userManager,
            [FromServices] IPaymentService useCase, 
            [FromBody] RequestPayment request)
        { 
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var result = await useCase.PayWithBoleto(userId, request);

            var response = new ResponseBoleto
            {
                BankSlipUrl = result.BankSlipUrl,
                ClientSecret = result.ClientSecret
            };

            return Ok(response);
        }
        //sistema de checkout para usar interface do stripe
        //[HttpPost("create-checkout-session")]
        //public IActionResult CreateCheckoutSession([FromServices] IStripeService stripeService, [FromBody] RequestCheckoutFormModel model)
        //{ 
        //    var session = stripeService.CreateCheckoutSessionAsync(
        //        model.ProductName,
        //        model.ProductDescription,
        //        model.Amount,
        //        model.Currency,
        //        $"{Request.Scheme}://{Request.Host}/checkout/success",
        //        $"{Request.Scheme}://{Request.Host}/checkout/cancel").GetAwaiter().GetResult();

        //    return Ok(new { sessionId = session});
        //}
    }
}
