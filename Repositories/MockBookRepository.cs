
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
    public class MockBookRepository : IBookRepository
    {
        public static ICollection<Book> _books;
        public static ICollection<Loan> _loans;
        private static MockLibraryRepository _libRepo;

        public MockBookRepository() {
            _libRepo = new MockLibraryRepository();
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
        }

        public Book AddNewBook(Book newBook)
        {
            if(newBook.Title == null || newBook.FirstName == null || newBook.LastName == null || newBook.DatePublished == null || newBook.ISBN == null){
                throw new ObjectNotFoundException("failed to add book");
            }
            _books.Add(newBook);
            return newBook;
        }

        public void DeleteBookByID(int bookID)
        {
            var bookid = _books.SingleOrDefault(x => x.ID == bookID);
            if(bookid == null) {
                throw new ObjectNotFoundException("not valid book id");
            }
            _books.Remove(_books.FirstOrDefault(x => x.ID == bookID));
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
                _loans = _libRepo.GetLoans();
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

        public BookDetailsViewModel GetBookByID(int book_id)
        {
            var bookid = _books.SingleOrDefault(x => x.ID == book_id);
            if(bookid == null) {
                throw new ObjectNotFoundException("not valid book id");
            }
            else {
                _loans = _libRepo.GetLoans();
                var book = (from b in _books
                        where b.ID == book_id
                        select new BookDetailsViewModel{
                            Title = b.Title,
                            Author = b.FirstName + " " + b.LastName,
                            DatePublished = b.DatePublished,
                            ISBN = b.ISBN,
                            loanHistory = (from l in _loans where l.bookID == book_id select l).ToList()
                        }).SingleOrDefault();
                return book;
            }
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

        public ICollection<Book> GetBooks(){
            return _books;
        }
    }
}
