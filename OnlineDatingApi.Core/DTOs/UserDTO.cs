using OnlineDatingApi.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OnlineDatingApi.Data.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public DateTime Dateofbirth { get; set; }
        [Required]
        public string? LookingFor { get; set; }
        [Required]
        public string Gender { get; set; } = string.Empty;

        public int Age => DateTime.Now.Year - Dateofbirth.Year;

        public DateTime CreatedAt => DateTime.UtcNow;


    }

    public class UserDTO

    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? LookingFor { get; set; }


        public DateTime Dateofbirth { get; set; } = DateTime.Now;

        public string Gender { get; set; } = string.Empty;

        public int Age => DateTime.Now.Year - Dateofbirth.Year;

        public ICollection<ImagesUrlDTO> UserImagesUrl { get; set; }


    }


}
