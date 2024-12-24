using Microsoft.AspNetCore.Mvc;
using ShopWithMe.Managers.Storage;

namespace ShopWithMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        #region Get()
        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get(string fileName)
        {
            var storageFile = await _manager.GetAsync(fileName);

            if (storageFile == null)
                return NotFound();

            using var fileStream = System.IO.File.OpenRead(storageFile.Path);

            using var reader = new MemoryStream();

            fileStream.CopyTo(reader);

            return File(reader.ToArray(), storageFile.ContentType, storageFile.Name);
        }
        #endregion
    }
}
