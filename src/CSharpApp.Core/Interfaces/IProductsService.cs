namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProducts();
    Task<Product> GetProductById(int id);
    Task<Product> CreateProduct(string title, int price, string description, List<string> images, int categoryId);
}