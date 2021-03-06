using Care.ViewModels;
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
    public partial class RegistrationForm : ContentPage
    {
        public RegistrationForm()
        {
            InitializeComponent();
            BindingContext = new AuthViewModel();
        }

        async void OnPreviousPageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
        }

    }
}