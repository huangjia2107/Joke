using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Models
{
    public class Artical
    {
        public int a { get; set; }
        public int t_all { get; set; }
        public int t { get; set; }
    }

    public class UserData
    {
        public Artical articles { get; set; }
    }

    public class LoginInfo
    {
        public UserData userdata { get; set; }
        public string token { get; set; }
        public User user { get; set; }
        public int err { get; set; }
    }
}
