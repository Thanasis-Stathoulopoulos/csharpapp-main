using MediatR;

namespace CSharpApp.Application.Categories.CreateCategory;

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
    private readonly ICategoriesService _categoriesService;

    public CreateCategoryRequestHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoriesService.CreateCategory(request.Category);
        return new CreateCategoryResponse(category);
    }
}