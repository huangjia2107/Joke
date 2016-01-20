using Joke.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Utils
{
    public enum JokeAPI
    {
        Latest,  //最新 （包含图片及视频）
        Hot,  //最热 （包含图片及视频）
        Suggest, //图文
        Text, //段子
        Img, //图片
        Video,  //视频

        Signin, //登录
        Comment,//评论
        Publish, //我的发表
        Participate,  //我的参与
        Collection,//我的收藏

        UserDetail,
        UserPublish,
        UserParticipate
    }

    public enum RequestMethod
    {
        GET,
        POST
    }

    public enum RequestCharset
    {
        CHARSET_UTF8,
        CHARSET_GB2312
    }

    public enum LoadStatus
    {
        Empty,
        Finish
    }

    public enum RequestHeader
    {
        AcceptEncoding,
        UserAgent,
        CacheControl,
        Accept,
        Uuid,
        Source
    }

    public class LoadStatusArgs
    {
        public LoadStatus Status { get; set; }
        public int ResponseTotalCount { get; set; }
        public int RealTotalCount { get; set; }
    }

    public class RequestParam
    {
        public JokeAPI jokeAPI { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public uint page { get; set; } = 1;
        public uint count { get; set; } = 20;
        public string token { get; set; }
        public RequestMethod method { get; set; } = RequestMethod.GET;
        public string[] args { get; set; }
    }

    public class UserCenterParam
    {
        public JokeAPI jokeAPI { get; set; }
        public LoginInfo loginInfo { get; set; } = new LoginInfo();
    }

    public class UserDetailParam
    {
        public JokeAPI jokeAPI { get; set; }
        public string token { get; set; }
        public User user { get; set; } = new User();
        public bool IsMine { get; set; } = true;
    }
}
