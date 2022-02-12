
using Care.Models;
using Care.Services;
using Care.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonkeyCache.FileStore;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Care
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Barrel.ApplicationId = AppInfo.PackageName;

            DependencyService.Register<MockDataStoreCondition>();
            DependencyService.Register<MockDataStoreCategory>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
