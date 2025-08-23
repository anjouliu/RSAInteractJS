using CommonLib.Dto;
using CommonLib.RSAUtil;
using Microsoft.AspNetCore.Mvc;
using RSAInteractJS.Dto;

namespace RSAInteractJS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        /// <summary>
        /// 获取安全码(RSA公钥)
        /// </summary>
        [HttpGet]
        [Route("GetSecureCode")]
        public async Task<StringDto> GetSecureCode([FromQuery] CreateRSAKeyInput input)
        {
            RSAKeyPair rsaKey = RSAHelper.GenBase64Key(input.Size);
            var output = new StringDto { Value = rsaKey.PublicKey };

            return await Task.FromResult(output);
        }

        /// <summary>
        /// 创建Baae64密钥(PEM)
        /// </summary>
        [HttpPost]
        [Route("CreateBase64Key")]
        public async Task<RSAKeyPair> CreateBase64Key([FromBody] CreateRSAKeyInput input)
        {
            RSAKeyPair rsaKey = RSAHelper.GenBase64Key(input.Size);

            return await Task.FromResult(rsaKey);
        }

        /// <summary>
        /// 创建XML密钥
        /// </summary>
        [HttpPost]
        [Route("CreateXmlKey")]
        public async Task<RSAKeyPair> CreateXmlKey([FromBody] CreateRSAKeyInput input)
        {
            RSAKeyPair rsaKey = RSAHelper.GenXmlKey(input.Size);
            return await Task.FromResult(rsaKey);
        }

        /// <summary>
        /// 公钥加密
        /// </summary>
        [HttpPost]
        [Route("Encrypt")]
        public async Task<StringDto> Encrypt(CryptInput input)
        {
            var value = RSAHelper.Encrypt(input.Key, input.Content);
            var output = new StringDto { Value = value };

            return await Task.FromResult(output);
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        [HttpPost]
        [Route("Decrypt")]
        public async Task<StringDto> Decrypt(CryptInput input)
        {
            var value = RSAHelper.Decrypt(input.Key, input.Content);
            var output = new StringDto { Value = value };

            return await Task.FromResult(output);
        }

    }
}
