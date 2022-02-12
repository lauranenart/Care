using Care.Models;
using Care.Services;
using Care.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketPage : ContentPage
    {
        private readonly ItemService context;

        private readonly UserService contextUsers;
        public ObservableCollection<ItemModel> Items { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<UserAndItemModel> UserItems { get; set; } = new ObservableCollection<UserAndItemModel>();
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Models.Condition> Conditions { get; set; } = new ObservableCollection<Models.Condition>();

        BaseViewModel baseViewModel;

        Label categoryLb;
        Label conditionLb;
        Label nameLb;
        int count = 0;
        Frame frame;

        public MarketPage()
        {
            context = new ItemService();
            contextUsers = new UserService();
            InitializeComponent();
            baseViewModel = new BaseViewModel();
        }

        protected override void OnAppearing()
        {
            CollectItems();
            CreatePageElements();
            base.OnAppearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                CollectItems();
            else
            {
                var items = new ObservableCollection<ItemModel>(Items);
                Items.Clear();
                try
                {
                    if ((int)Application.Current.Properties["UserId"] != 0)
                    {
                        var filteredItems = items.OrderByDescending(i => i.UserId).Reverse().Skip(items.Where(i => i.UserId == 0).Count()).Reverse();
                        if (filteredItems.Where(i => i.Name.StartsWith(e.NewTextValue)).Count() > 0)
                            Items.Add(filteredItems.Where(i => i.Name.StartsWith(e.NewTextValue)).FirstOrDefault());
                    }
                    else
                    {
                        if (items.Where(i => i.Name.StartsWith(e.NewTextValue)).Count() > 0)
                            Items.Add(items.Where(i => i.Name.StartsWith(e.NewTextValue)).FirstOrDefault());
                    }
                }catch
                {
                    if(items.Where(i => i.Name.StartsWith(e.NewTextValue)).Count() > 0)
                        Items.Add(items.Where(i => i.Name.StartsWith(e.NewTextValue)).FirstOrDefault());
                }
                
                CleanGrid();
            }
            if(Items.Count > 0)
                CreatePageElements();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ItemForm)}");
        }

        private async void Filters_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FiltersPage(), false);
        }

        private void Close_Frame_Tapped(object sender, EventArgs e)
        {
            userWithMostItemsFrame.IsVisible = false;
        }

        public async void CollectItems()
        {
            await GetFiltersAsync();
            var items = context.Items;
            Items.Clear();
            CleanGrid();

            if ((int)Application.Current.Properties["UserId"] != 0)
            {
                try
                {
                    items = items.OrderByDescending(i => i.UserId).Reverse().Skip(items.Where(i => i.UserId == 0).Count()).Reverse();
                }
                catch { }
            }

            foreach (var item in items)
            {
                Items.Add(item);
            }

            if(Conditions.Count > 0)
                Items.Where(i => !Conditions.Any(c => i.Condition.StartsWith(c.Name))).ToList().All(i => Items.Remove(i));

            if (Categories.Count > 0)
                Items.Where(i => !Categories.Any(c => i.Category.StartsWith(c.Name))).ToList().All(i => Items.Remove(i));
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

        public void GetUserWithMostItems(List<UserAndItemModel> UserItems)
        {
            var userWithMostItems = (from item in UserItems
                                     orderby item.UserId
                                     group item by item.UserId into itemGroup
                                     select new
                                     {
                                         UserId = itemGroup.Key,
                                         EmailAddress = itemGroup.Where(item => item.UserId == itemGroup.Key).ElementAt(0).EmailAddress,
                                         Count = itemGroup.Count()
                                     }).Aggregate(
                                    new
                                    {
                                        UserId = 0,
                                        EmailAddress = "",
                                        Count = 0
                                    },
                                    (current, next) => next.Count > current.Count ? next : current
                                );
        }

        public async Task GetFiltersAsync()
        {
            IsBusy = true;

            try
            {
               if (Categories.Count > 0)
                    Categories.Clear();
                if (Conditions.Count > 0)
                    Conditions.Clear();

                var categories = await baseViewModel.DataStoreCategory.GetItemsAsync(true);
                var conditions = await baseViewModel.DataStoreCondition.GetItemsAsync(true);

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                foreach (var condition in conditions)
                {
                    Conditions.Add(condition);
                }
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


        public void CreatePageElements()
        {
            List<UserAndItemModel> UserItems = (getItemInfo())
                .Where(item => item.UserId == (int)Application.Current.Properties["UserId"])
                .ToList();

            GetUserWithMostItems(UserItems);

            if(UserItems.Count == 0)
            {
                userWithMostItemsFrame.IsVisible = false;
            }
            else
                userWithMostItemsLb.Text = "User " + UserItems.First().EmailAddress + " has uploaded the most (" + UserItems.Count + ") items.";

            count = (Items.Count + 1) / 2;

            for (var i = 0; i < count; i++)
            {
                gridLayout.RowDefinitions.Add(new RowDefinition());
            }

            var productIndex = 0;
            for (int rowIndex = 0; rowIndex < count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 2; columnIndex++)
                {
                    if (productIndex >= Items.Count)
                    {
                        break;
                    }
                    var item = Items[productIndex];
                    productIndex += 1;

                    if (item != null)
                    {
                        CreateItemFrame(item);
                        gridLayout.Children.Add(frame, columnIndex, rowIndex);
                    }
                }
            }
        }

        public void CreateItemFrame(ItemModel item)
        {
            var InfoGrid = CreateInfoGrid();
            var ImageGrid = CreateImageGrid(item);

            CreateLabels(item);
            InfoGrid.Children.Add(CreateConditionInfo(), 0, 0);
            InfoGrid.Children.Add(conditionLb, 1, 0);
            InfoGrid.Children.Add(CreateCategoryInfo(), 0, 1);
            InfoGrid.Children.Add(categoryLb, 1, 1);

            var stackLayout = new StackLayout();
            stackLayout.Children.Add(ImageGrid);
            stackLayout.Children.Add(nameLb);
            stackLayout.Children.Add(InfoGrid);

            frame = CreateMainFrame();
            frame.Content = stackLayout;
        }

        public void CleanGrid()
        {
            foreach (var child in gridLayout.Children.Reverse())
            {
                gridLayout.Children.Remove(child);
            }

            foreach (var row in gridLayout.RowDefinitions.Reverse())
            {
                gridLayout.RowDefinitions.Remove(row);
            }
        }

        private Frame CreateCategoryInfo()
        {
            var frameCategory = new Frame
            {
                CornerRadius = 20,
                Padding = 0,
                Margin = new Thickness(-10, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 30,
                WidthRequest = 90,
                IsClippedToBounds = false,
                HasShadow = false,
                BackgroundColor = Color.FromHex("ddbea9")
            };

            var labelCategory = new Label
            {
                Text = "Category",
                TextColor = Color.White,
                Padding = 3,
                Margin = new Thickness(10, 0, 10, 0),
                HeightRequest = 25,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            frameCategory.Content = labelCategory;

            return frameCategory;
        }

        private Frame CreateConditionInfo()
        {
            var frameCondition = new Frame
            {
                CornerRadius = 20,
                Padding = 0,
                Margin = new Thickness(-10, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 30,
                WidthRequest = 90,
                IsClippedToBounds = false,
                HasShadow = false,
                BackgroundColor = Color.FromHex("ddbea9")
            };

            var labelCondition = new Label
            {
                Text = "Condition",
                TextColor = Color.White,
                Padding = 3,
                Margin = new Thickness(10, 0, 10, 0),
                HeightRequest = 25,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            frameCondition.Content = labelCondition;

            return frameCondition;
        }


        private Grid CreateInfoGrid()
        {
            Grid InfoGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
                }
            };
            return InfoGrid;
        } 

        private void CreateLabels(ItemModel item)
        {
            categoryLb = new Label
            {
                Text = item.Category,
                WidthRequest = 70,
                Margin = new Thickness(0, 0, -20, 0),
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            conditionLb = new Label
            {
                Text = item.Condition,
                WidthRequest = 70,
                Margin = new Thickness(0, 0, -20, 0),
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            nameLb = new Label
            {
                Text = item.Name,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center
            };
        }

        private Grid CreateImageGrid(ItemModel item)
        {
            var image = new Image
            {
                Source = item.ImageName,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                Aspect = Aspect.AspectFill,
                Margin = -20,
                HeightRequest = 100,
                WidthRequest = 100
            };

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var fileName = item.ImageName;
            var imagePath = Path.Combine(folderPath, fileName);

            if (File.Exists(imagePath))
                image.Source = ImageSource.FromFile(imagePath);
            else
                image.Source = ImageSource.FromFile("market_photo.png");


            Frame circleImageFrame = new Frame
            {
                Margin = 10,
                BorderColor = Color.Black,
                CornerRadius = 50,
                HeightRequest = 60,
                WidthRequest = 60,
                IsClippedToBounds = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            circleImageFrame.Content = image;

            Grid ImageGrid = new Grid();

            RowDefinition rowDefinition = new RowDefinition();

            rowDefinition.Height = new GridLength(1, GridUnitType.Auto);

            ImageGrid.RowDefinitions.Add(rowDefinition);

            ImageGrid.Children.Add(circleImageFrame, 0, 0);

            return ImageGrid;
        }

        private Frame CreateMainFrame()
        {
            var frame = new Frame
            {
                CornerRadius = 10,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                IsClippedToBounds = true,
                HeightRequest = 270,
                WidthRequest = 180,
                HasShadow = true,
                BackgroundColor = Color.White,
                Opacity = 0.9
            };
            return frame;
        }
    }
}