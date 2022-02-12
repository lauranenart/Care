using Care.Models;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Care.Services
{
    public class ServiceDbContext
    {
        static string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:31190" : "http://localhost:31190";

        static HttpClient client;

        public ServiceDbContext()
        {
            try
            {
                client = new HttpClient
                {
                    BaseAddress = new Uri(BaseUrl)
                };
            }
            catch
            {

            }
        }

        public async Task PostAsync<T>(string url, T el)
        {
            try
            {   
                var json = JsonConvert.SerializeObject(el);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get information from server {ex}");
                throw ex;
            }
        }

        public async Task PutAsync<T>(string url, T el)
        {
            try
            {
                var json = JsonConvert.SerializeObject(el);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(url, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get information from server {ex}");
                throw ex;
            }
        }

        public async Task DeleteAsync<T>(string url)
        {
            try
            {
                var response = await client.DeleteAsync(url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get information from server {ex}");
                throw ex;
            }
        }

        public async Task<T> GetAsync<T>(string url, string key, int mins = 1, bool forceRefresh = false)
        {
            var json = string.Empty;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                json = Barrel.Current.Get<string>(key);
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    json = await client.GetStringAsync(url).ConfigureAwait(false); 
                    Barrel.Current.Add(key, json, TimeSpan.FromMinutes(mins));
                }
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get information from server {ex}");
                throw ex;
            }
        }
    } 
}
