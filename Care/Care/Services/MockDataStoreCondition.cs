using Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Care.Services
{
    public class MockDataStoreCondition : IDataStore<Condition>
    {
        readonly List<Condition> conditions;

    public MockDataStoreCondition()
    {
        conditions = new List<Condition>();
    }

    public async Task<bool> AddItemAsync(Condition item)
    {
            conditions.Add(item);

        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateItemAsync(Condition item)
    {
        var oldItem = conditions.Where((Condition arg) => arg.Id == item.Id).FirstOrDefault();
            conditions.Remove(oldItem);
            conditions.Add(item);

        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteItemAsync(string id)
    {
        var oldItem = conditions.Where((Condition arg) => arg.Id == id).FirstOrDefault();
            conditions.Remove(oldItem);

        return await Task.FromResult(true);
    }

    public async Task<Condition> GetItemAsync(string id)
    {
        return await Task.FromResult(conditions.FirstOrDefault(s => s.Id == id));
    }

    public async Task<IEnumerable<Condition>> GetItemsAsync(bool forceRefresh = false)
    {
        return await Task.FromResult(conditions);
    }
}
}
