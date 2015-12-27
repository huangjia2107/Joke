using Joke.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Models
{
    public class User
    {
        public string id { get; set; }
        public string login { get; set; }   //aaaaaaaaa
        public string icon { get; set; } //http://img.qiushibaike.com/system/avtnew/2938/29383442/thumb/20150712143838.jpg
        public long last_visited_at { get; set; }
        public long created_at { get; set; }
        public string email { get; set; }
        public string last_device { get; set; }

        public string real_icon
        {
            get
            {
                if (string.IsNullOrEmpty(icon))
                    return @"ms-Appx:///Assets/Images/void.jpg";
                else
                    return string.Format("http://img.qiushibaike.com/system/avtnew/{0}/{1}/thumb/{2}", id.Substring(0, 4), id, icon);
            }
        }

        public string last_visited_time
        {
            get { return Algorithm.GetRealTime(last_visited_at).ToString("yyyy-MM-dd HH:mm"); }
        }

        public string created_time
        {
            get { return Algorithm.GetRealTime(created_at).ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
