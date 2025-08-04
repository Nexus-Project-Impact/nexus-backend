using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Review.GetAll;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Review.GetAll
{
    public class GetAllReviewUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetAll_Should_Return_All_Reviews_When_Reviews_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviews = new List<Nexus.Domain.Entities.Review>
            {
                new Nexus.Domain.Entities.Review
                {
                    Id = 1,
                    UserId = "user1",
                    PackageId = 1, // Mudou para int
                    Rating = 5,
                    Comment = "Excelente!",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Nexus.Domain.Entities.Review
                {
                    Id = 2,
                    UserId = "user2",
                    PackageId = 2, // Mudou para int
                    Rating = 4,
                    Comment = "Muito bom",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Nexus.Domain.Entities.Review
                {
                    Id = 3,
                    UserId = "user3",
                    PackageId = 1, // Mudou para int
                    Rating = 3,
                    Comment = "Ok",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            var expectedResponses = reviews.Select(r => new ResponseReviewJson
            {
                Id = r.Id.ToString(),
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reviews);

            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews))
                .Returns(expectedResponses);

            var useCase = new GetAllReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(3);
            
            var resultList = result.ToList();
            resultList[0].Id.ShouldBe("1");
            resultList[0].Rating.ShouldBe(5);
            resultList[0].Comment.ShouldBe("Excelente!");
            
            resultList[1].Id.ShouldBe("2");
            resultList[1].Rating.ShouldBe(4);
            resultList[1].Comment.ShouldBe("Muito bom");
            
            resultList[2].Id.ShouldBe("3");
            resultList[2].Rating.ShouldBe(3);
            resultList[2].Comment.ShouldBe("Ok");

            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetAll_Should_Return_Empty_Collection_When_No_Reviews_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var emptyReviews = new List<Nexus.Domain.Entities.Review>();
            var emptyResponses = new List<ResponseReviewJson>();

            repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyReviews);

            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReviewJson>>(emptyReviews))
                .Returns(emptyResponses);

            var useCase = new GetAllReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ResponseReviewJson>>(emptyReviews), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetAll_Should_Return_Reviews_With_Null_Comments()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviews = new List<Nexus.Domain.Entities.Review>
            {
                new Nexus.Domain.Entities.Review
                {
                    Id = 1,
                    UserId = "user1",
                    PackageId = 1, // Mudou para int
                    Rating = 5,
                    Comment = null,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Nexus.Domain.Entities.Review
                {
                    Id = 2,
                    UserId = "user2",
                    PackageId = 2, // Mudou para int
                    Rating = 4,
                    Comment = "Bom pacote",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            var expectedResponses = reviews.Select(r => new ResponseReviewJson
            {
                Id = r.Id.ToString(),
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reviews);

            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews))
                .Returns(expectedResponses);

            var useCase = new GetAllReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            
            var resultList = result.ToList();
            resultList[0].Comment.ShouldBeNull();
            resultList[1].Comment.ShouldBe("Bom pacote");

            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetAll_Should_Return_Reviews_With_Different_Ratings()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviews = new List<Nexus.Domain.Entities.Review>
            {
                new Nexus.Domain.Entities.Review
                {
                    Id = 1,
                    UserId = "user1",
                    PackageId = 1, // Mudou para int
                    Rating = 1,
                    Comment = "Péssimo",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Nexus.Domain.Entities.Review
                {
                    Id = 2,
                    UserId = "user2",
                    PackageId = 2, // Mudou para int
                    Rating = 5,
                    Comment = "Excelente",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            var expectedResponses = reviews.Select(r => new ResponseReviewJson
            {
                Id = r.Id.ToString(),
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reviews);

            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews))
                .Returns(expectedResponses);

            var useCase = new GetAllReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            
            var resultList = result.ToList();
            resultList[0].Rating.ShouldBe(1);
            resultList[0].Comment.ShouldBe("Péssimo");
            
            resultList[1].Rating.ShouldBe(5);
            resultList[1].Comment.ShouldBe("Excelente");

            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetAll_Should_Handle_Large_Number_Of_Reviews()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Review, int>>();
            var mapperMock = new Mock<IMapper>();

            var reviews = new List<Nexus.Domain.Entities.Review>();
            for (int i = 1; i <= 100; i++)
            {
                reviews.Add(new Nexus.Domain.Entities.Review
                {
                    Id = i,
                    UserId = $"user{i}",
                    PackageId = i % 10 + 1, // Mudou para int
                    Rating = (i % 5) + 1,
                    Comment = $"Comentário {i}",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }

            var expectedResponses = reviews.Select(r => new ResponseReviewJson
            {
                Id = r.Id.ToString(),
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reviews);

            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews))
                .Returns(expectedResponses);

            var useCase = new GetAllReviewUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(100);

            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ResponseReviewJson>>(reviews), Times.Once);
        }
    }
}