﻿using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly CategoryService categoryService;
        private readonly IMapper mapper;

        public NoteService(DataAccessFactory dataAccessFactory, IMapper mapper)
        {
            this.dataAccessFactory = dataAccessFactory;
            categoryService = new CategoryService(dataAccessFactory, mapper);
            this.mapper = mapper;
        }

        public async Task<NoteDTO> CreateNoteAsync(NoteDTO noteDTO)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            noteDTO.Priority = noteRepository.GetMaximumPriority().Result+1;
            var note = mapper.Map<Note>(noteDTO);   
            await noteRepository.CreateAsync(note);
            return mapper.Map<NoteDTO>(note);
        }

        public async Task<CreateNoteDTO> GetNoteByIdAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            var note = new CreateNoteDTO();
            note.NoteDTO = mapper.Map<NoteDTO>(await noteRepository.ReadAsync(id));
            note.Categories = await categoryService.GetAllCategorysAsync();
            return note;
        }

        public async Task<List<NoteDTO>> GetAllNotesAsync()
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            var notes = await noteRepository.ReadAllAsync();
            notes = notes.OrderByDescending(n => n.Priority).Reverse().ToList();
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
            if(noteDto.Priority > int.MaxValue - 1)
            {
                noteDto.Priority = int.MaxValue - 1;
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

        public async Task<CreateNoteDTO> CreateNoteViewAsync()
        {
            var data = new CreateNoteDTO();
            /*var noteEmpty = new NoteDTO { CategoryId = 1, DueDate = DateTime.MaxValue, NoteTitle = "", NoteDescription = "", Priority = 1000, Status = "ongoing", UserId = 1 };
            */
            var Categories = await categoryService.GetAllCategorysAsync();
            data.Categories = Categories;
            return data;
        }

        public async Task MarkDoneAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var note = await noteRepository.ReadAsync(id);
            note.Status = "Done";
            note.Priority = int.MaxValue;
            await noteRepository.UpdateAsync(note);
        }

        public async Task<List<NoteDTO>> SetPriorityAsync(List<NoteDTO> notes)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var noteList = new List<Note>();
            foreach (var note in notes)
            {
                var noteEntity = await noteRepository.ReadAsync(note.Id);
                noteEntity.Priority = note.Priority;
                noteList.Add(noteEntity);
            }
            await noteRepository.UpdateRangeAsync(noteList);
            return mapper.Map<List<NoteDTO>>(noteList);
        }


        
    }
}
