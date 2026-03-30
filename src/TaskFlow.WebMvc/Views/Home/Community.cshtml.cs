using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TaskFlow.WebMvc.Views.Home
{
    public class Community : PageModel
    {
        private readonly ILogger<Community> _logger;

        public Community(ILogger<Community> logger)
        {
            _logger = logger;
        }

        public void OnGet() { }
    }
}
