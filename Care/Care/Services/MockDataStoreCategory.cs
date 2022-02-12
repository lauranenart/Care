using Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Care.Services
{
    public class MockDataStoreCategory : IDataStore<Category>
    {
        readonly List<Category> categories;

        public MockDataStoreCategory()
        {
            categories = new List<Category>();
        }

        public async Task<bool> AddItemAsync(Category item)
        {
            categories.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Category item)
        {
            var oldItem = categories.Where((Category arg) => arg.Id == item.Id).FirstOrDefault();
            categories.Remove(oldItem);
            categories.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = categories.Where((Category arg) => arg.Id == id).FirstOrDefault();
            categories.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Category> GetItemAsync(string id)
        {
            return await Task.FromResult(categories.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Category>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(categories);
        }
    }
}
