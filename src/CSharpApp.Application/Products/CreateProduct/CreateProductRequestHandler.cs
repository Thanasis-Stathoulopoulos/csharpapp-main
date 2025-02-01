using MediatR;

namespace CSharpApp.Application.Products.CreateProduct;

public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly IProductsService _productsService;

    public CreateProductRequestHandler(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productsService.CreateProduct(request.Product);
        return new CreateProductResponse(product);
    }
}