using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ONLINEBOOKSTORE.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace ONLINEBOOKSTORE.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)   // Constructer for the values that include in the readonly method

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        // Get:Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }

        //Post:Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))  //checks if admin role exists if not then it will create admin role
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    //assign the role
                    //string role = model.Role ?? "User"; //Default
                    //if (role == "Admin" && !User.IsInRole("Admin"))
                    //{
                    //    ModelState.AddModelError("", "Only for Admin");
                    //    return View(model);
                    //}
                    //await _userManager.AddToRoleAsync(user, role);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Name),
                        new Claim(ClaimTypes.Email,model.Email),
                        new Claim(ClaimTypes.Role,"User")// custom role default im taking as user...
 
                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    await _signInManager.SignInAsync(user, isPersistent:false);//The session will not persist across browser sessions. This means the user will be signed out when they close their browser.
                    return RedirectToAction("Login", "");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // Get user claims
                        var userClaims = await _userManager.GetClaimsAsync(user);
                        var roles = await _userManager.GetRolesAsync(user);

                        // Create Claims Identity
                        var claimsIdentity = new ClaimsIdentity(userClaims, "ApplicationCookie");
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, model.Email));
                        foreach (var role in roles)
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }

                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);// Sign in with claims stores in the cookie

                        // Redirect based on role
                        if (roles.Contains("Admin"))
                            return RedirectToAction("Read", "Books");

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
      

    }
}