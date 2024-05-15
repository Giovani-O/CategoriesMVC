using CategoriesMVC.Models;
using CategoriesMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CategoriesMVC.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private string token = string.Empty;

    public ProductsController(IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        //extrai o token do cookie
        var result = await _productService.GetProducts(GetJwtToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CreateNewProduct()
    {
        ViewBag.CategoryId =
           new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");

        return View();
    }
    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> CreateNewProduct(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.CreateProduct(productViewModel, GetJwtToken());

            if (result != null)
                return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewBag.CategoryId =
            new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");
        }
        return View(productViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> ProductDetails(int id)
    {
        var product = await _productService.GetProductById(id, GetJwtToken());

        if (product is null)
            return View("Error");

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var result = await _productService.GetProductById(id, GetJwtToken());

        if (result is null)
            return View("Error");

        ViewBag.CategoryId =
          new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");

        return View(result);
    }
    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> UpdateProduct(int id, ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(id, productViewModel, GetJwtToken());

            if (result)
                return RedirectToAction(nameof(Index));
        }
        return View(productViewModel);
    }
    [HttpGet]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await _productService.GetProductById(id, GetJwtToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var result = await _productService.DeleteProduct(id, GetJwtToken());

        if (result)
            return RedirectToAction("Index");

        return View("Error");
    }

    private string GetJwtToken()
    {
        if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            token = HttpContext.Request.Cookies["X-Access-Token"].ToString();

        return token;
    }
}
