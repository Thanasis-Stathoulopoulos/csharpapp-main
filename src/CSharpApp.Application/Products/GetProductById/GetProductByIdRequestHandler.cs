using MediatR;

namespace CSharpApp.Application.Products.GetProductById;

public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
{
    private readonly IProductsService _productsService;

    public GetProductByIdRequestHandler(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await _productsService.GetProductById(request.Id);
        return new GetProductByIdResponse(product);
    }
}