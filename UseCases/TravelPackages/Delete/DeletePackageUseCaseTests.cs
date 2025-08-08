using Moq;
using Nexus.Application.UseCases.Packages.Delete;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.TravelPackages.Delete
{
    public class DeletePackageUseCaseTests
    {
        [Fact]
        public async Task ExecuteDelete_Should_Return_True_When_Package_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var packageId = 1;
            var existingPackage = new TravelPackage
            {
                Id = packageId,
                Title = "Pacote Paris",
                Description = "Viagem para Paris",
                Destination = "Paris",
                Duration = 7,
                DepartureDate = DateTime.Now.AddDays(30),
                ReturnDate = DateTime.Now.AddDays(37),
                Value = 2500.00
            };

            repositoryMock.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync(existingPackage);
            repositoryMock.Setup(r => r.DeleteAsync(packageId)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new DeletePackageUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(packageId);

            // Assert
            result.ShouldBeTrue();
            repositoryMock.Verify(r => r.GetByIdAsync(packageId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(packageId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteDelete_Should_Return_False_When_Package_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IPackageRepository<TravelPackage, int>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var packageId = 999;

            repositoryMock.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync((TravelPackage?)null);

            var useCase = new DeletePackageUseCase(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDelete(packageId);

            // Assert
            result.ShouldBeFalse();
            repositoryMock.Verify(r => r.GetByIdAsync(packageId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }
    }
}