using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL;
using BLL.validators;
using DAL.Repositories;

namespace BLL.Services
{
    public class CategoryService
    {
        #region Fields
        private readonly CategoryRepository categoryRepository;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public CategoryService(CategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }
        #endregion

        #region Methods

        #region CreateCategoryAsync
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO CategoryDTO)
        {
            var validator = new CategoryDTOValidator(categoryRepository);
            var validationResult = await validator.ValidateAsync(CategoryDTO);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    CategoryDTO.errors.Add(error.ErrorMessage);
                }
                return CategoryDTO;
            }
            
            var Category = mapper.Map<Category>(CategoryDTO);
            await categoryRepository.CreateAsync(Category);
            return mapper.Map<CategoryDTO>(Category);
        }
        #endregion

        #region GetCategoryByIdAsync
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var Category = await categoryRepository.ReadAsync(id);
            return mapper.Map<CategoryDTO>(Category);
        }
        #endregion

        #region GetAllCategorysAsync
        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            var Categorys = await categoryRepository.ReadAllAsync();
            return mapper.Map<List<CategoryDTO>>(Categorys);
        }
        #endregion

        #region UpdateCategoryAsync
        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO CategoryDto)
        {
            var Category = await categoryRepository.ReadAsync(CategoryDto.Id);
            if (Category == null)
            {
                return null;
            }

            mapper.Map(CategoryDto, Category);
            await categoryRepository.UpdateAsync(Category);
            return mapper.Map<CategoryDTO>(Category);
        }
        #endregion

        #region DeleteCategoryAsync
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await categoryRepository.DeleteAsync(id);
        }
        #endregion

        #endregion
    }
}
