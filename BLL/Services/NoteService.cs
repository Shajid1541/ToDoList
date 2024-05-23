using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class NoteService
    {
        #region Fields
        private readonly DataAccessFactory dataAccessFactory;
        private readonly CategoryService categoryService;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public NoteService(DataAccessFactory dataAccessFactory, IMapper mapper)
        {
            this.dataAccessFactory = dataAccessFactory;
            categoryService = new CategoryService(dataAccessFactory, mapper);
            this.mapper = mapper;
        }
        #endregion

        #region Methods

        #region Create
        public async Task<NoteDTO> CreateNoteAsync(NoteDTO noteDTO)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            noteDTO.Priority = noteRepository.GetMaximumPriority().Result+1;
            var note = mapper.Map<Note>(noteDTO);   
            await noteRepository.CreateAsync(note);

            return mapper.Map<NoteDTO>(note);
        }
        #endregion

        #region GetNoteById
        public async Task<CreateNoteDTO> GetNoteByIdAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var note = new CreateNoteDTO();
            note.NoteDTO = mapper.Map<NoteDTO>(await noteRepository.ReadAsync(id));
            note.Categories = await categoryService.GetAllCategorysAsync();

            return note;
        }
        #endregion

        #region GetAllNotes
        public async Task<List<NoteDTO>> GetAllNotesAsync()
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var notes = await noteRepository.ReadAllAsync();
            notes = sortPriority(notes);

            return mapper.Map<List<NoteDTO>>(notes);
        }
        #endregion

        #region UpdateNote
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
                noteDto.Priority = dataAccessFactory.CreateNoteData().GetMaximumPriority().Result + 1;
            }
            mapper.Map(noteDto, note);
            await noteRepository.UpdateAsync(note);

            return mapper.Map<NoteDTO>(note);
        }
        #endregion

        #region DeleteNote
        public async Task<bool> DeleteNoteAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();

            return await noteRepository.DeleteAsync(id);
        }
        #endregion

        #region CreateNoteView
        public async Task<CreateNoteDTO> CreateNoteViewAsync()
        {
            var data = new CreateNoteDTO();
            var Categories = await categoryService.GetAllCategorysAsync();
            data.Categories = Categories;

            return data;
        }
        #endregion

        #region MarkDone
        public async Task MarkDoneAsync(int id)
        {
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var note = await noteRepository.ReadAsync(id);
            note.Status = "Done";
            note.Priority = int.MaxValue;
            await noteRepository.UpdateAsync(note);
        }
        #endregion

        #region SetPriority
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
        #endregion

        #region GetNotesBySearchString
        public async Task<NoteViewDTO> GetNotesBySearchStringAsync(string searchString, int[] filterOptions, int pageNumber, int pageSize)
        {
            searchString = searchString.IsNullOrEmpty() ? "" : searchString;
            using var noteRepository = dataAccessFactory.CreateNoteData();
            var notes = await noteRepository.ReadAllAsync();
            notes = notes.Where(n => n.NoteTitle.Contains(searchString) || n.NoteDescription.Contains(searchString)).ToList();
            notes = sortPriority(notes);
            var data = new NoteViewDTO();
            if(filterOptions != null && filterOptions.Length > 0)
            {
                notes = (from n in notes where filterOptions.Contains(n.categoryId) select n).ToList();
            }
            var totalItems = notes.Count;
            var notesDto = mapper.Map<List<NoteDTO>>(notes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            
            data.Notes = new PagedResult<NoteDTO>
            {
                Items = notesDto,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            var categories = await categoryService.GetAllCategorysAsync();
            
            data.Categories = mapper.Map<List<CategoryDTO>>(categories);
            data.SearchString = searchString;
            data.FilterOptions = filterOptions;

            return data;
        }
        #endregion

        #region SortPriority
        private static List<Note> sortPriority(List<Note> notes)
        {
            notes = notes.OrderByDescending(n => n.Priority).Reverse().ToList();

            return notes;
        }
        #endregion
        #endregion
    }
}
