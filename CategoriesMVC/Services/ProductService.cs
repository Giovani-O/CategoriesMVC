using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CategoriesMVC.Models;

namespace CategoriesMVC.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/Products/";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productViewModel;
    private IEnumerable<ProductViewModel> productsViewModel;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>> GetProducts(string token)
    {
        var client = _clientFactory.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productsViewModel = await JsonSerializer
                               .DeserializeAsync<IEnumerable<ProductViewModel>>
                               (apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productsViewModel;
    }
    public async Task<ProductViewModel> GetProductById(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productViewModel = await JsonSerializer
                              .DeserializeAsync<ProductViewModel>
                              (apiResponse, _options);

            }
            else
            {
                return null;
            }
        }
        return productViewModel;
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel, string token)
    {
        var client = _clientFactory.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);

        var product = JsonSerializer.Serialize(productViewModel);
        StringContent content = new StringContent(product, Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productViewModel = await JsonSerializer
                             .DeserializeAsync<ProductViewModel>
                             (apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return productViewModel;
    }
    public async Task<bool> UpdateProduct(int id, ProductViewModel productViewModel, string token)
    {
        var client = _clientFactory.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.PutAsJsonAsync(apiEndpoint + id, productViewModel))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public async Task<bool> DeleteProduct(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductsApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
