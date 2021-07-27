using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class PurcahsedActivationCode
    {
        public int CartItemId { get; set; }
        [Key]
        public string ActivationCode { get; set; }
        //public PurcahsedActivationCode(int cartItemId, string activationCode)
        //{
        //    CartItemId = cartItemId;
        //    ActivationCode = activationCode;
        //}
    }
}
