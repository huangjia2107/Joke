﻿using System;
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
        Text, //段子
        Img, //图片
        Video,  //视频

        Signin, //登录
        Comment,//评论

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
}