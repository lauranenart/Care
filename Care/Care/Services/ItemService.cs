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
    class ItemService
    {
        private ServiceDbContext context;
        public ItemService()
        {
            this.context = new ServiceDbContext();
        }

        public IEnumerable<ItemModel> Items => context.GetAsync<IEnumerable<ItemModel>>("api/item", "getitems").Result;
        public ItemModel FindById(int id) => context.GetAsync<ItemModel>($"api/item/{id}", "getitem").Result;
        public Task Add(ItemModel item) => context.PostAsync<ItemModel>("api/item", item);
        public Task Update(int id, ItemModel item) => context.PutAsync<ItemModel>($"api/item/{id}", item);
        public Task Remove(int id) => context.DeleteAsync<ItemModel>($"api/item/{id}");
    }
}
