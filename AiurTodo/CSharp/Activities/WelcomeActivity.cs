using System.Text;
using AiurTodo.CSharp.Constants;
using AiurTodo.CSharp.Models.TokenModels;
using AiurTodo.CSharp.Utils;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using Newtonsoft.Json;

namespace AiurTodo.CSharp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class WelcomeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_welcome);
            
            FindViewById<TextView>(Resource.Id.btnLogin).Click += async (o, e) =>
            {
                StringBuilder archonUrlSb = new StringBuilder(UrlConstants.ARCHON_AUTH_URL);
                archonUrlSb.Append("?appId=")
                    .Append(LocalProperties.APP_ID)
                    .Append("&appSecret=")
                    .Append(LocalProperties.APP_SECRET);
                
                string result = await HttpClientUtils.GetJson(archonUrlSb.ToString());
                AccessTokenViewModel requestToken = JsonConvert.DeserializeObject<AccessTokenViewModel>(result);
                var memes = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext).Edit();
                memes.PutString("ACCESS_TOKEN", requestToken.AccessToken).Apply();
                memes.Commit();
                
                StringBuilder gatewayUrlSb = new StringBuilder(UrlConstants.GATEWAY_AUTH_URL);
                gatewayUrlSb.Append("?appId=")
                    .Append(LocalProperties.APP_ID)
                    .Append("&redirect_uri=")
                    .Append(UrlConstants.APP_URL)
                    .Append("&state=/");

                StartActivity(new Intent(Intent.ActionView, Uri.Parse(gatewayUrlSb.ToString())));
            };

            FindViewById<TextView>(Resource.Id.btnRegister).Click += (o, e) =>
            {
                StringBuilder gatewayUrlSb = new StringBuilder(UrlConstants.GATEWAY_REGISTER_URL);
                gatewayUrlSb.Append("?appId=")
                    .Append(LocalProperties.APP_ID)
                    .Append("&redirect_uri=")
                    .Append(UrlConstants.APP_URL)
                    .Append("&state=/");

                StartActivity(new Intent(Intent.ActionView, Uri.Parse(gatewayUrlSb.ToString())));
            };
        }
    }
}