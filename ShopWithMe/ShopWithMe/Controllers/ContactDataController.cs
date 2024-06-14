using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Models.ContactData;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactDataController : Controller
    {
        protected ContactData _contactData;

        #region ContactDataController()
        public ContactDataController(ContactData contactData)
        {
            _contactData = contactData;
        }
        #endregion

        #region Get()
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_contactData);
        }
        #endregion

        #region Update()
        [HttpPut]
        public IActionResult Update([FromBody] ContactDataFormModel formModel)
        {
            _contactData.UpdateContactData(formModel);
            return Ok(_contactData);
        }
        #endregion

        #region Delete()
        [HttpDelete]
        public IActionResult Delete()
        {
            _contactData.ClearContactData();
            return View();
        }
        #endregion
    }
}
