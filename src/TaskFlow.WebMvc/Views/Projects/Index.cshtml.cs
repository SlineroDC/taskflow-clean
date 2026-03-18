using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TaskFlow.WebMvc.Views.Projects
{
    public class Index(ILogger<Index> logger) : PageModel
    {
        private readonly ILogger<Index> _logger = logger;

        public void OnGet() { }
    }
}
