using MediatR;

namespace CSharpApp.Application.Products.CreateProduct;

public record CreateProductRequest(Product Product) : IRequest<CreateProductResponse>;