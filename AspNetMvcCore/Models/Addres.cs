﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetMvcCore.Models
{
    public class Addres
    {
        public int Id { set; get; }
        public string Country { set; get; }
        public string City { set; get; }
        public string Street { set; get; }
        public string Zip { set; get; }
        public int UserId { set; get; }
        public User User { set; get; }
    }
}
