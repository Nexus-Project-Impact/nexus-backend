using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Packages.GetByValue;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.GetByValue
{
    public class GetByValuePackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetByValue_Should_Return_Packages_Within_Value_Range()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var minValue = 1000.00;
            var maxValue = 3000.00;

            var packages = new List<TravelPackage>
            {
                new TravelPackage
                {
                    Id = 1,
                    Title = "Pacote Econômico",
                    Description = "Viagem econômica",
                    Destination = "Madrid",
                    Duration = 5,
                    DepartureDate = DateTime.Now.AddDays(30),
                    ReturnDate = DateTime.Now.AddDays(35),
                    Value = 1500.00
                },
                new TravelPackage
                {
                    Id = 2,
                    Title = "Pacote Intermediário",
                    Description = "Viagem com custo moderado",
                    Destination = "Barcelona",
                    Duration = 7,
                    DepartureDate = DateTime.Now.AddDays(45),
                    ReturnDate = DateTime.Now.AddDays(52),
                    Value = 2500.00
                }
            };

            var expectedResponse = new List<ResponsePackage>
            {
                new ResponsePackage
                {
                    Id = 1,
                    Title = "Pacote Econômico",
                    Description = "Viagem econômica",
                    Destination = "Madrid",
                    Duration = 5,
                    DepartureDate = DateTime.Now.AddDays(30),
                    ReturnDate = DateTime.Now.AddDays(35),
                    Value = 1500.00
                },
                new ResponsePackage
                {
                    Id = 2,
                    Title = "Pacote Intermediário",
                    Description = "Viagem com custo moderado",
                    Destination = "Barcelona",
                    Duration = 7,
                    DepartureDate = DateTime.Now.AddDays(45),
                    ReturnDate = DateTime.Now.AddDays(52),
                    Value = 2500.00
                }
            };

            repositoryMock.Setup(r => r.GetByValueAsync(minValue, maxValue)).ReturnsAsync(packages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage?>>(packages)).Returns(expectedResponse);

            var useCase = new GetByValuePackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByValue(minValue, maxValue);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            result.All(p => p?.Value >= minValue && p?.Value <= maxValue).ShouldBeTrue();
            repositoryMock.Verify(r => r.GetByValueAsync(minValue, maxValue), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetByValue_Should_Return_Empty_List_When_No_Packages_In_Range()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var minValue = 10000.00;
            var maxValue = 15000.00;

            var emptyPackages = new List<TravelPackage>();
            var emptyResponse = new List<ResponsePackage>();

            repositoryMock.Setup(r => r.GetByValueAsync(minValue, maxValue)).ReturnsAsync(emptyPackages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage?>>(emptyPackages)).Returns(emptyResponse);

            var useCase = new GetByValuePackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByValue(minValue, maxValue);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetByValueAsync(minValue, maxValue), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetByValue_Should_Handle_Zero_Values()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();

            var minValue = 0.0;
            var maxValue = 500.00;

            var packages = new List<TravelPackage>
            {
                new TravelPackage
                {
                    Id = 1,
                    Title = "Pacote Promocional",
                    Description = "Viagem promocional",
                    Destination = "Nacional",
                    Duration = 2,
                    DepartureDate = DateTime.Now.AddDays(15),
                    ReturnDate = DateTime.Now.AddDays(17),
                    Value = 200.00
                }
            };

            var expectedResponse = new List<ResponsePackage>
            {
                new ResponsePackage
                {
                    Id = 1,
                    Title = "Pacote Promocional",
                    Description = "Viagem promocional",
                    Destination = "Nacional",
                    Duration = 2,
                    DepartureDate = DateTime.Now.AddDays(15),
                    ReturnDate = DateTime.Now.AddDays(17),
                    Value = 200.00
                }
            };

            repositoryMock.Setup(r => r.GetByValueAsync(minValue, maxValue)).ReturnsAsync(packages);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponsePackage?>>(packages)).Returns(expectedResponse);

            var useCase = new GetByValuePackageUseCase(repositoryMock.Object, mapperMock.Object);

            // Act
            var result = await useCase.ExecuteGetByValue(minValue, maxValue);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(1);
            result.First()?.Value.ShouldBe(200.00);
            repositoryMock.Verify(r => r.GetByValueAsync(minValue, maxValue), Times.Once);
        }
    }
}