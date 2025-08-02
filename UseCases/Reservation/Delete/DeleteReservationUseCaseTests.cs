using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.Delete;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Reservation.Delete
{
    public class DeleteReservationUseCaseTests
    {
        [Fact]
        public async Task ExecuteDeleteAsync_Should_Return_True_When_Reservation_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 1;
            var reservation = new Nexus.Domain.Entities.Reservation
            {
                Id = reservationId,
                ReservationDate = DateTime.Now.Date,
                ReservationNumber = 123456,
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<Travelers>
                {
                    new Travelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" }
                }
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(reservation);
            repositoryMock.Setup(r => r.DeleteAsync(reservationId)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new DeleteReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDeleteAsync(reservationId);

            // Assert
            result.ShouldBeTrue();
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(reservationId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteDeleteAsync_Should_Return_False_When_Reservation_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 999;

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync((Nexus.Domain.Entities.Reservation?)null);

            var useCase = new DeleteReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDeleteAsync(reservationId);

            // Assert
            result.ShouldBeFalse();
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task ExecuteDeleteAsync_Should_Delete_Reservation_With_Multiple_Travelers()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 1;
            var reservation = new Nexus.Domain.Entities.Reservation
            {
                Id = reservationId,
                ReservationDate = DateTime.Now.Date,
                ReservationNumber = 123456,
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<Travelers>
                {
                    new Travelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" },
                    new Travelers { Id = 2, Name = "Maria Silva", RG = "98.765.432-1" },
                    new Travelers { Id = 3, Name = "Pedro Silva", RG = "11.111.111-1" }
                }
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(reservation);
            repositoryMock.Setup(r => r.DeleteAsync(reservationId)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new DeleteReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteDeleteAsync(reservationId);

            // Assert
            result.ShouldBeTrue();
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            repositoryMock.Verify(r => r.DeleteAsync(reservationId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}