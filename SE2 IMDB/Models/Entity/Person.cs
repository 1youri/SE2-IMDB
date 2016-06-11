using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE2_IMDB.Models.Entity
{
    public class Person
    {
        public Person()
        {
        }

        public Person(object iD, object rating, object name, object description, object imagePath)
        {
            ID = iD.ToInt();
            Rating = rating.ToInt();
            Name = name.ToString();
            Description = description.ToString();
            ImagePath = imagePath.ToString();
        }

        public int ID  { get; set; }
        public int Rating { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public string Role { get; set; }
        public bool inFilm { get; set; }
        public bool Selected { get; set; }

        public List<Film> Films { get; set; }


        public static List<Person> SortList(List<Person> persons, string mode = "")
        {
            List<Person> returnlist = new List<Person>();
            switch (mode)
            {
                case "rating-asc":
                    returnlist = persons.OrderBy(x => x.Rating).ToList();
                    break;
                case "name-asc":
                    returnlist = persons.OrderBy(x => x.Name).ToList();
                    break;
                case "rating":
                    returnlist = persons.OrderByDescending(x => x.Rating).ToList();
                    break;
                case "name":
                    returnlist = persons.OrderByDescending(x => x.Name).ToList();
                    break;
            }
            if(returnlist.Count < 1) returnlist = persons.OrderByDescending(x => x.Rating).ThenByDescending(x => x.Name).ToList();

            return returnlist;
        }
    }
}