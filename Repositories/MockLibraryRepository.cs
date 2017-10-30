
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
                                DatePublished = Convert.ToDateTime("26-06-1997"), 
                                ISBN = "123456789"
                             },
                    new Book {  ID = 2, 
                                Title ="Harry Potter and the chamber of secrets", 
                                FirstName = "J K", LastName = "Rowling", 
                                DatePublished = Convert.ToDateTime("02-07-1998"), 
                                ISBN = "234567890"
                            },
                    new Book {  ID = 3, 
                                Title ="Harry Potter and the chamber of secrets", 
                                FirstName = "J K", LastName = "Rowling", 
                                DatePublished = Convert.ToDateTime("02-07-1998"), 
                                ISBN = "345678901"
                            }
            };

            _friends = new List<Friend> {
                    new Friend {
                                ID = 2, 
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
        public void AddBookToUser(int userId, int bookId)
        {
            _loans.Add(new Loan { friendID = userId, bookID = bookId, hasReturned = false });
        }

        public void AddNewBook(Book newBook)
        {
            _books.Add(newBook);
        }

        public void AddNewUser(Friend newUser)
        {
            _friends.Add(newUser);
        }

        public ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            var newReview = new Review{
                friendID = userID,
                bookID = bookID,
                Rating = rating.Rating
            };

            var book = _books.Where(x => x.ID == bookID).SingleOrDefault();
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
            _books.Remove(_books.FirstOrDefault(x => x.ID == bookID));
        }

        public void DeleteReviewByUserForBook(int userID, int bookID)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUserById(int userId)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UserViewModel> GetAllUsers(String LoanDate, int LoanDuration)
        {
            throw new System.NotImplementedException();
        }

        public BookDetailsViewModel GetBookByID(int book_id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<BookViewModel> GetBooksByUserId(int userId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RecommendationViewModel> GetRecommendationsForUser(int userID)
        {
            throw new System.NotImplementedException();
        }

        public ReviewViewModel GetReviewByUserForBook(int userID, int bookID)
        {
            throw new System.NotImplementedException();
        }

        public ReviewViewModel GetReviewForBookByUser(int bookID, int userID)
        {
            throw new System.NotImplementedException();
        }

        public UserViewModel GetUserById(int userId)
        {
            throw new System.NotImplementedException();
        }

        public void OnStart()
        {
            throw new System.NotImplementedException();
        }

        public void ReturnBook(int userId, int bookId)
        {
            throw new System.NotImplementedException();
        }

        public Book UpdateBookByID(Book updatedBook, int bookID)
        {
            throw new System.NotImplementedException();
        }

        public Loan UpdateLoan(Loan updatedLoan, int userId, int bookId)
        {
            throw new System.NotImplementedException();
        }

        public ReviewViewModel UpdateReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            throw new System.NotImplementedException();
        }

        public Friend UpdateUserById(Friend updatedUser, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}