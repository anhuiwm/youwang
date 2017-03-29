using System;
using System.Security.Cryptography;
using System.Text;

namespace mSeed.Common
{
    /// <summary>
    /// Provide Unity-compatible AES-256 (Block 128) crypto-service.
    /// </summary>
    public class CryptProvider
    {
        private static readonly byte[] EMPTY_KEY = { 0, 0, 0, 0, 0, 0, 0, 0,    /* 256 bits = 32 bytes */
                                                     0, 0, 0, 0, 0, 0, 0, 0,
                                                     0, 0, 0, 0, 0, 0, 0, 0,
                                                     0, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] EMPTY_IV = { 0, 0, 0, 0, 0, 0, 0, 0,    /* 128 bits = 16 bytes */
                                                     0, 0, 0, 0, 0, 0, 0, 0 };

        private RijndaelManaged algorithm;

        private byte[] key;
        private byte[] iv;


        #region Constructors
        public CryptProvider(string base64_key, string base64_iv)
        {
            this.algorithm = new RijndaelManaged();
            this.algorithm.Mode = CipherMode.CBC;       // Cipher Block Chaining
            this.algorithm.Padding = PaddingMode.PKCS7; // Pad with added padding length
            this.algorithm.KeySize = 256;
            this.algorithm.BlockSize = 128;

            SetKey(base64_key);
            SetIV(base64_iv);
        }

        public CryptProvider() : this(null, null) { }
        #endregion


        public void SetKey(string base64str)
        {
            this.key = (String.IsNullOrEmpty(base64str) ? EMPTY_KEY : Convert.FromBase64String(base64str));
        }

        public void SetIV(string base64str)
        {
            this.iv = (String.IsNullOrEmpty(base64str) ? EMPTY_IV : Convert.FromBase64String(base64str));
        }

        public byte[] RawEncrypt(byte[] raw)
        {
            // 매번 Key, IV를 다시 세팅합니다.
            using (ICryptoTransform transform = algorithm.CreateEncryptor(key, iv))
            {
                return transform.TransformFinalBlock(raw, 0, raw.Length);
            }
        }

        public byte[] RawDecrypt(byte[] raw)
        {
            // 매번 Key, IV를 다시 세팅합니다.
            using (ICryptoTransform transform = algorithm.CreateDecryptor(key, iv))
            {
                return transform.TransformFinalBlock(raw, 0, raw.Length);
            }
        }

        /*
         * 암호화 모듈이 지원해줘야 하는 동작:
         *  (1) json을 암호화 (string -> base64-string)
         *  (2) json을 압축하고 암호화 (byte[] -> base64-string)
         *
         * 암호화는 항상 최종 단계에서 이루어지므로 최종 사용 형태인 base64-string으로만 출력
         * 단, 내부적으로 사용할 byte[] -> byte[]는 만들어둔다.
         */

        #region Encryption
        private string InternalEncrypt(string str, Encoding encoding, bool compress)
        {
            byte[] plain = encoding.GetBytes(str);

            if (compress)
            {
                plain = SimpleCompressor.Unchecked.Compress(plain);
            }

            byte[] cipher = RawEncrypt(plain);
            return Convert.ToBase64String(cipher);
        }

        /// <summary>
        /// 문자열 (Serialized 된 json 등)을 암호화합니다. Decrypt() 으로 복호화합니다.
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="encoding">인코딩</param>
        /// <returns>암호화 된 Base64 문자열</returns>
        public string Encrypt(string str, Encoding encoding) { return InternalEncrypt(str, encoding, false); }

        /// <summary>
        /// UTF-8 인코딩으로 문자열 (Serialized 된 json 등)을 암호화합니다. Decrypt() 으로 복호화합니다.
        /// </summary>
        /// <param name="str">문자열</param>
        /// <returns>암호화 된 Base64 문자열</returns>
        public string Encrypt(string str) { return InternalEncrypt(str, Encoding.UTF8, false); }

        /// <summary>
        /// 문자열을 압축 후 암호화합니다. DecryptInflation() 으로 복호화합니다.
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="encoding">인코딩</param>
        /// <returns>압축되고 암호화 된 Base64 문자열</returns>
        public string EncryptDeflation(string str, Encoding encoding) { return InternalEncrypt(str, encoding, true); }

        /// <summary>
        /// UTF-8 인코딩으로 문자열을 압축 후 암호화합니다. DecryptInflation() 으로 복호화합니다.
        /// </summary>
        /// <param name="str">문자열</param>
        /// <returns>압축되고 암호화 된 Base64 문자열</returns>
        public string EncryptDeflation(string str) { return InternalEncrypt(str, Encoding.UTF8, true); }
        #endregion

        #region Decryption
        private string InternalDecrypt(string base64str, Encoding encoding, bool compress)
        {
            byte[] cipher = Convert.FromBase64String(base64str);
            byte[] plain = RawDecrypt(cipher);

            if (compress)
            {
                plain = SimpleCompressor.Unchecked.Decompress(plain);
            }

            return encoding.GetString(plain);
        }

        /// <summary>
        /// Base64 문자열을 복호화해서 돌려줍니다. (EncryptString() 으로 암호화했던 것을 풀어줍니다.)
        /// </summary>
        /// <param name="base64str">암호화 된 Base64 문자열</param>
        /// <param name="encoding">인코딩</param>
        /// <returns>복호화 된 원래 문자열</returns>
        public string Decrypt(string base64str, Encoding encoding) { return InternalDecrypt(base64str, encoding, false); }

        /// <summary>
        /// Base64 문자열을 복호화해서 UTF-8 인코딩 문자열로 돌려줍니다. (EncryptString() 으로 암호화했던 것을 풀어줍니다.)
        /// </summary>
        /// <param name="base64str">암호화 된 Base64 문자열</param>
        /// <returns>복호화 된 원래 문자열</returns>
        public string Decrypt(string base64str) { return InternalDecrypt(base64str, Encoding.UTF8, false); }

        /// <summary>
        /// Base64 문자열을 복호화하고, 압축도 풀어서 돌려줍니다. (EncryptCompressed() 으로 암호화했던 것을 풀어줍니다.)
        /// </summary>
        /// <param name="base64str">암호화 된 Base64 문자열</param>
        /// <param name="encoding">인코딩</param>
        /// <returns>복호화 된 원래 문자열</returns>
        public string DecryptInflation(string base64str, Encoding encoding) { return InternalDecrypt(base64str, encoding, true); }

        /// <summary>
        /// Base64 문자열을 복호화하고, 압축도 풀어서 UTF-8 인코딩으로 돌려줍니다. (EncryptCompressed() 으로 암호화했던 것을 풀어줍니다.)
        /// </summary>
        /// <param name="base64str">암호화 된 Base64 문자열</param>
        /// <returns>복호화 된 원래 문자열</returns>
        public string DecryptInflation(string base64str) { return InternalDecrypt(base64str, Encoding.UTF8, true); }
        #endregion
    }
}
