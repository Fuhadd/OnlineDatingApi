using OnlineDatingApi.Data.Data;
using OnlineDatingApi.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.DTOs
{
    public class CreateImagesDTO
    {

        public Guid Id { get; set; }
        public byte[]? ImageBinary { get; set; }

        
        public int Size { get; set; }


        public string? ApiUserId { get; set; }

        public DateTime CreatedAt => DateTime.Now;


    }

    public class ImagesDTO : CreateImagesDTO
    {

        public UserDTO? ApiUser1 { get; set; }


    }
}
