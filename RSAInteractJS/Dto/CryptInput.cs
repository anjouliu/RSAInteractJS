namespace RSAInteractJS.Dto
{
    /// <summary>
    /// 加密、解密Input
    /// </summary>
    public class CryptInput()
    {
        public string Key { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
