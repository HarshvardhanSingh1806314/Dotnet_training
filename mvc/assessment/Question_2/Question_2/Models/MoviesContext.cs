using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Question_2.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext() : base("name = dbcs")
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}