namespace BLL.DTOs
{
    public class CategoryDTO
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> errors { get; set; } 
        #endregion

        #region Constructor
        public CategoryDTO()
        {
            errors = new List<string>();
        }
        #endregion
    }
}
