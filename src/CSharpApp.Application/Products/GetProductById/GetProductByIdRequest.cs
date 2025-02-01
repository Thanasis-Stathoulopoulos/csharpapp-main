using MediatR;

namespace CSharpApp.Application.Products.GetProductById;

public record GetProductByIdRequest(int Id) : IRequest<GetProductByIdResponse>;