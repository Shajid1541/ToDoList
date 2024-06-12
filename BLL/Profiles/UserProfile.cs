using AutoMapper;
using BLL.DTOs;
using DAL.Models;

namespace BLL.Profiles
{
    public class UserProfile : Profile
    {
        #region Constructor
        public UserProfile() {
            CreateMap<User, UserDTO>().ReverseMap();
        }
        #endregion
    }
}
