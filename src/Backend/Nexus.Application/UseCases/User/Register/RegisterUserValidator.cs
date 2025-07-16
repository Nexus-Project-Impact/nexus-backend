using FluentValidation;
using Nexus.Communication.Requests;
using Nexus.Exceptions;

namespace Nexus.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(user => user.Phone).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY);
            RuleFor(user => user.CPF).NotEmpty().WithMessage(ResourceMessagesException.DOCUMENT_EMPTY);
            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessagesException.PASSWORD_INVALID);
            When(user => string.IsNullOrEmpty(user.Email) == false, () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });

        }
    }
}
