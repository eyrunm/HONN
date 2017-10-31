using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IUserService
    {
        IEnumerable<UserViewModel> GetAllUsers(String LoanDate, int LoanDuration);
        UserViewModel GetUserById(int userId);
        Friend AddNewUser(Friend newUser);
        void DeleteUserById(int userId);
        Friend UpdateUserById(Friend updatedUser, int userId);
        IEnumerable<BookViewModel> GetBooksByUserId(int userId);
        Book AddBookToUser(int userId, int bookId);
        void ReturnBook(int userId, int bookId);
        Loan UpdateLoan(Loan updatedLoan, int userId, int bookId);
        void OnStart();
    }
}