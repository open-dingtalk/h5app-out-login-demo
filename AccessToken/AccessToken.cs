using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System.Configuration;

namespace ScanLoginDemo.AccessToken
{
    public class AccessToken
    {
        private static readonly string appKey = ConfigurationManager.AppSettings["appKey"];
        private static readonly string appSecret = ConfigurationManager.AppSettings["appSecret"];

        #region 获取AccessToken
        public static string GetAccessToken()
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
            OapiGettokenRequest request = new OapiGettokenRequest();
            request.Appkey = appKey;
            request.Appsecret = appSecret;
            request.SetHttpMethod("GET");
            OapiGettokenResponse response = client.Execute(request);
            return response.AccessToken;
        }
        #endregion
    }
}