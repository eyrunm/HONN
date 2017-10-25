using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibraryAPI.Repositories;
using LibraryAPI.Models;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using Newtonsoft.Json;

namespace LibraryAPI.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private AppDataContext _db;
        
        public LibraryRepository(AppDataContext db)
        {
            _db = db;
        }


//User Related Functions

    /// <summary>
	/// Fetches all users in the database
	/// </summary>
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            FillFriendsAndLoans();
            var users = (from f in _db.Friends
                        select new UserViewModel{
                            Name = f.FirstName + " " + f.LastName,
                            Address = f.Address,
                            Email = f.Email
                        }).OrderBy(x => x.Name).ToList();
            if(users == null){
                return null;
            }
            else{
                return users;
            }
        }

    /// <summary>
	/// Fills the database tables Friends and Loans with data from JSON files
    /// If the database is empty
	/// </summary>
        public void FillFriendsAndLoans(){
            if(!_db.Friends.Any()){
                using (StreamReader r = new StreamReader("friends.json"))
                {
                    string js = r.ReadToEnd();
                    List<Vinur> Friends = JsonConvert.DeserializeObject<List<Vinur>>(js);
                    foreach(var f in Friends){
                        _db.Friends.Add(new Friend{
                                ID = f.vinur_id,
                                FirstName = f.fornafn,
                                LastName = f.eftirnafn,
                                Email = f.netfang,
                                Address = f.heimilisfang
                        });
                        _db.SaveChanges();
                        if(f.lanasafn != null){
                            var l = f.lanasafn.ToList();
                            foreach(var i in l){
                                _db.Loans.Add(new Loan{
                                    bookID = i.bok_id,
                                    friendID = f.vinur_id,
                                    DateBorrowed = i.bok_lanadagsetning
                                });
                            }
                            _db.SaveChanges();
                        }
                    }
                    _db.SaveChanges();
                }
            }
            else{
                return;
            }
        }




//Book Related Functions
    /// <summary>
	/// Fills the database table books with data from JSON files
    /// If the database is empty
	/// </summary>
        public void FillBooks(){
            if(!_db.Books.Any()){
                using (StreamReader r = new StreamReader("books.json"))
                {
                    string json = r.ReadToEnd();
                    List<Bok> Books = JsonConvert.DeserializeObject<List<Bok>>(json);
                    foreach(var b in Books){
                        _db.Books.Add(new Book{
                            ID = b.bok_id,
                            Title = b.bok_titill,
                            FirstName = b.fornafn_hofundar,
                            LastName = b.eftirnafn_hofundar,
                            DatePublished = b.utgafudagur,
                            ISBN = b.ISBN
                        });
                    }
                    _db.SaveChanges();
                    }
                }
            else{
                return;
            }
        }

    ///<summary>
	///Returns all books in the database 
    ///(maybe it should only return books that are not being borrowed)
	/// </summary>
        public IEnumerable<BookViewModel> GetAllBooks()
        {
            FillBooks();
            var books = (from b in _db.Books
                        select new BookViewModel{
                            Title = b.Title,
                            Author = b.FirstName + " " + b.LastName,
                            DatePublished = b.DatePublished
                        }).OrderBy(x => x.Title).ToList();
            if(books == null){
                return null;
            }
            else{
                return books;
            }
        }

    /// <summary>
	/// Gets a single book by its ID
	/// </summary>
        public BookDetailsViewModel GetBookByID(int book_id)
        {
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
	/// </summary>
        public void AddNewBook(Book newBook)
        {
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
        }

    /// <summary>
	/// Removes the book with the given ID from the database
	/// </summary>
        public void DeleteBookByID(int bookID)
        {
            var book = (from b in _db.Books
                        where b.ID == bookID
                        select b).SingleOrDefault();
            if(book == null){
                throw new ObjectNotFoundException("a book with this ID was not found");
            }
            else{
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

        
    /// <summary>
	/// Updates the book with the given ID
	/// </summary>
        public Book UpdateBookByID(Book updatedBook, int bookID)
        {
            var book = (_db.Books.SingleOrDefault(b => b.ID == bookID));

            if(book == null){
                throw new ObjectNotFoundException("Book was not found");
            }
            
            book.Title = updatedBook.Title;
            book.FirstName = updatedBook.FirstName;
            book.LastName = updatedBook.LastName;
            book.DatePublished = updatedBook.DatePublished;
            book.ISBN = updatedBook.ISBN;

            _db.Update(book);
            _db.SaveChanges();

            return book;
            
        }

        /// <summary>
        /// Gets a single user by its id
        /// </summary>
        public UserViewModel GetUserById(int userId)
        {
             var user = (from u in _db.Friends
                        where u.ID == userId
                        select new UserViewModel{
                            Name = u.FirstName + " " + u.LastName,
                            Email = u.Email,
                            Address = u.Address
                        }).SingleOrDefault();
            if(user == null) {
                throw new ObjectNotFoundException("User ID not found");
            }
            else {
                return user;
            }
        }
    }
}
