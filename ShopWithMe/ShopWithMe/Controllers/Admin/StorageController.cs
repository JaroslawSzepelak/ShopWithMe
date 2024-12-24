using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.Storage;
using ShopWithMe.Models.Admin;
using ShopWithMe.Models.Storage;

namespace ShopWithMe.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    public class StorageController : ControllerBase
    {
        protected StorageManager _manager;
        protected IConfiguration _configuration;

        #region StorageController()
        public StorageController(StorageManager manager, IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
        }
        #endregion

        #region UploadFile()
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var path = _configuration.GetValue<string>("Storage:Path");

            DirectoryInfo di = Directory.CreateDirectory(path);

            var filePath = $"{path}/{file.FileName}";

            using var fileStream = System.IO.File.Create(filePath);

            stream.CopyTo(fileStream);
            
            var storageFile = new StorageFile()
            {
                Name = file.FileName,
                Path = filePath,
                ContentType = file.ContentType,
            };

            await _manager.CreateAsync(storageFile);

            return Ok(storageFile);
        }
        #endregion
    }
}
