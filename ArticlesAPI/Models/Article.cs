using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArticlesAPI.Models
{
    public class Article
    {
        public Article(string v2, string v3, string v4, int v5, List<int> list, bool published)
        {
           
            Title = v2;
            Lead = v3;
            Content = v4;
            CategoryId = v5;
            Tags = list;
            Published = published;
        }


        public string Title { get; set; }
        public string Lead { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<int> Tags { get; set; }
        public bool Published { get; set; }
    }
}

