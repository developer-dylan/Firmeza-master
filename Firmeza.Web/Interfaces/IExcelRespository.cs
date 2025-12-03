using Firmeza.Web.DTOs;

namespace Firmeza.Web.Interfaces
{
    public interface IExcelRepository
    {
        Task SaveProductsFromExcelAsync(IEnumerable<ExcelProductDto> excelProducts);
    }
}