using AdminModel = ShopWithMe.Models.Orders.Admin;
using PublicModel = ShopWithMe.Models.Orders.Public;

namespace ShopWithMe.Models.Orders
{
    public class OrderModelsMapper
    {
        public void Map(AdminModel.OrderFormModel mapFrom, Order mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Firstname = mapFrom.Firstname;
            mapTo.Lastname = mapFrom.Lastname;
            mapTo.Email = mapFrom.Email;
            mapTo.PhoneNumber = mapFrom.PhoneNumber;
            mapTo.Address = mapFrom.Address;
            mapTo.City = mapFrom.City;
            mapTo.Zip = mapFrom.Zip;
        }

        public void Map(Order mapFrom, AdminModel.OrderFormModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Firstname = mapFrom.Firstname;
            mapTo.Lastname = mapFrom.Lastname;
            mapTo.Email = mapFrom.Email;
            mapTo.PhoneNumber = mapFrom.PhoneNumber;
            mapTo.Address = mapFrom.Address;
            mapTo.City = mapFrom.City;
            mapTo.Zip = mapFrom.Zip;
        }

        public void Map(Order mapFrom, AdminModel.OrderDetailsModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Lines = mapFrom.Lines;
            mapTo.Firstname = mapFrom.Firstname;
            mapTo.Lastname = mapFrom.Lastname;
            mapTo.Email = mapFrom.Email;
            mapTo.PhoneNumber = mapFrom.PhoneNumber;
            mapTo.Address = mapFrom.Address;
            mapTo.City = mapFrom.City;
            mapTo.Zip = mapFrom.Zip;
            mapTo.DateCreated = mapFrom.DateCreated;
            mapTo.UserId = mapFrom.UserId;
        }

        public void Map(Order mapFrom, AdminModel.OrderListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Firstname = mapFrom.Firstname;
            mapTo.Lastname = mapFrom.Lastname;
            mapTo.Email = mapFrom.Email;
            mapTo.DateCreated = mapFrom.DateCreated;
        }

        public void Map(Order mapFrom, PublicModel.OrderDetailsModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Lines = mapFrom.Lines;
            mapTo.Firstname = mapFrom.Firstname;
            mapTo.Lastname = mapFrom.Lastname;
            mapTo.Email = mapFrom.Email;
            mapTo.PhoneNumber = mapFrom.PhoneNumber;
            mapTo.Address = mapFrom.Address;
            mapTo.City = mapFrom.City;
            mapTo.Zip = mapFrom.Zip;
        }

        public void Map(Order mapFrom, PublicModel.OrderListModel mapTo)
        {
            mapTo.Id = mapFrom.Id;
            mapTo.Firstname = mapFrom.Firstname;
            mapTo.Lastname = mapFrom.Lastname;
            mapTo.Email = mapFrom.Email;
        }

        public AdminModel.OrderFormModel MapToFormModel(Order mapFromList)
        {
            var mapTo = new AdminModel.OrderFormModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public List<AdminModel.OrderListModel> MapToAdminListModel(List<Order> mapFromList)
        {
            var results = new List<AdminModel.OrderListModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new AdminModel.OrderListModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }

        public AdminModel.OrderDetailsModel MapToAdminDetailsModel(Order mapFromList)
        {
            var mapTo = new AdminModel.OrderDetailsModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public PublicModel.OrderDetailsModel MapToPublicDetailsModel(Order mapFromList)
        {
            var mapTo = new PublicModel.OrderDetailsModel();
            Map(mapFromList, mapTo);

            return mapTo;
        }

        public List<PublicModel.OrderListModel> MapToPublicListModel(List<Order> mapFromList)
        {
            var results = new List<PublicModel.OrderListModel>();

            foreach (var mapFrom in mapFromList)
            {
                var mapTo = new PublicModel.OrderListModel();

                Map(mapFrom, mapTo);

                results.Add(mapTo);
            }

            return results;
        }
    }
}
