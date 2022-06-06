using Microsoft.AspNetCore.Http;
using OnlineDatingApi.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.DTOs
{
    public class RequiredImageUserInputDTO
    {
        public List<IFormFile> images { get; set; }

        public string? ApiUserId { get; set; }

       

    }

    public class CreateImagesUrlDTO:RequiredImageUserInputDTO
    {

        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }


        public long Size { get; set; }


        public string? ApiUserId { get; set; }

        public DateTime CreatedAt => DateTime.Now;


    }

    public class ImagesUrlDTO : CreateImagesUrlDTO
    {

        public UserDTO? ApiUser { get; set; }

    }

    public class CustomImagesUrlDTO
    {

        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }


        public long Size { get; set; }



    }
}
