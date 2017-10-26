using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class UserService : IUserService
    {
        private ILibraryRepository _repo;
        public UserService(ILibraryRepository repo)
        {
            _repo = repo;
        }

        public void AddBookToUser(int userId, int bookId)
        {
            _repo.AddBookToUser(userId, bookId);
        }

        public void AddNewUser(Friend newUser)
        {
            _repo.AddNewUser(newUser);
        }

        public void DeleteUserById(int userId)
        {
            _repo.DeleteUserById(userId);
        }

        public IEnumerable<UserViewModel> GetAllUsers(String LoanDate, int LoanDuration)
        {
            var users = _repo.GetAllUsers(LoanDate);
            if(users != null){
                return users;
            }
            else{
                return null;
            }
        }

        public IEnumerable<BookViewModel> GetBooksByUserId(int userId)
        {
            var items = _repo.GetBooksByUserId(userId);
            return items;
        }

        public UserViewModel GetUserById(int userId)
        {
            var user = _repo.GetUserById(userId);
            return user;
        }

        public void OnStart()
        {
            _repo.OnStart();
        }

        public void ReturnBook(int userId, int bookId)
        {
            _repo.ReturnBook(userId, bookId);
        }

        public Loan UpdateLoan(Loan updatedLoan, int userId, int bookId)
        {
            var loan = _repo.UpdateLoan(updatedLoan, userId, bookId);
            return loan;
        }

        public Friend UpdateUserById(Friend updatedUser, int userId)
        {
            var user = _repo.UpdateUserById(updatedUser, userId);
            return user;
        }
    }
}