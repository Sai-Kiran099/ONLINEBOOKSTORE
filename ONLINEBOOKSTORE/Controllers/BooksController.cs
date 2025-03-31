using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEBOOKSTORE.Data;
using ONLINEBOOKSTORE.Models;

namespace ONLINEBOOKSTORE.Controllers

{
    [Authorize(Policy = "Admin")]
    public class BooksController : Controller
    {

        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET:Books/create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST:Books/create
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(book);
        }

        //To print books in list[Read]
        public IActionResult Read()
        {
            //Fetch all books from the database
            var books = _context.Books.ToList();
            return View(books);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Book book)
        {

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            var Book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                Book.Author = book.Author;
                Book.Description = book.Description;
                Book.Title = book.Title;
                Book.Price = book.Price;
                Book.ImageUrl = book.ImageUrl;

                _context.Books.Update(Book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Read", "Books");
            }
            return NotFound("Not Found");
        }

        public async Task<IActionResult> Delete(Book viewmodel)
        {
            var book = await _context.Books
                .AsNoTracking() //tells enitity frameworkcore to do not track
                .FirstOrDefaultAsync(x => x.Id == viewmodel.Id); //finds the first book that matches with the user id
            if (book is not null)
            {
                _context.Books.Remove(viewmodel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Read", "Books");
        }



    }
}
