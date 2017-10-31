using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;
using Newtonsoft.Json;

namespace LibraryAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private AppDataContext _db;
        private readonly LibraryRepository _libRepo;

        public BookRepository(AppDataContext db){    
            _db = db;
            _libRepo = new LibraryRepository(db);
            _libRepo.OnStart();
        }
        
    ///<summary>
	///Returns all books in the database 
    ///(maybe it should only return books that are not being borrowed)
	/// </summary>
        public IEnumerable<BookViewModel> GetAllBooks(DateTime? LoanDate){     
            List<BookViewModel> books;     
            if(LoanDate == null){
                books = (from b in _db.Books
                            select new BookViewModel{
                                Title = b.Title,
                                Author = b.FirstName + " " + b.LastName,
                                DatePublished = b.DatePublished
                            }).OrderBy(x => x.Title).ToList();
            }
            else {
                DateTime dt = LoanDate.Value;
                books = (from b in _db.Books
                            join l in _db.Loans on b.ID equals l.bookID
                            where l.hasReturned == false
                            where DateTime.Compare(dt, l.DateBorrowed) < 0
                            select new BookViewModel{
                                Title = b.Title,
                                Author = b.FirstName + " " + b.LastName,
                                DatePublished = b.DatePublished
                                }).ToList();          
            }
            if( books == null){
                    throw new ObjectNotFoundException("No books found");
            }
            return books;
        }

    /// <summary>
	/// Returns a single book with the given ID from the database
	/// </summary>
        public BookDetailsViewModel GetBookByID(int book_id){
            var book = (from b in _db.Books
                        where b.ID == book_id
                        select new BookDetailsViewModel{
                            Title = b.Title,
                            Author = b.FirstName + " " + b.LastName,
                            DatePublished = b.DatePublished,
                            ISBN = b.ISBN,
                            loanHistory = (from l in _db.Loans
                                            where l.bookID == book_id
                                            select l).ToList()
                        }).SingleOrDefault();
            if(book == null){
                throw new ObjectNotFoundException("Book ID not found");
            }
            else{
                return book;
            }
        }

    /// <summary>
	/// Adds a new book to the database
    /// ADMIN function
	/// </summary>
        public Book AddNewBook(Book newBook){
            if(newBook == null){
                throw new ObjectNotFoundException("Book not valid");
            }
            var book = new Book{
                Title = newBook.Title,
                FirstName = newBook.FirstName,
                LastName = newBook.LastName,
                DatePublished = newBook.DatePublished,
                ISBN = newBook.ISBN
            };
            _db.Add(book);
            _db.SaveChanges();
            if(book != null){
                return book;
            }
            else{
                throw new ObjectNotFoundException("Book not valid");
            }
        }

    /// <summary>
	/// Removes the book with the given ID, and all loans and ratings
    /// associated with it, from the database
    /// ADMIN function
	/// </summary>
        public void DeleteBookByID(int bookID){
            var book = (from b in _db.Books
                        where b.ID == bookID
                        select b).SingleOrDefault();
            if(book == null){
                throw new ObjectNotFoundException("a book with this ID was not found");
            }
            else{
                var loans = (from l in _db.Loans
                                where l.bookID == bookID
                                select l).ToList();
                foreach(Loan l in loans){
                    _db.Loans.Remove(l);
                    _db.SaveChanges();
                }
                var reviews = (from r in _db.Reviews
                                where r.bookID == bookID
                                select r).ToList();
                foreach(Review r in reviews){
                    _db.Reviews.Remove(r);
                    _db.SaveChanges();
                }
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

        
    /// <summary>
	/// Updates the book with the given ID        
    /// ADMIN function
	/// </summary>
        public Book UpdateBookByID(Book updatedBook, int bookID){
            var book = (_db.Books.SingleOrDefault(b => b.ID == bookID));

            if(book == null){
                throw new ObjectNotFoundException("Book was not found");
            }
            
            book.Title = updatedBook.Title;
            book.FirstName = updatedBook.FirstName;
            book.LastName = updatedBook.LastName;
            book.DatePublished = updatedBook.DatePublished;
            book.ISBN = updatedBook.ISBN;

            _db.Books.Update(book);
            _db.SaveChanges();

            return book;
        }
    }
}