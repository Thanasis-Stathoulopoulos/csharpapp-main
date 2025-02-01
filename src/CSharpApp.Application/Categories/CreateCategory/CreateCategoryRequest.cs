using MediatR;

namespace CSharpApp.Application.Categories.CreateCategory;

public record CreateCategoryRequest(Category Category) : IRequest<CreateCategoryResponse>;