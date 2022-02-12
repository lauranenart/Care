using Android.Content.Res;
using Care.Models;
using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace Care.Helpers
{
    class Authenticator
    {
        public bool AuthenticateAdmin(AdminModel admin)
        {
            string text = string.Empty;
            string filename = "auth";

            AssetManager assets = Android.App.Application.Context.Assets;
            

            String hash, salt;
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    using (StreamReader authFile = new StreamReader(assets.Open(filename)))
                    {

                        hash = authFile.ReadLine();
                        salt = authFile.ReadLine();
                    }
                }
                else
                {
                    using (StreamReader authFile = new StreamReader(filename))
                    {
                        hash = authFile.ReadLine();
                        salt = authFile.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }

     

            if (hash == null || salt == null)
            {
                return false;
            }
            else
            {
                return PasswordManager.VerifyHashedPassword(admin.Password, hash, salt);
            }
        }

        public bool AuthenticateLogin(string pass, string hash, string salt)
        {
            bool authorizedLogin = PasswordManager.VerifyHashedPassword(pass, hash, salt);
            return authorizedLogin;
        }

        public enum UserType
        {
            NONE,
            USER,
            ADMIN
        }

        public static UserType GetUserType(Int32? UserType)
        {
            if (UserType == null)
            {
                return Authenticator.UserType.NONE;
            }
            else
            {
                return (UserType)UserType;
            }
        }
    }
}
