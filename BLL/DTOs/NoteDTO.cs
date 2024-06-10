namespace BLL.DTOs
{
    public class NoteDTO
    {
        #region Properties
        public int Id { get; set; }
        public string UserId { get; set; }
        
        public string NoteTitle { get; set; }
        public string NoteDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; } = int.MaxValue;
        public List<string> errors;
        #endregion

        #region Constructor
        public NoteDTO()
        {
            errors = new List<string>();
            DueDate = DateTime.Now;
        }
        #endregion
    }

    
}
