using BLL.DTOs;
using DAL;
using DAL.Repositories;
using FluentValidation;

namespace BLL.validators
{
    public class CategoryDTOValidator: AbstractValidator<CategoryDTO>
    {
        #region Fields
        private readonly CategoryRepository categoryRepository;
        #endregion

        #region Constructor
        public CategoryDTOValidator(CategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;

            RuleFor(category => category.Name)
                .MustAsync(async (Category, Name, cancellation) =>
                {
                    var categorys = await categoryRepository.ReadAllAsync();
                    return !categorys.Any(category => category.Name == Name);
                }).WithMessage("Category already exists");
        }
        #endregion
    }
}
