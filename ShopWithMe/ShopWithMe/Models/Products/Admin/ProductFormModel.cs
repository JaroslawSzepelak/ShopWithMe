using ShopWithMe.Models.Storage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopWithMe.Models.Products.Admin
{
    public class ProductFormModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Nazwa kategorii jest wymagana.")]
        public string Name { get; set; }
        public string Lead { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Cena jest wymagana.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proszę podać dodatnią cenę.")]
        [RegularExpression("[0-9]?[0-9]?[0-9?]?[0-9]?[0-9]?[0-9]((,|.)[0-9][0-9]?)?", ErrorMessage = "Podano nieprawidłową cenę.")]
        public decimal Price { get; set; }
        public long? CategoryId { get; set; }
        public string TechnicalData  { get; set; }
        public long? MainImageId { get; set; }
        [Range(0.00, double.MaxValue, ErrorMessage = "Proszę podać prawidłową cenę.")]
        [RegularExpression("[0-9]?[0-9]?[0-9?]?[0-9]?[0-9]?[0-9]((,|.)[0-9][0-9]?)?", ErrorMessage = "Podano nieprawidłową cenę.")]
        public decimal? SalePrice { get; set; }
        public DateTime? DateSaleFrom { get; set; }
        public DateTime? DateSaleTo { get; set; }
        public bool IsSaleOn { get; set; }

        public StorageFile MainImage { get; set; }
        public List<StorageFile> Images { get; set; }
    }
}
