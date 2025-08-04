using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Packages.GetById;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.GetById
{
    public class GetByIdPackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetById_Should_Return_Package_When_Package_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var packageId = 1;
            var package = new TravelPackage
            {
                Id = packageId,
                Title = "Pacote Paris",
                Description = "Viagem para Paris",
                Destination = "Paris",
                Duration = 7,
                DepartureDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(37),
                Value = 2500.00,
                ImageUrl = "https://example.com/paris.jpg"
            };

            var expectedResponse = new ResponsePackage
            {
                Id = packageId,
                Title = "Pacote Paris",
                Description = "Viagem para Paris",
                Destination = "Paris",
                Duration = 7,
                DepartureDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(37),
                Value = 2500.00,
                ImageUrl = "https://example.com/paris.jpg"
            };

            repositoryMock.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync(package);
            mapperMock.Setup(m => m.Map<ResponsePackage>(package)).Returns(expectedResponse);

            var useCase = new GetByIdPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(packageId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(packageId);
            result.Title.ShouldBe("Pacote Paris");
            result.Destination.ShouldBe("Paris");
            result.Value.ShouldBe(2500.00);
            repositoryMock.Verify(r => r.GetByIdAsync(packageId), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetById_Should_Return_Null_When_Package_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var packageId = 999;

            repositoryMock.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync((TravelPackage?)null);
            mapperMock.Setup(m => m.Map<ResponsePackage>(It.IsAny<TravelPackage>())).Returns((ResponsePackage?)null);

            var useCase = new GetByIdPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetById(packageId);

            // Assert
            result.ShouldBeNull();
            repositoryMock.Verify(r => r.GetByIdAsync(packageId), Times.Once);
        }
    }
}