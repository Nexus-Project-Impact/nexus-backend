using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Packages.Update;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.Update
{
    public class UpdatePackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteUpdate_Should_Return_ResponsePackage_When_Package_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var packageId = 1;
            var request = new RequestUpdatePackage
            {
                Id = packageId,
                Title = "Pacote Paris Atualizado",
                Description = "Viagem para Paris atualizada",
                Destination = "Paris",
                Duration = 10,
                DepartureDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(40),
                Value = 3000.00,
                ImageUrl = "https://example.com/paris-updated.jpg"
            };

            var existingPackage = new TravelPackage
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
                Title = request.Title,
                Description = request.Description,
                Destination = request.Destination,
                Duration = request.Duration,
                DepartureDate = request.DepartureDate,
                ReturnDate = request.ReturnDate,
                Value = request.Value,
                ImageUrl = request.ImageUrl
            };

            repositoryMock.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync(existingPackage);
            
            // Setup AutoMapper to modify the existing package in place (as AutoMapper.Map does)
            mapperMock.Setup(m => m.Map(request, existingPackage))
                .Callback<RequestUpdatePackage, TravelPackage>((src, dest) =>
                {
                    dest.Title = src.Title;
                    dest.Description = src.Description;
                    dest.Destination = src.Destination;
                    dest.Duration = src.Duration;
                    dest.DepartureDate = src.DepartureDate;
                    dest.ReturnDate = src.ReturnDate;
                    dest.Value = src.Value;
                    dest.ImageUrl = src.ImageUrl;
                })
                .Returns(existingPackage);
            
            mapperMock.Setup(m => m.Map<ResponsePackage>(It.IsAny<TravelPackage>()))
                .Returns(expectedResponse);
            
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TravelPackage>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new UpdatePackageUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteUpdate(packageId, request);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(packageId);
            result.Title.ShouldBe(request.Title);
            repositoryMock.Verify(r => r.GetByIdAsync(packageId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TravelPackage>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteUpdate_Should_Return_Null_When_Package_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var packageId = 999;
            var request = new RequestUpdatePackage
            {
                Id = packageId,
                Title = "Pacote Inexistente",
                Description = "Viagem inexistente",
                Destination = "Lugar Inexistente",
                Duration = 5,
                DepartureDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(35),
                Value = 1000.00
            };

            repositoryMock.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync((TravelPackage?)null);

            var useCase = new UpdatePackageUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteUpdate(packageId, request);

            // Assert
            result.ShouldBeNull();
            repositoryMock.Verify(r => r.GetByIdAsync(packageId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TravelPackage>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }
    }
}