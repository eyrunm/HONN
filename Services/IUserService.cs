using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IUserService
    {
        IEnumerable<UserViewModel> GetAllUsers();
        UserViewModel GetUserById(int userId);
        void AddNewUser(Friend newUser);
        void DeleteUserById(int userId);
        Friend UpdateUserById(Friend updatedUser, int userId);
        IEnumerable<BookViewModel> GetBooksByUserId(int userId);
    }
}