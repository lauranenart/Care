using Care.Models;
using Care.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostDetailPage : ContentPage
    {
        public PostDetailPage()
        {
            InitializeComponent();
            BindingContext = new PostDetailViewModel();
        }

        private async void VisitClicked(object sender, EventArgs e)
        {
            var orgLink = ((Button)sender).BindingContext as String;
            await Launcher.OpenAsync(new Uri(orgLink));
        }

        protected override void OnAppearing()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var fileName = postPhoto.BindingContext.ToString();
            var imagePath = Path.Combine(folderPath, fileName);

            if (File.Exists(imagePath))
                postPhoto.Source = ImageSource.FromFile(imagePath);
            else
                postPhoto.Source = ImageSource.FromFile("helping.jpg");

            try
            {
                var fileNameLogo = postLogo.BindingContext.ToString();
                var imagePathLogo = Path.Combine(folderPath, fileNameLogo);

                if (File.Exists(imagePathLogo))
                {
                    logoFr.IsVisible = true;
                    postLogo.Source = ImageSource.FromFile(imagePathLogo);
                }
            }
            catch
            {
            }

            base.OnAppearing();
        }
    }
}