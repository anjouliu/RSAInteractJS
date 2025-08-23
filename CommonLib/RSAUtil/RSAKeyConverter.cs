using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using System.Xml;

namespace CommonLib.RSAUtil
{
    /// <summary>
    /// RSA 密钥转换。XML、PEM格式密钥互转
    /// </summary>
    public class RSAKeyConverter
    {
        //  PEM 公钥前缀、后缀
        private const string pemPublicKeyPrefix = "-----BEGIN PUBLIC KEY-----";
        private const string pemPublicKeyPostfix = "-----END PUBLIC KEY-----";
        private const string pemPrivateKeyPrefix1 = "-----BEGIN RSA PRIVATE KEY-----";
        //  PEM 私钥前缀、后缀
        private const string pemPrivateKeyPrefix2 = "-----BEGIN PRIVATE KEY-----";
        private const string pemPrivateKeyPostfix1 = "-----END RSA PRIVATE KEY-----";
        private const string pemPrivateKeyPostfix2 = "-----END PRIVATE KEY-----";

        #region 公钥转换
        /// <summary>
        /// XML 公钥转 Base64（公钥）
        /// </summary>
        public static string XmlPublicKeyToBase64(string xmlPublicKey)
        {
            XmlDocument doc = new();
            doc.LoadXml(xmlPublicKey);
            BigInteger m = new(1, Convert.FromBase64String(doc.DocumentElement!.GetElementsByTagName("Modulus")[0]!.InnerText));
            BigInteger p = new(1, Convert.FromBase64String(doc.DocumentElement!.GetElementsByTagName("Exponent")[0]!.InnerText));
            RsaKeyParameters pub = new(false, m, p);
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Convert.ToBase64String(serializedPublicBytes);
        }

        /// <summary>
        /// Base64 公钥转 XML（公钥）
        /// </summary>
        public static string Base64PublicKeyToXml(string base64PublicKey)
        {
            base64PublicKey = base64PublicKey.Trim('\r', '\n');
            if (base64PublicKey.IndexOf(pemPublicKeyPrefix) > -1)
            {
                base64PublicKey = base64PublicKey[pemPublicKeyPrefix.Length..];
            }
            if (base64PublicKey.IndexOf(pemPublicKeyPostfix) > -1)
            {
                base64PublicKey = base64PublicKey[..^pemPublicKeyPostfix.Length];
            }

            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory
                .CreateKey(Convert.FromBase64String(base64PublicKey));

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                                 Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                                 Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }
        #endregion

        #region 私钥转换
        /// <summary>
        /// XML 私钥转 Base64（私钥）
        /// </summary>
        public static string XmlPrivateKeyToBase64(string xmlPrivateKey)
        {
            XmlDocument doc = new();
            doc.LoadXml(xmlPrivateKey);
            BigInteger m = new(1, Convert.FromBase64String(doc.DocumentElement!.GetElementsByTagName("Modulus")[0]!.InnerText));
            BigInteger exp = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0]!.InnerText));
            BigInteger d = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0]!.InnerText));
            BigInteger p = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0]!.InnerText));
            BigInteger q = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0]!.InnerText));
            BigInteger dp = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0]!.InnerText));
            BigInteger dq = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0]!.InnerText));
            BigInteger qinv = new(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0]!.InnerText));
            RsaPrivateCrtKeyParameters privateKeyParam = new(m, exp, d, p, q, dp, dq, qinv);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            return Convert.ToBase64String(serializedPrivateBytes);
        }

        /// <summary>
        /// Base64 私钥转 XML（私钥）
        /// </summary>
        public static string Base64PrivateKeyToXml(string base64PrivateKey)
        {
            base64PrivateKey = base64PrivateKey.Trim('\r', '\n');
            if (base64PrivateKey.IndexOf(pemPrivateKeyPrefix1) > -1)
            {
                base64PrivateKey = base64PrivateKey[pemPrivateKeyPrefix1.Length..];
            }
            else if (base64PrivateKey.IndexOf(pemPrivateKeyPrefix2) > -1)
            {
                base64PrivateKey = base64PrivateKey[pemPrivateKeyPrefix2.Length..];
            }
            if (base64PrivateKey.IndexOf(pemPrivateKeyPostfix1) > -1)
            {
                base64PrivateKey = base64PrivateKey[..^pemPrivateKeyPostfix1.Length];
            }
            else if (base64PrivateKey.IndexOf(pemPrivateKeyPostfix2) > -1)
            {
                base64PrivateKey = base64PrivateKey[..^pemPrivateKeyPostfix2.Length];
            }

            try
            {
                var keyBytes = Convert.FromBase64String(base64PrivateKey);
                RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(keyBytes);
                return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                                     Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                                     Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region 从私钥提取公钥
        /// <summary>
        /// 根据XML私钥获取公钥
        /// </summary>
        public static string GetXmlPublicKeyFromPrivateKey(string xmlPrivateKey)
        {
            using var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(xmlPrivateKey);
            return rsaProvider.ToXmlString(false);
        }

        /// <summary>
        /// 根据Base64私钥获取公钥
        /// </summary>
        public static string GetBase64PublicKeyFromPrivateKey(string pemPrivateKey)
        {
            using var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(Base64PrivateKeyToXml(pemPrivateKey));
            return XmlPublicKeyToBase64(rsaProvider.ToXmlString(false));
        }
        #endregion
    }
}
