namespace ShopWithMe.Models.Products.Admin
{
    public class ProductFormModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Lead { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public long? CategoryId { get; set; }
    }
}
