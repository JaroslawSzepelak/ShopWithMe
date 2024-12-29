using AdminModel = ShopWithMe.Models.Products.Admin;
using PublicModel = ShopWithMe.Models.Products.Public;

namespace ShopWithMe.Models.Products
{
    public class ProductModelsMapper
    {
        public void Map(AdminModel.ProductFormModel mapFrom, Product mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Lead = mapFrom.Lead;
            mapTo.Description = mapFrom.Description;
            mapTo.Price = mapFrom.Price;
            mapTo.CategoryId = mapFrom.CategoryId;
            mapTo.TechnicalData = mapFrom.TechnicalData;
            mapTo.MainImageId = mapFrom.MainImageId;
            mapTo.SalePrice = mapFrom.SalePrice;
            mapTo.DateSaleFrom = mapFrom.DateSaleFrom;
            mapTo.DateSaleTo = mapFrom.DateSaleTo;
            mapTo.IsSaleOn = mapFrom.IsSaleOn;
        }

        public void Map(Product mapFrom, AdminModel.ProductFormModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Lead = mapFrom.Lead;
            mapTo.Description = mapFrom.Description;
            mapTo.Price = mapFrom.Price;
            mapTo.CategoryId = mapFrom.CategoryId;
            mapTo.TechnicalData = mapFrom.TechnicalData;
            mapTo.MainImageId = mapFrom.MainImageId;
            mapTo.MainImage = mapFrom.MainImage;
            mapTo.Images = mapFrom.Images.Select(i => i.StorageFile).ToList();
            mapTo.SalePrice = mapFrom.SalePrice;
            mapTo.DateSaleFrom = mapFrom.DateSaleFrom;
            mapTo.DateSaleTo = mapFrom.DateSaleTo;
            mapTo.IsSaleOn = mapFrom.IsSaleOn;
        }

        public void Map(Product mapFrom, AdminModel.ProductListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Price = mapFrom.Price;
            mapTo.Category = mapFrom.Category?.Name;
        }

        public void Map(Product mapFrom, AdminModel.ProductAutocompleteModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
        }

        public void Map(Product mapFrom, PublicModel.ProductDetailsModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Lead = mapFrom.Lead;
            mapTo.Description = mapFrom.Description;
            mapTo.Price = mapFrom.Price;
            mapTo.Category = mapFrom.Category?.Name;
            mapTo.TechnicalData = mapFrom.TechnicalData;
            mapTo.MainImage = mapFrom.MainImage;
            mapTo.Images = mapFrom.Images.Select(i => i.StorageFile).ToList();

            if (mapFrom.IsSaleOn &&
                (!mapFrom.DateSaleFrom.HasValue || mapFrom.DateSaleFrom.Value <= DateTime.Now) &&
                (!mapFrom.DateSaleTo.HasValue || mapFrom.DateSaleTo.Value > DateTime.Now))
            {
                mapTo.SalePrice = mapFrom.SalePrice;
            }
        }

        public void Map(Product mapFrom, PublicModel.ProductListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Lead = mapFrom.Lead;
            mapTo.Price = mapFrom.Price;
            mapTo.Category = mapFrom.Category?.Name;
            mapTo.MainImage = mapFrom.MainImage;

            if (mapFrom.IsSaleOn && 
                (!mapFrom.DateSaleFrom.HasValue || mapFrom.DateSaleFrom.Value <= DateTime.Now) &&
                (!mapFrom.DateSaleTo.HasValue || mapFrom.DateSaleTo.Value > DateTime.Now))
            {
                mapTo.SalePrice = mapFrom.SalePrice;
            }
        }

        public void Map(Product mapFrom, PublicModel.ProductAutocompleteModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
        }

        public AdminModel.ProductFormModel MapToFormModel(Product mapFromList)
        {
            var mapTo = new AdminModel.ProductFormModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public List<AdminModel.ProductListModel> MapToAdminListModel(List<Product> mapFromList)
        {
            var results = new List<AdminModel.ProductListModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new AdminModel.ProductListModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }

        public List<AdminModel.ProductAutocompleteModel> MapToAdminAutocompleteModel(List<Product> mapFromList)
        {
            var results = new List<AdminModel.ProductAutocompleteModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new AdminModel.ProductAutocompleteModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }

        public PublicModel.ProductDetailsModel MapToDetailsModel(Product mapFromList)
        {
            var mapTo = new PublicModel.ProductDetailsModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public List<PublicModel.ProductListModel> MapToPublicListModel(List<Product> mapFromList)
        {
            var results = new List<PublicModel.ProductListModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new PublicModel.ProductListModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }

        public List<PublicModel.ProductAutocompleteModel> MapToPublicAutocompleteModel(List<Product> mapFromList)
        {
            var results = new List<PublicModel.ProductAutocompleteModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new PublicModel.ProductAutocompleteModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }
    }
}
