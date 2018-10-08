using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApsRedes.Models
{
    public class Player
    {
        [Required]
        [StringLength(5)]
        [DisplayName("Username")]
        public string name { get; set; }

        public char mark { get; set; }
    }
}