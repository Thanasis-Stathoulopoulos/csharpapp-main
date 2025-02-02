using MediatR;

namespace CSharpApp.Application.Categories.CreateCategory;

public record CreateCategoryRequest(
    string Name,
    string Image
) : IRequest<CreateCategoryResponse>;