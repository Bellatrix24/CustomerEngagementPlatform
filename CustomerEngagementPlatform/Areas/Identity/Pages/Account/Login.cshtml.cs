using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CustomerEngagementPlatform.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string? ReturnUrl { get; set; }

        public string? Portal { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null, string? portal = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            portal ??= string.Empty;

            // Automatically detect portal from returnUrl if not explicitly specified
            if (string.IsNullOrEmpty(portal))
            {
                if (returnUrl != null && returnUrl.Contains("CustomerPortal", StringComparison.OrdinalIgnoreCase))
                {
                    portal = "customer";
                }
                else if (returnUrl != null && returnUrl.Contains("Dashboard", StringComparison.OrdinalIgnoreCase))
                {
                    portal = "staff";
                }
            }

            // Set default redirections if returnUrl is root or null
            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl == Url.Content("~/"))
            {
                if (portal.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = "/Dashboard";
                }
                else if (portal.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = "/CustomerPortal";
                }
            }

            ReturnUrl = returnUrl;
            Portal = portal;

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null, string? portal = null)
        {
            portal ??= string.Empty;

            // Automatically detect portal from returnUrl if not explicitly specified
            if (string.IsNullOrEmpty(portal))
            {
                if (returnUrl != null && returnUrl.Contains("CustomerPortal", StringComparison.OrdinalIgnoreCase))
                {
                    portal = "customer";
                }
                else if (returnUrl != null && returnUrl.Contains("Dashboard", StringComparison.OrdinalIgnoreCase))
                {
                    portal = "staff";
                }
            }

            // Set default redirections if returnUrl is root or null
            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl == Url.Content("~/"))
            {
                if (portal.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = "/Dashboard";
                }
                else if (portal.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = "/CustomerPortal";
                }
            }

            ReturnUrl = returnUrl;
            Portal = portal;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
