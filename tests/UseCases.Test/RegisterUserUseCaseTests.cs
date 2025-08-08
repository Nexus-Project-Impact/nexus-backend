using Xunit;
using Moq;
using Nexus.Application.UseCases.User.Register;
using Nexus.Communication.Requests;
using Nexus.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using System.Threading.Tasks;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Execute_DeveRetornarMensagemDeSucesso_QuandoUsuarioValido()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(/* parâmetros necessários */);
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        // Configuração dos mocks omitida para foco no Shouldly...

        var useCase = new RegisterUserUseCase(userManagerMock.Object, mapperMock.Object, unitOfWorkMock.Object);
        var request = new RequestRegisterUserJson { /* propriedades */ };

        // Act
        var result = await useCase.Execute(request);

        // Assert com Shouldly
        result.Message.ShouldBe("Usuário cadastrado com sucesso!");
    }
}