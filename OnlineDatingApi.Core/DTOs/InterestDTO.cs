using OnlineDatingApi.Data.DTOs;

namespace OnlineDatingApi.Core.DTOs
{
    public class CreateInterestDTO
    {
        public Guid Id { get; set; }
        public string Interest { get; set; } = string.Empty;
        public string ApiUserId { get; set; }


    }

    public class InterestDTO : CreateInterestDTO
    {

        public UserDTO? ApiUser { get; set; }


    }
    public class CustomInterestDTO
    {

        public Guid Id { get; set; }
        public string Interest { get; set; } = string.Empty;


    }
}
