using Joke.Models;
using Newtonsoft.Json;
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
        public async static Task<JokeResponse<T>> GetJokeInfoList<T>(RequestParam requestParam)
        {
            return await GetObjResult<JokeResponse<T>>(requestParam);
        }

        public async static Task<LoginInfo> GetLoginInfo(string loginName, string loginPass)
        {
            LoginInfo loginInfo = null;
            

            return await GetObjResult<LoginInfo>(new RequestParam {jokeAPI= JokeAPI.Signin, username = loginName, password = loginPass, method = RequestMethod.POST });
        }


        public async static Task<T> GetObjResult<T>(RequestParam requestParam)
        {
            //请求并获取返回的json串，截取符合json解析公共方法的部分。
            string str_json = await GetJsonString(requestParam);

            if (string.IsNullOrEmpty(str_json))
                return default(T);

            T requestResult;
            try
            {
                requestResult = JsonConvert.DeserializeObject<T>(str_json);
            }
            catch
            {
                requestResult = default(T);
            }

            return requestResult;
        }

        public async static Task<string> GetJsonString(RequestParam requestParam)
        {
            if (requestParam == null)
                return null;

            string jsonString = "";
            try
            {
                if (requestParam.method == RequestMethod.GET)
                    jsonString = unicode_js(await GetReponseStringByGet(requestParam));
                else
                    jsonString = unicode_js(await GetReponseStringByPost(requestParam));
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

        private async static Task<string> GetReponseStringByGet(RequestParam requestParam)
        {
            string requestUrl = string.Empty;
            string param = string.Empty;

            switch (requestParam.jokeAPI)
            {
                case JokeAPI.Signin:
                    param = "{\"login\":\"" + requestParam.username + "\",\"pass\":\"" + requestParam.password + "\"}";
                    break;
                case JokeAPI.Comment:
                    param = "page=" + requestParam.page + "&count=" + requestParam.count;
                    requestUrl = string.Format(HashMap.JokeAPIMap[requestParam.jokeAPI], requestParam.args) + "?" + param;
                    break;
//                 case JokeAPI.Publish:
//                 case JokeAPI.Participate:
//                 case JokeAPI.Collection:
//                 break;
                default:
                    param = "page=" + requestParam.page + "&count=" + requestParam.count;
                    requestUrl = HashMap.JokeAPIMap[requestParam.jokeAPI] + "?" + param;
                    break;
            }

            string resultStr = "";
            HttpClient httpClient = new HttpClient();

            if (!string.IsNullOrEmpty(requestParam.token))
                httpClient.DefaultRequestHeaders.Add("Qbtoken", requestParam.token);

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

        private async static Task<string> GetReponseStringByPost(RequestParam requestParam)
        {                                 
            string requestUrl = string.Empty;
            string param = string.Empty;

            switch (requestParam.jokeAPI)
            {
                case JokeAPI.Signin:
                    param = "{\"login\":\"" + requestParam.username + "\",\"pass\":\"" + requestParam.password + "\"}";
                    requestUrl = HashMap.JokeAPIMap[requestParam.jokeAPI] + "?" + param;
                    break;
                default:
                    return null;
            }

            string resultStr = "";
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
