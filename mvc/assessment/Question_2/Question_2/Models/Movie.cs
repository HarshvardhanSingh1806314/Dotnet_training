using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Question_2.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MovieName { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}