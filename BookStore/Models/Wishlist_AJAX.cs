using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Wishlist_AJAX
    {
        public int Id { get; set; }
        public bool WishlistStatus { get; set; }
        public string wishlistID { get; set; }
        public virtual Session sessionID { get; set; }
        public virtual Book item { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
