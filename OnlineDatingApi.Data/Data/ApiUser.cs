using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Data.Data
{
    public class ApiUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime Dateofbirth { get; set; } 

        public string? Gender { get; set; }

        public string? LookingFor { get; set; }

        public int Age { get; set; }

        public DateTime CreatedAt { get; set; }


        public virtual ICollection<UserImages>? UserImages { get; set; }
        public virtual ICollection<UserImagesUrl>? UserImagesUrl { get; set; }

        public virtual ICollection<UserInterests>? UserInterests { get; set; }


    }
}
