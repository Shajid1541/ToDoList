using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.IO;

namespace ToDoList.Controllers
{
    public class NoteController : Controller
    {
        #region Fields
        private readonly NoteService noteService;
        #endregion

        #region Constructor
        public NoteController(NoteService noteService)
        {
            this.noteService = noteService;
        }
        #endregion

        #region Methods

        #region Index
        public async Task<IActionResult> Index(int[] filterOptions, string searchString = "", int pageNumber = 1, int pageSize = 3)
        {
            var data = await noteService.GetNotesBySearchStringAsync(searchString, filterOptions, pageNumber, pageSize);

            return View(data);
        }
        #endregion

        #region Create GET
        public async Task<IActionResult> Create()
        {
            var data = await noteService.CreateNoteViewAsync();

            return View(data);
        }
        #endregion

        #region Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoteDTO createNoteDTO)
        {
            try
            {
                await noteService.CreateNoteAsync(createNoteDTO.NoteDTO);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var data = await noteService.CreateNoteViewAsync();

                return View(data);
            }
        }
        #endregion

        #region MarkDone
        public async Task<IActionResult> MarkDone(int id, int[] filterOptions, string searchString, int pageNumber, int pageSize)
        {
            await noteService.MarkDoneAsync(id);

            return RedirectToAction("Index", new
            {
                filterOptions = filterOptions,
                searchString = searchString,
                pageNumber = pageNumber,
                pageSize = pageSize
            });
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await noteService.GetNoteByIdAsync(id);

            return View(data);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteDTO noteDTO)
        {
            await noteService.UpdateNoteAsync(noteDTO);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id, int[] filterOptions, string searchString, int pageNumber, int pageSize)
        {
            await noteService.DeleteNoteAsync(id);

            return RedirectToAction("Index", new
            {
                filterOptions = filterOptions,
                searchString = searchString,
                pageNumber = pageNumber,
                pageSize = pageSize
            });
        }
        #endregion

        #region SetPriority
        [HttpGet]
        public async Task<IActionResult> SetPriority()
        {
            var data = await noteService.GetAllNotesAsync();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPriority(List<NoteDTO> noteDTOs)
        {
            await noteService.SetPriorityAsync(noteDTOs);

            return RedirectToAction("Index");
        }
        #endregion

        #endregion
    }
}
