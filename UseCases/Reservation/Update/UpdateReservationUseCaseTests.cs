using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.Update;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Reservation.Update
{
    public class UpdateReservationUseCaseTests
    {
        [Fact]
        public async Task ExecuteUpdateAsync_Should_Return_ResponseReservation_When_Reservation_Exists()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Reservation, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 1;
            var request = new RequestRegisterReservationJson
            {
                UserId = "user1-updated",
                TravelPackageId = 2,
                Traveler = new List<RequestTravelers>
                {
                    new RequestTravelers { Name = "João Silva Atualizado", RG = "12.345.678-9" }
                }
            };

            var existingReservation = new Nexus.Domain.Entities.Reservation
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

            var expectedResponse = new ResponseRegisteredReservationJson
            {
                Message = "Reserva atualizada com sucesso!"
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(existingReservation);
            
            // Setup AutoMapper to modify the existing reservation in place
            mapperMock.Setup(m => m.Map(request, existingReservation))
                .Callback<RequestRegisterReservationJson, Nexus.Domain.Entities.Reservation>((src, dest) =>
                {
                    dest.UserId = src.UserId;
                    dest.TravelPackageId = src.TravelPackageId;
                })
                .Returns(existingReservation);
            
            mapperMock.Setup(m => m.Map<ResponseRegisteredReservationJson>(It.IsAny<Nexus.Domain.Entities.Reservation>()))
                .Returns(expectedResponse);
            
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Reservation>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new UpdateReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteUpdateAsync(reservationId, request);

            // Assert
            result.ShouldNotBeNull();
            result.Message.ShouldBe("Reserva atualizada com sucesso!");
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Reservation>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteUpdateAsync_Should_Return_Null_When_Reservation_Does_Not_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Reservation, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 999;
            var request = new RequestRegisterReservationJson
            {
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<RequestTravelers>
                {
                    new RequestTravelers { Name = "João Silva", RG = "12.345.678-9" }
                }
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync((Nexus.Domain.Entities.Reservation?)null);

            var useCase = new UpdateReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteUpdateAsync(reservationId, request);

            // Assert
            result.ShouldBeNull();
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Reservation>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task ExecuteUpdateAsync_Should_Update_Reservation_With_Multiple_Travelers()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<Nexus.Domain.Entities.Reservation, int>>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var reservationId = 1;
            var request = new RequestRegisterReservationJson
            {
                UserId = "user1-updated",
                TravelPackageId = 3,
                Traveler = new List<RequestTravelers>
                {
                    new RequestTravelers { Name = "João Silva Atualizado", RG = "12.345.678-9" },
                    new RequestTravelers { Name = "Maria Silva Atualizada", RG = "98.765.432-1" },
                    new RequestTravelers { Name = "Pedro Silva Novo", RG = "11.111.111-1" }
                }
            };

            var existingReservation = new Nexus.Domain.Entities.Reservation
            {
                Id = reservationId,
                ReservationDate = DateTime.Now.Date,
                ReservationNumber = 123456,
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<Travelers>
                {
                    new Travelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" },
                    new Travelers { Id = 2, Name = "Maria Silva", RG = "98.765.432-1" }
                }
            };

            var expectedResponse = new ResponseRegisteredReservationJson
            {
                Message = "Reserva atualizada com sucesso!"
            };

            repositoryMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(existingReservation);
            
            mapperMock.Setup(m => m.Map(request, existingReservation))
                .Callback<RequestRegisterReservationJson, Nexus.Domain.Entities.Reservation>((src, dest) =>
                {
                    dest.UserId = src.UserId;
                    dest.TravelPackageId = src.TravelPackageId;
                })
                .Returns(existingReservation);
            
            mapperMock.Setup(m => m.Map<ResponseRegisteredReservationJson>(It.IsAny<Nexus.Domain.Entities.Reservation>()))
                .Returns(expectedResponse);
            
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Reservation>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new UpdateReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteUpdateAsync(reservationId, request);

            // Assert
            result.ShouldNotBeNull();
            result.Message.ShouldBe("Reserva atualizada com sucesso!");
            repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Nexus.Domain.Entities.Reservation>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}