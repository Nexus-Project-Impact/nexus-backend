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
                        col.Item().Text($"Período: {startDate?.ToShortDateString()} - {endDate?.ToShortDateString()}").FontSize(12);
                        col.Item().Text($"Total de Reservas: {filteredReservations.Count}").FontSize(12);
                        col.Item().PaddingVertical(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);  // Nº Reserva
                                columns.ConstantColumn(70);  // Data
                                columns.RelativeColumn(2);   // Nome Cliente
                                columns.ConstantColumn(70);  // Status Pagamento
                                columns.RelativeColumn(2);   // Nome Pacote
                                columns.RelativeColumn(1);   // Destino
                                columns.ConstantColumn(60);  // Valor
                            });

                            // Cabeçalho
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("Nº Reserva");
                                header.Cell().Element(HeaderCellStyle).Text("Data");
                                header.Cell().Element(HeaderCellStyle).Text("Nome Cliente");
                                header.Cell().Element(HeaderCellStyle).Text("Status Pag.");
                                header.Cell().Element(HeaderCellStyle).Text("Nome Pacote");
                                header.Cell().Element(HeaderCellStyle).Text("Destino");
                                header.Cell().Element(HeaderCellStyle).Text("Valor");
                            });

                            // Dados limpos
                            foreach (var r in filteredReservations)
                            {
                                table.Cell().Element(DataCellStyle).Text(r.ReservationNumber.ToString());
                                table.Cell().Element(DataCellStyle).Text(r.ReservationDate.ToString("dd/MM/yyyy"));
                                table.Cell().Element(DataCellStyle).Text(r.User?.Name ?? "-");
                                table.Cell().Element(DataCellStyle).Text(r.Payment == null ? "Pendente" : r.Payment.Status);
                                table.Cell().Element(DataCellStyle).Text(r.TravelPackage?.Title ?? "-");
                                table.Cell().Element(DataCellStyle).Text(r.TravelPackage?.Destination ?? "-");
                                table.Cell().Element(DataCellStyle).Text(r.TravelPackage?.Value.ToString("C") ?? "-");
                            }

                            // Estilo para cabeçalho - fonte maior e em negrito
                            static IContainer HeaderCellStyle(IContainer container) => 
                                container
                                    .PaddingVertical(5)
                                    .DefaultTextStyle(x => x.FontSize(11).SemiBold());

                            // Estilo para dados - fonte maior
                            static IContainer DataCellStyle(IContainer container) => 
                                container
                                    .PaddingVertical(3)
                                    .DefaultTextStyle(x => x.FontSize(10));
                        });
                    });
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}


