using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ryeol.Security.Cryptography;

public partial class Encrypt_Decrypt : System.Web.UI.Page
{
    const string key = "dpaTlem!@#";
    const string iv = "gnlahfdkclsmsqkfka";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            // 인스턴스 만들기
            StringEncrypter encrypter;
            StringEncrypter decrypter;

            var encryptkey = Request.Params["encryptkey"];
            var encryptdata = Request.Params["encryptdata"];
            var decryptkey = Request.Params["decryptkey"];
            var decryptdata = Request.Params["decryptdata"];


            if (encryptkey == "")
            {
                encrypter = new StringEncrypter(key, iv);
            }
            else
            {
                encrypter = new StringEncrypter(encryptkey, iv);
            }

            if (decryptkey == "")
            {
                decrypter = new StringEncrypter(key, iv);
            }
            else
            {
                decrypter = new StringEncrypter(decryptkey, iv);
            }


            // 키 해시 알고리즘 설정. MD5 와 SHA2-256 을 지원합니다.
            // 아래 코드는 해시 크기가 256 비트이기 때문에, AES-256 이 사용됩니다.
            encrypter.KeyHashAlgorithm = StringEncrypterKeyHashAlgorithm.SHA2_256;
            decrypter.KeyHashAlgorithm = StringEncrypterKeyHashAlgorithm.SHA2_256;

            if (Request.Params["encryptdata"] != null && Request["encryptdata"] != "")
            {
                // 문자열 암호화
                string encrypted = encrypter.Encrypt(encryptdata);
                Response.Write("EncryptData : " + encrypted + "<br><br>");
            }
            if (Request.Params["decryptdata"] != null && Request["decryptdata"] != "")
            {
                // 문자열 복호화
                string decrypted = decrypter.Decrypt(decryptdata);
                Response.Write("DecryptData : " + decrypted);
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}