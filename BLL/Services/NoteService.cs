using AutoMapper;
using BLL.DTOs;
using BLL.validators;
using DAL;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BLL.Services
{
    public class NoteService
    {
        #region Fields
        private readonly CategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string userId;
        private readonly NoteRepository noteRepository;
        
        #endregion

        #region Constructor
        public NoteService(NoteRepository noteRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, CategoryRepository categoryRepository)
        {
            this.noteRepository = noteRepository;
            categoryService = new CategoryService(categoryRepository, mapper);
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            this.userId = GetUserId();
        }
        #endregion

        #region Methods

        #region GetUserId
        public string GetUserId()
        {
            //return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            _httpContextAccessor.HttpContext?.Session.SetString("UserId", userId);

            return _httpContextAccessor.HttpContext?.Session.GetString("UserId") ?? string.Empty;
        }
        #endregion

        #region CreateNoteAsync
        public async Task<NoteDTO> CreateNoteAsync(NoteDTO noteDTO)
        {
            noteDTO.UserId = GetUserId();
            var validator = new NoteDTOValidator(noteRepository);
            var validationResult = await validator.ValidateAsync(noteDTO);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    noteDTO.errors.Add(error.ErrorMessage);
                }
                return noteDTO;
            }
            noteDTO.Priority = noteRepository.GetMaximumPriorityById(noteDTO.UserId).Result+1;
            
             
            var note = mapper.Map<Note>(noteDTO);
            try
            {
                await noteRepository.CreateAsync(note);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
            return mapper.Map<NoteDTO>(note);
        }
        #endregion

        #region GetNoteById
        public async Task<CreateNoteDTO> GetNoteByIdAsync(int id)
        {
            var userId = GetUserId();
            var note = new CreateNoteDTO();
            note.NoteDTO = mapper.Map<NoteDTO>(await noteRepository.ReadAsync(id));
            if( note.NoteDTO == null || !note.NoteDTO.UserId.Equals(userId))
            {
                return null;
            }

            note.Categories = await categoryService.GetAllCategorysAsync();

            return note;
        }
        #endregion

        #region GetAllNotes
        public async Task<List<NoteDTO>> GetAllNotesAsync()
        {
            var notes = await noteRepository.ReadAllAsync();
            notes = notes.Where(x => x.userId == GetUserId()).ToList();
            notes = sortPriority(notes);

            return mapper.Map<List<NoteDTO>>(notes);
        }
        #endregion

        #region UpdateNote
        public async Task<NoteDTO> UpdateNoteAsync(NoteDTO noteDto)
        {
            var userId = GetUserId();
            var note = await noteRepository.ReadAsync(noteDto.Id);
            if (note == null || !note.userId.Equals(userId))
            {
                return null;
            }
            if(noteDto.Priority > int.MaxValue - 1)
            {
                noteDto.Priority = noteRepository.GetMaximumPriorityById(GetUserId()).Result + 1;
            }
            mapper.Map(noteDto, note);
            await noteRepository.UpdateAsync(note);

            return mapper.Map<NoteDTO>(note);
        }
        #endregion

        #region DeleteNote
        public async Task<bool> DeleteNoteAsync(int id)
        {
            var userId = GetUserId();
            var note = await noteRepository.ReadAsync(id);
            if (note == null || !note.userId.Equals(userId))
            {
                return false;
            }

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
        public async Task<bool> MarkDoneAsync(int id)
        {
            var userId = GetUserId();
            var note = await noteRepository.ReadAsync(id);
            if (note == null || !note.userId.Equals(userId))
            {
                return false;
            }
            note.Status = "Done";
            note.Priority = int.MaxValue;
            await noteRepository.UpdateAsync(note);
            var notes = await noteRepository.ReadAllAsync();
            notes = notes.Where(x => x.userId == GetUserId()).ToList();
            await SynchronizePriority(noteRepository, notes);
            return true;
        }
        #endregion

        #region SetPriority
        public async Task<List<NoteDTO>> SetPriorityAsync(List<NoteDTO> notes)
        {
            var noteList = new List<Note>();
            foreach (var note in notes)
            {
                var noteEntity = await noteRepository.ReadAsync(note.Id);
                noteEntity.Priority = note.Priority;
                noteList.Add(noteEntity);
            }
            noteList = await SynchronizePriority(noteRepository, noteList);

            return mapper.Map<List<NoteDTO>>(noteList);
        }
        #endregion

        #region SynchronizePriority
        private static async Task<List<Note>> SynchronizePriority(DAL.Repositories.NoteRepository noteRepository, List<Note> noteList)
        {
            var current = 1;
            noteList = sortPriority(noteList);
            foreach (var note in noteList)
            {
                if (note.Priority >= int.MaxValue)
                {
                    break;
                }
                note.Priority = current;
                current++;
            }
            await noteRepository.UpdateRangeAsync(noteList);
            return noteList;
        }
        #endregion

        #region GetNotesBySearchString
        /*public async Task<NoteViewDTO> GetNotesBySearchStringAsync(string searchString, int[] filterOptions, int pageNumber, int pageSize)
        {
            searchString = searchString.IsNullOrEmpty() ? "" : searchString;
            searchString = searchString.ToLower();
            var notes = await noteRepository.ReadAllAsync();
            notes = notes.Where(n => n.userId == GetUserId() &&
                         (n.NoteTitle.ToLower().Contains(searchString) ||
                          n.NoteDescription.ToLower().Contains(searchString)))
             .ToList();
            notes = sortPriority(notes);
            var data = new NoteViewDTO();
            if(filterOptions != null && filterOptions.Length > 0)
            {
                notes = (from n in notes where filterOptions.Contains(n.categoryId) select n).ToList();
            }
            var totalItems = notes.Count;
            if((pageNumber - 1)*pageSize >= totalItems)
            {
                pageNumber--;
            }
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
            data.FilterOptions = filterOptions ?? [];

            return data;
        }*/
        public async Task<NoteViewDTO> GetNotesBySearchStringAsync(string searchString, int[] filterOptions, int pageNumber, int pageSize)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString.ToLower();

            var notes = await GetFilteredNotesAsync(searchString);
            notes = FilterByUser(notes, GetUserId());
            notes = FilterBySearchString(notes, searchString);
            notes = sortPriority(notes);

            if (filterOptions != null && filterOptions.Length > 0)
            {
                notes = FilterByCategory(notes, filterOptions);
            }

            var pagedNotes = GetPagedNotes(notes, pageNumber, pageSize, out int totalItems);

            var notesDto = mapper.Map<List<NoteDTO>>(pagedNotes);
            var categories = await GetCategoriesAsync();

            return new NoteViewDTO
            {
                Notes = new PagedResult<NoteDTO>
                {
                    Items = notesDto,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                },
                Categories = mapper.Map<List<CategoryDTO>>(categories),
                SearchString = searchString,
                FilterOptions = filterOptions ?? Array.Empty<int>()
            };
        }

        private async Task<List<Note>> GetFilteredNotesAsync(string searchString)
        {
            return await noteRepository.ReadAllAsync();
        }

        private List<Note> FilterByUser(List<Note> notes, string userId)
        {
            return notes.Where(n => n.userId == userId).ToList();
        }

        private List<Note> FilterBySearchString(List<Note> notes, string searchString)
        {
            return notes.Where(n => n.NoteTitle.ToLower().Contains(searchString) ||
                                    n.NoteDescription.ToLower().Contains(searchString)).ToList();
        }

        private List<Note> FilterByCategory(List<Note> notes, int[] filterOptions)
        {
            return notes.Where(n => filterOptions.Contains(n.categoryId)).ToList();
        }

        private List<Note> GetPagedNotes(List<Note> notes, int pageNumber, int pageSize, out int totalItems)
        {
            totalItems = notes.Count;
            if ((pageNumber - 1) * pageSize >= totalItems)
            {
                pageNumber--;
            }

            return notes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        private async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            return await categoryService.GetAllCategorysAsync();
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
