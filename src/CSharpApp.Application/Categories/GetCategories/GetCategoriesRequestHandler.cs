using MediatR;

namespace CSharpApp.Application.Categories.GetCategories;

public class GetCategoriesRequestHandler : IRequestHandler<GetCategoriesRequest, GetCategoriesResponse>
{
    private readonly ICategoriesService _categoriesService;

    public GetCategoriesRequestHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public async Task<GetCategoriesResponse> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = await _categoriesService.GetCategories();
        return new GetCategoriesResponse(categories);
    }
}