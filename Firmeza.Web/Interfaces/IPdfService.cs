using Firmeza.Web.Models.Entities;
using Firmeza.Identity;

namespace Firmeza.Web.Interfaces;

public interface IPdfService
{
    byte[] GenerateProductListPdf(IEnumerable<Product> products);
    byte[] GenerateClientListPdf(IEnumerable<AppUser> clients);
    byte[] GenerateSaleListPdf(IEnumerable<Sale> sales);
    Task<string> GenerateSaleReceiptAsync(Sale sale);
}
