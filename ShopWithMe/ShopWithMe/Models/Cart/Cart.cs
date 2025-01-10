using ShopWithMe.Models.Products;

namespace ShopWithMe.Models.Cart
{
    public class Cart
    {
        #region Lines
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        #endregion

        #region AddItem()
        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = Lines
                .Where(p => p.Product.Id == product.Id)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                    Price = product.SalePrice != 0 ? product.SalePrice.Value : product.Price
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        #endregion

        #region AddItem()
        public virtual void UpdateItem(CartLine line, int? quantity)
        {
            if (quantity.HasValue)
            {
                line.Quantity = quantity.Value;
            }
        }
        #endregion

        #region RemoveLine()
        public virtual void RemoveLine(Product product) => Lines.RemoveAll(l => l.Product.Id == product.Id);
        #endregion

        #region ComputeTotalValue()
        public decimal ComputeTotalValue() => Lines.Sum(e => e.Product.Price * e.Quantity);
        #endregion

        #region Clear()
        public virtual void Clear() => Lines.Clear();
        #endregion
    }
}
