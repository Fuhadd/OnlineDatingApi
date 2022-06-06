using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Core.Repositories;
using OnlineDatingApi.Core.Services.UserClaims;
using OnlineDatingApi.Data.Data;
using OnlineDatingApi.Data.DTOs;

namespace OnlineDatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    
    public class ImagesBinaryController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserClaims _userClaims;
        byte[]? imageBytes;
        CreateImagesDTO imagesDTO;

        public ImagesBinaryController(IUnitofWork unitofWork, IMapper mapper,IWebHostEnvironment hostingEnvironment,IUserClaims userClaims)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _userClaims = userClaims;
        }

        [HttpGet]
        [Authorize]
        
        public async Task<IActionResult> GetImages()
        {
            var images = await _unitofWork.UserImages.GetAll();
            var results = _mapper.Map<IList<ImagesDTO>>(images);
            return Ok(results);
        }

        [HttpGet]
        [Route("all/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitofWork.ApiUsers.GetAll(includes: new List<string> { "ImagesDTO" });
            var results = _mapper.Map<IList<UserDTO>>(users);
            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> images)
        {
            List< CreateImagesUrlDTO> primes = new List<CreateImagesUrlDTO>();

            if (images.Count == 0)
                return BadRequest();

            string directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Images");
            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var image in images)
            {
                long imageSize;
                string imagePath = Path.Combine(directoryPath, image.FileName);
                using(var stream = new FileStream(imagePath, FileMode.Create))
                {

                   await image.CopyToAsync(stream);
                   imageSize = stream.Length;


                }
                

                CreateImagesUrlDTO userImage = new CreateImagesUrlDTO();
                userImage.Id = Guid.NewGuid();
                userImage.ImageUrl = imagePath;
                userImage.Size = imageSize;
                userImage.ApiUserId = _userClaims.GetMyId(); ;

                var finalUserImage = _mapper.Map<UserImagesUrl>(userImage);
                await _unitofWork.UserImagesUrl.Create(finalUserImage);
                await _unitofWork.Save();

                primes.Add(userImage);
            }

            return Ok(primes);


        }

        [HttpPost]
        [Authorize]
        [Route("binary")]
        public async Task<IActionResult> UploadImageToBiary(List<IFormFile> images)
        {
            List<CreateImagesDTO> primes = new List<CreateImagesDTO>();

            if (images.Count == 0)
                return BadRequest();

            

            string directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Images");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var image in images)
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();

                }
                
                CreateImagesDTO userImage = new CreateImagesDTO();
                userImage.Id = Guid.NewGuid();
                userImage.ImageBinary = imageBytes;
                userImage.Size = imageBytes.Length;
                userImage.ApiUserId = _userClaims.GetMyId(); ;

                primes.Add(userImage);
            }

            return Ok(primes);


        }

        [HttpGet]
        [Route("user+id")]
        public async Task<IActionResult> GetAllImagesByASingleUser()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserImages.GetAll(expression:o => o.ApiUserId == id,includes: new List<string> { "ApiUser" });
            var results = _mapper.Map<IList<ImagesDTO>>(users);
            return Ok(results);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetImageById()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserImages.GetAll(expression: o => o.ApiUserId == id);
            var results = _mapper.Map<IList<CreateImagesDTO>>(users);
            return Ok(results);
        }

        [HttpGet]
        [Route("ordered/{id}")]
        public async Task<IActionResult> GetOrderedImagesById()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserImages.GetAll(expression: o => o.ApiUserId == id, orderBy: s => s.OrderByDescending(o => o.CreatedAt),limit:4);
            var results = _mapper.Map<IList<CreateImagesDTO>>(users);
            return Ok(results);
        }

    }
}
