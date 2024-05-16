using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Profiles
{
    public class CatgoryProfile : Profile
    {
        public CatgoryProfile() { 
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
