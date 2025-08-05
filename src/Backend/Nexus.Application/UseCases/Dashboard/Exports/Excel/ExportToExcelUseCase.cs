using System.Data;
using ClosedXML.Excel;
using Nexus.Domain.Repositories.Reservation;
using Nexus.Domain.Entities;

namespace Nexus.Application.UseCases.Dashboard.Exports.Excel
{
    public class ExportToExcelUseCase : IExportToExcelUseCase
    {
        private readonly IReservationRepository _reservationRepository;

        public ExportToExcelUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<byte[]> Execute(DateTime? startDate, DateTime? endDate)
        {
            // Busca as reservas no período
            var allReservations = await _reservationRepository.GetAllAsync();
            if (startDate.HasValue && endDate.HasValue)
            {
                allReservations = allReservations.Where(r => r.ReservationDate >= startDate.Value.Date && r.ReservationDate <= endDate.Value.Date);
            }
            var filteredReservations = allReservations.ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório de Reservas e Pacotes");

            // Cabeçalho
            worksheet.Cell(1, 1).Value = "ID da Reserva";
            worksheet.Cell(1, 2).Value = "Data da Reserva";
            worksheet.Cell(1, 3).Value = "Número da Reserva";
            worksheet.Cell(1, 4).Value = "Status do Pagamento";
            worksheet.Cell(1, 5).Value = "ID do Usuário";
            worksheet.Cell(1, 6).Value = "Nome do Usuário";
            worksheet.Cell(1, 7).Value = "ID do Pacote";
            worksheet.Cell(1, 8).Value = "Nome do Pacote";
            worksheet.Cell(1, 9).Value = "Destino do Pacote";
            worksheet.Cell(1, 10).Value = "Valor do Pacote";

            var headerRange = worksheet.Range(1, 1, 1, 10);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.BabyBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var r in filteredReservations)
            {
                worksheet.Cell(row, 1).Value = r.Id;
                worksheet.Cell(row, 2).Value = r.ReservationDate.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 3).Value = r.ReservationNumber;
                worksheet.Cell(row, 4).Value = r.Payment == null ? "Pendente" : r.Payment.Status;
                worksheet.Cell(row, 5).Value = r.UserId;
                worksheet.Cell(row, 6).Value = r.User?.Name;
                worksheet.Cell(row, 7).Value = r.TravelPackageId;
                worksheet.Cell(row, 8).Value = r.TravelPackage?.Title;
                worksheet.Cell(row, 9).Value = r.TravelPackage?.Destination;
                worksheet.Cell(row, 10).Value = r.TravelPackage?.Value;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
