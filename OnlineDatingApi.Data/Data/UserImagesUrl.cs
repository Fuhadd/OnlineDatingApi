using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Data.Data
{
    public class UserImagesUrl
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public long Size { get; set; }



        public string ApiUserId { get; set; }

        [ForeignKey("ApiUserId")]
        public virtual ApiUser? ApiUser { get; set; }

    }
}
