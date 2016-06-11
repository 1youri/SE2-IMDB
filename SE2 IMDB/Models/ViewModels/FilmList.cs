using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE2_IMDB.Models;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.ViewModels
{
    public class FilmList
    {
        public FilmList(List<Entity.Film> films, string currentsort)
        {
            Films = films;

            SortList = new List<string>();
            SortList.Add(currentsort == "title-asc" ? "title" : "title-asc");
            SortList.Add(currentsort == "releaseyear" ? "releaseyear-asc" : "releaseyear");
            SortList.Add(currentsort == "popularity" ? "popularity-asc" : "popularity");
        }

        public List<Entity.Film> Films { get; set; }
        public List<string> SortList { get; set; }
    }
}