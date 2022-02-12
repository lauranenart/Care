using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }

        private async void MarketTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MarketPage(), true);
        }

        private async void AddOrganizationTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(OrganizationForm)}");
        }
    }
}