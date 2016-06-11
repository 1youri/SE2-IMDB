using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SE2_IMDB.Models.Entity;
using SE2_IMDB.Models.Repositories;
using SE2_IMDB.Models.ViewModels;

namespace SE2_IMDB.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index(string sort = "")
        {
            List<Person> persons = Models.Repositories.PersonRepo.GetPersons();
            PersonList ViewModel = new PersonList(persons, sort);
            ViewModel.Persons = Person.SortList(persons, sort);

            foreach (var p in ViewModel.Persons)
            {
                if (p.Description.Length > 300)
                    p.Description = p.Description.Substring(0, p.Description.IndexOf(" ", 280)) + "...";
            }

            return View(ViewModel);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Film/Create
        [HttpPost]
        public ActionResult Create(Person person)
        {
            if(ModelState.IsValid) if(Models.Repositories.PersonRepo.CreatePerson(person)) return RedirectToAction("Edit","Person", new {id= -1337});
            return View();
        }

        // GET: Person/Edit
        public ActionResult Edit(int id = 0)
        {
            if (id < 0) id = id * -1;
            Person person;
            if (id != 0) person = Models.Repositories.PersonRepo.GetPerson(id);
            else person = new Person() { ID = 0 };
            return View(person);
        }

        // POST: Person/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../../Uploads/Persons"),
                                        Path.GetFileName(upload.FileName),
                                        DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    upload.SaveAs(filePath);
                    person.ImagePath = upload.FileName;
                }
                if (Models.Repositories.PersonRepo.UpdatePerson(person))
                {
                    return RedirectToAction("Index");
                    //return RedirectToAction("Details","Person", new {id = person.ID});
                }
            }

            return View(person);
        }

        public ActionResult Details(int id = 0)
        {
            if (id < 0) id = id * -1;
            Person person;
            if (id != -1)
            {
                person = Models.Repositories.PersonRepo.GetPerson(id);
                person.Films = Models.Repositories.PersonRepo.GetPlayingFilms(id);

                Account current = Account.GetID(System.Web.HttpContext.Current);
                if (current.ID != 0)
                {
                    person.Like = LikeRepo.checkLike(current.ID, id, 0);
                }
            }
            else person = new Person() { ID = -1 };
            return View(person);
        }

        public ActionResult Score(int id = 0, int value = 0)
        {
            Account current = Account.GetID(System.Web.HttpContext.Current);
            if (id != 0 && value != 0 && current.ID != 0 && (value == 1 || value == -1))
            {
                if(LikeRepo.UpdateLike(current.ID, value, id, 0))
                {
                    if (value == 1) StaticData.Confirmation = "Liked!";
                    if (value == -1) StaticData.Confirmation = "Disliked!";
                }
            }
            return RedirectToAction("Details", "Person", new { id = id });
        }
    }
}