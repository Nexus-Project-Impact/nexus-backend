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
            RuleFor(user => user.Phone).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY)
                .Matches(@"^(\d{10}|\d{11})$").WithMessage(ResourceMessagesException.PHONE_INVALID);
            RuleFor(user => user.CPF).NotEmpty().WithMessage(ResourceMessagesException.DOCUMENT_EMPTY);

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage(ResourceMessagesException.PASSWORD_INVALID)
                .MinimumLength(6).WithMessage(ResourceMessagesException.PASSWORD_TOO_SHORT)
                .Matches(@"[A-Z]").WithMessage(ResourceMessagesException.PASSWORD_MISSING_UPPERCASE)
                .Matches(@"\d").WithMessage(ResourceMessagesException.PASSWORD_MISSING_DIGIT)
                .Matches(@"[\W_]").WithMessage(ResourceMessagesException.PASSWORD_MISSING_SPECIAL_CHAR);

            ;
            When(user => string.IsNullOrEmpty(user.Email) == false, () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });

        }
    }
}
