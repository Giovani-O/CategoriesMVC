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

    [HttpGet]
    public IActionResult CreateNewCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> CreateNewCategory(CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.CreateCategory(categoryViewModel);

            if (result is not null)
                return RedirectToAction(nameof(Index));
        }
        ViewBag.Erro = "Erro ao criar categoria";
        return View(categoryViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCategory(int id)
    {
        var result = await _categoryService.GetCategoryById(id);

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> UpdateCategory(int id, CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.UpdateCategory(id, categoryViewModel);

            if (result)
                return RedirectToAction(nameof(Index));
        }
        ViewBag.Erro = "Erro ao atualizar categoria";
        return View(categoryViewModel);
    }
}
