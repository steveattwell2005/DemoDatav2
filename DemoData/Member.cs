using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoData
{
    public class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string OriginalId { get; set; }
        public string NewId { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }
}
