using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService
    {
        private readonly DataAccessFactory dataAccessFactory;
        private readonly IMapper mapper;

        public CategoryService(DataAccessFactory dataAccessFactory, IMapper mapper)
        {
            this.dataAccessFactory = dataAccessFactory;
            this.mapper = mapper;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO CategoryDTO)
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();
            var Category = mapper.Map<Category>(CategoryDTO);
            await CategoryRepository.CreateAsync(Category);
            return mapper.Map<CategoryDTO>(Category);
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            var Category = await CategoryRepository.ReadAsync(id);
            return mapper.Map<CategoryDTO>(Category);
        }

        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            var Categorys = await CategoryRepository.ReadAllAsync();
            return mapper.Map<List<CategoryDTO>>(Categorys);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO CategoryDto)
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            var Category = await CategoryRepository.ReadAsync(CategoryDto.Id);
            if (Category == null)
            {
                return null;
            }

            mapper.Map(CategoryDto, Category);
            await CategoryRepository.UpdateAsync(Category);
            return mapper.Map<CategoryDTO>(Category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            return await CategoryRepository.DeleteAsync(id);
        }
    }
}
