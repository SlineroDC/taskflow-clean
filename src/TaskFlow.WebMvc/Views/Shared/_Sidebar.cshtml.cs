using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TaskFlow.WebMvc.Views.Shared
{
    public class _Sidebar(ILogger<_Sidebar> logger) : PageModel
    {
        private readonly ILogger<_Sidebar> _logger = logger;

        public void OnGet() { }
    }
}
