
using Care.Helpers;
using Care.Models;
using Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        [Obsolete]
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void LoginAsUser(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MethodPage(), false);
        }

        private async void LoginAsAdmin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminForm(), false);
        }
    }
}