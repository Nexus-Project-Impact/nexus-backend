using Moq;
using Nexus.Application.UseCases.Dashboard;
using Nexus.Communication.Responses;
using Nexus.Domain.DTOs;
using Nexus.Domain.Repositories.Dashboard;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UseCases.Dashboard
{
    public class GetDashboardMetricsUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Return_Dashboard_Metrics_With_All_Data()
        {
            // Arrange
            var dashboardRepositoryMock = new Mock<IDashboardMetricsRepositoy>();

            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;

            var salesByDestination = new List<SalesByDestinationDto>
            {
                new SalesByDestinationDto { Destination = "Paris", Quantity = 15 },
                new SalesByDestinationDto { Destination = "Londres", Quantity = 10 },
                new SalesByDestinationDto { Destination = "Tokyo", Quantity = 8 }
            };

            var salesByPeriod = new List<SalesByPeriodDto>
            {
                new SalesByPeriodDto { Date = DateTime.Now.AddDays(-7), Quantity = 5 },
                new SalesByPeriodDto { Date = DateTime.Now.AddDays(-14), Quantity = 8 },
                new SalesByPeriodDto { Date = DateTime.Now.AddDays(-21), Quantity = 12 }
            };

            var summary = new DashboardSummaryDto
            {
                TotalReservations = 50,
                TotalClients = 35,
                TopDestinations = new List<string> { "Paris", "Londres", "Tokyo" }
            };

            dashboardRepositoryMock.Setup(d => d.GetSalesByDestinationAsync(startDate, endDate))
                .ReturnsAsync(salesByDestination);

            dashboardRepositoryMock.Setup(d => d.GetSalesByPeriodAsync(startDate, endDate))
                .ReturnsAsync(salesByPeriod);

            dashboardRepositoryMock.Setup(d => d.GetSummaryAsync(startDate, endDate))
                .ReturnsAsync(summary);

            var useCase = new GetDashboardMetricsUseCase(dashboardRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            
            result.SalesByDestination.ShouldNotBeNull();
            result.SalesByDestination.Count().ShouldBe(3);
            result.SalesByDestination.First().Destination.ShouldBe("Paris");
            result.SalesByDestination.First().Quantity.ShouldBe(15);

            result.SalesByPeriod.ShouldNotBeNull();
            result.SalesByPeriod.Count().ShouldBe(3);
            result.SalesByPeriod.First().Quantity.ShouldBe(5);

            result.Summary.ShouldNotBeNull();
            result.Summary.TotalReservations.ShouldBe(50);
            result.Summary.TotalClients.ShouldBe(35);
            result.Summary.TopDestinations.ShouldNotBeNull();
            result.Summary.TopDestinations.Count.ShouldBe(3);

            dashboardRepositoryMock.Verify(d => d.GetSalesByDestinationAsync(startDate, endDate), Times.Once);
            dashboardRepositoryMock.Verify(d => d.GetSalesByPeriodAsync(startDate, endDate), Times.Once);
            dashboardRepositoryMock.Verify(d => d.GetSummaryAsync(startDate, endDate), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Return_Dashboard_Metrics_With_Null_Dates()
        {
            // Arrange
            var dashboardRepositoryMock = new Mock<IDashboardMetricsRepositoy>();

            DateTime? startDate = null;
            DateTime? endDate = null;

            var salesByDestination = new List<SalesByDestinationDto>
            {
                new SalesByDestinationDto { Destination = "Roma", Quantity = 20 }
            };

            var salesByPeriod = new List<SalesByPeriodDto>
            {
                new SalesByPeriodDto { Date = DateTime.Now, Quantity = 15 }
            };

            var summary = new DashboardSummaryDto
            {
                TotalReservations = 100,
                TotalClients = 75,
                TopDestinations = new List<string> { "Roma", "Berlim" }
            };

            dashboardRepositoryMock.Setup(d => d.GetSalesByDestinationAsync(startDate, endDate))
                .ReturnsAsync(salesByDestination);

            dashboardRepositoryMock.Setup(d => d.GetSalesByPeriodAsync(startDate, endDate))
                .ReturnsAsync(salesByPeriod);

            dashboardRepositoryMock.Setup(d => d.GetSummaryAsync(startDate, endDate))
                .ReturnsAsync(summary);

            var useCase = new GetDashboardMetricsUseCase(dashboardRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.SalesByDestination.ShouldNotBeNull();
            result.SalesByDestination.Count().ShouldBe(1);
            result.SalesByDestination.First().Destination.ShouldBe("Roma");

            result.Summary.TotalReservations.ShouldBe(100);
            result.Summary.TotalClients.ShouldBe(75);

            dashboardRepositoryMock.Verify(d => d.GetSalesByDestinationAsync(null, null), Times.Once);
            dashboardRepositoryMock.Verify(d => d.GetSalesByPeriodAsync(null, null), Times.Once);
            dashboardRepositoryMock.Verify(d => d.GetSummaryAsync(null, null), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Return_Empty_Data_When_No_Results_Found()
        {
            // Arrange
            var dashboardRepositoryMock = new Mock<IDashboardMetricsRepositoy>();

            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;

            var emptySalesByDestination = new List<SalesByDestinationDto>();
            var emptySalesByPeriod = new List<SalesByPeriodDto>();
            var emptySummary = new DashboardSummaryDto
            {
                TotalReservations = 0,
                TotalClients = 0,
                TopDestinations = new List<string>()
            };

            dashboardRepositoryMock.Setup(d => d.GetSalesByDestinationAsync(startDate, endDate))
                .ReturnsAsync(emptySalesByDestination);

            dashboardRepositoryMock.Setup(d => d.GetSalesByPeriodAsync(startDate, endDate))
                .ReturnsAsync(emptySalesByPeriod);

            dashboardRepositoryMock.Setup(d => d.GetSummaryAsync(startDate, endDate))
                .ReturnsAsync(emptySummary);

            var useCase = new GetDashboardMetricsUseCase(dashboardRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.SalesByDestination.ShouldNotBeNull();
            result.SalesByDestination.Count().ShouldBe(0);

            result.SalesByPeriod.ShouldNotBeNull();
            result.SalesByPeriod.Count().ShouldBe(0);

            result.Summary.ShouldNotBeNull();
            result.Summary.TotalReservations.ShouldBe(0);
            result.Summary.TotalClients.ShouldBe(0);
            result.Summary.TopDestinations.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Execute_Should_Handle_Large_Dataset()
        {
            // Arrange
            var dashboardRepositoryMock = new Mock<IDashboardMetricsRepositoy>();

            var startDate = DateTime.Now.AddMonths(-12);
            var endDate = DateTime.Now;

            var largeSalesByDestination = new List<SalesByDestinationDto>();
            for (int i = 1; i <= 50; i++)
            {
                largeSalesByDestination.Add(new SalesByDestinationDto 
                { 
                    Destination = $"Destination{i}", 
                    Quantity = i * 2 
                });
            }

            var largeSalesByPeriod = new List<SalesByPeriodDto>();
            for (int i = 1; i <= 365; i++)
            {
                largeSalesByPeriod.Add(new SalesByPeriodDto 
                { 
                    Date = DateTime.Now.AddDays(-i), 
                    Quantity = i % 10 + 1 
                });
            }

            var largeSummary = new DashboardSummaryDto
            {
                TotalReservations = 5000,
                TotalClients = 3500,
                TopDestinations = largeSalesByDestination.Take(10).Select(s => s.Destination).ToList()
            };

            dashboardRepositoryMock.Setup(d => d.GetSalesByDestinationAsync(startDate, endDate))
                .ReturnsAsync(largeSalesByDestination);

            dashboardRepositoryMock.Setup(d => d.GetSalesByPeriodAsync(startDate, endDate))
                .ReturnsAsync(largeSalesByPeriod);

            dashboardRepositoryMock.Setup(d => d.GetSummaryAsync(startDate, endDate))
                .ReturnsAsync(largeSummary);

            var useCase = new GetDashboardMetricsUseCase(dashboardRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.SalesByDestination.Count().ShouldBe(50);
            result.SalesByPeriod.Count().ShouldBe(365);
            result.Summary.TotalReservations.ShouldBe(5000);
            result.Summary.TotalClients.ShouldBe(3500);
            result.Summary.TopDestinations.Count.ShouldBe(10);
        }

        [Fact]
        public async Task Execute_Should_Handle_Different_Date_Ranges()
        {
            // Arrange
            var dashboardRepositoryMock = new Mock<IDashboardMetricsRepositoy>();

            var startDate = DateTime.Now.AddYears(-1);
            var endDate = DateTime.Now.AddDays(-30);

            var salesByDestination = new List<SalesByDestinationDto>
            {
                new SalesByDestinationDto { Destination = "Barcelona", Quantity = 25 }
            };

            var salesByPeriod = new List<SalesByPeriodDto>
            {
                new SalesByPeriodDto { Date = startDate.AddDays(10), Quantity = 3 }
            };

            var summary = new DashboardSummaryDto
            {
                TotalReservations = 25,
                TotalClients = 20,
                TopDestinations = new List<string> { "Barcelona" }
            };

            dashboardRepositoryMock.Setup(d => d.GetSalesByDestinationAsync(startDate, endDate))
                .ReturnsAsync(salesByDestination);

            dashboardRepositoryMock.Setup(d => d.GetSalesByPeriodAsync(startDate, endDate))
                .ReturnsAsync(salesByPeriod);

            dashboardRepositoryMock.Setup(d => d.GetSummaryAsync(startDate, endDate))
                .ReturnsAsync(summary);

            var useCase = new GetDashboardMetricsUseCase(dashboardRepositoryMock.Object);

            // Act
            var result = await useCase.Execute(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.SalesByDestination.First().Destination.ShouldBe("Barcelona");
            result.SalesByDestination.First().Quantity.ShouldBe(25);
            result.Summary.TotalReservations.ShouldBe(25);

            dashboardRepositoryMock.Verify(d => d.GetSalesByDestinationAsync(startDate, endDate), Times.Once);
            dashboardRepositoryMock.Verify(d => d.GetSalesByPeriodAsync(startDate, endDate), Times.Once);
            dashboardRepositoryMock.Verify(d => d.GetSummaryAsync(startDate, endDate), Times.Once);
        }
    }
}