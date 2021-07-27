using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public double UnitPrice { set; get; }
        public string Image { set; get; }
        public string Tags { set; get; }
    }
}
