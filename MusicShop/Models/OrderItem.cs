﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicShop.Models
{
    public class OrderItem
    {
        public Guitar ItemOrdered { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}