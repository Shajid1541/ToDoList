using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using DAL;
using BLL.validators;

namespace BLL.Services
{
    public class CategoryService
    {
        #region Fields
        private readonly DataAccessFactory dataAccessFactory;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public CategoryService(DataAccessFactory dataAccessFactory, IMapper mapper)
        {
            this.dataAccessFactory = dataAccessFactory;
            this.mapper = mapper;
        }
        #endregion

        #region Methods

        #region CreateCategoryAsync
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO CategoryDTO)
        {
            var validator = new CategoryDTOValidator(dataAccessFactory);
            var validationResult = await validator.ValidateAsync(CategoryDTO);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    CategoryDTO.errors.Add(error.ErrorMessage);
                }
                return CategoryDTO;
            }
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();
            var Category = mapper.Map<Category>(CategoryDTO);
            await CategoryRepository.CreateAsync(Category);
            return mapper.Map<CategoryDTO>(Category);
        }
        #endregion

        #region GetCategoryByIdAsync
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            var Category = await CategoryRepository.ReadAsync(id);
            return mapper.Map<CategoryDTO>(Category);
        }
        #endregion

        #region GetAllCategorysAsync
        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            var Categorys = await CategoryRepository.ReadAllAsync();
            return mapper.Map<List<CategoryDTO>>(Categorys);
        }
        #endregion

        #region UpdateCategoryAsync
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
        #endregion

        #region DeleteCategoryAsync
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            using var CategoryRepository = dataAccessFactory.CreateCategoryData();

            return await CategoryRepository.DeleteAsync(id);
        }
        #endregion

        #endregion
    }
}
