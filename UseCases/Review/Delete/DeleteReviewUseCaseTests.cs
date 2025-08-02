using Moq;
using Nexus.Application.UseCases.Review.Delete;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Review.Delete
{
    public class DeleteReviewUseCaseTests
    {
        [Fact]
        public async Task ExecuteDelete_Should_Return_True_When_Review_Exists_And_Is_Deleted_Successfully()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reviewId = 1;
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 5,
                Comment = "Excelente pacote!",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.DeleteAsync(reviewId))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new DeleteReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(reviewId);

            // Assert
            result.ShouldBeTrue();
            
            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(reviewId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteDelete_Should_Return_False_When_Review_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reviewId = 999;

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            var useCase = new DeleteReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(reviewId);

            // Assert
            result.ShouldBeFalse();
            
            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task ExecuteDelete_Should_Handle_Zero_Id()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reviewId = 0;

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            var useCase = new DeleteReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(reviewId);

            // Assert
            result.ShouldBeFalse();
            
            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task ExecuteDelete_Should_Handle_Negative_Id()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reviewId = -1;

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            var useCase = new DeleteReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(reviewId);

            // Assert
            result.ShouldBeFalse();
            
            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task ExecuteDelete_Should_Delete_Review_With_Different_Properties()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reviewId = 10;
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "different_user789",
                PackageId = 123, // Mudou para int
                Rating = 2,
                Comment = "Não recomendo",
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.DeleteAsync(reviewId))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new DeleteReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(reviewId);

            // Assert
            result.ShouldBeTrue();
            
            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(reviewId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteDelete_Should_Delete_Review_Without_Comment()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reviewId = 5;
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "user456",
                PackageId = 789, // Mudou para int
                Rating = 4,
                Comment = null,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            repositoryMock.Setup(r => r.DeleteAsync(reviewId))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

            var useCase = new DeleteReviewUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(reviewId);

            // Assert
            result.ShouldBeTrue();
            
            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(reviewId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}