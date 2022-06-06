using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Data;
using OnlineDatingApi.Data.Data;
using OnlineDatingApi.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Core.Models
{
    public class CustomUserResult
    {
        public UserDTO ApiUser { get; set; }

        public IList<CustomImagesUrlDTO> ImagesUrl { get; set; }

        public IList<CustomInterestDTO> Interests { get; set; }

        public int matches{ get; set; }




    }
}
