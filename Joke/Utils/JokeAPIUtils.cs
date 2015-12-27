using Joke.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Joke.Utils
{
    public class JokeAPIUtils
    {
        public async static Task<JokeResponse<T>> GetJokeInfoList<T>(JokeAPI jokeAPI, uint page = 1, uint count = 50, params string[] args)
        {
            JokeResponse<T> jokeResponse = null;
            string param = "page=" + page + "&count=" + count;
            string apiUrl = args != null ? string.Format(HashMap.JokeAPIMap[jokeAPI], args) : HashMap.JokeAPIMap[jokeAPI];

            switch (jokeAPI)
            {
                case JokeAPI.Signin:
                    break;
                default:
                    jokeResponse = await GetObjResult<JokeResponse<T>>(apiUrl, param);
                    break;
            }

            return jokeResponse;
        }

        public async static Task<LoginInfo> GetLoginInfo(string loginName, string loginPass)
        {
            LoginInfo loginInfo = null;
            string param = "{\"login\":\"" + loginName + "\",\"pass\":\"" + loginPass + "\"}";

            loginInfo = await GetObjResult<LoginInfo>(HashMap.JokeAPIMap[JokeAPI.Signin], param);
            return loginInfo;
        }


        public async static Task<T> GetObjResult<T>(string apiUrl, string param, RequestMethod method = RequestMethod.GET)
        {
            //请求并获取返回的json串，截取符合json解析公共方法的部分。
            string str_json = await GetJsonString(apiUrl, param, method);

            if (string.IsNullOrEmpty(str_json))
                return default(T);

            T requestResult;
            try
            {
                requestResult = JsonUtils.Deserialize<T>(str_json);
            }
            catch
            {
                requestResult = default(T);
            }

            return requestResult;
        }

        public async static Task<string> GetJsonString(String apiurl, String paramString, RequestMethod method = RequestMethod.GET)
        {
            if (string.IsNullOrEmpty(paramString))
                return null;

            string jsonString = "";
            try
            {
                if (method == RequestMethod.GET)
                    jsonString = unicode_js(await GetReponseStringByGet(apiurl, paramString));
                else
                    jsonString = unicode_js(await GetReponseStringByPost(apiurl, paramString));
            }
            catch
            {
                jsonString = null;
            }

            return jsonString;
        }

        /// <summary>
        /// unicode转中文（符合js规则的）
        /// </summary>
        /// <returns></returns>
        private static string unicode_js(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate (Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }

        private async static Task<string> GetReponseStringByGet(string apiUrl, string param)
        {
            string resultStr = "";
            string requestUrl = apiUrl + "?" + param;
            HttpClient httpClient = new HttpClient();

            try
            {
                resultStr = await httpClient.GetStringAsync(new Uri(requestUrl));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                httpClient.Dispose();
            }

            return resultStr;
        }

        private async static Task<string> GetReponseStringByPost(string apiUrl, string param)
        {
            string resultStr = "";
            string requestUrl = apiUrl;
            HttpClient httpClient = new HttpClient();

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(
                    new Uri(requestUrl),
                    new HttpStringContent(param, Windows.Storage.Streams.UnicodeEncoding.Utf8)
                    );

                resultStr = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                httpClient.Dispose();
            }

            return resultStr;
        }
    }
}
