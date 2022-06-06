using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Data.Data
{
    public class UserImages
    {

        public Guid Id { get; set; }
        public byte[]? ImageBinary { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Size { get; set; }



        public string ApiUserId { get; set; }

        [ForeignKey("ApiUserId")]
        public virtual ApiUser? ApiUser { get; set; }



    }
}
