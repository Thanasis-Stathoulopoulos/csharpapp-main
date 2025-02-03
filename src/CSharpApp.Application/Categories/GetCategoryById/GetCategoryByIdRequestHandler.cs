using MediatR;

namespace CSharpApp.Application.Categories.GetCategoryById;

public class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, GetCategoryByIdResponse>
{
    private readonly ICategoriesService _categoriesService;

    public GetCategoryByIdRequestHandler(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    public async Task<GetCategoryByIdResponse> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoriesService.GetCategoryById(request.Id);
        return new GetCategoryByIdResponse(category);
    }
}