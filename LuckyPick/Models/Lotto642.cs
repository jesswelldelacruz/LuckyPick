﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LuckyPick.Models
{
    public class Lotto642
    {
        [Key]
        public string Combination { get; set; }
        public DateTime DrawDate { get; set; }
    }
}
