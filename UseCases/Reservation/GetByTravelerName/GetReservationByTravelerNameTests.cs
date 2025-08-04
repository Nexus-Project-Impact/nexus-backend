using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.GetBytravelerName;
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

namespace UseCases.Reservation.GetByTravelerName
{
    public class GetReservationByTravelerNameTests
    {
        [Fact]
        public async Task ExecuteGetReservationByTravelerNameAsync_Should_Return_Reservations_When_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var travelerName = "João Silva";
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
                    UserId = "user2",
                    TravelPackageId = 2,
                    Traveler = new List<Travelers>
                    {
                        new Travelers { Id = 2, Name = "João Silva", RG = "11.111.111-1" }
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
                    UserId = "user2",
                    TravelPackageId = 2
                }
            };

            repositoryMock.Setup(r => r.GetReservationByTravelerNameAsync(travelerName)).ReturnsAsync(reservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(reservations)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerName(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerNameAsync(travelerName);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            repositoryMock.Verify(r => r.GetReservationByTravelerNameAsync(travelerName), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetReservationByTravelerNameAsync_Should_Return_Empty_List_When_No_Reservations_Found()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var travelerName = "Nome Inexistente";
            var emptyReservations = new List<Nexus.Domain.Entities.Reservation>();
            var emptyResponse = new List<ResponseReservationJson>();

            repositoryMock.Setup(r => r.GetReservationByTravelerNameAsync(travelerName)).ReturnsAsync(emptyReservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(emptyReservations)).Returns(emptyResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerName(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerNameAsync(travelerName);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetReservationByTravelerNameAsync(travelerName), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetReservationByTravelerNameAsync_Should_Handle_Partial_Name_Match()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var travelerName = "Maria";
            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 111222,
                    UserId = "user3",
                    TravelPackageId = 3,
                    Traveler = new List<Travelers>
                    {
                        new Travelers { Id = 3, Name = "Maria Santos", RG = "33.333.333-3" },
                        new Travelers { Id = 4, Name = "José Santos", RG = "44.444.444-4" }
                    }
                }
            };

            var expectedResponse = new List<ResponseReservationJson>
            {
                new ResponseReservationJson
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 111222,
                    UserId = "user3",
                    TravelPackageId = 3
                }
            };

            repositoryMock.Setup(r => r.GetReservationByTravelerNameAsync(travelerName)).ReturnsAsync(reservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(reservations)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerName(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerNameAsync(travelerName);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(1);
            result.First().ReservationNumber.ShouldBe(111222);
            repositoryMock.Verify(r => r.GetReservationByTravelerNameAsync(travelerName), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetReservationByTravelerNameAsync_Should_Handle_Case_Insensitive_Search()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var travelerName = "PEDRO OLIVEIRA";
            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 555666,
                    UserId = "user4",
                    TravelPackageId = 4,
                    Traveler = new List<Travelers>
                    {
                        new Travelers { Id = 5, Name = "Pedro Oliveira", RG = "55.555.555-5" }
                    }
                }
            };

            var expectedResponse = new List<ResponseReservationJson>
            {
                new ResponseReservationJson
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 555666,
                    UserId = "user4",
                    TravelPackageId = 4
                }
            };

            repositoryMock.Setup(r => r.GetReservationByTravelerNameAsync(travelerName)).ReturnsAsync(reservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(reservations)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetReservationByTravelerName(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetReservationByTravelerNameAsync(travelerName);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(1);
            result.First().ReservationNumber.ShouldBe(555666);
            repositoryMock.Verify(r => r.GetReservationByTravelerNameAsync(travelerName), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}