using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SE2_IMDB.Models.Entity;
using SE2_IMDB.Models;
using SE2_IMDB.Models.ViewModels;

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
            if(ModelState.IsValid) if(Models.Repositories.FilmRepo.CreateFilm(film)) return RedirectToAction("Index");
            return View();
        }

        // GET: Film/Edit
        public ActionResult Edit(int id = 0)
        {
            Film film;
            if (id != -1) film = Models.Repositories.FilmRepo.GetFilm(id);
            else film = new Film() {ID = -1};
            return View(film);
        }

        // POST: Film/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Film film, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../../Uploads/Films"),
                                           Path.GetFileName(upload.FileName));
                    upload.SaveAs(filePath);
                    film.ImagePath = upload.FileName;
                }
                if (Models.Repositories.FilmRepo.UpdateFilm(film))
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public ActionResult Details(int id = 0)
        {
            Film film;
            if (id != -1) film = Models.Repositories.FilmRepo.GetFilm(id);
            else film = new Film() { ID = -1 };
            return View(film);
        }

    }
}