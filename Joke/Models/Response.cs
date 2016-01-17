using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Models
{
    public class JokeResponse<T>
    {
        public int count { get; set; }
        public T[] items { get; set; }
        public int total { get; set; }
        public int page { get; set; }

        public string err_msg { get; set; }
        public int err { get; set; }
    }

    public class Err
    {
        public string err_msg { get; set; } //aaaaaaaaaaaa
        public int err { get; set; }
    }

    public class UserResponse
    {
        public UserData userdata { get; set; } = new UserData();
        public int err { get; set; }
    }
}
