using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Session
    {
        [Required]
        [MaxLength(36)]
        public string Id { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
