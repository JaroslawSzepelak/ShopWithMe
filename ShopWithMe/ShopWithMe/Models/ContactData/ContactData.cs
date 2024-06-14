namespace ShopWithMe.Models.ContactData
{
    public class ContactData
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Addrress { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }

        #region UpdateContactData()
        public virtual void UpdateContactData(ContactDataFormModel formModel)
        {
            Firstname = formModel.Firstname;
            Lastname = formModel.Lastname;
            Email = formModel.Email;
            PhoneNumber = formModel.PhoneNumber;
            Addrress = formModel.Adrdress;
            City = formModel.City;
            Zip = formModel.Zip;
        }
        #endregion

        #region ClearContactData()
        public virtual void ClearContactData()
        {
            Firstname = null;
            Lastname = null;
            Email = null;
            PhoneNumber = null;
            Addrress = null;
            City = null;
            Zip = null;
        }
        #endregion
    }
}
