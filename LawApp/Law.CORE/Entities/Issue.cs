using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.Entities
{
    public class Issue
    {
        [Key]
        public int Id { get; set; } 
        public int Code {  get; set; }
        public string Name { get; set; }
    }
}
