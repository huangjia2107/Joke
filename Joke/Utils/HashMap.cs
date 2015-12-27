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
            {JokeAPI.Hot,        @"http://m2.qiushibaike.com/article/list/hot"},
            {JokeAPI.Latest,     @"http://m2.qiushibaike.com/article/list/latest"},
            {JokeAPI.Text,       @"http://m2.qiushibaike.com/article/list/text"},
            {JokeAPI.Img,        @"http://m2.qiushibaike.com/article/list/imgrank"},
            {JokeAPI.Video,      @"http://m2.qiushibaike.com/article/list/video"},
            {JokeAPI.Signin,     @"http://m2.qiushibaike.com/user/signin"},
            {JokeAPI.Comment,    @"http://m2.qiushibaike.com/article/{0}/comments"},
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
    }
}
