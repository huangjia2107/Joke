using GalaSoft.MvvmLight;
using Joke.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Joke.Models
{
    public class User : ViewModelBase
    {
        public User()
        {
            _login = "请登录";
            _real_icon = @"ms-Appx:///Assets/Images/void.png";
        }

        private string _id;
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                UpdateRealIcon(_icon, value);
                RaisePropertyChanged("id");
            }
        }

        private string _login;
        public string login
        {
            get { return _login; }
            set { _login = value; RaisePropertyChanged("login"); }
        }

        private string _icon; //http://img.qiushibaike.com/system/avtnew/2938/29383442/thumb/20150712143838.jpg
        public string icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                UpdateRealIcon(value, _id);
                RaisePropertyChanged("icon");
            }
        }

        private string _real_icon;
        public string real_icon
        {
            get { return _real_icon; }
            set { _real_icon = value; RaisePropertyChanged("real_icon"); }
        }

        private string _last_visited_at;
        public string last_visited_at
        {
            get { return _last_visited_at; }
            set
            {
                _last_visited_at = !Regex.IsMatch(value, @"^[0-9]*$") ? value : Algorithm.GetRealTime(long.Parse(value)).ToString("yyyy-MM-dd HH:mm");
                RaisePropertyChanged("last_visited_at");
            }
        }

        private string _created_at;
        public string created_at
        {
            get { return _created_at; }
            set
            {
                _created_at = !Regex.IsMatch(value, @"^[0-9]+$") ? value : Algorithm.GetRealTime(long.Parse(value)).ToString("yyyy-MM-dd");
                RaisePropertyChanged("created_at");
            }
        }

        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged("email"); }
        }

        private string _last_device;
        public string last_device
        {
            get { return _last_device; }
            set { _last_device = value; RaisePropertyChanged("last_device"); }
        }

        private void UpdateRealIcon(string _icon, string _id)
        {
            if (string.IsNullOrEmpty(_icon) || string.IsNullOrEmpty(_id))
                _real_icon = @"ms-Appx:///Assets/Images/void.png";
            else
            {
                _real_icon = string.Format("http://img.qiushibaike.com/system/avtnew/{0}/{1}/thumb/{2}", _id.Substring(0, _id.Length - 4), _id, _icon);

                RaisePropertyChanged("real_icon");
            }
        }
    }
}
