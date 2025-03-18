using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RSAInteractJS.Pages
{
    public class RsaModel : PageModel
    {
        private readonly ILogger<RsaModel> _logger;

        public RsaModel(ILogger<RsaModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
