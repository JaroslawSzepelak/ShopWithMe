using ShopWithMe.Models.ProductCategories.Admin;
using ShopWithMe.Models.ProductCategories.Public;
using ShopWithMe.Models.Products;
using AdminModel = ShopWithMe.Models.ProductCategories.Admin;
using PublicModel = ShopWithMe.Models.ProductCategories.Public;

namespace ShopWithMe.Models.ProductCategories
{
    public class ProductCategoryModelsMapper
    {
        protected ProductModelsMapper _mapper;

        #region ProductCategoryModelsMapper()
        public ProductCategoryModelsMapper(ProductModelsMapper mapper)
        {
            _mapper = mapper;
        }
        #endregion

        public void Map(AdminModel.ProductCategoryFormModel mapFrom, ProductCategory mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
        }

        public void Map(ProductCategory mapFrom, AdminModel.ProductCategoryFormModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
        }

        public void Map(ProductCategory mapFrom, AdminModel.ProductCategoryListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
        }

        public void Map(ProductCategory mapFrom, PublicModel.ProductCategoryDetailsModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
        }

        public void Map(ProductCategory mapFrom, PublicModel.ProductCategoryListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Name = mapFrom.Name;
            mapTo.Products = _mapper.MapToPublicListModel(mapFrom.Products.ToList());
        }

        public ProductCategoryFormModel MapToFormModel(ProductCategory mapFromList)
        {
            var mapTo = new ProductCategoryFormModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public List<AdminModel.ProductCategoryListModel> MapToAdminListModel(List<ProductCategory> mapFromList)
        {
            var results = new List<AdminModel.ProductCategoryListModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new AdminModel.ProductCategoryListModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }

        public ProductCategoryDetailsModel MapToDetailsModel(ProductCategory mapFromList)
        {
            var mapTo = new ProductCategoryDetailsModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public List<PublicModel.ProductCategoryListModel> MapToPublicListModel(List<ProductCategory> mapFromList)
        {
            var results = new List<PublicModel.ProductCategoryListModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new PublicModel.ProductCategoryListModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }
    }
}
