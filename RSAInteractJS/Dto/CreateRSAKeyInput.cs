using System.ComponentModel;

namespace RSAInteractJS.Dto
{
    public class CreateRSAKeyInput
    {
        [DefaultValue(1024)]
        public virtual int Size { get; set; }
    }
}
