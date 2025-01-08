using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiAssessment.Models
{
    public class CountryContext : DbContext
    {
        public CountryContext() : base("name = dbcs")
        {

        }

        public DbSet<Country> Countries { get; set; }
    }
}