using MediatR;
namespace CSharpApp.Application.Categories.GetCategoryById;

public record GetCategoryByIdRequest(int Id) : IRequest<GetCategoryByIdResponse>;