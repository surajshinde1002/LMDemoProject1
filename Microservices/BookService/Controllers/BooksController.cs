using BookService.Data;
using BookService.Models.EntityModels;
using BookService.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _db;
        public BooksController(BookDbContext db) { 
            _db = db;
        }

        //Get /api/books/all       
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        public ActionResult<List<Book>> GetAll() 
        {
           return _db.Books.ToList();          
        }

        //Get /api/books/1
        [HttpGet("{id}",Name ="GetByRoute")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(Book))]
        public ActionResult<Book> GetById(int id)
        {
            var book = _db.Books.Find(id);
            return book == null ? NotFound() : Ok(book);
        }


        //Post /api/books
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User")]
        public ActionResult<Book> Create(BookRequestModel model)
        {
            //TODO : Validate the book object and save the object .
            TryValidateModel(model);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Price = model.price,
                Genre = model.Genre,
                Pages = model.Pages
            };
            _db.Books.Add(book);
            _db.SaveChanges();
            //return CreatedAtAction("GetById", new{id = book.Id},book);

            return CreatedAtRoute("GetByRoute", new { id = book.Id }, book);
        }

    }
}
