using DAL;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        
        public string NoteTitle { get; set; }
        public string NoteDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; } = int.MaxValue;

        public List<string> errors;

        public NoteDTO()
        {
            errors = new List<string>();
            DueDate = DateTime.Now;
        }
    }

    
}
