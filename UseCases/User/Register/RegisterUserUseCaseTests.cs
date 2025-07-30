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

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Execute_Should_Return_()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null
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

        var user = new User { Email = request.Email, Name = request.Name };

        mapperMock.Setup(m => m.Map<User>(It.IsAny<RequestRegisterUserJson>())).Returns(user);
        userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);
        userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "User"))
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