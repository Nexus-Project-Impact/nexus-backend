using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Nexus.Application.Services.Email;
using Nexus.Application.UseCases.Payments.Create;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IStripeService _stripeService;
        private readonly ICreatePaymentUseCase _createPaymentUseCase;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;

        public PaymentService(IStripeService stripeService, ICreatePaymentUseCase createPaymentUseCase, IEmailService emailService, UserManager<User> userManager)
        {
            _stripeService = stripeService;
            _createPaymentUseCase = createPaymentUseCase;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<ResponseBoleto> PayWithBoleto(string userId, RequestPayment request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            Console.WriteLine(userId);
            Console.WriteLine(user.Name, user.Email, user.PhoneNumber, user.CPF);
            Console.WriteLine("email");
            Console.WriteLine(user.Email);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            // realizar instância do pagamento no banco de dados como pendente

            request.Status = "Pendente";
            request.PaymentMethod = "Boleto";
            request.Date = DateTime.Now;

            var payment = await _createPaymentUseCase.Execute(request);

            if (payment == null)
            {
                throw new Exception("Erro ao criar pagamento no banco de dados.");
            }

            // gerar boleto via stripe
            var dataStripe = await _stripeService.CreatePaymentAsync(request.AmountPaid, "brl", "pagamento de uma reserva", user.Name, user.Email, user.CPF);

            if (dataStripe.clientSecret == null)
            {
                throw new Exception("Erro ao gerar boleto no Stripe.");
            }

            // enviar email para o usuário
            await _emailService.SendEmailAsync(
                user.Email,
                "Boleto gerado com sucesso",
                $"Seu boleto foi gerado com sucesso. Acesse o link para visualizar: {dataStripe.boletoUrl}",
                $"<p>Seu boleto foi gerado com sucesso. Acesse o link para visualizar: <a href='{dataStripe.boletoUrl}'>{dataStripe.boletoUrl}</a></p>");

            return new ResponseBoleto
            {
                BankSlipUrl = dataStripe.boletoUrl,
                ClientSecret = dataStripe.clientSecret
            };
        }
    }
}
