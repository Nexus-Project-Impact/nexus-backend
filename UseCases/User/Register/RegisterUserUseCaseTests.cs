using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Nexus.Application.UseCases.User.Register;
using Nexus.Communication.Requests;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
namespace UseCases.User.Register;
public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Execute_Should_Return_()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
            Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
        );
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var request = new RequestRegisterUserJson
        {
            Name = "Teste",
            Email = "teste@email.com",
            Password = "Senha123!",
            Phone = "11999999999",
            CPF = "12345678900"
        };

        var user = new Nexus.Domain.Entities.User { Email = request.Email, Name = request.Name };

        mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.User>(It.IsAny<RequestRegisterUserJson>())).Returns(user);
        userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((Nexus.Domain.Entities.User)null);
        userManagerMock.Setup(m => m.CreateAsync(It.IsAny<Nexus.Domain.Entities.User>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<Nexus.Domain.Entities.User>(), "User"))
            .ReturnsAsync(IdentityResult.Success);
        unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

        var useCase = new RegisterUserUseCase(userManagerMock.Object, mapperMock.Object, unitOfWorkMock.Object);


        // Act
        var result = await useCase.Execute(request);

        // Assert com Shouldly
        result.ShouldNotBeNull();
        result.Message.ShouldNotBeNullOrEmpty();
        result.Message.ShouldBe("Usuário cadastrado com sucesso!");
    }
}