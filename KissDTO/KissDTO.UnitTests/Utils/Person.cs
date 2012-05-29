using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KissDTO.UnitTests.Utils
{
    public class Person
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public Location Residence { get; set; }

    }
}
