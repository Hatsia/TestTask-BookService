using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        /// <summary>
        /// Get a single book with the highest published cost.
        /// </summary>
        /// <returns></returns>
        public async Task<Book> GetBook() => await _context.Books.OrderByDescending(b => b.Price * b.QuantityPublished).FirstOrDefaultAsync();

        /// <summary>
        /// Get a list of books that contain "Red" in the title and are published after "Carolus Rex" album release.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Book>> GetBooks()
        {
            DateTime carolusRexReleaseDate = new DateTime(2012, 5, 25);

            return await _context.Books
                .Where(b => b.Title.Contains("Red") && b.PublishDate > carolusRexReleaseDate)
                .ToListAsync();
        }
    }
}
