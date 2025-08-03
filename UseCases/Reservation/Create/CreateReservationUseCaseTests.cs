using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.Create;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Reservation.Create
{
    public class CreateReservationUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Create_Reservation_Successfully()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestRegisterReservationJson
            {
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<RequestTravelers>
                {
                    new RequestTravelers { Name = "João Silva", RG = "12.345.678-9" },
                    new RequestTravelers { Name = "Maria Silva", RG = "98.765.432-1" }
                }
            };

            var reservation = new Nexus.Domain.Entities.Reservation
            {
                Id = 1,
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
                Message = "Reserva realizada com sucesso!"
            };

            mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.Reservation>(request)).Returns(reservation);
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Reservation>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new CreateReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Message.ShouldBe("Reserva realizada com sucesso!");
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Reservation>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Create_Reservation_With_Empty_Travelers_List()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var request = new RequestRegisterReservationJson
            {
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<RequestTravelers>()
            };

            var reservation = new Nexus.Domain.Entities.Reservation
            {
                Id = 1,
                ReservationDate = DateTime.Now.Date,
                ReservationNumber = 123456,
                UserId = "user1",
                TravelPackageId = 1,
                Traveler = new List<Travelers>()
            };

            mapperMock.Setup(m => m.Map<Nexus.Domain.Entities.Reservation>(request)).Returns(reservation);
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Reservation>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new CreateReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.Execute(request);

            // Assert
            result.ShouldNotBeNull();
            result.Message.ShouldBe("Reserva realizada com sucesso!");
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Nexus.Domain.Entities.Reservation>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}