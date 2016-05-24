using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE2_IMDB.Models.Entity
{
    public class Comment
    {
        public Comment(int id, string commentText, string commenter)
        {
            ID = id;
            CommentText = commentText;
            Commenter = commenter;
        }

        public int ID { get; set; }
        public string CommentText { get; set; }
        public string Commenter { get; set; }
        public int MyProperty { get; set; }
    }
}