using System.ComponentModel.DataAnnotations;

namespace CategoriesMVC.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
    }
}
