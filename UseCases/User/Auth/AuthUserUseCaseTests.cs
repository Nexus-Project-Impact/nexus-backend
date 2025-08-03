using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Nexus.Application.Services.Auth;
using Nexus.Application.UseCases.User.Auth;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.User.Auth
{
    public class AuthUserUseCaseTests
    {
        [Fact]
        public async Task Execute_Login_Should_Return_Success_When_Valid_Credentials()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            var request = new RequestLoginUserJson
            {
                Email = "test@email.com",
                Password = "Password123!"
            };

            var user = new Nexus.Domain.Entities.User
            {
                Id = "user123",
                Email = "test@email.com",
                Name = "Test User"
            };

            var roles = new List<string> { "User" };
            var expectedToken = "jwt_token_example";

            userManagerMock.Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(user, request.Password, false))
                .ReturnsAsync(SignInResult.Success);

            userManagerMock.Setup(m => m.GetRolesAsync(user))
                .ReturnsAsync(roles);

            jwtServiceMock.Setup(j => j.GenerateToken(user.Id, user.Email, user.Name, roles))
                .Returns(expectedToken);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Token.ShouldBe(expectedToken);
            result.Message.ShouldBe("Login bem sucedido!");
        }

        [Fact]
        public async Task Execute_Login_Should_Return_Error_When_User_Not_Found()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            var request = new RequestLoginUserJson
            {
                Email = "nonexistent@email.com",
                Password = "Password123!"
            };

            userManagerMock.Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync((Nexus.Domain.Entities.User?)null);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Token.ShouldBe(string.Empty);
            result.Message.ShouldBe("User not found.");
        }

        [Fact]
        public async Task Execute_Login_Should_Return_Error_When_Invalid_Password()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            var request = new RequestLoginUserJson
            {
                Email = "test@email.com",
                Password = "WrongPassword!"
            };

            var user = new Nexus.Domain.Entities.User
            {
                Id = "user123",
                Email = "test@email.com",
                Name = "Test User"
            };

            userManagerMock.Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(user, request.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Token.ShouldBe(string.Empty);
            result.Message.ShouldBe("Invalid password.");
        }

        [Fact]
        public async Task Execute_Login_Should_Return_Error_When_User_Data_Incomplete()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            var request = new RequestLoginUserJson
            {
                Email = "test@email.com",
                Password = "Password123!"
            };

            var user = new Nexus.Domain.Entities.User
            {
                Id = null, // Dados incompletos
                Email = "test@email.com",
                Name = "Test User"
            };

            userManagerMock.Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Token.ShouldBe(string.Empty);
            result.Message.ShouldBe("Dados do usuário estão incompletos.");
        }

        [Fact]
        public async Task Execute_ForgotPassword_Should_Return_Success_When_User_Exists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            var request = new RequestForgotPassword
            {
                Email = "test@email.com"
            };

            var user = new Nexus.Domain.Entities.User
            {
                Id = "user123",
                Email = "test@email.com",
                Name = "Test User"
            };

            userManagerMock.Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Usuário encontrado");
        }

        [Fact]
        public async Task Execute_ForgotPassword_Should_Return_Error_When_User_Not_Found()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            var request = new RequestForgotPassword
            {
                Email = "nonexistent@email.com"
            };

            userManagerMock.Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync((Nexus.Domain.Entities.User?)null);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Usuário não encontrado");
        }

        [Fact]
        public async Task Logout_Should_Call_SignOut()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Nexus.Domain.Entities.User>>(
                Mock.Of<IUserStore<Nexus.Domain.Entities.User>>(), null, null, null, null, null, null, null, null
            );
            var configMock = new Mock<IConfiguration>();
            var jwtServiceMock = new Mock<IJwtService>();
            var mapperMock = new Mock<IMapper>();
            var signInManagerMock = new Mock<SignInManager<Nexus.Domain.Entities.User>>(
                userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<Nexus.Domain.Entities.User>>(), null, null, null, null
            );

            signInManagerMock.Setup(m => m.SignOutAsync())
                .Returns(Task.CompletedTask);

            var useCase = new AuthUserUseCase(userManagerMock.Object, configMock.Object, 
                jwtServiceMock.Object, mapperMock.Object, signInManagerMock.Object);

            // Act
            await useCase.Logout();

            // Assert
            signInManagerMock.Verify(m => m.SignOutAsync(), Times.Once);
        }
    }
}