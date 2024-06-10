using AutoMapper;
using BLL.DTOs;
using DAL.Models;

namespace BLL.Profiles
{
    public class NoteProfile : Profile
    {
        #region Constructor
        public NoteProfile() {
            CreateMap<Note, NoteDTO>().ReverseMap();
        }
        #endregion
    }
}
