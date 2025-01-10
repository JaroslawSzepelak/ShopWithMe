using System.ComponentModel.DataAnnotations;

namespace ShopWithMe.Models.ProductCategories.Admin
{
    public class ProductCategoryFormModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Nazwa kategorii jest wymagana.")]
        public string Name { get; set; }
    }
}
