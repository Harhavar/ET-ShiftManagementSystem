using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftMgtDbContext.Entities
{
    public class Email
    {

        public string to { get; set; } = string.Empty;

        public string subject { get; set; } = string.Empty;

        public string body { get; set; } = string.Empty;
    }
}
