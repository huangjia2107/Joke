﻿using System;
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
        };

        public static Dictionary<JokeAPI, string> JokeTitleMap = new Dictionary<JokeAPI, string>()
        {
            {JokeAPI.Suggest,             "图文"},
            {JokeAPI.Hot,                 "热门"},
            {JokeAPI.Latest,              "最新"},
            {JokeAPI.Text,                "文字"},
            {JokeAPI.Img,                 "图片"},
            {JokeAPI.Video,               "视频"},
            {JokeAPI.Publish,             "我的发表"},
            {JokeAPI.Participate,         "我的参与"},
            {JokeAPI.Collection,          "我的收藏"},
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
