using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Web;

using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using mSeed.Platform.Cache;
using ServiceStack.Text;

namespace mSeed.Platform
{   
    public partial class FCMManager
    {
        //private const string pushKey = "AIzaSyCWwS0nsePTsR32XxM-G26Ft6M4dEW4uIg";
        private const string pushKey = "AIzaSyDW4HBgOGR74YxwxaFHPLAjNZ1-48W21Pw";
        //private const string pushKey = "AIzaSyATcp9GUUu-hZMaY11WPPybBaZ_hZ9F1DE";
        private const string FCM_SendURL = "https://fcm.googleapis.com/fcm/send";

        public static string SendToAOS(List<string> ids, Dictionary<string, string> message)
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(FCM_SendURL);

            request.Method = "POST";    // 기본값 "GET"
            request.Headers.Add(string.Format("Authorization: key={0}", pushKey));
            request.ContentType = "application/json";

            JsonObject msgBody = new JsonObject();
            //msgBody["to"] = ids.First();
            msgBody["registration_ids"] = JsonSerializer.SerializeToString(ids);
            msgBody["notification"] = JsonSerializer.SerializeToString(message);
            string jsonbody = msgBody.ToJson();

            // request param to byte array for IO stream
            byte[] byteDataParams = UTF8Encoding.UTF8.GetBytes(jsonbody);
            request.ContentLength = byteDataParams.Length;

            // reqesut byte array write to IO stream
            Stream stDataParams = request.GetRequestStream();
            stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
            stDataParams.Close();

            string retBody = "";
            // SEND MESSAGE
            try
            {
                WebResponse Response = request.GetResponse();
                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                //if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                //{
                //    var text = "Unauthorized - need new token";
                //}
                //else if (!ResponseCode.Equals(HttpStatusCode.OK))
                //{
                //    var text = "Response from web service isn't OK";
                //}

                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                retBody = Reader.ReadToEnd();
                Reader.Close();
            }
            catch (Exception e)
            {
                mSeed.Common.mLogger.mLogger.Debug(e.Message, "FCM");
                return e.Message;
            }

            return retBody;
        }
    }
}
