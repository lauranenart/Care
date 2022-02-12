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
    public partial class MethodPage : ContentPage
    {
        public MethodPage()
        {
            InitializeComponent();
        }

        async void OnPreviousPageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
        }

        async void ClickedRegister(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationForm(), false);
        }
        async void ClickedLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginForm(), false);
        }

    }
}