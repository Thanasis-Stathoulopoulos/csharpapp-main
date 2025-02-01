using CSharpApp.Application.Categories.CreateCategory;
using CSharpApp.Application.Categories.GetCategoryById;
using CSharpApp.Application.Categories.GetCategories;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace CSharpApp.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/categories")]
[ApiVersion("1.0")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<Category>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetCategoriesRequest());
        return Ok(response.Categories);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _mediator.Send(new GetCategoryByIdRequest(id));
        return Ok(response.Category);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        var response = await _mediator.Send(new CreateCategoryRequest(category));
        return CreatedAtAction(nameof(GetById), new { id = response.Category.Id }, response.Category);
    }
}