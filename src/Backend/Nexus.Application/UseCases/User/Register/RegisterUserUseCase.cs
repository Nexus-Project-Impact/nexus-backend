using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Exceptions;
using Nexus.Exceptions.ExceptionsBase;

namespace Nexus.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserUseCase(
            UserManager<Domain.Entities.User> userManager,
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            //Validar a request
            await Validate(request);

            //Mapear a request em uma entidade

            var user = _mapper.Map<Domain.Entities.User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                throw new UserCreationException();
            }

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailExist = await _userManager.FindByEmailAsync(request.Email);
            if (emailExist != null)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
