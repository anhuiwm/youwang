using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ryeol.Security.Cryptography;
using System.Security.Cryptography;
using NAMU;

/// <summary>
/// TheSoulEncrypt의 요약 설명입니다.
/// </summary>
public static class TheSoulEncrypt
{
    public const string base64_key = "Sy4yS4erGZz4gyr053rtznBhQP9z1Im/xmsl78ydO7g=";    // "dpaTlem!@#"  -> to BASE64 : 엠씨드!@#\
    const string key = "dpaTlem!@#";    // 엠씨드!@#
    const string iv = "ToN80QS/DWc/EkzT8AUapg=="; // 128bit ->  base 64 string
    public const string baseEncrypt = key;

    public static string CreateKey_IV()
    {
        AesCryptoServiceProvider AESCS = new AesCryptoServiceProvider();
        AESCS.BlockSize = 128;
        AESCS.KeySize = 256;
        AESCS.GenerateKey();
        return Convert.ToBase64String(AESCS.Key);
        //string AESKey = Convert.ToBase64String(AESCS.Key);
        //return AESKey;

        //AESCS.GenerateIV();
        //string AESIv = Convert.ToBase64String(AESCS.IV);

        //AESKey = Convert.ToBase64String(AESCS.Key);
        //AESIv = Convert.ToBase64String(AESCS.IV);

    }

    public static void EncryptData(int reqtype, string reqval, string varkey, ref string val)
	{
        StringEncrypter encrypter;
        // 인스턴스 만들기
        //if (varkey == "0")
        if (varkey == baseEncrypt)
        {
            encrypter = new StringEncrypter(key, iv);
        }
        else
        {
            encrypter = new StringEncrypter(varkey, iv);
        }
        // 키 해시 알고리즘 설정. MD5 와 SHA2-256 을 지원합니다.
        // 아래 코드는 해시 크기가 256 비트이기 때문에, AES-256 이 사용됩니다.
        encrypter.KeyHashAlgorithm = StringEncrypterKeyHashAlgorithm.SHA2_256;

        if (reqtype == 0)
        {
            val = encrypter.Encrypt(reqval);
        }
        else
        {
            // 특수문자를 다시 텍스트로 디코딩
            //string htmlDecodeString = HttpUtility.UrlDecode(reqval);
            //htmlDecodeString = htmlDecodeString.Replace(" ", "+");        // don't use replace encrypt string (why use this?) , instead use urlencode for encrypt base 64 string
            val = encrypter.Decrypt(reqval);
        }
	}

    public static void EncryptData(int reqtype, string reqval, string varkey, ref byte[] val, ref string retVal)
    {
        StringEncrypter encrypter;
        // 인스턴스 만들기
        //if (varkey == "0")
        if (varkey == baseEncrypt)
        {
            encrypter = new StringEncrypter(key, iv);
        }
        else
        {
            encrypter = new StringEncrypter(varkey, iv);
        }
        // 키 해시 알고리즘 설정. MD5 와 SHA2-256 을 지원합니다.
        // 아래 코드는 해시 크기가 256 비트이기 때문에, AES-256 이 사용됩니다.
        encrypter.KeyHashAlgorithm = StringEncrypterKeyHashAlgorithm.SHA2_256;

        //if (reqtype == 0)
        //{
        //    val = encrypter.Encrypt(reqval);
        //}
        //else
        //{
        //    // 특수문자를 다시 텍스트로 디코딩
        //    //string htmlDecodeString = HttpUtility.UrlDecode(reqval);
        //    //htmlDecodeString = htmlDecodeString.Replace(" ", "+");        // don't use replace encrypt string (why use this?) , instead use urlencode for encrypt base 64 string
        //    val = encrypter.Decrypt(reqval);
        //}
    }

    public static void DecryptDataOld(string reqval, string varkey, ref string val)
    {
        TheSoulEncrypt.EncryptData(1, reqval, varkey, ref val);
    }

    public static void EncryptDataOld(string reqval, string varkey, ref string val)
    {
        TheSoulEncrypt.EncryptData(0, reqval, varkey, ref val);
    }

    // Explicit definition for use - add by manstar 2015/06/03
    public static void DecryptData(string reqval, string varkey, ref string val)
    {
        CryptProvider cp = new CryptProvider(varkey, iv);
        val = cp.Decrypt(reqval);        
        //TheSoulEncrypt.EncryptData(1, reqval, varkey, ref val);
    }


    public static void EncryptData(string reqval, string varkey, ref string val)
    {
        CryptProvider cp = new CryptProvider(varkey, iv);
        val = cp.Encrypt(reqval);

        //TheSoulEncrypt.EncryptData(0, reqval, varkey, ref val);
    }

    public static void CompressionDecrypt(string reqval, string varkey, ref string val)
    {
        CryptProvider cp = new CryptProvider(varkey, iv);
        val = cp.DecryptInflation(reqval);
    }

    public static void CompressionEncrypt(string reqval, string varkey, ref string val)
    {
        CryptProvider cp = new CryptProvider(varkey, iv);
        val = cp.EncryptDeflation(reqval);
    }
}