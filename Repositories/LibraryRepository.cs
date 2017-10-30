using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibraryAPI.Repositories;
using LibraryAPI.Models;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using Newtonsoft.Json;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private AppDataContext _db;
        //private readonly BookRepository _bookRepo;
        
        //should maybe make helper functions for checking if users-books-reviews-loans exist in the db
        public LibraryRepository(AppDataContext db)
        {
            _db = db;
        }

/// Functions for filling the database -----------------------------------------------------------------------------------------
    /// <summary>
	/// Fills the database with data
	/// </summary>
        public void OnStart()
        {
            FillBooks();
            FillFriendsAndLoans();
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
                                    DateBorrowed = Convert.ToDateTime(i.bok_lanadagsetning),
                                    hasReturned = false,
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
                            DatePublished = Convert.ToDateTime(b.utgafudagur),
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


/// User Related Functions ------------------------------------------------------------------------------------------------

        /// <summary>
	    /// Fetches all users in the database
	    /// </summary>
        public IEnumerable<UserViewModel> GetAllUsers(String LoanDate, int LoanDuration)
        {
            if(!LoanDate.Equals("")){
                DateTime dt = Convert.ToDateTime(LoanDate);
                var users = (from friend in _db.Friends
                            join l in _db.Loans on friend.ID equals l.friendID
                            join book in _db.Books on l.bookID equals book.ID
                            where DateTime.Compare(dt, l.DateBorrowed) > 0
                            where l.hasReturned == false
                             select new UserViewModel {
                                 Name = friend.FirstName + " " + friend.LastName,
                                Address = friend.Address,
                                Email = friend.Email,
                                loanHistory = (from lo in _db.Loans
                                                where lo.friendID == friend.ID
                                                select lo).ToList()
                             }).OrderBy(x => x.Name).ToList();
                if(users == null){
                    throw new ObjectNotFoundException("No users found");
                }
                return users; 
            }
            else if(LoanDuration != 0){
                DateTime dt = DateTime.Now.AddDays(-LoanDuration);
                    var users = (from friend in _db.Friends
                                join l in _db.Loans on friend.ID equals l.friendID
                                join book in _db.Books on l.bookID equals book.ID
                                where DateTime.Compare(dt, l.DateBorrowed) < 0
                                where l.hasReturned == false
                                select new UserViewModel {
                                    Name = friend.FirstName + " " + friend.LastName,
                                    Address = friend.Address,
                                    Email = friend.Email,
                                    loanHistory = (from lo in _db.Loans
                                                    where lo.friendID == friend.ID
                                                    select lo).ToList()
                                }).OrderBy(x => x.Name).ToList();
                if(users == null){
                    throw new ObjectNotFoundException("No users found");
                }
                return users;
            }
            else {
                var users = (from f in _db.Friends
                        select new UserViewModel {
                            Name = f.FirstName + " " + f.LastName,
                            Address = f.Address,
                            Email = f.Email,
                            loanHistory = (from l in _db.Loans
                                            where l.friendID == f.ID
                                            select l).ToList()
                            }).OrderBy(x => x.Name).ToList();
                if(users == null){
                    throw new ObjectNotFoundException("No users found");
                }
                return users; 
            }
        }

        /// <summary>
        /// Returns a single user with the given ID from the database
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
        /// ADMIN function
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
        /// ADMIN function
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
        /// ADMIN function
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

            _db.Friends.Update(user);
            _db.SaveChanges();

            return user;            
        }

        /// <summary>
        /// Returns all books registered for user with given Id
        /// </summary>
        public IEnumerable<BookViewModel> GetBooksByUserId(int userId) 
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userId);

            if (user == null) 
            {
                throw new ObjectNotFoundException("User not found");
            }

            var books = (from l in _db.Loans
                            where l.friendID == userId && l.hasReturned == false
                            join b in _db.Books on l.bookID equals b.ID
                            select new BookViewModel
                            {
                                Title = b.Title,
                                Author = b.FirstName + " " + b.LastName,
                                DatePublished = b.DatePublished
                            }).ToList();
            if(books == null){
                throw new ObjectNotFoundException("This user has not borrowed any books");
            }
            return books;
        }

        /// <summary>
        /// Registers a book to a user with these given ids
        /// </summary>
        public void AddBookToUser(int userId, int bookId) 
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userId);
            var book = _db.Books.SingleOrDefault(b => b.ID == bookId);

            if(book == null){
                throw new ObjectNotFoundException("A book with the given ID does not exist");
            }
            if(user == null){
                throw new ObjectNotFoundException("User with this ID cannot be found");
            }

            _db.Loans.Add(
                    new Loan { 
                        bookID = bookId, 
                        friendID = userId, 
                        DateBorrowed = DateTime.Today
                    }
                );
            _db.SaveChanges();
        }

        /// <summary>
        /// Returns the book with the given ID
        /// marks the Loan as returned
        /// </summary>
        public void ReturnBook(int userId, int bookId) 
        {
            var friend = (from f in _db.Friends
                            where f.ID == userId
                            select f).SingleOrDefault();
            if(friend == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }
            var loan = (from l in _db.Loans
                        where l.friendID == userId && l.bookID == bookId
                        select l).SingleOrDefault();
            if(loan == null){
                throw new ObjectNotFoundException("Loan was not found");
            }
            else{
                loan.hasReturned = true;
                _db.Loans.Update(loan);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a loan for the user with the given ID for a book with the given ID
        /// ADMIN function
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

            _db.Loans.Update(loan);
            _db.SaveChanges();

            return loan;
        }


///Review related functions --------------------------------------------------------------------------------------------------

    /// <summary>
	/// Returns all reviews from the user with the given ID
	/// </summary>
        public IEnumerable<ReviewViewModel> GetAllReviewsByUser(int userID)
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User ID was not found");
            }
            
            var reviews = (from r in _db.Reviews
                            where r.friendID == userID
                            select new ReviewViewModel{
                                BookTitle = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.Title).SingleOrDefault(),
                                AuthorFirstName = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.FirstName).SingleOrDefault(),
                                AuthorLastName = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.LastName).SingleOrDefault(),
                                Rating = r.Rating,
                                DatePublished = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.DatePublished).SingleOrDefault().ToString()
                                }).ToList();
            return reviews;
        }

    /// <summary>
	/// Adds a new review for the book with the given ID 
    /// from a user with the given ID to the database
	/// </summary>
        public ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            if(rating.Rating > 5 || rating.Rating < 0){
                throw new RatingException("Rating can only be from 0 - 5");
            }
            var user = _db.Friends.SingleOrDefault( f => f.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User ID not found");
            }

            var book = _db.Books.SingleOrDefault( b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book ID not found");
            }

            var oldReview = _db.Reviews.SingleOrDefault( r => r.bookID == bookID && r.friendID == userID);
            
            if(oldReview != null){ //the user has already reviewed this book - he should just update the old one!
                throw new RatingException("User has already Rated this book - please update review");
            }

            _db.Reviews.Add(new Review{
                bookID = bookID,
                friendID = userID,
                Rating = rating.Rating
            });
            _db.SaveChanges();
            return new ReviewViewModel{
                BookTitle = book.Title,
                AuthorFirstName = book.FirstName,
                AuthorLastName = book.LastName,
                Rating = rating.Rating
            };
        }
    /// <summary>
	/// Returns all reviews made by the user with the given ID for a book with the given ID
	/// </summary>
        public ReviewViewModel GetReviewByUserForBook(int userID, int bookID)
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }

            var book = _db.Books.SingleOrDefault(b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found");
            }

            var review = (from r in _db.Reviews 
                            where r.bookID == bookID && r.friendID == userID
                            select new ReviewViewModel {
                                BookTitle = book.Title,
                                AuthorFirstName = book.FirstName,
                                AuthorLastName = book.LastName,
                                Rating = r.Rating,
                                DatePublished = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.DatePublished).SingleOrDefault().ToString()
                            }).SingleOrDefault();
            if(review == null){
                throw new RatingException("No review by user with the given ID for a book with the given ID has been made");
            }
            else{
                return review;
            }
        }

    /// <summary>
	/// Removes a review made by the user with the given ID for a book with the given 
    /// ID from the database
	/// </summary>
        public void DeleteReviewByUserForBook(int userID, int bookID)
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }

            var book = _db.Books.SingleOrDefault(b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found");
            }

            var review = _db.Reviews.SingleOrDefault(r => r.bookID == bookID && r.friendID == userID);
            if(review == null){
                throw new RatingException("No review from the user with the given ID for book with the given ID was found");
            }
            else{
                _db.Reviews.Remove(review);
                _db.SaveChanges();
            }
        }

    /// <summary>
	/// Updates a rating made by the user with the given ID for a book with the given 
    /// ID from the database, returns the updated review
	/// </summary>
        public ReviewViewModel UpdateReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            var user = _db.Friends.SingleOrDefault(u => u.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }

            var book = _db.Books.SingleOrDefault(b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found");
            }

            var oldReview = (from r in _db.Reviews 
                            where r.bookID == bookID && r.friendID == userID
                            select r).SingleOrDefault();
            if(oldReview == null){
                throw new RatingException("No review by user with the given ID for a book with the given ID has been made");
            }
            
            oldReview.Rating = rating.Rating;
            _db.Reviews.Update(oldReview);
            _db.SaveChanges();
            var review = new ReviewViewModel {
                                BookTitle = book.Title,
                                AuthorFirstName = book.FirstName,
                                AuthorLastName = book.LastName,
                                Rating = oldReview.Rating
                            };
            return review;
            
        }


///Book Related Functions -----------------------------------------------------------------------------------------------------------
/* 
    ///<summary>
	///Returns all books in the database 
    ///(maybe it should only return books that are not being borrowed)
	/// </summary>
        public IEnumerable<BookViewModel> GetAllBooks(DateTime? LoanDate)
        {       
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
        public BookDetailsViewModel GetBookByID(int book_id)
        {
            var book = (from b in _db.Books
                        where b.ID == book_id
                        select new BookDetailsViewModel{
                            Title = b.Title,
                            Author = b.FirstName + " " + b.LastName,
                            DatePublished = b.DatePublished,
                            Rating = (from r in _db.Reviews
                                        where r.bookID == book_id
                                        select r.Rating).Average(),
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
	/// Removes the book with the given ID, and all loans and ratings
    /// associated with it, from the database
    /// ADMIN function
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

            _db.Books.Update(book);
            _db.SaveChanges();

            return book;
            
        }
*/
        public IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks()
        {
            var reviews = (from r in _db.Reviews
                            select new ReviewViewModel {
                                BookTitle = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.Title).SingleOrDefault(),
                                AuthorFirstName = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.FirstName).SingleOrDefault(),
                                AuthorLastName = (from b in _db.Books
                                        where b.ID == r.bookID
                                        select b.LastName).SingleOrDefault(),
                                Rating = r.Rating
                            }).ToList();
            if(reviews == null){
                throw new ObjectNotFoundException("No ratings were found in the database");
            }
            return reviews;
        }
        /// <summary>
        /// Returns all reviews for a book with the given ID
        /// </summary>
        public IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID)
        {
            var book = _db.Books.SingleOrDefault(b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found0");
            }
            var reviews = (from r in _db.Reviews
                            where r.bookID == bookID
                            select new ReviewViewModel {
                                BookTitle = book.Title,
                                AuthorFirstName = book.FirstName,
                                AuthorLastName = book.LastName,
                                Rating = r.Rating
                            }).ToList();
            if(reviews == null){
                throw new ObjectNotFoundException("No ratings for this book were found in the database");
            }
            return reviews;
        }
        /// <summary>
        /// Returns a review by the user with the given userID for a book with the given bookID
        /// </summary>
        public ReviewViewModel GetReviewForBookByUser(int bookID, int userID)
        {
            var review = GetReviewByUserForBook(userID, bookID);
            return review;
        }

        public IEnumerable<RecommendationViewModel> GetRecommendationsForUser(int userID)
        {
            var ratedBooks = (from x in _db.Reviews where x.friendID == userID select x).ToList();
            var rentedBooks = (from x in _db.Loans where x.friendID == userID select x).ToList();
            if(ratedBooks.Count() == 0 || rentedBooks.Count() == 0){        
                // if the user has not rated or read any books just return the 5 top rated books
                var recommendations = (from b in _db.Books
                                        join r in _db.Reviews on b.ID equals r.bookID
                                        where r.Rating > 4
                                        select new RecommendationViewModel {
                                                    Title = b.Title,
                                                    Author = b.FirstName + " " + b.LastName,
                                                    DatePublished = b.DatePublished,
                                                    ISBN = b.ISBN
                                        }).Distinct().OrderBy(x => x.Title).Take(5).ToList();
                if(recommendations == null){
                    throw new ObjectNotFoundException("cannot find top rated books");
                }
                return recommendations;
            }
            else{         //we return the books by his favourite authors
                    List<RecommendationViewModel> recommendations  = new List<RecommendationViewModel>();
                    var authors = (from b in _db.Books
                             join r in _db.Reviews on b.ID equals r.bookID
                             where r.friendID == userID
                             select b
                             );
                foreach(var a in authors)
                {   
                    var booksByAuthors = (from b in _db.Books
                                        where a.LastName == b.LastName && a.FirstName == b.FirstName
                                        && a.Title != b.Title
                                        select new RecommendationViewModel{
                                            Title = b.Title,
                                                    Author = b.FirstName + " " + b.LastName,
                                                    DatePublished = b.DatePublished,
                                                    ISBN = b.ISBN
                                        });
                

                    foreach(RecommendationViewModel b in booksByAuthors)
                    {
                        recommendations.Add(b);
                    }   
                }

                if(recommendations.Count() == 0){        // there were no more books by his favourite author
                    recommendations = (from b in _db.Books
                                        join r in _db.Reviews on b.ID equals r.bookID
                                        join l in _db.Loans on b.ID equals l.bookID
                                        where r.Rating >= 4 && l.friendID != userID
                                        select new RecommendationViewModel {
                                                    Title = b.Title,
                                                    Author = b.FirstName + " " + b.LastName,
                                                    DatePublished = b.DatePublished,
                                                    ISBN = b.ISBN
                                        }).Distinct().OrderBy(x => x.Title).Take(5).ToList();
                }
                return recommendations;                
            }
        }
    }
}
