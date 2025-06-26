using ScentsSymphonyWeb.Models;

namespace ScentsSymphonyWeb.Services
{
    public interface IInvoiceService
    {
        byte[] GenerateInvoice(Order order);
    }
}