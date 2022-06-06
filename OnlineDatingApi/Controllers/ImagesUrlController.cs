using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class ImagesUrlController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserClaims _userClaims;
        byte[]? imageBytes;
        CreateImagesUrlDTO imagesUrlDTO;

        public ImagesUrlController(IUnitofWork unitofWork, IMapper mapper, IWebHostEnvironment hostingEnvironment,IUserClaims userClaims)
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
            var images = await _unitofWork.UserImagesUrl.GetAll();
            var results = _mapper.Map<IList<ImagesUrlDTO>>(images);
            var test = _userClaims.GetMyName();
            Console.WriteLine(test);
            Console.WriteLine(_userClaims.GetMyEmail());
            Console.WriteLine(_userClaims.GetMyId());
            return Ok(results);
        }

        [HttpGet]
        [Route("all/users")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitofWork.ApiUsers.GetAll();
            var results = _mapper.Map<IList<UserDTO>>(users);
            return Ok(users);
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> images)
        {
            List<CreateImagesUrlDTO> primes = new List<CreateImagesUrlDTO>();

            if (images.Count == 0)
                return BadRequest();

            string directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", "Images");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var image in images)
            {
                long imageSize;
                string imagePath = Path.Combine(directoryPath, image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
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



        [HttpGet]
        [Route("user+id")]
        public async Task<IActionResult> GetAllImagesByASingleUser()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserImagesUrl.GetAll(expression: o => o.ApiUserId == id, includes: new List<string> { "ApiUser" });
            var results = _mapper.Map<IList<ImagesUrlDTO>>(users);

            return Ok(results);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetImageById()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserImagesUrl.GetAll(expression: o => o.ApiUserId == id);
            var results = _mapper.Map<IList<CreateImagesUrlDTO>>(users);
            return Ok(results);
        }

        [HttpGet]
        [Route("ordered/id")]
        public async Task<IActionResult> GetOrderedImagesById()
        {
            string id = _userClaims.GetMyId();
            var users = await _unitofWork.UserImagesUrl.GetAll(expression: o => o.ApiUserId == id, orderBy: s => s.OrderByDescending(o => o.CreatedAt), limit: 4);
            var results = _mapper.Map<IList<CreateImagesUrlDTO>>(users);
            return Ok(results);
        }

    }
}
