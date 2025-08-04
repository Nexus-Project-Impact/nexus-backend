using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Packages.Create;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.Create
{
    public class CreatePackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteCreate_Should_Return_ResponseCreatedPackage()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestCreatePackage
            {
                Title = "Pacote Paris",
                Description = "Viagem para Paris",
                Destination = "Paris",
                Duration = 7,
                DepartureDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(37),
                Value = 2500.00,
                ImageUrl = "https://example.com/paris.jpg"
            };

            var travelPackage = new TravelPackage
            {
                Id = 1,
                Title = request.Title,
                Description = request.Description,
                Destination = request.Destination,
                Duration = request.Duration,
                DepartureDate = request.DepartureDate,
                ReturnDate = request.ReturnDate,
                Value = request.Value,
                ImageUrl = request.ImageUrl
            };

            mapperMock.Setup(m => m.Map<TravelPackage>(It.IsAny<RequestCreatePackage>())).Returns(travelPackage);
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<TravelPackage>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new CreatePackageUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteCreate(request);

            // Assert
            result.ShouldNotBeNull();
            result.Title.ShouldBe(request.Title);
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<TravelPackage>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}