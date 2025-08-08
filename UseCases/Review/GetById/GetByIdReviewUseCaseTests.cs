using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Review.GetId;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Review.GetById
{
    public class GetByIdReviewUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetById_Should_Return_ResponseReviewJson_When_Review_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = 1;
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 5,
                Comment = "Excelente pacote de viagem!",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var expectedResponse = new ResponseReviewJson
            {
                Id = reviewId.ToString(),
                Rating = 5,
                Comment = "Excelente pacote de viagem!",
                CreatedAt = existingReview.CreatedAt
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(existingReview))
                .Returns(expectedResponse);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe("1");
            result.Rating.ShouldBe(5);
            result.Comment.ShouldBe("Excelente pacote de viagem!");
            result.CreatedAt.ShouldBe(existingReview.CreatedAt);

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            mapperMock.Verify(m => m.Map<ResponseReviewJson>(existingReview), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Return_Null_When_Review_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = 999;

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns((ResponseReviewJson?)null);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldBeNull();

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            mapperMock.Verify(m => m.Map<ResponseReviewJson>(null), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Return_Review_With_Null_Comment()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = 1;
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 4,
                Comment = null,
                CreatedAt = DateTime.UtcNow.AddHours(-5)
            };

            var expectedResponse = new ResponseReviewJson
            {
                Id = reviewId.ToString(),
                Rating = 4,
                Comment = null,
                CreatedAt = existingReview.CreatedAt
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(existingReview))
                .Returns(expectedResponse);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe("1");
            result.Rating.ShouldBe(4);
            result.Comment.ShouldBeNull();
            result.CreatedAt.ShouldBe(existingReview.CreatedAt);

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            mapperMock.Verify(m => m.Map<ResponseReviewJson>(existingReview), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Handle_Different_Rating_Values()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = 1;
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 1,
                Comment = "Muito ruim",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            };

            var expectedResponse = new ResponseReviewJson
            {
                Id = reviewId.ToString(),
                Rating = 1,
                Comment = "Muito ruim",
                CreatedAt = existingReview.CreatedAt
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(existingReview))
                .Returns(expectedResponse);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe("1");
            result.Rating.ShouldBe(1);
            result.Comment.ShouldBe("Muito ruim");
            result.CreatedAt.ShouldBe(existingReview.CreatedAt);

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            mapperMock.Verify(m => m.Map<ResponseReviewJson>(existingReview), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Handle_Zero_Id()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = 0;

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns((ResponseReviewJson?)null);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldBeNull();

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Handle_Negative_Id()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = -1;

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync((Nexus.Domain.Entities.Review?)null);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(It.IsAny<Nexus.Domain.Entities.Review>()))
                .Returns((ResponseReviewJson?)null);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldBeNull();

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Handle_Long_Comment()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviewId = 1;
            var longComment = new string('A', 1000);
            var existingReview = new Nexus.Domain.Entities.Review
            {
                Id = reviewId,
                UserId = "user123",
                PackageId = 456, // Mudou para int
                Rating = 3,
                Comment = longComment,
                CreatedAt = DateTime.UtcNow.AddMinutes(-30)
            };

            var expectedResponse = new ResponseReviewJson
            {
                Id = reviewId.ToString(),
                Rating = 3,
                Comment = longComment,
                CreatedAt = existingReview.CreatedAt
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reviewId))
                .ReturnsAsync(existingReview);

            mapperMock.Setup(m => m.Map<ResponseReviewJson>(existingReview))
                .Returns(expectedResponse);

            var useCase = new GetByIdReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(reviewId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe("1");
            result.Rating.ShouldBe(3);
            result.Comment.ShouldBe(longComment);
            result.CreatedAt.ShouldBe(existingReview.CreatedAt);

            repositoryMock.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            mapperMock.Verify(m => m.Map<ResponseReviewJson>(existingReview), Times.Once);
        }
    }
}