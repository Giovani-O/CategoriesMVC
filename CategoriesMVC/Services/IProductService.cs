using CategoriesMVC.Models;

namespace CategoriesMVC.Services;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetProducts(string token);
    Task<ProductViewModel> GetProductById(int id, string token);
    Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel, string token);
    Task<bool> UpdateProduct(int id, ProductViewModel productViewModel, string token);
    Task<bool> DeleteProduct(int id, string token);
}
