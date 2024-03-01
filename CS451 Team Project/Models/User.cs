using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS451_Team_Project.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Key { get; set; }
    }
}
