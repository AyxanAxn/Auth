using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public AccountController(IAuthorizationService authorizationService)
        {
            this._authorizationService = authorizationService;
        }
        [Authorize(Roles = "User")]
        public IActionResult Secret()
        {

            var allIdentities = User.Identities;
            //Current user's name:
            var currentUser = User.FindFirst(i => i.Type == ClaimTypes.Name).Value;


            return View();
        }
        //add requirement in scope
        public async Task<IActionResult> CostomClaimBase()
        {
            int admin = 5;
            int user = 10;
            var adminPolicy = new AuthorizationPolicyBuilder().RequireRole("Admin").Build();
            var userPolicy = new AuthorizationPolicyBuilder().RequireRole("User").Build();
            var result = await _authorizationService.AuthorizeAsync(User, adminPolicy);
            if (result.Succeeded)
            {
                Console.WriteLine(admin);
            }
            else
            {
                result = await _authorizationService.AuthorizeAsync(User, userPolicy);

                if (result.Succeeded)
                {
                    Console.WriteLine(user);
                }
            }
            return View();
        }

        [Authorize(Policy = "AgeRequirement")]
        public IActionResult ClaimBase()
        {
            return View();
        }
        public async Task<IActionResult> Authenticate()
        {
            var claim = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier,"5"),
                new Claim(ClaimTypes.Name,"Ayxan"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"User"),
                new Claim(ClaimTypes.DateOfBirth,DateTime.Now.AddYears(-21).ToString()),
            };

            var driverClaims = new List<Claim>() {
                new Claim("Driver licence id","AZ1321141"),
                new Claim("Drver licence type","B,c"),
            };
            var passportIdentity = new ClaimsIdentity(claim, "CookieAuth");
            var driverLicenceIdentity = new ClaimsIdentity(driverClaims, "CookieAuth");


            var principle = new ClaimsPrincipal(new[] { passportIdentity, driverLicenceIdentity });
            await HttpContext.SignInAsync("CookieAuth", principle);
            return RedirectToAction("Index", "Home");
        }
    }
}