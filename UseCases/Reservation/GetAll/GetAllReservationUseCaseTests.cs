using AutoMapper;
using Moq;
using Nexus.Application.UseCases.Reservation.GetAll;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using Shouldly;

namespace UseCases.Reservation.GetAll
{
    public class GetAllReservationUseCaseTests
    {
        [Fact]
        public async Task ExecuteGetAllAsync_Should_Return_All_Reservations()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

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
                        new Travelers { Id = 2, Name = "Maria Santos", RG = "98.765.432-1" }
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
                    TravelPackageId = 1,
                    Traveler = new List<ResponseTravelers>
                    {
                        new ResponseTravelers { Id = 1, Name = "João Silva", RG = "12.345.678-9" }
                    }
                },
                new ResponseReservationJson
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.Date,
                    ReservationNumber = 654321,
                    UserId = "user2",
                    TravelPackageId = 2,
                    Traveler = new List<ResponseTravelers>
                    {
                        new ResponseTravelers { Id = 2, Name = "Maria Santos", RG = "98.765.432-1" }
                    }
                }
            };

            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(reservations)).Returns(expectedResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetAllReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetAllAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            result.First().ReservationNumber.ShouldBe(123456);
            result.Last().ReservationNumber.ShouldBe(654321);
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task ExecuteGetAllAsync_Should_Return_Empty_List_When_No_Reservations_Exist()
        {
            // Arrange
            var repositoryMock = new Mock<IReservationRepository>();
            var mapperMock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var emptyReservations = new List<Nexus.Domain.Entities.Reservation>();
            var emptyResponse = new List<ResponseReservationJson>();

            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyReservations);
            mapperMock.Setup(m => m.Map<IEnumerable<ResponseReservationJson>>(emptyReservations)).Returns(emptyResponse);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var useCase = new GetAllReservationUseCase(repositoryMock.Object, mapperMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await useCase.ExecuteGetAllAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(0);
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}