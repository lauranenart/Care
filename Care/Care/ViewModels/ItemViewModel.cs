using Care.Helpers.Validators;
using Care.Helpers.Validators.Rules;
using Care.Models;
using Care.Services;
using Care.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Care.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private readonly ItemService context;
        public ValidatableObject<string> Name { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Condition { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Category { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<MediaFile> ImageFile { get; set; } = new ValidatableObject<MediaFile>();
        public ValidatableObject<string> ImageName { get; set; } = new ValidatableObject<string>();
        public ImageSource ChosenPhoto { get; set; }

        public int ImageId { get; set; }

        public ItemViewModel()
        {
            this.context = new ItemService();
            AddValidationRules();
        }

        public ItemViewModel(int ImageId)
        {
            this.context = new ItemService();
            AddValidationRules();
            LoadItemId(ImageId);
        }
        public event PropertyChangedEventHandler PropertyChanged;


        public void LoadItemId(int itemId)
        {
            var item = context.FindById(itemId);
            ImageId = itemId;
            Name.Value = item.Name;
            Condition.Value = item.Condition;
            Category.Value = item.Category;
        }

        public void AddValidationRules()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Name Required" });
            Name.Validations.Add(new IsValidNameLength<string> { ValidationMessage = "Maximum 25 symbols length" });
            Condition.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Condition Required" });
            Category.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Category Required" });
            ImageFile.Validations.Add(new IsNotNullOrEmptyRule<MediaFile> { ValidationMessage = "Item Photo Required" });
        }

        public ICommand AddItemCommand => new Command(async () =>
        {
            bool isNameValid = Name.Validate();
            bool isConditionValid = Condition.Validate();
            bool isCategoryValid = Category.Validate();
            bool isImageFileValid = ImageFile.Validate();

            if (isNameValid && isConditionValid && isCategoryValid && isImageFileValid)
            {
                ItemModel item = new ItemModel();
                item.Name = Name.Value;
                item.UserId = (int)Application.Current.Properties["UserId"];
                item.Condition = Condition.Value;
                item.Category = Category.Value;
                item.ImageFile = ImageFile.Value;

                SaveFile(ImageFile.Value.Path, item);

                await context.Add(item);
                await Shell.Current.GoToAsync($"//{nameof(MarketPage)}");
            }
            else
            {

            }
        });

        public ICommand EditItemCommand => new Command(async () =>
        {
            bool isNameValid = Name.Validate();
            bool isConditionValid = Condition.Validate();
            bool isCategoryValid = Category.Validate();

            if (isNameValid && isConditionValid && isCategoryValid)
            {
                ItemModel previousItem = context.FindById(ImageId);

                ItemModel updatedItem = new ItemModel();
                updatedItem.Name = Name.Value;
                updatedItem.UserId = previousItem.UserId;
                updatedItem.Condition = Condition.Value;
                updatedItem.Category = Category.Value;
                updatedItem.ImageName = previousItem.ImageName;
                updatedItem.ImageId = previousItem.ImageId;

                await context.Update(ImageId, updatedItem);
                await Shell.Current.GoToAsync($"//{nameof(MarketPage)}");
            }
        });

        public async void SaveFile(string path, ItemModel item)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var fileNameWithExtenstion = Path.GetFileName(path);
            string fileName = Path.GetFileNameWithoutExtension(fileNameWithExtenstion);
            string extension = Path.GetExtension(fileNameWithExtenstion);

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string filePath = Path.Combine(folderPath, fileName);

            item.ImageName = fileName;

            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await ImageFile.Value.GetStream().CopyToAsync(fileStream);
            }
        }

        public ICommand AddPhotoCommand => new Command(async () =>
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Full
            };
            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            ImageFile.Value = selectedImageFile;

            ChosenPhoto = ImageSource.FromStream(() => selectedImageFile.GetStream());
        });
    }
}