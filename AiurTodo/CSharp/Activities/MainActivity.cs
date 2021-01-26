using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AiurTodo.CSharp.Constants;
using AiurTodo.CSharp.Models.TokenModels;
using AiurTodo.CSharp.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using Uri = Android.Net.Uri;

namespace AiurTodo.CSharp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    [IntentFilter(new [] {Intent.ActionView},
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataScheme = "http",
        DataHost = "aiurtodo.com",
        DataPathPrefix = "/open",
        AutoVerify=true)]
    public class MainActivity : AppCompatActivity
    {
        TextView txt;
        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // SupportActionBar.Hide();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            txt = FindViewById<TextView>(Resource.Id.txt);

            string accessToken = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext)
                .GetString("ACCESS_TOKEN", String.Empty);
            string code = GetAccessToken();
            string openId = await GetOpenId(accessToken, code);
            
            StringBuilder gatewayUserInfoSb = new StringBuilder(UrlConstants.GATEWAY_USERINFO_URL);
            gatewayUserInfoSb.Append("?AccessToken=")
                .Append(accessToken)
                .Append("&OpenId=")
                .Append(openId);
            string result = await HttpClientUtils.GetJson(gatewayUserInfoSb.ToString());
            
            txt.Text = result;
        }

        private string GetAccessToken()
        {
            Uri uri = Intent.Data;
            string code = String.Empty;
            if (uri != null)
            {
                code = uri.GetQueryParameter("code");
            }

            return code;
        }
        private async Task<string> GetOpenId(string accessToken, string code)
        {
            StringBuilder gatewayOpenIdSb = new StringBuilder(UrlConstants.GATEWAY_OPENID_URL);
            gatewayOpenIdSb.Append("?AccessToken=")
                .Append(accessToken)
                .Append("&Code=")
                .Append(code);

            string result = await HttpClientUtils.GetJson(gatewayOpenIdSb.ToString());
            
            CodeToOpenIdViewModel openId = JsonConvert.DeserializeObject<CodeToOpenIdViewModel>(result);

            return openId.OpenId;
        }
    }
}