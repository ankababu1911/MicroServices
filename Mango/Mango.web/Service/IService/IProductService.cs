using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
