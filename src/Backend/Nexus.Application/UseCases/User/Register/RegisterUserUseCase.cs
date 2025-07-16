using AutoMapper;
using Nexus.Application.Services.Cryptography;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.User;
using Nexus.Exceptions;
using Nexus.Exceptions.ExceptionsBase;

namespace Nexus.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {

        private readonly IUserWriteOnlyRepository _writeOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserUseCase(
            IUserWriteOnlyRepository writeOnlyRepository, 
            IUserReadOnlyRepository readOnlyRepository, 
            IMapper mapper, 
            PasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            //Validar a request
            await Validate(request);

            //Mapear a request em uma entidade

            var user = _mapper.Map<Domain.Entities.User>(request);

            //Criptografar a senha
            
            user.Password = _passwordEncripter.Encrypt(request.Password);


            //Salvar no banco de dados
            await _writeOnlyRepository.Add(user);

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

            var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (emailExist)
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
