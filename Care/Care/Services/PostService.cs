using Care.Models;
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
    class PostService
    {
        private ServiceDbContext context;
        public PostService()
        {
            this.context = new ServiceDbContext();
        }

        public IEnumerable<PostModel> Posts => context.GetAsync<IEnumerable<PostModel>>("api/post", "getposts").Result;
        public PostModel FindById(int id) => context.GetAsync<PostModel>($"api/post/{id}", "getpost").Result;
        public Task Add(PostModel post) => context.PostAsync<PostModel>("api/post", post);
        public Task Update(int id, PostModel post) => context.PutAsync<PostModel>($"api/post/{id}", post);
        public Task Remove(int id) => context.DeleteAsync<PostModel>($"api/post/{id}");
    }
}
