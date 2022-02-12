using Android.Content.Res;
using Care.Helpers;
using Care.Helpers.Validators;
using Care.Helpers.Validators.Rules;
using Care.Models;
using Care.Services;
using Care.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Care.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly UserService context;
        private readonly Authenticator authenticator;

        public event PropertyChangedEventHandler PropertyChanged;

        public ValidatableObject<string> EmailAddress { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> FullName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public AuthViewModel()
        {
            this.context = new UserService();
            this.authenticator = new Authenticator();
        }
        public void AddRegistrationValidationRules()
        {
            FullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Name Required" });

            EmailAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            EmailAddress.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Invalid Email" });
            EmailAddress.Validations.Add(new IsAvailableEmailRule<string> { ValidationMessage = "An account with this email already exists" });

            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Invalid Password" });
        }

        public void AddUserLoginValidationRules()
        {
            EmailAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
        }

        public ICommand RegisterCommand => new Command(async () =>
        {
            AddRegistrationValidationRules();

            bool isNameValid = FullName.Validate();
            bool isEmailValid = EmailAddress.Validate();
            bool isPasswordValid = Password.Validate();

            FullName.Validations.Clear();
            EmailAddress.Validations.Clear();
            Password.Validations.Clear();

            if (isNameValid && isEmailValid && isPasswordValid)
            {
                UserRegistrationModel userModel = new UserRegistrationModel();
                userModel.EmailAddress = EmailAddress.Value;
                userModel.FullName = FullName.Value;
                userModel.Password = Password.Value;

                UserModel newUser = new UserModel();
                newUser.EmailAddress = userModel.EmailAddress;
                string salt = PasswordManager.CreateSalt();
                newUser.PasswordHash = PasswordManager.HashPassword(userModel.Password, salt);
                newUser.PasswordSalt = salt;

                await context.Add(newUser);

                var storedUser = context.FindUserByEmail(newUser.EmailAddress);

                Application.Current.Properties["UserType"] = (int)Authenticator.UserType.USER;
                Application.Current.Properties["UserId"] = storedUser.UserId;
                Application.Current.Properties["User"] = newUser.EmailAddress;
                Application.Current.Properties["New"] = "Yes";

                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {

            }
        });

        public ICommand LoginCommand => new Command(async () =>
        {
            AddUserLoginValidationRules();

            bool isEmailValid = EmailAddress.Validate();
            bool isPasswordValid = Password.Validate();
            var userExists = false;

            if (isEmailValid)
            {
                userExists = UserModel.Equals(context.FindUserByEmail(EmailAddress.Value), null) ? false : true;
                if (!userExists)
                {
                    EmailAddress.Validations.Add(new IsValidRule<string> { ValidationMessage = "No such user!" });
                    EmailAddress.Validate();
                    EmailAddress.Validations.Clear();
                }
            }

            if (isEmailValid && isPasswordValid && userExists)
            {
                var storedUser = context.FindUserByEmail(EmailAddress.Value);
                if (authenticator.AuthenticateLogin(Password.Value, storedUser.PasswordHash, storedUser.PasswordSalt))
                {
                    Application.Current.Properties["UserType"] = (int)Authenticator.UserType.USER;
                    Application.Current.Properties["UserId"] = storedUser.UserId;
                    Application.Current.Properties["User"] = storedUser.EmailAddress;
                    Application.Current.Properties["New"] = "No";

                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else
                {
                    Password.Validations.Add(new IsValidRule<string> { ValidationMessage = "Invalid password!" });
                    Password.Validate();
                    Password.Validations.Clear();
                }
            }
            else
            {

            }
        });

        public ICommand LoginAdminCommand => new Command(async () =>
        {
            AdminModel admin = new AdminModel();
            admin.Password = Password.Value;

            if (authenticator.AuthenticateAdmin(admin))
            {
                Application.Current.Properties["UserType"] = (int)Authenticator.UserType.ADMIN;
                Application.Current.Properties["UserId"] = 0;
                Application.Current.Properties["New"] = "No";

                await Shell.Current.GoToAsync($"//{nameof(AdminPage)}");
            }
            else
            {
                Password.Validations.Add(new IsValidRule<string> { ValidationMessage = "Invalid password!" });
                Password.Validate();
                Password.Validations.Clear();
            }
        });

        
    }
}
