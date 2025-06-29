using Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Application.Utils
{
    public class PdfReportGenerator
    {
        public byte[] Generate(List<Punch> punches)
        {
            using var memoryStream = new MemoryStream();

            var document = new Document(PageSize.A4, 40, 40, 40, 40);
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Fontes
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLACK);
            var subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.DARK_GRAY);
            var cellHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

            // Título
            var title = new Paragraph("Relatório de Pontos", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Subtítulo
            var subtitle = new Paragraph($"Total de registros: {punches.Count}", subtitleFont)
            {
                Alignment = Element.ALIGN_LEFT,
                SpacingAfter = 15
            };
            document.Add(subtitle);

            // Tabela
            var table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                SpacingBefore = 10
            };

            table.SetWidths(new float[] { 2f, 1.5f, 4f }); // Ajusta larguras

            // Cabeçalho
            AddCell(table, "Data/Hora", cellHeaderFont, BaseColor.GRAY, true);
            AddCell(table, "Tipo", cellHeaderFont, BaseColor.GRAY, true);
            AddCell(table, "Endereço", cellHeaderFont, BaseColor.GRAY, true);

            foreach (var punch in punches)
            {
                AddCell(table, punch.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), cellFont);
                AddCell(table, punch.Type == 0 ? "Entrada" : "Saída", cellFont);
                AddCell(table, punch.Address ?? "-", cellFont);
            }

            document.Add(table);
            document.Close();

            return memoryStream.ToArray();
        }

        private void AddCell(PdfPTable table, string text, Font font, BaseColor backgroundColor = null, bool isHeader = false)
        {
            var cell = new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5,
                BackgroundColor = backgroundColor ?? BaseColor.WHITE,
                BorderWidth = 0.5f
            };

            if (isHeader)
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }

            table.AddCell(cell);
        }
    }
}
