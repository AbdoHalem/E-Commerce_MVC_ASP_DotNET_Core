using System.ComponentModel.DataAnnotations;

namespace ECommerce_WebSite.Areas.Admin.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        [Display(Name = "Parent Category (Optional)")]
        public int? ParentCategoryId { get; set; }
    }
}
