using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.GetByID;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Reservation.GetById
{
    public class GetByIdReservationUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetByIdAsync_Should_Return_Reservation_When_Exists()
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

            var expectedResponse = new ResponseReservationJson
            {
                Id = reservationId,
                ReservationDate = DateTime.Now.Date,
                ReservationNumber = 123456,
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<ResponseTravelers>
                {
                    new ResponseTravelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" }
                }
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(reservation);
            mapperMock.Setup(m => m.Map<ResponseReservationJson>(reservation)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetByIdReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetByIdAsync(reservationId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(reservationId);
            result.ReservationNumber.ShouldBe(123456);
            result.UserId.ShouldBe("user1");
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetByIdAsync_Should_Return_Null_When_Reservation_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 999;

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync((Nexus.Domain.Entities.Reservation?)null);
            mapperMock.Setup(m => m.Map<ResponseReservationJson>(It.IsAny<Nexus.Domain.Entities.Reservation>())).Returns((ResponseReservationJson?)null);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetByIdReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetByIdAsync(reservationId);

            // Assert
            result.ShouldBeNull();
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}