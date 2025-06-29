using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Law.CORE.ViewModels
{
    public class OperationResult
    {
        public bool Success { get; set; } = false;
        public List<string> Errors { get; set; } = new();
        public string Message { get; set; } = "";
    }
}
