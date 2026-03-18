using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TaskFlow.WebMvc.Views.Shared
{
    public class _PublicLayout : PageModel
    {
        private readonly ILogger<_PublicLayout> _logger;

        public _PublicLayout(ILogger<_PublicLayout> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}