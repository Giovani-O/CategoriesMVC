using CategoriesMVC.Models;
using CategoriesMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoriesMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Index()
    {
        var result = await _categoryService.GetCategories();

        if (result is null)
            return View("Error");

        return View(result);
    }
}
