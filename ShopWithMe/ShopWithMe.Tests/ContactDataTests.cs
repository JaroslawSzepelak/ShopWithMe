using ShopWithMe.Models.ContactData;

namespace ShopWithMe.Tests
{
    public class ContactDataTests
    {
        #region Can_UpdateContactData()
        [Fact]
        public void Can_UpdateContactData()
        {
            // Przygotowanie - modelu formularza
            var formModel = new ContactDataFormModel()
            {
                Firstname = "Imię",
                Lastname = "Nazwisko",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Address = "Test 1",
                City = "Test",
                Zip = "00-000"
            };

            // Przygotowanie - utworzenie modelu danych
            var result = new ContactData();

            // Działanie
            result.UpdateContactData(formModel);

            // Asercje
            Assert.Equal(formModel.Firstname, result.Firstname);
            Assert.Equal(formModel.Lastname, result.Lastname);
            Assert.Equal(formModel.Email, result.Email);
            Assert.Equal(formModel.PhoneNumber, result.PhoneNumber);
            Assert.Equal(formModel.Address, result.Address);
            Assert.Equal(formModel.City, result.City);
            Assert.Equal(formModel.Zip, result.Zip);
        }
        #endregion

        #region Can_ClearContactData()
        [Fact]
        public void Can_ClearContactData()
        {
            // Przygotowanie - utworzenie modelu danych
            var result = new ContactData()
            {
                Firstname = "Imię",
                Lastname = "Nazwisko",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Address = "Test 1",
                City = "Test",
                Zip = "00-000"
            };

            // Działanie
            result.ClearContactData();

            // Asercje
            Assert.Null(result.Firstname);
            Assert.Null(result.Lastname);
            Assert.Null(result.Email);
            Assert.Null(result.PhoneNumber);
            Assert.Null(result.Address);
            Assert.Null(result.City);
            Assert.Null(result.Zip);
        }
        #endregion
    }
}
