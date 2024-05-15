using CategoriesMVC.Models;

namespace CategoriesMVC.Services
{
    public interface IAuthentication
    {
        Task<TokenViewModel> AuthenticateUser(UserViewModel userViewModel);
    }
}
