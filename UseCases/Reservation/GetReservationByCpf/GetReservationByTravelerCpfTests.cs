using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.GetReservationByCpf;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Reservation.GetReservationByCpf
{
    public class GetReservationByTravelerCpfTests
    {
        [Fact]
        public async Task ExecuteGetReservationByTravelerCpfAsync_Should_Return_Reservations_When_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var cpf = "12345678900";
            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 123456,
                    UserId = "user1",
                    TravelPackageId = 1,
                    Traveler = new List<Travelers>
                    {
                        new Travelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" }
                    }
                },
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 654321,
                    UserId = "user1",
                    TravelPackageId = 2,
                    Traveler = new List<Travelers>
                    {
                        new Travelers { Id = 2, Name = "Maria Silva", RG = "98.765.432-1" }
                    }
                }
            };

            var expectedResponse = new List<ResponseReservationJson>
            {
                new ResponseReservationJson
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 123456,
                    UserId = "user1",
                    TravelPackageId = 1
                },
                new ResponseReservationJson
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 654321,
                    UserId = "user1",
                    TravelPackageId = 2
                }
            };

            repositoryMock.Setup(r => r.GetReservationByCpfAsync(cpf)).ReturnsAsync(reservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(reservations)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerCpf(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerCpfAsync(cpf);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            repositoryMock.Verify(r => r.GetReservationByCpfAsync(cpf), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetReservationByTravelerCpfAsync_Should_Return_Empty_List_When_No_Reservations_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var cpf = "99999999999";
            var emptyReservations = new List<Nexus.Domain.Entities.Reservation>();
            var emptyResponse = new List<ResponseReservationJson>();

            repositoryMock.Setup(r => r.GetReservationByCpfAsync(cpf)).ReturnsAsync(emptyReservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(emptyReservations)).Returns(emptyResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerCpf(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerCpfAsync(cpf);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetReservationByCpfAsync(cpf), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetReservationByTravelerCpfAsync_Should_Handle_Single_Reservation()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var cpf = "11111111111";
            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 123456,
                    UserId = "user1",
                    TravelPackageId = 1,
                    Traveler = new List<Travelers>
                    {
                        new Travelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" }
                    }
                }
            };

            var expectedResponse = new List<ResponseReservationJson>
            {
                new ResponseReservationJson
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 123456,
                    UserId = "user1",
                    TravelPackageId = 1
                }
            };

            repositoryMock.Setup(r => r.GetReservationByCpfAsync(cpf)).ReturnsAsync(reservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(reservations)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerCpf(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerCpfAsync(cpf);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(1);
            result.First().ReservationNumber.ShouldBe(123456);
            repositoryMock.Verify(r => r.GetReservationByCpfAsync(cpf), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}