using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTS_72220577.Data
{
    internal class course
    {
        public int courseId { get; set; }
        public string name { get; set; }
        public string imageName { get; set; }
        public int duration { get; set; }
        public string description { get; set; }
        public int categoryId { get; set; }
        // Foreign Key to Categories
        public category category { get; set; }
    }
}
