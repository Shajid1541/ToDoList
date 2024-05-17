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
                var data = await noteService.CreateNoteViewAsync();
                return View(data);
            }
            
            
        }
        #endregion
       
        public async Task<IActionResult> MarkDone(int id)
        {
            await noteService.MarkDoneAsync(id);
            return RedirectToAction("Index");
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            await noteService.DeleteNoteAsync(id);
            return RedirectToAction("Index");
        }

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
    }
}
