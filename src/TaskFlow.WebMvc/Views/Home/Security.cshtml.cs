using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TaskFlow.WebMvc.Views.Home
{
    public class Security : PageModel
    {
        private readonly ILogger<Security> _logger;

        public Security(ILogger<Security> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}