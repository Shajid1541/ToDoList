using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User: IdentityUser
    {
        #region Properties
        [Required]
        public string Name { get; set; }
        #endregion
    }
}
