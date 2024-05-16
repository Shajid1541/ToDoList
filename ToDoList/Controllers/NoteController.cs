using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteService noteService;

        public NoteController(NoteService noteService)
        {
            this.noteService = noteService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await noteService.GetAllNotesAsync();

            return View(data);
        }

        #region create GET
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
                return View(createNoteDTO);
            }
            
            
        }
        #endregion
    }
}
