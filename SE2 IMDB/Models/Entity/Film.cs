using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE2_IMDB.Models.Entity
{
    public class Film
    {
        public Film(int iD, string title, int releaseYear, int popularity, string storyLine)
        {
            ID = iD;
            Title = title;
            ReleaseYear = releaseYear;
            Popularity = popularity;
            StoryLine = storyLine;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int Popularity { get; set; }
        public string StoryLine { get; set; }
    }
}