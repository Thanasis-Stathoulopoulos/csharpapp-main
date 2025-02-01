using MediatR;

namespace CSharpApp.Application.Products.GetProducts;

public class GetProductsRequestHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
{
    private readonly IProductsService _productsService;

    public GetProductsRequestHandler(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var products = await _productsService.GetProducts();
        return new GetProductsResponse(products);
    }
}