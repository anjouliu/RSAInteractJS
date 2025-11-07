using System.ComponentModel;

namespace RSAInteractJS.Dto
{
    /// <summary>
    /// RSA密钥Input
    /// </summary>
    public class CreateRSAKeyInput
    {
        /// <summary>
        /// 密钥大小，默认1024。有效值: 512, 1024, 2048, 3072, 4096。
        /// 明文最大长度=密钥长度/8-11。
        /// </summary>
        [DefaultValue(1024)]
        public virtual int Size { get; set; }
    }
}
