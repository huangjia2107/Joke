using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Utils
{
    public class HashMap
    {
        public static Dictionary<JokeAPI, string> JokeAPIMap = new Dictionary<JokeAPI, string>()
        {
            {JokeAPI.Hot,                @"http://m2.qiushibaike.com/article/list/hot"},
            {JokeAPI.Latest,             @"http://m2.qiushibaike.com/article/list/latest"},
            {JokeAPI.Suggest,            @"http://m2.qiushibaike.com/article/list/suggest"},
            {JokeAPI.Text,               @"http://m2.qiushibaike.com/article/list/text"},
            {JokeAPI.Img,                @"http://m2.qiushibaike.com/article/list/imgrank"},
            {JokeAPI.Video,              @"http://m2.qiushibaike.com/article/list/video"},
                                       
            {JokeAPI.Signin,             @"http://m2.qiushibaike.com/user/signin"},            //Post
            {JokeAPI.Comment,            @"http://m2.qiushibaike.com/article/{0}/comments"},   //Get
            {JokeAPI.Publish,            @"http://m2.qiushibaike.com/user/my/articles"},      //Get
            {JokeAPI.Participate,        @"http://m2.qiushibaike.com/user/my/participate"},  //Get
            {JokeAPI.Collection,         @"http://m2.qiushibaike.com/collect/list"},      //Get

            {JokeAPI.UserDetail,         @"http://nearby.qiushibaike.com/user/{0}/detail"},      //Get
            {JokeAPI.UserPublish,        @"http://m2.qiushibaike.com//user/{0}/articles"},      //Get
            {JokeAPI.UserParticipate,    @"http://m2.qiushibaike.com/user/{0}/participate"},      //Get
        };

        public static Dictionary<JokeAPI, string> JokeTitleMap = new Dictionary<JokeAPI, string>()
        {
            {JokeAPI.Suggest,             "图文"},
            {JokeAPI.Hot,                 "热门"},
            {JokeAPI.Latest,              "最新"},
            {JokeAPI.Text,                "文字"},
            {JokeAPI.Img,                 "图片"},
            {JokeAPI.Video,               "视频"},
            {JokeAPI.Publish,             "我发表的"},
            {JokeAPI.Participate,         "我参与的"},
            {JokeAPI.Collection,          "我收藏的"},
            {JokeAPI.UserPublish,         "他(她)发表的"},
            {JokeAPI.UserParticipate,     "他(她)发表的"},
        };

        public static Dictionary<RequestMethod, string> RequestMethodMap = new Dictionary<RequestMethod, string>()
        {
            {RequestMethod.POST,                "Post" },
            {RequestMethod.GET,                 "Get" },
        };

        public static Dictionary<RequestCharset, string> RequestCharsetMap = new Dictionary<RequestCharset, string>()
        {
            {RequestCharset.CHARSET_UTF8,        "UTF-8" },
            {RequestCharset.CHARSET_GB2312,      "GB2312" }
        };

        public static Dictionary<RequestHeader, string> RequestHeaderMap = new Dictionary<RequestHeader, string>()
        {
            {RequestHeader.AcceptEncoding,          "gzip,deflate" },
            {RequestHeader.UserAgent,               "QiuBai/1.0.0 (Windows; Windows 10; zh_CN) PLHttpClient/1_WIFI" },
            {RequestHeader.CacheControl,            "no-cache" },
            {RequestHeader.Accept,                  "*/*" },
            {RequestHeader.Uuid,                    "windows_"+Guid.NewGuid().ToString("N") },
            {RequestHeader.Source,                  "windows_1.0.0" }
        };
    }
}
