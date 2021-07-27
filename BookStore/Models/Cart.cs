using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CheckoutTime { set; get; }
        public bool IsCheckOut { get; set; }

        public int Quantity { get; set; }
        public double Value { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
        public Cart(int customerId)
        {
            CustomerId = customerId;
            CreationTime = DateTime.Now;
            IsCheckOut = false;
            CartItems = new List<CartItem>();
        }
    }
}
