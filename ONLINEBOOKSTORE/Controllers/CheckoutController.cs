using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEBOOKSTORE.Data; 
using ONLINEBOOKSTORE.Models; 



public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CheckoutController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Checkout()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        var cartItems = await _context.CartItems
            .Include(c => c.Book)
            .Where(c => c.UserId == user.Id)
            .ToListAsync();

        var viewModel = new CheckoutViewModel { CartItems = cartItems };//created new instance to pass the required data to the view

        return View(viewModel);
    }

    public IActionResult PaymentAddress()
    {
        return View();
    }

    public IActionResult Summary()
    {
        return View();
    }
}