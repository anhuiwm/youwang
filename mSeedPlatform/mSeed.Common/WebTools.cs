using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using ServiceStack.Text;

namespace mSeed.Common
{
    public class WebTools
    {
        public static string GetReqeustURL(string Url, string dataParams, bool doPost = true, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                HttpWebRequest request;
                if (doPost)
                {
                    /* POST */
                    request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Method = "POST";    // 기본값 "GET"
                    request.ContentType = contentType;

                    // request param to byte array for IO stream
                    byte[] byteDataParams = UTF8Encoding.UTF8.GetBytes(dataParams);
                    request.ContentLength = byteDataParams.Length;

                    // reqesut byte array write to IO stream
                    Stream stDataParams = request.GetRequestStream();
                    stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
                    stDataParams.Close();
                }
                else
                {
                    /* GET */
                    request = (HttpWebRequest)WebRequest.Create(Url + "?" + dataParams);
                    request.Method = "GET";
                }

                // 요청, 응답 받기
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 응답 Stream 읽기
                Stream stReadData = response.GetResponseStream();
                StreamReader srReadData = new StreamReader(stReadData, Encoding.Default);
                return srReadData.ReadToEnd();
            }
            catch (Exception ex)
            {
                mSeed.Common.mLogger.mLogger.Debug("http request fail for " + Url + "?" + dataParams + ex.Message, "网址不通");
                return string.Empty;
            }
        }
        
        public string ParamDecrypt(string Value, string encryptKey = CommonEncrypter.baseEncrypt)
        {
            Value = Value.Replace(" ", "+");
            var DecryptString = "";
            if (!string.IsNullOrEmpty(Value)) 
                CommonEncrypter.DecryptData(Value, encryptKey, ref DecryptString);
            return DecryptString;
        }

        public string ParamEncrypt(object obj, bool compression, string encryptKey = CommonEncrypter.baseEncrypt)
        {
            string json = mJsonSerializer.ToJsonString(obj);
            string EncryptString = "";
            if (compression)
                CommonEncrypter.CompressionEncrypt(json, encryptKey, ref EncryptString);
            else
                CommonEncrypter.EncryptData(json, encryptKey, ref EncryptString);

            json = mJsonSerializer.AddJson("{}", DefineError.retEncryptData, EncryptString);
            json = mJsonSerializer.AddJson(json, DefineError.retResult, compression ? "1" : "0");

            return json;
        }
    }
}