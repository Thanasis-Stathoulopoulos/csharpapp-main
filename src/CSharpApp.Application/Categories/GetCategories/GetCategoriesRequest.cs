using MediatR;

namespace CSharpApp.Application.Categories.GetCategories;
public record GetCategoriesRequest : IRequest<GetCategoriesResponse>;