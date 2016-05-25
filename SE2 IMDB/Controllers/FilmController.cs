using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SE2_IMDB.Models.Entity;
using SE2_IMDB.Models;
using SE2_IMDB.Models.ViewModels.Film;

namespace SE2_IMDB.Controllers
{
    public class FilmController : Controller
    {
        // GET: Film
        public ActionResult Index()
        {
            List<Film> Films = Models.Repositories.FilmRepo.GetFilms();
            FilmList ViewModel = new FilmList(Films);
            return View(ViewModel);
        }

        // GET: Film/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Film/Create
        [HttpPost]
        public ActionResult Create(Film film)
        {
            if(Models.Repositories.FilmRepo.CreateFilm(film)) return RedirectToAction("Index");
            return View();
        }

        // GET: Film/Edit
        public ActionResult Edit(int id)
        {
            Film film = Models.Repositories.FilmRepo.GetFilm(id);
            return View(film);
        }

        // POST: Film/Edit
        [HttpPost]
        public ActionResult Edit(Film film)
        {
            if (Models.Repositories.FilmRepo.UpdateFilm(film)) return RedirectToAction("Index");
            else
            {
                List<string> Errors = new List<string>();
                if (film.ReleaseYear < 0 || film.ReleaseYear > DateTime.Now.Year + 50)
                    Errors.Add("Releaseyear entered incorrectly/impossible");
                return View();
            }
            
        }

    }
}