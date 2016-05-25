using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE2_IMDB.Models.Entity
{
    public class Film
    {
        public Film() { }
        public Film(object iD, object title, object description, object releaseYear, object popularity, object storyLine)
        {
            ID = iD.ToInt();
            Title = title.ToString();
            Description = description.ToString();
            ReleaseYear = releaseYear.ToInt();
            Popularity = popularity.ToInt();
            StoryLine = storyLine.ToString();
        }
        public int ID { get; set; }
        public int Popularity { get; set; }


        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, 3000)]
        public int ReleaseYear { get; set; }
        [Required]
        public string StoryLine { get; set; }
    }
}