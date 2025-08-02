using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Review.Moderate;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Review.Moderate
{
    public class ModerateReviewUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Return_ResponseModeratedReviewJson_When_Review_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestModerateReviewJson
            {
                ReviewId = 1,
                ModeratorId = "moderator123",
                Action = "approve",
                Reason = "Conteúdo apropriado"
            };

            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 5,
                Comment = "Excelente pacote!",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(request.ReviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new ModerateReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.ReviewId.ShouldBe(1);
            result.ActionTaken.ShouldBe("approve");
            result.Mensagem.ShouldBe("Avaliação moderada com sucesso.");
            
            repositoryMock.Verify(r => r.GetByIdAsync(request.ReviewId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Return_Null_When_Review_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestModerateReviewJson
            {
                ReviewId = 999,
                ModeratorId = "moderator123",
                Action = "approve",
                Reason = "Conteúdo apropriado"
            };

            repositoryMock.Setup(r => r.GetByIdAsync(request.ReviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            var useCase = new ModerateReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldBeNull();
            
            repositoryMock.Verify(r => r.GetByIdAsync(request.ReviewId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task Execute_Should_Handle_Reject_Action()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestModerateReviewJson
            {
                ReviewId = 1,
                ModeratorId = "moderator123",
                Action = "REJECT",
                Reason = "Conteúdo inapropriado"
            };

            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 1,
                Comment = "Comentário inadequado",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(request.ReviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new ModerateReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.ReviewId.ShouldBe(1);
            result.ActionTaken.ShouldBe("reject");
            result.Mensagem.ShouldBe("Avaliação moderada com sucesso.");
            
            repositoryMock.Verify(r => r.GetByIdAsync(request.ReviewId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Handle_Different_Moderator_Ids()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestModerateReviewJson
            {
                ReviewId = 1,
                ModeratorId = "different_moderator456",
                Action = "approve",
                Reason = "Review válida"
            };

            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 4,
                Comment = "Bom pacote de viagem",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(request.ReviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new ModerateReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.ReviewId.ShouldBe(1);
            result.ActionTaken.ShouldBe("approve");
            result.Mensagem.ShouldBe("Avaliação moderada com sucesso.");
        }

        [Fact]
        public async Task Execute_Should_Handle_Request_Without_Reason()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestModerateReviewJson
            {
                ReviewId = 1,
                ModeratorId = "moderator123",
                Action = "approve",
                Reason = null
            };

            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = 1,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 3,
                Comment = "Pacote ok",
                CreatedAt = DateTime.UtcNow.AddHours(-5)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(request.ReviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new ModerateReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.ReviewId.ShouldBe(1);
            result.ActionTaken.ShouldBe("approve");
            result.Mensagem.ShouldBe("Avaliação moderada com sucesso.");
        }
    }
}