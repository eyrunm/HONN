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

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var users = _repo.getAllUsers();
            if(users != null){
                return users;
            }
            else{
                return null;
            }
        }
    }
}