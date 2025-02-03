namespace CSharpApp.Application.Categories.GetCategories;

public record GetCategoriesResponse(IReadOnlyCollection<Category> Categories);