using Care.Models;
using Care.Services;
using Care.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryPage : ContentPage
    {
        InventoryViewModel _viewModel;
        ItemService context;
        public InventoryPage()
        {
            context = new ItemService();
            InitializeComponent();
            BindingContext = _viewModel = new InventoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }

        private void Item_Tapped(object sender, EventArgs e)
        {
            if (((StackLayout)sender).Children.Count() > 2)
                ((StackLayout)sender).Children.Remove(((StackLayout)sender).Children.Last());
            else
            {
                StackLayout btnLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal
                };

                Button edit = new Button
                {
                    Text = "Edit",
                    CornerRadius = 15,
                    ImageSource = "edit.png",
                    HeightRequest = 50,
                    BackgroundColor = Color.White,
                    BorderColor = Color.FromHex("34a0a4"),
                    BorderWidth = 1
                };
                edit.Clicked += (s, eventArgs) => OnEditClicked(sender, eventArgs);


                Button delete = new Button
                {
                    Text = "Delete",
                    CornerRadius = 15,
                    ImageSource = "delete.png",
                    HeightRequest = 50,
                    BackgroundColor = Color.White,
                    BorderColor = Color.FromHex("e44f27"),
                    BorderWidth = 1
                };
                delete.Clicked += async (s, eventArgs) => await OnActionSheetCancelDeleteClicked(sender, eventArgs);

                btnLayout.Children.Add(edit);
                btnLayout.Children.Add(delete);
                ((StackLayout)sender).Children.Add(btnLayout);
            }
        }

        private async Task OnActionSheetCancelDeleteClicked(object sender, EventArgs e)
        {
            UserAndItemModel userAndItemModel = ((StackLayout)sender).BindingContext as UserAndItemModel;
            string action = await DisplayActionSheet("Are you sure you want to delete " + userAndItemModel.ItemName + "?", "Cancel", "Delete");
            if (action == "Delete")
            {
                await context.Remove(userAndItemModel.ImageId);
                ((StackLayout)sender).Children.Remove(((StackLayout)sender).Children.Last());
                refreshView.IsRefreshing = true;
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            UserAndItemModel userAndItemModel = ((StackLayout)sender).BindingContext as UserAndItemModel;
            await PopupNavigation.Instance.PushAsync(new ItemEditView(userAndItemModel.ImageId));
        }
    }
}