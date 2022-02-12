using Care.Helpers.Validators;
using Care.Helpers.Validators.Rules;
using Care.Models;
using Care.Services;
using Care.Views;
using Microsoft.AspNetCore.Http;
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
    public class AdminViewModel : INotifyPropertyChanged
    {
        private readonly PostService context;
        public ValidatableObject<string> OrgName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> OrgShortDescr { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> OrgLongDescr { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> OrgLink { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<MediaFile> OrgPhotoFile { get; set; } = new ValidatableObject<MediaFile>();
        public ValidatableObject<MediaFile> OrgLogoFile { get; set; } = new ValidatableObject<MediaFile>();
        public ValidatableObject<string> OrgPhotoName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> OrgLogoName { get; set; } = new ValidatableObject<string>();
        public ImageSource ChosenPhoto { get; set; }
        public ImageSource ChosenLogo { get; set; }

        public AdminViewModel()
        {
            this.context = new PostService();
            AddValidationRules();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void AddValidationRules()
        {
            OrgName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Title Required" });
            OrgShortDescr.Validations.Add(new IsValidLength<string> { ValidationMessage = "Must be 25-50 symbols length" });
            OrgLongDescr.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Long Description Required" });
            OrgLongDescr.Validations.Add(new IsValidLongDescrLength<string> { ValidationMessage = "Must be at least 50 symbols" });
            OrgPhotoFile.Validations.Add(new IsNotNullOrEmptyRule<MediaFile> { ValidationMessage = "Organization Photo Required" });
            OrgLogoFile.Validations.Add(new IsNotNullOrEmptyRule<MediaFile> { ValidationMessage = "Organization Logo Required" });
        }

        public ICommand AddOrgCommand => new Command(async () =>
        {
            bool isTitleValid = OrgName.Validate();
            bool isShortDescrValid = OrgShortDescr.Validate();
            bool isLongDescrValid = OrgLongDescr.Validate();
            bool isPhotoFileValid = OrgPhotoFile.Validate();
            bool isLogoFileValid = OrgLogoFile.Validate();

            if (isTitleValid && isLongDescrValid && isShortDescrValid && isPhotoFileValid && isLogoFileValid)
            {
                PostModel post = new PostModel();
                post.OrgName = OrgName.Value;
                post.OrgShortDescr = OrgShortDescr.Value;
                post.OrgLongDescr = OrgLongDescr.Value;
                post.OrgLink = OrgLink.Value;
                post.OrgPhotoFile = OrgPhotoFile.Value;
                post.OrgLogoFile = OrgLogoFile.Value;

                SaveFile(OrgPhotoFile.Value.Path, post);
                SaveFile(OrgLogoFile.Value.Path, post);

                await context.Add(post);
                await Shell.Current.GoToAsync($"//{nameof(AdminPage)}");
            }
            else
            {

            }
        });

        public async void SaveFile(string path, PostModel post)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var fileNameWithExtenstion = Path.GetFileName(path);
            string fileName = Path.GetFileNameWithoutExtension(fileNameWithExtenstion);
            string extension = Path.GetExtension(fileNameWithExtenstion);

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string filePath = Path.Combine(folderPath, fileName);

            if (path == OrgPhotoFile.Value.Path)
            {
                post.OrgPhotoName = fileName;

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    await OrgPhotoFile.Value.GetStream().CopyToAsync(fileStream);
                }
            }
            else
            {
                post.OrgLogoName = fileName;

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    await OrgLogoFile.Value.GetStream().CopyToAsync(fileStream);
                }
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

            OrgPhotoFile.Value = selectedImageFile;

            ChosenPhoto = ImageSource.FromStream(() => selectedImageFile.GetStream());
        });

        public ICommand AddLogoCommand => new Command(async () =>
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
            var selectedLogoFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            OrgLogoFile.Value = selectedLogoFile;

            ChosenLogo = ImageSource.FromStream(() => selectedLogoFile.GetStream());
        });
    }
}