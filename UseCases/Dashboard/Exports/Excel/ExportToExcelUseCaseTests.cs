using Moq;
using Nexus.Application.UseCases.Dashboard.Exports.Excel;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Reservation;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Dashboard.Exports.Excel
{
    public class ExportToExcelUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Return_Excel_File_With_All_Reservations_When_No_Date_Filter()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            DateTime? startDate = null;
            DateTime? endDate = null;

            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.AddDays(-5),
                    ReservationNumber = 12345,
                    UserId = "user1",
                    TravelPackageId = 1,
                    User = new Nexus.Domain.Entities.User { Name = "João Silva" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Paris", 
                        Destination = "Paris", 
                        Value = 2500.00 
                    }
                },
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.AddDays(-10),
                    ReservationNumber = 67890,
                    UserId = "user2",
                    TravelPackageId = 2,
                    User = new Nexus.Domain.Entities.User { Name = "Maria Santos" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Londres", 
                        Destination = "Londres", 
                        Value = 3000.00 
                    }
                }
            };

            reservationRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reservations);

            var useCase = new ExportToExcelUseCase(reservationRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            
            reservationRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Return_Excel_File_With_Filtered_Reservations_When_Date_Range_Provided()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            var startDate = DateTime.Now.AddDays(-15);
            var endDate = DateTime.Now.AddDays(-5);

            var allReservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.AddDays(-7), // Dentro do range
                    ReservationNumber = 12345,
                    UserId = "user1",
                    TravelPackageId = 1,
                    User = new Nexus.Domain.Entities.User { Name = "João Silva" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Paris", 
                        Destination = "Paris", 
                        Value = 2500.00 
                    }
                },
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.AddDays(-20), // Fora do range
                    ReservationNumber = 67890,
                    UserId = "user2",
                    TravelPackageId = 2,
                    User = new Nexus.Domain.Entities.User { Name = "Maria Santos" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Londres", 
                        Destination = "Londres", 
                        Value = 3000.00 
                    }
                },
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 3,
                    ReservationDate = DateTime.Now.AddDays(-10), // Dentro do range
                    ReservationNumber = 11111,
                    UserId = "user3",
                    TravelPackageId = 3,
                    User = new Nexus.Domain.Entities.User { Name = "Pedro Costa" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Tokyo", 
                        Destination = "Tokyo", 
                        Value = 4000.00 
                    }
                }
            };

            reservationRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(allReservations);

            var useCase = new ExportToExcelUseCase(reservationRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            
            reservationRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Return_Excel_File_When_No_Reservations_Exist()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            DateTime? startDate = DateTime.Now.AddDays(-30);
            DateTime? endDate = DateTime.Now;

            var emptyReservations = new List<Nexus.Domain.Entities.Reservation>();

            reservationRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyReservations);

            var useCase = new ExportToExcelUseCase(reservationRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0); // Deve retornar pelo menos o cabeçalho
            
            reservationRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Handle_Reservations_With_Null_User_And_Package()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            DateTime? startDate = null;
            DateTime? endDate = null;

            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.AddDays(-5),
                    ReservationNumber = 12345,
                    UserId = "user1",
                    TravelPackageId = 1,
                    User = null, // User nulo
                    TravelPackage = null // Package nulo
                },
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.AddDays(-10),
                    ReservationNumber = 67890,
                    UserId = "user2",
                    TravelPackageId = 2,
                    User = new Nexus.Domain.Entities.User { Name = "Maria Santos" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Londres", 
                        Destination = "Londres", 
                        Value = 3000.00 
                    }
                }
            };

            reservationRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reservations);

            var useCase = new ExportToExcelUseCase(reservationRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            
            reservationRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Handle_Large_Dataset()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            DateTime? startDate = null;
            DateTime? endDate = null;

            var largeReservationList = new List<Nexus.Domain.Entities.Reservation>();
            for (int i = 1; i <= 1000; i++)
            {
                largeReservationList.Add(new Nexus.Domain.Entities.Reservation
                {
                    Id = i,
                    ReservationDate = DateTime.Now.AddDays(-i % 30),
                    ReservationNumber = 10000 + i,
                    UserId = $"user{i}",
                    TravelPackageId = i % 10,
                    User = new Nexus.Domain.Entities.User { Name = $"User {i}" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = $"Pacote {i}", 
                        Destination = $"Destination {i % 5}", 
                        Value = 1000.00 + (i * 10) 
                    }
                });
            }

            reservationRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(largeReservationList);

            var useCase = new ExportToExcelUseCase(reservationRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            
            reservationRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Handle_Edge_Case_Date_Filtering()
        {
            // Arrange
            var reservationRepositoryMock = new Mock<IReservationRepository>();

            var startDate = DateTime.Now.Date; // Mesmo dia
            var endDate = DateTime.Now.Date;

            var reservations = new List<Nexus.Domain.Entities.Reservation>
            {
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 1,
                    ReservationDate = DateTime.Now.Date, // Exatamente no range
                    ReservationNumber = 12345,
                    UserId = "user1",
                    TravelPackageId = 1,
                    User = new Nexus.Domain.Entities.User { Name = "João Silva" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Paris", 
                        Destination = "Paris", 
                        Value = 2500.00 
                    }
                },
                new Nexus.Domain.Entities.Reservation
                {
                    Id = 2,
                    ReservationDate = DateTime.Now.Date.AddDays(1), // Fora do range
                    ReservationNumber = 67890,
                    UserId = "user2",
                    TravelPackageId = 2,
                    User = new Nexus.Domain.Entities.User { Name = "Maria Santos" },
                    TravelPackage = new TravelPackage 
                    { 
                        Title = "Pacote Londres", 
                        Destination = "Londres", 
                        Value = 3000.00 
                    }
                }
            };

            reservationRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(reservations);

            var useCase = new ExportToExcelUseCase(reservationRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            
            reservationRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}