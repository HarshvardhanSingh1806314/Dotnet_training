using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiAssessment.Models;

namespace WebApiAssessment.Controllers
{
    [RoutePrefix("api/Countries")]
    public class CountryController : ApiController
    {
        private readonly CountryContext _db;

        public struct CountryUpdateModel
        {
            public string Name { get; set; }

            public string Capital { get; set; }
        }

        public CountryController()
        {
            _db = new CountryContext();
        }

        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetAllCountries()
        {
            List<Country> countries = _db.Countries.ToList();
            return Ok(countries);
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddCountry([FromBody] Country country)
        {
            if(country == null || (country.Name == null || country.Name.Length == 0) || (country.Capital == null || country.Capital.Length == 0))
            {
                return BadRequest("Country Cannot be null");
            }

            Country newAddedCountry = _db.Countries.Add(country);
            if(newAddedCountry != null && _db.SaveChanges() > 0)
            {
                return Created("Country", newAddedCountry);
            }

            return BadRequest("Not Able To Add Country");
        }

        [HttpDelete]
        [Route("Remove")]
        public IHttpActionResult RemoveCountry([FromUri] int CountryId)
        {
            if(CountryId <= 0)
            {
                return BadRequest("Country Id Cannot be less than or equal to 0");
            }

            // checking if the country exist
            Country countryExist = _db.Countries.Find(CountryId);
            if(countryExist == null)
            {
                return NotFound();
            }

            Country removedCountry = _db.Countries.Remove(countryExist);
            if(removedCountry != null && _db.SaveChanges() > 0)
            {
                return Ok(removedCountry);
            }

            return BadRequest("Country Cannot be removed");
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateCountry(int CountryId, [FromBody] CountryUpdateModel country)
        {
            if((country.Name == null || country.Name.Length == 0) && (country.Capital == null || country.Capital.Length == 0))
            {
                return BadRequest("Country Name cannot be Empty");
            }

            // checking if the country exist or not
            Country countryExist = _db.Countries.Find(CountryId);
            if(countryExist == null)
            {
                return BadRequest("Country you are trying to update does not exist");
            }

            // updating the country
            countryExist.Name = country.Name != null && country.Name.Length > 0 ? country.Name : countryExist.Name;
            countryExist.Capital = country.Capital != null && country.Capital.Length > 0 ? country.Capital : countryExist.Capital;

            _db.SaveChanges();

            return Ok(countryExist);
        }
    }
}
