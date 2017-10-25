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

        // User Related Functions

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
        /// Gets a single user by its id
        /// </summary>
        public UserViewModel GetUserById(int userId)
        {
             var user = (from u in _db.Friends
                        where u.ID == userId
                        select new UserViewModel {
                            Name = u.FirstName + " " + u.LastName,
                            Email = u.Email,
                            Address = u.Address,
                            loanHistory = (from l in _db.Loans
                                            where l.friendID == userId
                                            select l).ToList()
                        }).SingleOrDefault();
            if(user == null) {
                throw new ObjectNotFoundException("User ID not found");
            }
            else {
                return user;
            }
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        public void AddNewUser(Friend newUser)
        {
            if(newUser == null){
                throw new ObjectNotFoundException("User not valid");
            }
            var user = new Friend {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Address = newUser.Address
            };
            _db.Add(user);
            _db.SaveChanges();
        }
        
        /// <summary>
        /// Deletes user with given id from the database
        /// </summary>
        public void DeleteUserById(int userId) 
        {
            var user = (from u in _db.Friends
                        where u.ID == userId
                        select u).SingleOrDefault();
            if(user == null){
                throw new ObjectNotFoundException("a user with this ID was not found");
            }
            else{
                _db.Friends.Remove(user);
                _db.SaveChanges();
            }
        }

        /// <summary>
	    /// Updates the user with the given ID
	    /// </summary>
        public Friend UpdateUserById(Friend updatedUser, int userId)
        {
            var user = (_db.Friends.SingleOrDefault(u => u.ID == userId));

            if(user == null){
                throw new ObjectNotFoundException("User was not found");
            }
            
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Address = updatedUser.Address;

            _db.Update(user);
            _db.SaveChanges();

            return user;            
        }

        /// <summary>
        /// Gets a list of books registered by user with given Id
        /// </summary>
        public IEnumerable<BookViewModel> GetBooksByUserId(int userId) 
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userId);

            if (user == null) 
            {
                return null;
            }

            var books = (from l in _db.Loans
                            where l.friendID == userId
                            join b in _db.Books on l.bookID equals b.ID
                            select new BookViewModel
                            {
                                Title = b.Title,
                                Author = b.FirstName + " " + b.LastName,
                                DatePublished = b.DatePublished
                            }).ToList();

            return books;
        }

        /// <summary>
        /// Registers a book to a user with these given ids
        /// </summary>
        public void AddBookToUser(int userId, int bookId) 
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userId);
            var book = _db.Books.SingleOrDefault(b => b.ID == bookId);

            _db.Loans.Add(
                    new Loan { 
                        bookID = bookId, 
                        friendID = userId, 
                        DateBorrowed = DateTime.Now.ToString("yyyy-MM-dd") 
                    }
                );
            _db.SaveChanges();
        }

        /// <summary>
        /// Deletes the loan when book is returned
        /// </summary>
        public void ReturnBook(int userId, int bookId) 
        {
            var loan = (from l in _db.Loans
                        where l.friendID == userId && l.bookID == bookId
                        select l).SingleOrDefault();
            if(loan == null){
                throw new ObjectNotFoundException("Loan with these ID was not found");
            }
            else{
                _db.Loans.Remove(loan);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the loan
        /// </summary>
        public Loan UpdateLoan(Loan updatedLoan, int userId, int bookId)
        {
             var loan = (_db.Loans.SingleOrDefault(l => l.friendID == userId && l.bookID == bookId));

            if(loan == null){
                throw new ObjectNotFoundException("Loan was not found");
            }
            
            loan.friendID = updatedLoan.friendID;
            loan.bookID = updatedLoan.bookID;
            loan.DateBorrowed = updatedLoan.DateBorrowed;

            _db.Update(loan);
            _db.SaveChanges();

            return loan;
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
        private void FillBooks(){
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
    }
}
