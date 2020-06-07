using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionUnitTest.Sample
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Joined { get; set; }
    }
}
