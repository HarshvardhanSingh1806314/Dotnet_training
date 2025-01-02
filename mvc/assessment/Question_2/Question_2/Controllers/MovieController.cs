using Question_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Question_2.Controllers
{
    public class MovieController : Controller
    {
        public readonly MoviesContext _db;

        public MovieController()
        {
            _db = new MoviesContext();
        }

        // GET: Movie
        public ActionResult Index()
        {
            var movieList = _db.Movies.ToList();
            return View(movieList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            _db.Movies.Add(movie);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int Id)
        {
            return View(_db.Movies.Find(Id));
        }

        [HttpPost]
        public ActionResult DeleteMovie(int Id)
        {
            Movie movie = _db.Movies.Find(Id);
            _db.Movies.Remove(movie);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}