using GalaSoft.MvvmLight;
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
        public Artical articles { get; set; } = new Artical();
    }

    public class LoginInfo
    {
        public UserData userdata { get; set; } = new UserData();
        public string token { get; set; }
        public User user { get; set; } = new User();
        public int err { get; set; } = -1;

        public string userid { get; set; }
        public string userpassword { get; set; }
    }
}
