using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Category
    {
        #region Properties
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        #endregion

        #region Constructor
        public Category() { 
            Notes = new List<Note>();
        }
        #endregion
    }
}
