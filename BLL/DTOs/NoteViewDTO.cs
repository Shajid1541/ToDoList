namespace BLL.DTOs
{
    public class NoteViewDTO
    {
        #region Properties
        public PagedResult<NoteDTO> Notes { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public string SearchString { get; set; }
        public int[] FilterOptions { get; set; }
        #endregion
    }
}
