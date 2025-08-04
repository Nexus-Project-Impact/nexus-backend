using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Nexus.Application.Services.Email;
using Nexus.Application.UseCases.Payments.Create;
using Nexus.Application.UseCases.Payments.Read;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.DTOs;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Services;
using Stripe.V2;
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
        private readonly IReadPaymentUseCase _readPaymentUseCase;

        public PaymentService(IStripeService stripeService, ICreatePaymentUseCase createPaymentUseCase, IEmailService emailService, UserManager<User> userManager, IReadPaymentUseCase readPaymentUseCase)
        {
            _stripeService = stripeService;
            _createPaymentUseCase = createPaymentUseCase;
            _emailService = emailService;
            _userManager = userManager;
            _readPaymentUseCase = readPaymentUseCase;
        }

        private string BuildConfirmationEmailBody(string userName, double amount, string paymentMethod, string? extraInfo = null, string? extraHtml = null)
        {
            var style = "font-family: Arial, sans-serif; color: #222; background: #f9f9f9; padding: 24px; border-radius: 8px; max-width: 480px; margin: 0 auto;";
            var titleStyle = "color: #2a7ae2; font-size: 22px; margin-bottom: 12px;";
            var infoStyle = "margin: 12px 0; font-size: 16px;";
            var extra = string.IsNullOrEmpty(extraHtml) ? "" : $"<div style='margin-top:18px'>{extraHtml}</div>";
            return $@"
                <div style='{style}'>
                    <div style='{titleStyle}'>Pagamento Recebido - {paymentMethod}</div>
                    <div style='{infoStyle}'>Olá, <b>{userName}</b>!</div>
                    <div style='{infoStyle}'>Recebemos sua solicitação de pagamento no valor de <b>R$ {amount:N2}</b> via <b>{paymentMethod}</b>.</div>
                    {(!string.IsNullOrEmpty(extraInfo) ? $"<div style='{infoStyle}'>{extraInfo}</div>" : "")}
                    {extra}
                    <div style='margin-top:24px; font-size:13px; color:#888;'>Se você não reconhece esta operação, entre em contato imediatamente.</div>
                </div>";
        }

        public async Task<ResponseBoleto> PayWithBoleto(string userId, RequestPayment request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            // realizar instância do pagamento no banco de dados como pendente

            var paymentDto = new PaymentDto
            {
                ReservationId = request.ReservationId,
                AmountPaid = request.AmountPaid,
                Receipt = request.Receipt,
                Status = "Pendente",
                PaymentMethod = "Boleto",
                Date = DateTime.Now
            };

            var payment = await _createPaymentUseCase.Execute(paymentDto);

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

            
            var subject = "Confirmação de Geração de Boleto";
            var extraInfo = $"Seu boleto foi gerado com sucesso. <br/>Acesse o link abaixo para visualizar e realizar o pagamento.";
            var extraHtml = $"<a href='{dataStripe.boletoUrl}' style='display:inline-block;padding:10px 18px;background:#2a7ae2;color:#fff;text-decoration:none;border-radius:5px;'>Visualizar Boleto</a>";
            var htmlBody = BuildConfirmationEmailBody(user.Name, request.AmountPaid, "Boleto", extraInfo, extraHtml);
            await _emailService.SendEmailAsync(user.Email, subject, $"Seu boleto foi gerado com sucesso: {dataStripe.boletoUrl}", htmlBody);

            return new ResponseBoleto
            {
                BankSlipUrl = dataStripe.boletoUrl,
                ClientSecret = dataStripe.clientSecret
            };
        }           

        public async Task<ResponsePayment> PayWithCard(string userId, RequestPayment request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            var paymentDto = new PaymentDto
            {
                ReservationId = request.ReservationId,
                AmountPaid = request.AmountPaid,
                Receipt = request.Receipt,
                Status = "Pendente",
                PaymentMethod = "Cartão",
                Date = DateTime.Now
            };

            var payment = await _createPaymentUseCase.Execute(paymentDto);


            if (payment == null)
                throw new Exception("Erro ao criar pagamento no banco de dados.");


            var clientSecret = await _stripeService.CreatePaymentAsync(
              request.AmountPaid, // valor pago
              "BRL", // ou outro valor fixo ou obtido de outro local
              $"Pagamento via Cartão"
          );

         
            var subject = "Confirmação de Pagamento com Cartão";
            var extraInfo = $"Seu pagamento com cartão foi iniciado. Assim que confirmado, você receberá a confirmação.";
            var htmlBody = BuildConfirmationEmailBody(user.Name, request.AmountPaid, "Cartão de Crédito", extraInfo);
            await _emailService.SendEmailAsync(user.Email, subject, "Seu pagamento com cartão foi iniciado.", htmlBody);

            return new ResponsePayment
            {
                PaymentIntent = clientSecret
            };
        }

        public async Task<ResponsePix> PayWithPix(string userId, RequestPayment request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            var paymentDto = new PaymentDto
            {
                ReservationId = request.ReservationId,
                AmountPaid = request.AmountPaid,
                Receipt = request.Receipt,
                Status = "Pendente",
                PaymentMethod = "Pix",
                Date = DateTime.Now
            };

            var payment = await _createPaymentUseCase.Execute(paymentDto);
            if (payment == null)
                throw new Exception("Erro ao criar pagamento no banco de dados.");

            var payloadPix = "00020126360014BR.GOV.BCB.PIX0114+55819999999990214Pagamento Pix5204000053039865802BR5920Empresa Fictícia6009RECIFE62100506PIX01";

            var response = new ResponsePix
            {
                TransactionId = Guid.NewGuid().ToString(),
                Amount = request.AmountPaid,
                PayerName = user.Name,
                QrCode = payloadPix,
                QrCodeImageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=300x300&data={Uri.EscapeDataString(payloadPix)}"
            };

           
            var subject = "Confirmação de Pagamento com Pix";
            var extraInfo = $"Seu pagamento via Pix foi iniciado. Utilize o QR Code abaixo para concluir o pagamento.";
            var extraHtml = $"<img src='{response.QrCodeImageUrl}' alt='QR Code Pix' style='margin-top:10px;max-width:220px;'><div style='margin-top:8px;font-size:13px;color:#555;word-break:break-all;'>{payloadPix}</div>";
            var htmlBody = BuildConfirmationEmailBody(user.Name, request.AmountPaid, "Pix", extraInfo, extraHtml);
            await _emailService.SendEmailAsync(user.Email, subject, "Seu pagamento via Pix foi iniciado.", htmlBody);

            return response;
        }

        public async Task<IEnumerable<PaymentDto>> GetPayments(string userId)
        {
            return await _readPaymentUseCase.GetAllByUserId(userId);
        }
    }
}
