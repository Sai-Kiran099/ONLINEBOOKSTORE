
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEBOOKSTORE.Data;
using ONLINEBOOKSTORE.Models;
using System.Linq;
using System.Threading.Tasks;




public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    //  Action to Add Item to Cart
    public async Task<IActionResult> AddToCart(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); // Redirect if not logged in
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound(); // If book doesn't exist, return error
        }

        // Check if item is already in the cart
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.BookId == id && c.UserId == user.Id);

        if (cartItem != null)
        {
            cartItem.Quantity += 1;  // If book is already in cart, increase quantity
        }
        else
        {
            // If not, add a new entry
            cartItem = new CartItem
            {
                UserId = user.Id,
                BookId = book.Id,
                Title = book.Title,
                Price = (decimal)(decimal?)book.Price,
                Quantity = 1,
                ImageUrl = book.ImageUrl
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Cart");  // Redirect to Cart Page after adding item
    }

    // Action to Display Cart Items
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = await _context.CartItems
            .Where(c => c.UserId == user.Id)
            .ToListAsync();

        return View(cartItems);
    }


    //  Action to Remove Item from Cart
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.BookId == id && c.UserId == user.Id);
        //retrives the first matching cart item from the database if no match found returns null
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");  // Redirect back to cart after removing item
    }
    [HttpPost]
    public async Task<IActionResult> IncreaseQuantity(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.BookId == id && c.UserId == user.Id);
        if (cartItem != null)
        {
            cartItem.Quantity += 1;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    // Action to Decrease Quantity of Item in Cart
    [HttpPost]
    public async Task<IActionResult> DecreaseQuantity(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.BookId == id && c.UserId == user.Id);
        if (cartItem != null && cartItem.Quantity > 1)
        {
            cartItem.Quantity -= 1;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}

