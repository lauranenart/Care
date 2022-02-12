using Care.Models;
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
    public partial class FiltersPage : ContentPage
    {
        FiltersViewModel filtersViewModel;
        public FiltersPage()
        {
            InitializeComponent();
            this.BindingContext = new FiltersViewModel();
            filtersViewModel = new FiltersViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            CreateButtons();

            await filtersViewModel.GetFiltersAsync();
            
            foreach(Button child in ConditionButtons.Children)
            {
                string name = child.Text;
                if (filtersViewModel.Conditions.Where(c => c.Name.StartsWith(name)).FirstOrDefault() != null)
                    RedesignButton(child);
                else
                    ResetButton(child);
            }

            foreach (Button child in CategoryButtons.Children)
            {
                var name = child.Text;
                if (filtersViewModel.Categories.Where(c => c.Name.StartsWith(name)).FirstOrDefault() != null)
                    RedesignButton(child);
                else
                    ResetButton(child);
            }
        }

        private async void Condition_Clicked(object sender, EventArgs e)
        {
            await filtersViewModel.GetFiltersAsync();
            var name = ((Button)sender).Text;

            if (filtersViewModel.Conditions.Where(c => c.Name.StartsWith(name)).FirstOrDefault() != null)
                RedesignButton(sender);
            else
                ResetButton(sender);
        }

        private async void Category_Clicked(object sender, EventArgs e)
        {
            await filtersViewModel.GetFiltersAsync();
            var name = ((Button)sender).Text;

            if (filtersViewModel.Categories.Where(c => c.Name.StartsWith(name)).FirstOrDefault() != null)
                RedesignButton(sender);
            else
                ResetButton(sender);
        }

        public void RedesignButton(object sender)
        {
            ((Button)sender).TextColor = Color.White;
            ((Button)sender).BackgroundColor = Color.FromHex("b2967d");
            ((Button)sender).BorderColor = Color.FromHex("b2967d");
        }

        public void ResetButton(object sender)
        {
            ((Button)sender).TextColor = Color.Black;
            ((Button)sender).BackgroundColor = Color.FromHex("eae0d5");
            ((Button)sender).BorderColor = Color.FromHex("b2967d");
        }

        public void CreateButtons()
        {
            List<string> conditionTexts = new List<string>
            {
                "New", "Very Good", "Normal", "Satisfactory"
            };

            List<string> categoryTexts = new List<string>
            {
                "Clothes", "Shoes", "Home Appliance", "Food", "Furniture", "Household goods", "Books"
            };

            foreach (string text in conditionTexts)
            {
                Button btn = new Button
                {
                    Text = text,
                    CornerRadius = 15,
                    BorderColor = Color.FromHex("b2967d"),
                    BorderWidth = 1,
                    BackgroundColor = Color.FromHex("eae0d5")
                };
                btn.SetBinding(Button.CommandProperty, new Binding("ConditionCommand"));
                btn.CommandParameter = text;
                btn.Clicked += Condition_Clicked;

                ConditionButtons.Children.Add(btn);
            }

            foreach (string text in categoryTexts)
            {
                Button btn = new Button
                {
                    Text = text,
                    CornerRadius = 15,
                    BorderColor = Color.FromHex("b2967d"),
                    BorderWidth = 1,
                    BackgroundColor = Color.FromHex("eae0d5")
                };
                btn.SetBinding(Button.CommandProperty, new Binding("CategoryCommand"));
                btn.CommandParameter = text;
                btn.Clicked += Category_Clicked;

                CategoryButtons.Children.Add(btn);
            }
        }

    }
}