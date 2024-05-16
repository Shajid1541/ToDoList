using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; } = 1;
        public string NoteTitle { get; set; }
        public string NoteDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
    }
}
