using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Description { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public DateTime DateTime { get; set; }
    }

}
