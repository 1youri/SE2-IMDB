using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SE2_IMDB.Models.Entity;
using SE2_IMDB.Models;
using SE2_IMDB.Models.Repositories;
using SE2_IMDB.Models.ViewModels;

namespace SE2_IMDB.Controllers
{
    public class FilmController : Controller
    {
        // GET: Film
        public ActionResult Index(string sort = "")
        {
            List<Film> Films = Models.Repositories.FilmRepo.GetFilms();
            FilmList ViewModel = new FilmList(Films, sort);
            ViewModel.Films = Film.SortList(ViewModel.Films, sort);
            foreach (var f in ViewModel.Films)
            {
                if (f.Description.Length > 300)
                    f.Description = f.Description.Substring(0, f.Description.IndexOf(" ", 280)) + "...";
            }
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
            if(ModelState.IsValid) if(Models.Repositories.FilmRepo.CreateFilm(film)) return RedirectToAction("Edit","Film", new {id = -1337});
            return View();
        }

        // GET: Film/Edit
        public ActionResult Edit(int id = 0)
        {
            if (id < 0 && id != -1337) id = id*-1;
            Film film;
            if (id != 0)
            {
                film = Models.Repositories.FilmRepo.GetFilm(id);

                List<Person> persons = FilmRepo.GetPersonsFromFilm(id);
                film.Persons = FilmRepo.GetPersonsNotFromFilm(id, persons);
            }
            else film = new Film() {ID = 0};

            if (id == -1337) return RedirectToAction("Edit", "Film", new {id = film.ID});
            return View(film);
        }

        // POST: Film/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Film film, HttpPostedFileBase upload)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../../Uploads/Films"),
                                        Path.GetFileName(upload.FileName),
                                        DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    upload.SaveAs(filePath);
                    film.ImagePath = upload.FileName;
                    
                }
                if (Models.Repositories.FilmRepo.UpdateFilm(film))
                {
                    bool personsdiffer = false;

                    FilmRepo.DeleteRoles(film.ID);
                    foreach (var p in film.Persons)
                    {
                        if (p.Selected) FilmRepo.InsertRole(film.ID, p);
                        if (p.Selected != p.inFilm)
                            personsdiffer = true;
                    }
                    if (personsdiffer)
                        return RedirectToAction("Edit", "Film", new { id = film.ID });

                    return RedirectToAction("Details","Film", new { id = film.ID} );
                }
                
            }

            return View(film);
        }

        public ActionResult Details(int id = 0)
        {
            if (id < 0) id = id * -1;
            Film film;
            if (id != 0)
            {
                film = Models.Repositories.FilmRepo.GetFilm(id);
                film.Persons = Models.Repositories.FilmRepo.GetPersonsFromFilm(id);
            }
            else film = new Film() { ID = 0 };
            return View(film);
        }

    }
}