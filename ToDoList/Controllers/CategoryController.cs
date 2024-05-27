﻿using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
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

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryDTO categoryDTO)
        {
            var data = categoryService.CreateCategoryAsync(categoryDTO);
            return RedirectToAction("Index","Note");
        }
        #endregion
    }
}