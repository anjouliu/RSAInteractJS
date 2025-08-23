namespace CommonLib.Dto
{
    /// <summary>
    /// RSA密钥
    /// </summary>
    public class RSAKeyPair(string publickey = "", string privatekey = "")
    {
        public string PublicKey { get; set; } = publickey;

        public string PrivateKey { get; set; } = privatekey;

        public override string ToString()
        {
            return string.Format(
                "PrivateKey: {0}\r\nPublicKey: {1}", PrivateKey, PublicKey);
        }
    }
}
