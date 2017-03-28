using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TheSoulWebServer.Tools;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;

namespace TheSoulWebServer
{
    public partial class Oauth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string retJson = "";

            string authCode = queryFetcher.QueryParam_Fetch("code");
            string breakDebug = queryFetcher.QueryParam_Fetch("debug");

            //if (string.IsNullOrEmpty(breakDebug))
            //{
            //    authCode = GlobalManager.GetGooglePlusCodeExchange(authCode);
            //}

            retJson = authCode;           

            System.Web.HttpContext.Current.Response.Write(retJson);

        }
    }
}