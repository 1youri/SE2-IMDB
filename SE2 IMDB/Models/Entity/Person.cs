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

        public Person(int iD, int rating, string name, string description, string imagePath)
        {
            ID = iD;
            Rating = rating;
            Name = name;
            Description = description;
            ImagePath = imagePath;
        }

        public int ID  { get; set; }
        public int Rating { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(1024)]
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}