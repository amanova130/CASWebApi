using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class ExtendedLink
    {
        public int LinkId { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }

        public string Fac_name { get; set; }
        public string Image { get; set; }
    }
}
