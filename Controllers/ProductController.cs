using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace ScanLoginDemo.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private static readonly string agentId = ConfigurationManager.AppSettings["agentId"];// 从配置文件中获取应用Id
        private static readonly string appKey = ConfigurationManager.AppSettings["appKey"];// 从配置文件中获取appKey
        private static readonly string appSecret = ConfigurationManager.AppSettings["appSecret"];// 从配置文件中获取appSecret

        #region 通用方法
        /// <summary>
        /// 时间戳转换
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }
        #endregion

        #region 从配置文件中获取配置相关
        public string getAppKey()
        {
            return ConfigurationManager.AppSettings["appKey"].ToString();
        }

        public string getLoginUrl()
        {
            return ConfigurationManager.AppSettings["loginUrl"].ToString();
        }
        #endregion

        #region 通过免登授权码获取用户UserId
        public string getUserByCode(string code)
        {
            var unionId = "";
            {
                // 通过临时授权码获取用户UnionId
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/getuserinfo_bycode");
                OapiSnsGetuserinfoBycodeRequest req = new OapiSnsGetuserinfoBycodeRequest();
                req.TmpAuthCode = code;
                OapiSnsGetuserinfoBycodeResponse response = client.Execute(req, appKey, appSecret);
                unionId = response.UserInfo.Unionid;
            }
            var userId = "";
            {
                // 通过用户UnionId获取用户UserId
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/user/getbyunionid");
                OapiUserGetbyunionidRequest req = new OapiUserGetbyunionidRequest();
                req.Unionid = unionId;
                OapiUserGetbyunionidResponse rsp = client.Execute(req, AccessToken.AccessToken.GetAccessToken());
                userId = rsp.Result.Userid;
            }
            var name = "";
            {
                // 通过用户UserId获取用户信息
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/user/get");
                OapiV2UserGetRequest req = new OapiV2UserGetRequest();
                req.Userid = userId;
                req.Language = "zh_CN";
                OapiV2UserGetResponse rsp = client.Execute(req, AccessToken.AccessToken.GetAccessToken());
                name = rsp.Result.Name;
            }

            return JsonConvert.SerializeObject(new { userId = userId, name = name });
        }
        #endregion

    }
}