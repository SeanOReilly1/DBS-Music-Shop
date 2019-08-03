using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicShop.Models
{
    public class Guitar
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Image { get; set; }
        
        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ItemType { get; set; }

        [Required]
        public string Brand { get; set; }
    }
}