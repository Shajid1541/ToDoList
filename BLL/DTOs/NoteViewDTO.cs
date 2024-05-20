using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class NoteViewDTO
    {
        public PagedResult<NoteDTO> Notes { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
}
