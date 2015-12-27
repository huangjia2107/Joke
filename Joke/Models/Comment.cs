using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Models
{
    public class Comment
    {
        public string content { get; set; }
        public int floor { get; set; }
        public string id { get; set; }
        public User user { get; set; }
    }
}
