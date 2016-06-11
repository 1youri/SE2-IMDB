using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE2_IMDB.Models.Entity;

namespace SE2_IMDB.Models.ViewModels
{
    public class PersonList
    {
        public PersonList(List<Entity.Person> persons, string currentsort)
        {
            Persons = persons;

            this.SortList = new List<string>();
            this.SortList.Add(currentsort == "rating" ? "rating-asc" : "rating");
            this.SortList.Add(currentsort == "name-asc" ? "name" : "name-asc");
    }

        public List<Entity.Person> Persons { get; set; }
        public List<string> SortList { get; set; }
    }
}