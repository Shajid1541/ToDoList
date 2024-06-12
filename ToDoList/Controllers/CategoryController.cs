using BLL.DTOs;
using BLL.Services;
using DAL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        #region Fields
        private readonly CategoryService categoryService;
        #endregion

        #region Constructor
        public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        #endregion

        #region Methods

        #region Index
        public async Task<IActionResult> Index()
        {
            var data = await categoryService.GetAllCategorysAsync();
            return View(data);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var data = new CategoryDTO();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO categoryDTO)
        {
            
            var data = await categoryService.CreateCategoryAsync(categoryDTO);
            if (data.errors.Count > 0)
            {
                return View(data);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var data = await categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }
        #endregion

        #endregion
    }
}
