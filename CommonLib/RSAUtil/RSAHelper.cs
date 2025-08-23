using CommonLib.Dto;
using System.Security.Cryptography;
using System.Text;

namespace CommonLib.RSAUtil
{
    /// <summary>
    /// RSA 密钥生成及加解密
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 生成Base64密钥(PEM)
        /// </summary>
        public static RSAKeyPair GenBase64Key(int keySize = 2048)
        {
            RSAKeyPair rsaKey = GenXmlKey(keySize);

            rsaKey.PublicKey = RSAKeyConverter.XmlPublicKeyToBase64(rsaKey.PublicKey);
            rsaKey.PrivateKey = RSAKeyConverter.XmlPrivateKeyToBase64(rsaKey.PrivateKey);

            return rsaKey;
        }

        /// <summary>
        /// 生成XML密钥
        /// </summary>
        public static RSAKeyPair GenXmlKey(int keySize = 2048)
        {
            using var rsaProvider = new RSACryptoServiceProvider(keySize);
            var publicXmlKey = rsaProvider.ToXmlString(false);
            var privateXmlKey = rsaProvider.ToXmlString(true);
            return new RSAKeyPair(publicXmlKey, privateXmlKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        public static string Encrypt(string base64PublicKey, string content)
        {
            return EncryptByXmlKey(RSAKeyConverter.Base64PublicKeyToXml(base64PublicKey), content);
        }

        /// <summary>
        /// 加密
        /// </summary>
        public static string EncryptByXmlKey(string xmlPublicKey, string content)
        {
            using RSACryptoServiceProvider rsaProvider = new();
            rsaProvider.FromXmlString(xmlPublicKey);
            byte[] encrypteddata = rsaProvider.Encrypt(Encoding.Default.GetBytes(content), false);
            return Convert.ToBase64String(encrypteddata);
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static string Decrypt(string base64PrivateKey, string content)
        {
            return DecryptByXmlKey(RSAKeyConverter.Base64PrivateKeyToXml(base64PrivateKey), content);
        }

        /// <summary>
        /// 解密
        /// </summary>
        public static string DecryptByXmlKey(string xmlPrivateKey, string content)
        {
            using RSACryptoServiceProvider rsaProvider = new();
            rsaProvider.FromXmlString(xmlPrivateKey);
            byte[] decryptedData = rsaProvider.Decrypt(Convert.FromBase64String(content), false);
            return Encoding.UTF8.GetString(decryptedData);
        }

        #region 签名
        /// <summary>
        /// 签名
        /// </summary>
        public static string Sign(string base64PrivateKey, string content, string hashName = "SHA256")
        {
            return SignByXmlKey(RSAKeyConverter.Base64PrivateKeyToXml(base64PrivateKey), content, hashName);
        }

        /// <summary>
        /// 签名
        /// </summary>
        public static string SignByXmlKey(string xmlPrivateKey, string content, string hashName = "SHA256")
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            using System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();
            rsa.FromXmlString(xmlPrivateKey);
            contentBytes = rsa.SignData(contentBytes, new HashAlgorithmName(hashName), RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(contentBytes);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        public static bool Verify(string base64PublicKey, string content, string signedData, string hashName = "SHA256")
        {
            return VerifyByXmlKey(RSAKeyConverter.Base64PublicKeyToXml(base64PublicKey), content, signedData, hashName);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        public static bool VerifyByXmlKey(string xmlPublicKey, string content, string signedData, string hashName = "SHA256")
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] signedDataBytes = Convert.FromBase64String(signedData);
            using RSA rsa = RSA.Create();
            rsa.FromXmlString(xmlPublicKey);
            var ok = rsa.VerifyData(contentBytes, signedDataBytes, new HashAlgorithmName(hashName), RSASignaturePadding.Pkcs1);
            return ok;
        }

        #endregion
    }
}
