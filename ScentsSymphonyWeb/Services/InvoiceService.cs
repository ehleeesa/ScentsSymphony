using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScentsSymphonyWeb.Data;
using ScentsSymphonyWeb.Models;
using System.IO;
using System.Linq;

namespace ScentsSymphonyWeb.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _db;

        public InvoiceService(ApplicationDbContext db)
        {
            _db = db;
        }

        public byte[] GenerateInvoice(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                        .Text("Factură Scents Symphony")
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        var user = _db.Users.FirstOrDefault(u => u.Id == order.UserID);
                        var userName = order.FullName ?? user?.UserName ?? "Client";
                        var email = user?.Email ?? "-";

                        col.Item().Text($"Client: {userName} ({email})").FontSize(12);
                        col.Item().Text($"Adresă: {order.Address}, {order.City}, {order.County}, {order.PostalCode}");
                        col.Item().Text($"Telefon: {order.Phone}");
                        col.Item().Text($"Metodă livrare: {order.DeliveryMethod}");

                        col.Item().Element(container =>
                            container
                                .PaddingVertical(10)
                                .Text("Produse comandate:")
                                .FontSize(14)
                                .SemiBold()
                        );

                        foreach (var item in order.OrderItems)
                        {
                            var product = _db.Perfume.FirstOrDefault(p => p.ProductID == item.ProductID);
                            var name = product?.Name ?? "Produs necunoscut";
                            var subtotal = item.Quantity * item.Price;

                            col.Item().Text($"- {name} x{item.Quantity} — {subtotal:F2} RON");
                        }

                        col.Item().Element(x => x
                            .PaddingTop(15)
                            .Text($"Total: {order.TotalAmount:F2} RON")
                            .SemiBold()
                            .FontSize(16)
                        );
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("© 2025 Scents Symphony").FontSize(10);
                    });
                });
            });

            using var memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
