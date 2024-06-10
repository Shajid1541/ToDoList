namespace BLL.DTOs
{
    public class CreateNoteDTO
    {
        #region Properties
        public NoteDTO NoteDTO { get; set; } = new NoteDTO();
        public List<CategoryDTO> Categories { get; set; }
        #endregion
    }
}
