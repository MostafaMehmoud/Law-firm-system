﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.Entities
{
    public class Center
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public int Code {  get; set; }
        public int CourtId {  get; set; }
    }
}
