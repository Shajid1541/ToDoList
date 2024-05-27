using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int categoryId { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        [ForeignKey("userId")]
        public virtual User User { get; set; }
        [ForeignKey("categoryId")]
        public virtual Category Category { get; set; }
    }
}
