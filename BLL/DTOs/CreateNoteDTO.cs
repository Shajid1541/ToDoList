using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CreateNoteDTO
    {
        public NoteDTO NoteDTO { get; set; } = new NoteDTO();
        public List<CategoryDTO> Categories { get; set; }
        
    }
}
