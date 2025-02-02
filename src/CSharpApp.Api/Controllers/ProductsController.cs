using Asp.Versioning;
using CSharpApp.Application.Categories.CreateCategory;
using CSharpApp.Application.Products.CreateProduct;
using CSharpApp.Application.Products.GetProductById;
using CSharpApp.Application.Products.GetProducts;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products")]
[ApiVersion("1.0")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<Product>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetProductsRequest());
        return Ok(response.Products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _mediator.Send(new GetProductByIdRequest(id));
        return Ok(response.Product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var response = await _mediator.Send(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Category.Id }, response.Category);
    }
}