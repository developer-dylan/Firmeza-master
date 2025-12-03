using Firmeza.Web.Models.Entities;
using Firmeza.Identity;

namespace Firmeza.Web.Interfaces
{
    public interface IExcelService
    {
        Task<bool> ProcessExcelAsync(IFormFile file);
        byte[] ExportProducts(IEnumerable<Product> products);
        byte[] ExportClients(IEnumerable<AppUser> clients);
        byte[] ExportSales(IEnumerable<Sale> sales);
    }
}