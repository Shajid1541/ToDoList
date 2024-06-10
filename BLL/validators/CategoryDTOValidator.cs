using BLL.DTOs;
using DAL;
using FluentValidation;

namespace BLL.validators
{
    public class CategoryDTOValidator: AbstractValidator<CategoryDTO>
    {
        #region Fields
        private readonly DataAccessFactory dataAccessFactory;
        #endregion

        #region Constructor
        public CategoryDTOValidator(DataAccessFactory dataAccessFactory)
        {
            this.dataAccessFactory = dataAccessFactory;

            RuleFor(category => category.Name)
                .MustAsync(async (Category, Name, cancellation) =>
                {
                    var categorys = await dataAccessFactory.CreateCategoryData().ReadAllAsync();
                    return !categorys.Any(category => category.Name == Name);
                }).WithMessage("Category already exists");
        }
        #endregion
    }
}
