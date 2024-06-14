using ShopWithMe.Models.Cart;
using ShopWithMe.Models.Products;

namespace ShopWithMe.CartTests
{
    public class CartTests
    {
        #region Can_Add_New_Lines()
        [Fact]
        public void Can_Add_New_Lines()
        {
            // Przygotowanie - utworzenie produktów testowych
            var p1 = new Product { Id = 1, Name = "P1" };
            var p2 = new Product { Id = 2, Name = "P2" };

            // Przygotowanie - utworzenie nowego koszyka
            var target = new Cart();

            // Dzia³anie
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            var results = target.Lines.ToArray();

            // Asercje
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }
        #endregion

        #region Can_Add_Quantity_For_Existing_Lines()
        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Przygotowanie - utworzenie produktów testowych
            var p1 = new Product { Id = 1, Name = "P1" };
            var p2 = new Product { Id = 2, Name = "P2" };

            // Przygotowanie - utworzenie nowego koszyka
            var target = new Cart();

            // Dzia³anie
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            var results = target.Lines.OrderBy(c => c.Product.Id).ToArray();

            // Asercje
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }
        #endregion

        #region Can_Remove_Line()
        [Fact]
        public void Can_Remove_Line()
        {
            // Przygotowanie - utworzenie produktów testowych
            var p1 = new Product { Id = 1, Name = "P1" };
            var p2 = new Product { Id = 2, Name = "P2" };
            var p3 = new Product { Id = 3, Name = "P3" };

            // Przygotowanie - utworzenie nowego koszyka
            var target = new Cart();

            // Przygotowanie - dodanie kilku produktów do koszyka
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Dzia³anie
            target.RemoveLine(p2);

            // Asercje
            Assert.Empty(target.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, target.Lines.Count);
        }
        #endregion

        #region Calculate_Cart_Total()
        [Fact]
        public void Calculate_Cart_Total()
        {
            // Przygotowanie - utworzenie produktów testowych
            var p1 = new Product { Id = 1, Name = "P1", Price = 100M };
            var p2 = new Product { Id = 2, Name = "P2", Price = 50M };

            // Przygotowanie - utworzenie nowego koszyka
            var target = new Cart();

            // Dzia³anie
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Asercje
            Assert.Equal(450M, result);
        }
        #endregion

        #region Can_Clear_Contents()
        [Fact]
        public void Can_Clear_Contents()
        {
            // Przygotowanie - utworzenie produktów testowych
            var p1 = new Product { Id = 1, Name = "P1", Price = 100M };
            var p2 = new Product { Id = 2, Name = "P2", Price = 50M };

            // Przygotowanie - utworzenie nowego koszyka
            var target = new Cart();

            // Przygotowanie - dodanie kilku produktów do koszyka
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Dzia³anie
            target.Clear();

            // Asercje
            Assert.Empty(target.Lines);
        }
        #endregion
    }
}