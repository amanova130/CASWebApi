using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateTime LogIn { get; set; }
        public DateTime LogOff { get; set; }
    }
}
