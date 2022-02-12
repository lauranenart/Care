using Care.Models;
using Care.Services;
using Care.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly PostService context;
        private readonly UserService userContext;
        public HomePage()
        {
            context = new PostService();
            userContext = new UserService();
            InitializeComponent();
            BindingContext = this;
        }

        public ObservableCollection<PostModel> Posts { get; set; } = new ObservableCollection<PostModel>();

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }

        private void Close_Frame_Tapped(object sender, EventArgs e)
        {
            newUserFrame.IsVisible = false;
        }
        protected override void OnAppearing()
        {
            if (!Application.Current.Properties["New"].Equals("Yes"))
            {
                newUserFrame.IsVisible = false;
            }
            else
            {
                var newUser = userContext.Users.OrderByDescending(u => u.UserId).Take(1).FirstOrDefault();
                newUserLb.Text = newUser.EmailAddress + " thank you for joining us!";
            }
            base.OnAppearing();
            var posts = context.Posts;

            Posts.Clear();

            foreach(var post in posts)
            {
                Posts.Add(post);
            }
        }
        private async void PostTapped(object sender, EventArgs e)
        {
            var orgId = ((TappedEventArgs)e).Parameter.ToString();

            PostModel post = context.FindById(Int32.Parse(orgId));
            await Shell.Current.GoToAsync($"{nameof(PostDetailPage)}?{nameof(PostDetailViewModel.PostId)}={post.OrgId}");
        }
    }
}