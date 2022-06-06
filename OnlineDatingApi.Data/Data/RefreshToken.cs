﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Data.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string? Token { get; set; }

        public string? JwtId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string? ApiUserId { get; set; }

        [ForeignKey("ApiUserId")]
        public virtual ApiUser? ApiUser { get; set; }



    }
}
