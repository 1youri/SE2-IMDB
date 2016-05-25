using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE2_IMDB.Models;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.ViewModels.Film
{
    public class FilmList
    {
        public FilmList(List<Entity.Film> films)
        {
            Films = films;
        }

        public List<Entity.Film> Films { get; set; }
    }
}