using MediatR;

namespace CSharpApp.Application.Products.CreateProduct;

public record CreateProductRequest(
    string Title,
    int Price,
    string Description,
    List<string> Images,
    int CategoryId
) : IRequest<CreateProductResponse>;