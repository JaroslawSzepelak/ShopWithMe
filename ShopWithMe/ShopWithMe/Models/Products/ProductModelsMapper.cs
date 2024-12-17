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
        }

        public void Map(Product mapFrom, PublicModel.ProductListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Lead = mapFrom.Lead;
            mapTo.Price = mapFrom.Price;
            mapTo.Category = mapFrom.Category?.Name;
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
    }
}
