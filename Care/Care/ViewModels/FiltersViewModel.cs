using Care.Helpers;
using Care.Models;
using Care.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Care.ViewModels
{
    public class FiltersViewModel : BaseViewModel
    {
        public string Category { get; set; }
        public string Condition { get; set; }
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Models.Condition> Conditions { get; set; } = new ObservableCollection<Models.Condition>();

        public FiltersViewModel()
        {
           
        }

        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public ICommand CategoryCommand => new Command(async (parameter) =>
        {
            await GetFiltersAsync();
            var name = parameter.ToString();
            var foundCategory = Categories.Where(c => c.Name.StartsWith(name)).FirstOrDefault();

            if (foundCategory == null)
            {
                Category newCategory = new Category()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
                await DataStoreCategory.AddItemAsync(newCategory);
            }
            else
            {
                await DataStoreCategory.DeleteItemAsync(foundCategory.Id);
            }
        });

        public ICommand ConditionCommand => new Command(async (parameter) =>
        {
            await GetFiltersAsync();
            var name = parameter.ToString();
            var foundCondition = Conditions.Where(c => c.Name.StartsWith(name)).FirstOrDefault();

            if (foundCondition == null)
            {
                Models.Condition newCondition = new Models.Condition()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name
                };
                await DataStoreCondition.AddItemAsync(newCondition);
            }
            else
            {
                await DataStoreCondition.DeleteItemAsync(foundCondition.Id);
            }
        });

        public ICommand DoneCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(MarketPage)}");
        });

        public ICommand ResetCommand => new Command(async () =>
        {
            await GetFiltersAsync();

            foreach(var cat in Categories)
                await DataStoreCategory.DeleteItemAsync(cat.Id);
            
            Categories.Clear();

            foreach (var con in Conditions)
                await DataStoreCondition.DeleteItemAsync(con.Id);

            Conditions.Clear();

            await Shell.Current.GoToAsync($"//{nameof(MarketPage)}");
        });

        public async Task GetFiltersAsync()
        {
            IsBusy = true;

            try
            {
                if (Categories.Count > 0)
                    Categories.Clear();
                if (Conditions.Count > 0)
                    Conditions.Clear();

                var categories = await DataStoreCategory.GetItemsAsync(true);
                var conditions = await DataStoreCondition.GetItemsAsync(true);

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                foreach (var condition in conditions)
                {
                    Conditions.Add(condition);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}