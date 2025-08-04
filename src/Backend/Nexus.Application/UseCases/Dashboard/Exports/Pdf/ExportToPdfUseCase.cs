using Nexus.Domain.Repositories.Reservation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Nexus.Application.UseCases.Dashboard.Exports.Pdf
{
    public class ExportToPdfUseCase : IExportToPdfUseCase
    {
        private readonly IReservationRepository _reservationRepository;

        public ExportToPdfUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<byte[]> Execute(DateTime? startDate, DateTime? endDate)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var allReservations = await _reservationRepository.GetAllAsync();
            if (startDate.HasValue && endDate.HasValue)
            {
                allReservations = allReservations
                    .Where(r => r.ReservationDate >= startDate.Value.Date && r.ReservationDate <= endDate.Value.Date);
            }
            var filteredReservations = allReservations.ToList();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Relatório de Reservas e Pacotes").SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Período: {startDate?.ToShortDateString()} - {endDate?.ToShortDateString()}");
                        col.Item().Text($"Total de Reservas: {filteredReservations.Count}");
                        col.Item().PaddingVertical(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // ID
                                columns.ConstantColumn(80);  // Data
                                columns.ConstantColumn(80);  // Número da reserva
                                columns.ConstantColumn(60);  // UserId
                                columns.RelativeColumn();    // Nome Usuário
                                columns.ConstantColumn(60);  // PacoteId
                                columns.RelativeColumn();    // Nome Pacote
                                columns.RelativeColumn();    // Destino
                                columns.ConstantColumn(60);  // Valor
                            });

                            // Cabeçalho
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID");
                                header.Cell().Element(CellStyle).Text("Data");
                                header.Cell().Element(CellStyle).Text("Nº Reserva");
                                header.Cell().Element(CellStyle).Text("UserId");
                                header.Cell().Element(CellStyle).Text("Nome Usuário");
                                header.Cell().Element(CellStyle).Text("PacoteId");
                                header.Cell().Element(CellStyle).Text("Nome Pacote");
                                header.Cell().Element(CellStyle).Text("Destino");
                                header.Cell().Element(CellStyle).Text("Valor");
                            });

                            // Dados
                            foreach (var r in filteredReservations)
                            {
                                table.Cell().Element(CellStyle).Text(r.Id.ToString());
                                table.Cell().Element(CellStyle).Text(r.ReservationDate.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(r.ReservationNumber.ToString());
                                table.Cell().Element(CellStyle).Text(r.UserId ?? "-");
                                table.Cell().Element(CellStyle).Text(r.User?.Name ?? "-");
                                table.Cell().Element(CellStyle).Text(r.TravelPackageId.ToString());
                                table.Cell().Element(CellStyle).Text(r.TravelPackage?.Title ?? "-");
                                table.Cell().Element(CellStyle).Text(r.TravelPackage?.Destination ?? "-");
                                table.Cell().Element(CellStyle).Text(r.TravelPackage?.Value.ToString("C") ?? "-");
                            }

                            // Defina o estilo de célula com padding
                            static IContainer CellStyle(IContainer container) => container.PaddingVertical(4);
                        });
                    });
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}


