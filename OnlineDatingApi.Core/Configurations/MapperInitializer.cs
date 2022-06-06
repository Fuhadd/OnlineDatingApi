using AutoMapper;
using OnlineDatingApi.Core.DTOs;
using OnlineDatingApi.Data.Data;
using OnlineDatingApi.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDatingApi.Data.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<ApiUser, CreateUserDTO>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
            CreateMap<UserImages, ImagesDTO>().ReverseMap();
            CreateMap<UserImages, CreateImagesDTO>().ReverseMap();
            CreateMap<UserImagesUrl, ImagesUrlDTO>().ReverseMap();
            CreateMap<UserImagesUrl, CustomImagesUrlDTO>().ReverseMap();
            CreateMap<UserImagesUrl, CreateImagesUrlDTO>().ReverseMap();
            CreateMap<UserInterests, InterestDTO>().ReverseMap();
            CreateMap<UserInterests, CustomInterestDTO>().ReverseMap();
            CreateMap<UserInterests, CreateInterestDTO>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenDTO>().ReverseMap();

        }
        
    }
}
