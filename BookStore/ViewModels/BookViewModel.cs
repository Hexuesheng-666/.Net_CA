using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class BookViewModel
    {
        public string Name { set; get; }
        public string Desc { set; get; }
        public double UnitPrice { set; get; }
        public string Image { set; get; }
    }
}
