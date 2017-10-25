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

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var users = _repo.GetAllUsers();
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

        public Friend UpdateUserById(Friend updatedUser, int userId)
        {
            var user = _repo.UpdateUserById(updatedUser, userId);
            return user;
        }
    }
}