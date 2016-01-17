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
    public class UserData : ViewModelBase
    {
        public UserData()
        {
            _real_icon = @"ms-Appx:///Assets/Images/void.png";
        }

        public string emotion { get; set; }
        public int qb_age { get; set; }   
        public string location { get; set; }
        public int bg { get; set; }
        public string relationship { get; set; }

        private int _qs_cnt;
        public int qs_cnt
        {
            get { return _qs_cnt; }
            set { _qs_cnt = value; RaisePropertyChanged("qs_cnt"); }
        }


        private int _smile_cnt;
        public int smile_cnt
        {
            get { return _smile_cnt; }
            set { _smile_cnt = value; RaisePropertyChanged("smile_cnt"); }
        }


        private string _mobile_brand;
        public string mobile_brand
        {
            get { return _mobile_brand; }
            set { _mobile_brand = value; RaisePropertyChanged("mobile_brand"); }
        }

        private int _age;
        public int age
        {
            get { return _age; }
            set { _age = value; RaisePropertyChanged("age"); }
        }

        private string _login;
        public string login
        {
            get { return _login; }
            set { _login = value; RaisePropertyChanged("login"); }
        }

        private string _hometown;
        public string hometown
        {
            get { return _hometown; }
            set { _hometown = DetailValue(value); RaisePropertyChanged("hometown"); }
        }

        private string _haunt;
        public string haunt
        {
            get { return _haunt; }
            set { _haunt = DetailValue(value); RaisePropertyChanged("haunt"); }
        }

        private string _astrology;
        public string astrology
        {
            get { return _astrology; }
            set { _astrology = value; RaisePropertyChanged("astrology"); }
        }

        private string _big_cover;
        public string big_cover
        {
            get { return _big_cover; }
            set { _big_cover = value; RaisePropertyChanged("big_cover"); }
        }

        private string _hobby;
        public string hobby
        {
            get { return _hobby; }
            set { _hobby = DetailValue(value); RaisePropertyChanged("hobby"); }
        }

        private string _introduce;
        public string introduce
        {
            get { return _introduce; }
            set { _introduce = DetailValue(value); RaisePropertyChanged("introduce"); }
        }
        private string _job;
        public string job
        {
            get { return _job; }
            set { _job = DetailValue(value); RaisePropertyChanged("job"); }
        }

        private string _gender;
        public string gender
        {
            get { return _gender; }
            set
            {
                _gender = value == "M" ? "男" : value == "F" ? "女" : value;
                RaisePropertyChanged("gender");
            }
        }

        private string _signature;
        public string signature
        {
            get { return _signature; }
            set { _signature = DetailValue(value); RaisePropertyChanged("signature"); }
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

        private string _uid;
        public string uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                UpdateRealIcon(_icon, value);
                RaisePropertyChanged("uid");
            }
        }

        private string _icon;
        public string icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                UpdateRealIcon(value, _uid);
                RaisePropertyChanged("icon");
            }
        }

        private string _real_icon;
        public string real_icon
        {
            get { return _real_icon; }
            set { _real_icon = value; RaisePropertyChanged("real_icon"); }
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

        private string DetailValue(string value)
        {
            return string.IsNullOrEmpty(value) ? "无" : value;
        }
    }
}
