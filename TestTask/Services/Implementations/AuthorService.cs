using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        /// <summary>
        /// Returns the author who wrote the book with the longest title.
        /// If there are several such authors, the author with the lowest Id is selected.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Author> GetAuthor()
        {
            var longestBook = await _context.Books
                .OrderByDescending(b => b.Title.Length)
                .ThenBy(b => b.AuthorId)
                .Select(b => new
                {
                    b.Author.Id,
                    b.Author.Name,
                    b.Author.Surname,
                })
                .FirstOrDefaultAsync();

            if (longestBook == null)
                return null;

            return new Author
            {
                Id = longestBook.Id,
                Name = longestBook.Name,
                Surname = longestBook.Surname
            };
        }

        /// <summary>
        /// Returns a list of authors who have written an even number of books published after 2015.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<Author>> GetAuthors()
        {
            var authors = _context.Authors
                .Where(author =>
                    _context.Books
                        .Where(book => book.AuthorId == author.Id && book.PublishDate.Year > 2015)
                        .Count() % 2 == 0)
                .ToListAsync();

            return authors;
        }
    }
}
