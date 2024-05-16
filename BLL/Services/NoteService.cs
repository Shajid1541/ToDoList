using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NoteService
    {
        private readonly DataAccessFactory dataAccessFactory;
        private readonly IMapper mapper;

        public NoteService(DataAccessFactory dataAccessFactory, IMapper mapper)
        {
            this.dataAccessFactory = dataAccessFactory;
            this.mapper = mapper;
        }

        public async Task<NoteDTO> CreateNoteAsync(NoteDTO noteDTO)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var note = mapper.Map<Note>(noteDTO);
            await noteRepository.CreateAsync(note);
            return mapper.Map<NoteDTO>(note);
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            var note = await noteRepository.ReadAsync(id);
            return mapper.Map<NoteDTO>(note);
        }

        public async Task<List<NoteDTO>> GetAllNotesAsync()
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            var notes = await noteRepository.ReadAllAsync();
            return mapper.Map<List<NoteDTO>>(notes);
        }

        public async Task<NoteDTO> UpdateNoteAsync(NoteDTO noteDto)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            var note = await noteRepository.ReadAsync(noteDto.Id);
            if (note == null)
            {
                return null;
            }

            mapper.Map(noteDto, note);
            await noteRepository.UpdateAsync(note);
            return mapper.Map<NoteDTO>(note);
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            return await noteRepository.DeleteAsync(id);
        }
    }
}
