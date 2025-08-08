using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Review.Register;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Review.Register
{
    public class RegisterReviewUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Return_ResponseRegisteredReviewJson_When_Review_Is_Created_Successfully()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestRegisterReviewJson
            {
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 5,
                Comment = "Excelente pacote de viagem!"
            };

            var review = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = request.UserId,
                PackageId = request.PackageId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.Review>(request))
                .Returns(review);

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new RegisterReviewUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.PackageId.ShouldBe(456); // Mudou para int
            result.Rating.ShouldBe(5);
            result.Comment.ShouldBe("Excelente pacote de viagem!");
            result.Mensage.ShouldBe("Avaliação registrada com sucesso!");
            
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            mapperMock.Verify(m => m.Map<Nexus.Domain.Entities.Review>(request), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Create_Review_With_Minimum_Rating()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestRegisterReviewJson
            {
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 1,
                Comment = "Não gostei do pacote."
            };

            var review = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = request.UserId,
                PackageId = request.PackageId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.Review>(request))
                .Returns(review);

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new RegisterReviewUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Rating.ShouldBe(1);
            result.Comment.ShouldBe("Não gostei do pacote.");
            result.Mensage.ShouldBe("Avaliação registrada com sucesso!");
            
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Create_Review_Without_Comment()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestRegisterReviewJson
            {
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 4,
                Comment = null
            };

            var review = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = request.UserId,
                PackageId = request.PackageId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.Review>(request))
                .Returns(review);

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new RegisterReviewUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Rating.ShouldBe(4);
            result.Comment.ShouldBeNull();
            result.Mensage.ShouldBe("Avaliação registrada com sucesso!");
            
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Set_CreatedAt_DateTime()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestRegisterReviewJson
            {
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 3,
                Comment = "Pacote mediano."
            };

            var createdAt = DateTime.UtcNow;
            var review = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = request.UserId,
                PackageId = request.PackageId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = createdAt
            };

            mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.Review>(request))
                .Returns(review);

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new RegisterReviewUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.CreatedAt.ShouldBe(createdAt);
            result.Mensage.ShouldBe("Avaliação registrada com sucesso!");
        }
    }
}