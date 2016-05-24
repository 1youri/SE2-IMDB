using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE2_IMDB.Models;

namespace SE2_IMDB.Models.ViewModels.Film
{
    public class FilmList
    {
        public List<Entity.Film> Films { get; set; }
    }
}