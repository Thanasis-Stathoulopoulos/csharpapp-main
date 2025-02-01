using MediatR;

namespace CSharpApp.Application.Products.GetProducts;

public record GetProductsRequest : IRequest<GetProductsResponse>;