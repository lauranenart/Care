using Care.ViewModels;
using Care.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Care
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PostDetailPage), typeof(PostDetailPage));
            Routing.RegisterRoute(nameof(OrganizationForm), typeof(OrganizationForm));
            Routing.RegisterRoute(nameof(ItemForm), typeof(ItemForm));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AuthPage");
        }
    }
}
