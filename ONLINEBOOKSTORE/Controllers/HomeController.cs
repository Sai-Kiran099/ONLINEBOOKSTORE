using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ONLINEBOOKSTORE.Data;
using ONLINEBOOKSTORE.Models;
using Microsoft.Extensions.Logging;
using System.Linq;


//using ONLINEBOOKSTORE.Services;

namespace ONLINEBOOKSTORE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public  IActionResult Index(string searchtitle)
    {
        var books = _context.Books.ToList();
        if (!string.IsNullOrEmpty(searchtitle))
        {
            books = books.Where(b => b.Title.Contains(searchtitle)).ToList();
        }
        return View(books);

    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
