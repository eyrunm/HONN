
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Repositories
{
    public class MockLibraryRepository : ILibraryRepository
    {
        private static ICollection<Book> _books;
        private static ICollection<Friend> _friends;
        private static ICollection<Review> _reviews;
        private static ICollection<Loan> _loans;

        public MockLibraryRepository() {
            
            _books = new List<Book> {
                    new Book {  ID = 1, 
                                Title ="Harry Potter and the sorcerer's stone", 
                                FirstName = "J K", LastName = "Rowling", 
                                DatePublished = Convert.ToDateTime("1997-06-26"), 
                                ISBN = "123456789"
                             },
                    new Book {  ID = 2, 
                                Title ="Harry Potter and the chamber of secrets", 
                                FirstName = "J K", LastName = "Rowling", 
                                DatePublished = Convert.ToDateTime("1998-07-02"), 
                                ISBN = "234567890"
                            },
                    new Book {  ID = 3, 
                                Title ="Harry Potter and the chamber of secrets", 
                                FirstName = "J K", LastName = "Rowling", 
                                DatePublished = Convert.ToDateTime("1998-07-02"), 
                                ISBN = "345678901"
                            }
            };

            _friends = new List<Friend> {
                    new Friend {
                                ID = 1, 
                                FirstName = "Sigga", LastName = "Jóns",
                                Email = "sigga@sigga.is",
                                Address = "laugavegur 1"
                            },
                    new Friend {
                                ID = 2, 
                                FirstName = "Jón", LastName = "Jónsson",
                                Email = "jon@jon.is",
                                Address = "Jónsgata 1"
                            },
                    new Friend {
                                ID = 3, 
                                FirstName = "Páll", LastName = "Pálsson",
                                Email = "pall@pall.is",
                                Address = "Pálsstígur 4"
                            },
                    new Friend {
                                ID = 4, 
                                FirstName = "Jón Páll", LastName = "Sigmarsson",
                                Email = "jonpall@jon.is",
                                Address = "Eggertsgata 15",
                            }
            };

            _loans = new List<Loan> {
                    new Loan {
                                ID = 1,
                                bookID = 1,
                                friendID = 1,
                                DateBorrowed = Convert.ToDateTime("2016-10-01"),
                                hasReturned = true
                    }, new Loan {
                                ID = 2,
                                bookID = 1,
                                friendID = 2,
                                DateBorrowed = Convert.ToDateTime("2016-10-01"),
                                hasReturned = true
                    }, new Loan {
                                ID = 3,
                                bookID = 1,
                                friendID = 3,
                                DateBorrowed = Convert.ToDateTime("2016-04-04"),
                                hasReturned = true
                    }, new Loan {
                                ID = 4,
                                bookID = 2,
                                friendID = 3,
                                DateBorrowed = Convert.ToDateTime("2016-05-05"),
                                hasReturned = true
                    }, new Loan {
                                ID = 5,
                                bookID = 3,
                                friendID = 3,
                                DateBorrowed = Convert.ToDateTime("2016-10-11"),
                                hasReturned = false
                    }
            };

            _reviews = new List<Review> {
                    new Review {    
                                    ID = 1,
                                    bookID = 1,
                                    friendID = 1,
                                    Rating = 5
                    }, new Review {    
                                    ID = 2,
                                    bookID = 1,
                                    friendID = 2,
                                    Rating = 3
                    }, new Review {    
                                    ID = 3,
                                    bookID = 1,
                                    friendID = 3,
                                    Rating = 3
                    }, new Review {    
                                    ID = 4,
                                    bookID = 2,
                                    friendID = 3,
                                    Rating = 3
                    }
            };
        }
        public Book AddBookToUser(int userId, int bookId)
        {
            var bookid = _books.SingleOrDefault(x => x.ID == bookId);
            var userid = _friends.SingleOrDefault(x => x.ID == userId);
            if(bookid == null) {
                throw new ObjectNotFoundException("Not valid book id");
            }
            if(userid == null) {
                throw new ObjectNotFoundException("Not valid user id");
            }

            _loans.Add(new Loan { friendID = userId, bookID = bookId, hasReturned = false });
            var book = _books.SingleOrDefault(x => x.ID == bookId);
            return book;
        }

        public void AddNewBook(Book newBook)
        {
            if(newBook.Title == null || newBook.FirstName == null || newBook.LastName == null || newBook.DatePublished == null || newBook.ISBN == null){
                throw new ObjectNotFoundException("failed to add book");
            }
            _books.Add(newBook);
        }

        public Friend AddNewUser(Friend newUser)
        {
            if(newUser.FirstName == null || newUser.LastName == null || newUser.Email == null || newUser.Address == null){
                throw new Exception("User not valid");
            }
             _friends.Add(newUser);
             var user = _friends.Where(x => x.ID == newUser.ID).SingleOrDefault();
             if(user == null){
                 throw new Exception("Failed to add user to database");
             }
             return user;
        }

        public ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            if(rating.Rating > 5 || rating.Rating < 0){
                throw new RatingException("Rating can only be from 0 - 5");
            }
            var user = _friends.SingleOrDefault( f => f.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User ID not found");
            }
            var book = _books.SingleOrDefault( b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book ID not found");
            }
            var newReview = new Review{
                friendID = userID,
                bookID = bookID,
                Rating = rating.Rating
            };

            var rat = _reviews.Where(x => x.bookID == bookID).ToList();
            var sum = 0;
            foreach(var x in rat){
                sum += x.Rating;
            }
            var avg = sum / rat.Count();
            _reviews.Add(newReview);
            var rev = new ReviewViewModel{
                BookTitle = book.Title,
                AuthorFirstName = book.FirstName,
                AuthorLastName = book.LastName,
                Rating = avg
            };
            return rev;
        }

        public void DeleteBookByID(int bookID)
        {
            var bookid = _books.SingleOrDefault(x => x.ID == bookID);
            if(bookid == null) {
                throw new ObjectNotFoundException("not valid book id");
            }
            _books.Remove(_books.FirstOrDefault(x => x.ID == bookID));
        }

        public void DeleteReviewByUserForBook(int userID, int bookID)
        {
            var user = _friends.SingleOrDefault(u => u.ID == userID);
            var book = _books.SingleOrDefault(b => b.ID == bookID);
            var review = _reviews.SingleOrDefault(r => r.bookID == bookID && r.friendID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found");
            }
            if(review == null){
                throw new RatingException("No review from the user with the given ID for book with the given ID was found");
            }
            _reviews.Remove(_reviews.FirstOrDefault(x => x.friendID == userID && x.bookID == bookID));
        }

        public void DeleteUserById(int userId)
        {
            _friends.Remove(_friends.FirstOrDefault(x => x.ID == userId));
        }

        public IEnumerable<BookViewModel> GetAllBooks(DateTime? LoanDate)
        {       
            List<BookViewModel> books = new List<BookViewModel>();     
            if(LoanDate == null){
                foreach ( var b in _books)
                {
                    books.Add(new BookViewModel{
                            Title = b.Title,
                            Author = b.FirstName + " " + b.LastName,
                            DatePublished = b.DatePublished
                    });
                }
                return books;
            }
            else {
                DateTime dt = LoanDate.Value;
                books = (from b in _books
                            join l in _loans on b.ID equals l.bookID
                            where DateTime.Compare(dt, l.DateBorrowed) < 0
                            select new BookViewModel{
                                Title = b.Title,
                                Author = b.FirstName + " " + b.LastName,
                                DatePublished = b.DatePublished}).ToList();       
            }
            return books;
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsByUser(int userID)
        {
            List<ReviewViewModel> reviews = new List<ReviewViewModel>();

            var user = _friends.SingleOrDefault(x => x.ID == userID);
            if(user == null) {
                throw new ObjectNotFoundException("not valid user id");
            }
            else {
                reviews = (from r in _reviews
                            join b in _books on r.bookID equals b.ID
                            where r.friendID == userID
                            select new ReviewViewModel{
                                BookTitle = b.Title,
                                AuthorFirstName = b.FirstName,
                                AuthorLastName = b.LastName,
                                Rating = r.Rating,
                                DatePublished = b.DatePublished.ToString()
                            }).ToList();
            }
            return reviews;
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks()
        {
            var reviews = (from r in _reviews
                            select new ReviewViewModel {
                                BookTitle = (from b in _books
                                        where b.ID == r.bookID
                                        select b.Title).SingleOrDefault(),
                                AuthorFirstName = (from b in _books
                                        where b.ID == r.bookID
                                        select b.FirstName).SingleOrDefault(),
                                AuthorLastName = (from b in _books
                                        where b.ID == r.bookID
                                        select b.LastName).SingleOrDefault(),
                                Rating = r.Rating
                            }).ToList();
            if(reviews == null){
                throw new ObjectNotFoundException("No ratings were found in the database");
            }
            return reviews;
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID)
        {
            var book = _books.SingleOrDefault(b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found0");
            }
            var reviews = (from r in _reviews
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

        public IEnumerable<UserViewModel> GetAllUsers(String LoanDate, int LoanDuration)
        {
            List<UserViewModel> users = new List<UserViewModel>();     
            if(LoanDuration != 0){
                DateTime dt = Convert.ToDateTime(LoanDate);
                users = (from f in _friends
                         join l in _loans on f.ID equals l.friendID
                         join b in _books on l.bookID equals b.ID
                         where ((DateTime.Now - l.DateBorrowed).TotalDays > LoanDuration)
                         where l.hasReturned == false
                         select new UserViewModel {
                            Name = f.FirstName + " " + f.LastName,
                            Address = f.Address,
                            Email = f.Email,
                            loanHistory = (from lo in _loans
                                           where lo.friendID == f.ID
                                           select lo).ToList()
                            }).ToList();
                return users;                
            }
            else if(LoanDate != null) {
                DateTime dt = Convert.ToDateTime(LoanDate);
                users = (from f in _friends
                         join l in _loans on f.ID equals l.friendID
                         where DateTime.Compare(dt, l.DateBorrowed) < 0
                         select new UserViewModel{
                            Name = f.FirstName + " " + f.LastName,
                            Email = f.Email, 
                            Address = f.Address,
                            loanHistory =  (from lo in _loans
                                            where lo.friendID == f.ID
                                            select lo).ToList()
                            }).ToList();
                return users;       
            }
            else {
                foreach ( var f in _friends)
                {
                    users.Add(new UserViewModel{
                                Name = f.FirstName + " " + f.LastName,
                                Email = f.Email, 
                                Address = f.Address,
                                loanHistory =  (from l in _loans
                                                where l.friendID == f.ID
                                                select l).ToList()
                    });
                }
                return users;
            }
        }

        public BookDetailsViewModel GetBookByID(int book_id)
        {
            var bookid = _books.SingleOrDefault(x => x.ID == book_id);
            if(bookid == null) {
                throw new ObjectNotFoundException("not valid book id");
            }
            else {
                var book = (from b in _books
                        where b.ID == book_id
                        select new BookDetailsViewModel{
                            Title = b.Title,
                            Author = b.FirstName + " " + b.LastName,
                            DatePublished = b.DatePublished,
                            ISBN = b.ISBN
                        }).SingleOrDefault();
                return book;
            }
        }

        public IEnumerable<BookViewModel> GetBooksByUserId(int userId)
        {
            var userid = _friends.SingleOrDefault(x => x.ID == userId);
            if(userid == null) {
                throw new ObjectNotFoundException("not valid user id");
            }
            else {
                var books = (from b in _books
                             join l in _loans on b.ID equals l.bookID
                             where l.friendID == userId
                             select new BookViewModel{
                                Title = b.Title,
                                Author = b.FirstName + " " + b.LastName,
                                DatePublished = b.DatePublished
                        }).ToList();
                return books;
            }
        }

        public IEnumerable<RecommendationViewModel> GetRecommendationsForUser(int userID)
        {
            var userid = _friends.SingleOrDefault(x => x.ID == userID);
            if(userid == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }
            var ratedBooks = (from x in _reviews where x.friendID == userID select x).ToList(); //has the user rated any books?
            var rentedBooks = (from x in _loans where x.friendID == userID select x).ToList();   //has the user read any books?
            if(ratedBooks.Count() == 0 || rentedBooks.Count() == 0){        
                // if the user has not rated or read any books we return the 5 top rated books
                var topRated = (from b in _books
                                join r in _reviews on b.ID equals r.bookID
                                where r.Rating > 4
                                select new RecommendationViewModel {
                                    Title = b.Title,
                                    Author = b.FirstName + " " + b.LastName,
                                    DatePublished = b.DatePublished,
                                    ISBN = b.ISBN
                                }).Distinct().OrderBy(x => x.Title).Take(10).ToList();
                if(topRated == null){
                    throw new ObjectNotFoundException("cannot find top rated books");
                }
                return topRated;
            }
            else{         //otherwise we try to return the books by his favourite authors
                    List<RecommendationViewModel> topRated  = new List<RecommendationViewModel>();
                    var favAuthors = (from b in _books
                             join r in _reviews on b.ID equals r.bookID
                             where r.friendID == userID
                             select b);
                foreach(var a in favAuthors)
                {   
                    var booksByFavAuthors = (from b in _books
                                        where a.LastName == b.LastName && a.FirstName == b.FirstName
                                        && a.Title != b.Title
                                        select new RecommendationViewModel{
                                            Title = b.Title,
                                            Author = b.FirstName + " " + b.LastName,
                                            DatePublished = b.DatePublished,
                                            ISBN = b.ISBN
                                        });

                    foreach(RecommendationViewModel b in booksByFavAuthors){
                        topRated.Add(b);
                    }   
                }
                if(topRated.Count() == 0){        // there were no more books by the user's favourite author so then we just return top rated books again
                    topRated = (from b in _books
                                        join r in _reviews on b.ID equals r.bookID
                                        join l in _loans on b.ID equals l.bookID
                                        where r.Rating >= 4 && l.friendID != userID
                                        select new RecommendationViewModel {
                                                    Title = b.Title,
                                                    Author = b.FirstName + " " + b.LastName,
                                                    DatePublished = b.DatePublished,
                                                    ISBN = b.ISBN
                                        }).Distinct().OrderBy(x => x.Title).Take(10).ToList();
                }
                return topRated;
            }
        }

        public ReviewViewModel GetReviewByUserForBook(int userID, int bookID)
        {
            var user = _friends.SingleOrDefault(u => u.ID == userID);
            if(user == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }

            var book = _books.SingleOrDefault(b => b.ID == bookID);
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found");
            }

            var review = (from r in _reviews 
                            where r.bookID == bookID && r.friendID == userID
                            select new ReviewViewModel {
                                BookTitle = book.Title,
                                AuthorFirstName = book.FirstName,
                                AuthorLastName = book.LastName,
                                Rating = r.Rating,
                                DatePublished = (from b in _books
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

        public ReviewViewModel GetReviewForBookByUser(int bookID, int userID)
        {
            var review = GetReviewByUserForBook(userID, bookID);
            return review;
        }

        public UserViewModel GetUserById(int userId)
        {
            var userid = _friends.SingleOrDefault(x => x.ID == userId);
            if(userid == null) {
                throw new ObjectNotFoundException("not valid user id");
            }
            else {
                var user = (from f in _friends
                            where f.ID == userId
                            select new UserViewModel{
                            Name = f.FirstName + " " + f.LastName,
                            Email = f.Email,
                            Address = f.Address,
                            loanHistory = (from l in _loans
                                            where l.friendID == f.ID
                                            select l).ToList()
                        }).SingleOrDefault();
                return user;
            }
        }

        public void OnStart()
        {
            throw new System.NotImplementedException();
        }

        public void ReturnBook(int userId, int bookId)
        {
            var userid = _friends.SingleOrDefault(x => x.ID == userId);
            var bookid = _books.SingleOrDefault(x => x.ID == bookId);
            var loan = _loans.SingleOrDefault(x => x.friendID == userId && x.bookID == bookId);
            if(userid == null || bookid == null || loan == null) {
                throw new ObjectNotFoundException("Invalid Id, check if userId and bookId are correct");
            }
            loan.hasReturned = true;
            loan.DateReturned = DateTime.Now;            
        }

        public Book UpdateBookByID(Book updatedBook, int bookID)
        {
            var book = _books.SingleOrDefault(x => x.ID == bookID);
            if(book == null) {
                throw new ObjectNotFoundException("not valid book id");
            }
            if(updatedBook.Title == null || updatedBook.FirstName == null || updatedBook.LastName == null || updatedBook.DatePublished == null || updatedBook.ISBN == null){
                throw new ObjectNotFoundException("failed to update book");
            }
            book.Title = updatedBook.Title;
            book.FirstName = updatedBook.FirstName;
            book.LastName = updatedBook.LastName;
            book.DatePublished = updatedBook.DatePublished;
            book.ISBN = updatedBook.ISBN;

            return book;
        }

        public Loan UpdateLoan(Loan updatedLoan, int userId, int bookId)
        {
            var loan = (_loans.SingleOrDefault(l => l.friendID == userId && l.bookID == bookId));
            if(loan == null){
                throw new ObjectNotFoundException("Loan was not found");
            }

            loan.friendID = updatedLoan.friendID;
            loan.bookID = updatedLoan.bookID;
            loan.DateBorrowed = updatedLoan.DateBorrowed;
            loan.DateReturned = updatedLoan.DateReturned;
            loan.hasReturned = updatedLoan.hasReturned;

            return loan;
        }

        public ReviewViewModel UpdateReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            var user = _friends.SingleOrDefault(u => u.ID == userID);
            var book = _books.SingleOrDefault(b => b.ID == bookID);

            if(user == null){
                throw new ObjectNotFoundException("User with the given ID was not found");
            }
            if(book == null){
                throw new ObjectNotFoundException("Book with the given ID was not found");
            }

            var oldReview = (from r in _reviews 
                             where r.bookID == bookID && r.friendID == userID
                             select r).SingleOrDefault();
            if(oldReview == null){
                throw new RatingException("No review by user with the given ID for a book with the given ID has been made");
            }

            oldReview.Rating = rating.Rating;
            
            var review = new ReviewViewModel {
                                BookTitle = book.Title,
                                AuthorFirstName = book.FirstName,
                                AuthorLastName = book.LastName,
                                Rating = oldReview.Rating
                            };
            return review;
        }

        public Friend UpdateUserById(Friend updatedUser, int userId)
        {
            var user = _friends.SingleOrDefault(x => x.ID == userId);
            if(user == null) {
                throw new ObjectNotFoundException("not valid user id");
            }
            if(updatedUser.FirstName == null || updatedUser.LastName == null || updatedUser.Email == null || updatedUser.Address == null){
                throw new ObjectNotFoundException("failed to add user");
            }
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Address = updatedUser.Address;

            return user;
        }

        public ICollection<Loan> GetLoans(){
            return _loans;
        }
        public ICollection<Review> GetReviews(){
            return _reviews;
        }
        public ICollection<Friend> GetFriends(){
            return _friends;
        }
    }
}