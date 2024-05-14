using CategoriesMVC.Models;
using System.Text;
using System.Text.Json;

namespace CategoriesMVC.Services
{
    public class CategoryService : ICategoryService
    {
        private const string apiEndpoint = "/categories/";
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private CategoryViewModel categoryViewModel;
        private IEnumerable<CategoryViewModel> categoriesViewModel;

        public CategoryService(IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            // Create HttpClient using HttClientFactory
            var client = _clientFactory.CreateClient("CategoriesApi");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoriesViewModel = await JsonSerializer
                        .DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return categoriesViewModel;
        }

        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            // Create HTTP Client
            var client = _clientFactory.CreateClient("CategoriesApi");

            // Call GetAsync and check if it was successful
            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoryViewModel = await JsonSerializer
                        .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return categoryViewModel;
        }

        public async Task<CategoryViewModel> CreateCategory(CategoryViewModel categoryViewModel)
        {
            // Creates HTTP Client
            var client = _clientFactory.CreateClient("CategoriesApi");

            // Serializes to JSON
            var category = JsonSerializer.Serialize(categoryViewModel);
            // Creates HTTP Content
            StringContent content = new StringContent(category, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoryViewModel = await JsonSerializer
                        .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return categoryViewModel;
        }
        public async Task<bool> UpdateCategory(int id, CategoryViewModel categoryViewModel)
        {
            var client = _clientFactory.CreateClient("CategoriesApi");
            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, categoryViewModel))
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

        public async Task<bool> DeleteCategory(int id)
        {
            var client = _clientFactory.CreateClient("CategoriesApi");
            using (var response = await client.DeleteAsync(apiEndpoint + id))
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

    }
}
