using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public virtual Book Book { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual DateTime CheckoutTime { get; set; }

        public virtual List<PurcahsedActivationCode> ActivationCodes { set; get; }
        public CartItem()
        {
            ActivationCodes = new List<PurcahsedActivationCode>();
        }
        public CartItem(int cartId, int productId)
        {
            CartId = cartId;
            BookId = productId;
            Quantity = 1;
            ActivationCodes = new List<PurcahsedActivationCode>();
        }
    }
}
