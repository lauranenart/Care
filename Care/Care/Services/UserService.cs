using Care.Models;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Care.Services
{
    public class UserService
    {
        private ServiceDbContext context;
        public UserService()
        {
            this.context = new ServiceDbContext();
        }

        public IEnumerable<UserModel> Users => context.GetAsync<IEnumerable<UserModel>>("api/user", "getusers").Result;
        public UserModel FindUserByEmail(string emailAddress) => context.GetAsync<UserModel>($"api/user/email/{emailAddress}", "getuserbyemail").Result;
        public UserModel FindById(int id) => context.GetAsync<UserModel>($"api/user/{id}", "getuser").Result;
        public Task Add(UserModel user) => context.PostAsync<UserModel>("api/user", user);
        public Task Update(int id, UserModel user) => context.PutAsync<UserModel>($"api/user/{id}", user);
        public Task Remove(int id) => context.DeleteAsync<UserModel>($"api/user/{id}");
 
    }
}
