using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Packages.GetAll;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.GetAll
{
    public class GetAllPackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetAll_Should_Return_All_Packages()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var packages = new List<TravelPackage>
            {
                new TravelPackage
                {
                    Id = 1,
                    Title = "Pacote Paris",
                    Description = "Viagem para Paris",
                    Destination = "Paris",
                    Duration = 7,
                    DepartureDate = DateTime.Now.AddDays(30),
                    ReturnDate = DateTime.Now.AddDays(37),
                    Value = 2500.00
                },
                new TravelPackage
                {
                    Id = 2,
                    Title = "Pacote Londres",
                    Description = "Viagem para Londres",
                    Destination = "Londres",
                    Duration = 5,
                    DepartureDate = DateTime.Now.AddDays(45),
                    ReturnDate = DateTime.Now.AddDays(50),
                    Value = 2000.00
                }
            };

            var expectedResponse = new List<ResponsePackage>
            {
                new ResponsePackage
                {
                    Id = 1,
                    Title = "Pacote Paris",
                    Description = "Viagem para Paris",
                    Destination = "Paris",
                    Duration = 7,
                    DepartureDate = DateTime.Now.AddDays(30),
                    ReturnDate = DateTime.Now.AddDays(37),
                    Value = 2500.00
                },
                new ResponsePackage
                {
                    Id = 2,
                    Title = "Pacote Londres",
                    Description = "Viagem para Londres",
                    Destination = "Londres",
                    Duration = 5,
                    DepartureDate = DateTime.Now.AddDays(45),
                    ReturnDate = DateTime.Now.AddDays(50),
                    Value = 2000.00
                }
            };

            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(packages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage>>(packages)).Returns(expectedResponse);

            var useCase = new GetAllPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            result.First().Title.ShouldBe("Pacote Paris");
            result.Last().Title.ShouldBe("Pacote Londres");
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetAll_Should_Return_Empty_List_When_No_Packages_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var emptyPackages = new List<TravelPackage>();
            var emptyResponse = new List<ResponsePackage>();

            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyPackages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage>>(emptyPackages)).Returns(emptyResponse);

            var useCase = new GetAllPackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetAll();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}