using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Packages.GetByDestination;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.GetByDestination
{
    public class GetByDestinationPackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetByDestination_Should_Return_Packages_For_Valid_Destination()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var destination = "Paris";

            var packages = new List<TravelPackage>
            {
                new TravelPackage
                {
                    Id = 1,
                    Title = "Pacote Paris Econômico",
                    Description = "Viagem econômica para Paris",
                    Destination = "Paris",
                    Duration = 5,
                    DepartureDate = DateTime.Now.AddDays(30),
                    ReturnDate = DateTime.Now.AddDays(35),
                    Value = 2000.00
                },
                new TravelPackage
                {
                    Id = 2,
                    Title = "Pacote Paris Luxo",
                    Description = "Viagem de luxo para Paris",
                    Destination = "Paris",
                    Duration = 10,
                    DepartureDate = DateTime.Now.AddDays(45),
                    ReturnDate = DateTime.Now.AddDays(55),
                    Value = 5000.00
                }
            };

            var expectedResponse = new List<ResponsePackage>
            {
                new ResponsePackage
                {
                    Id = 1,
                    Title = "Pacote Paris Econômico",
                    Description = "Viagem econômica para Paris",
                    Destination = "Paris",
                    Duration = 5,
                    DepartureDate = DateTime.Now.AddDays(30),
                    ReturnDate = DateTime.Now.AddDays(35),
                    Value = 2000.00
                },
                new ResponsePackage
                {
                    Id = 2,
                    Title = "Pacote Paris Luxo",
                    Description = "Viagem de luxo para Paris",
                    Destination = "Paris",
                    Duration = 10,
                    DepartureDate = DateTime.Now.AddDays(45),
                    ReturnDate = DateTime.Now.AddDays(55),
                    Value = 5000.00
                }
            };

            repositoryMock.Setup(r => r.GetByDestinationAsync(destination)).ReturnsAsync(packages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage?>>(packages)).Returns(expectedResponse);

            var useCase = new GetByDestinationPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByDestination(destination);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            result.All(p => p?.Destination == "Paris").ShouldBeTrue();
            repositoryMock.Verify(r => r.GetByDestinationAsync(destination), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetByDestination_Should_Return_Empty_List_When_Destination_Is_Empty()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var emptyDestination = "";

            var useCase = new GetByDestinationPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByDestination(emptyDestination);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetByDestinationAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteGetByDestination_Should_Return_Empty_List_When_Destination_Is_WhiteSpace()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var whiteSpaceDestination = "   ";

            var useCase = new GetByDestinationPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByDestination(whiteSpaceDestination);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetByDestinationAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteGetByDestination_Should_Return_Empty_List_When_No_Packages_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var destination = "DestinationWithoutPackages";
            var emptyPackages = new List<TravelPackage>();
            var emptyResponse = new List<ResponsePackage>();

            repositoryMock.Setup(r => r.GetByDestinationAsync(destination)).ReturnsAsync(emptyPackages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage?>>(emptyPackages)).Returns(emptyResponse);

            var useCase = new GetByDestinationPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByDestination(destination);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetByDestinationAsync(destination), Times.Once);
        }
    }
}