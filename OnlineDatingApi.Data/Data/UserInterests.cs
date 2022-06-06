using OnlineDatingApi.Data.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineDatingApi.Data
{
    public class UserInterests
    {

        public Guid Id { get; set; }


        public string? Interest { get; set; }



        public string? ApiUserId { get; set; }

        [ForeignKey("ApiUserId")]
        public virtual ApiUser? ApiUser { get; set; }





    }
}
