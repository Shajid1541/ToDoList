using AutoMapper;
using BLL.DTOs;
using DAL.Models;

namespace BLL.Profiles
{
    public class CatgoryProfile : Profile
    {
        #region Constructor
        public CatgoryProfile() { 
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
        #endregion
    }
}
