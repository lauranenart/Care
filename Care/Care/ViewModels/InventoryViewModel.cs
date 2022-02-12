using Care.Models;
using Care.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Care.ViewModels
{
    public class InventoryViewModel : ContentPage
    {
        public ObservableCollection<UserAndItemModel> Items { get; }
        public UserAndItemModel Item { get; set; }
        public Command LoadItemsCommand { get; }

        private readonly ItemService context;
        private readonly UserService contextUsers;
        public InventoryViewModel()
        {
            context = new ItemService();
            contextUsers = new UserService();
            Items = new ObservableCollection<UserAndItemModel>();
            Item = new UserAndItemModel();
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());
        }

        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                List<UserAndItemModel> UsersAndItems = (getItemInfo())
                .Where(item => item.UserId == (int)Application.Current.Properties["UserId"])
                .ToList();
                Items.Clear();
                foreach (var item in UsersAndItems)
                {
                    Items.Add(item);
                }
                Item = Items.First();
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

        public void OnAppearing()
        {
            IsBusy = true;
            ExecuteLoadItemsCommand();
        }
        

        public List<UserAndItemModel> getItemInfo()
        {
            var Users = contextUsers.Users.ToList();
            var Items = context.Items.ToList();

            var list = (from item in Items
                        join user in Users on item.UserId equals user.UserId
                        orderby item.UserId
                        select new
                        {
                            UserId = item.UserId,
                            EmailAdress = user.EmailAddress,
                            ItemName = item.Name,
                            ImageName = item.ImageName,
                            ImageId = item.ImageId,
                            Condition = item.Condition,
                            Category = item.Category
                        }).AsEnumerable().Select(linq => new UserAndItemModel
                        {
                            UserId = linq.UserId,
                            EmailAddress = linq.EmailAdress,
                            ItemName = linq.ItemName,
                            ImageName = linq.ImageName,
                            ImageId = linq.ImageId,
                            Condition = linq.Condition,
                            Category = linq.Category
                        }).ToList();

            return list;
        }
    }
}