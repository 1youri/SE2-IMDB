using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE2_IMDB.Models.Entity
{
    public class Film
    {
        public Film()
        {
            Persons = new List<Person>();
        }
        public Film(object iD, object title, object description, object releaseYear, object popularity, object storyLine, object picture)
        {
            ID = iD.ToInt();
            Title = title.ToString();
            Description = description.ToString();
            ReleaseYear = releaseYear.ToInt();
            Popularity = popularity.ToInt();
            StoryLine = storyLine.ToString();
            ImagePath = picture.ToString();

        }
        public int ID { get; set; }
        public int Popularity { get; set; }


        [Required]
        [StringLength(32)]
        public string Title { get; set; }

        [Required]
        [Range(1900, 3000)]
        public int ReleaseYear { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string StoryLine { get; set; }
        public string ImagePath { get; set; }

        public List<Person> Persons { get; set; }

        public string Role { get; set; }
        public bool InPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mode">0:default, 1:popularity, 2:title, 3:releaseyear, negative is asc</param>
        /// <returns></returns>
        public static List<Film> SortList(List<Film> films, string mode = "")
        {
            List<Film> returnlist = new List<Film>();
            switch (mode)
            {
                case "releaseyear-asc":
                    returnlist = films.OrderBy(x => x.ReleaseYear).ToList();
                    break;
                case "title-asc":
                    returnlist = films.OrderBy(x => x.Title).ToList();
                    break;
                case "popularity-asc":
                    returnlist = films.OrderBy(x => x.Popularity).ToList();
                    break;
                case "popularity":
                    returnlist = films.OrderByDescending(x => x.Popularity).ToList();
                    break;
                case "title":
                    returnlist = films.OrderByDescending(x => x.Title).ToList();
                    break;
                case "releaseyear":
                    returnlist = films.OrderByDescending(x => x.ReleaseYear).ToList();
                    break;
                case null:
                    returnlist = films.OrderByDescending(x => x.Popularity).ThenByDescending(x => x.ReleaseYear).ThenBy(x => x.Title).ToList();
                    break;
            }
            if(returnlist.Count <1) returnlist = films.OrderByDescending(x => x.Popularity).ThenByDescending(x => x.ReleaseYear).ThenBy(x => x.Title).ToList();

            return returnlist;
        } 
    }
}