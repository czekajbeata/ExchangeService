using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Blazor;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeService.Client.Services
{
    public class AppState
    {
        public event Action OnChange;

        private readonly HttpClient http;
        private readonly LocalStorage localStorage;
        private UserView userProfile;      
        public bool IsUserLoggedIn {get; private set;}
        public string Token { get; set; }

        public AppState(HttpClient http, LocalStorage localStorage)
        {
            this.http = http;
            this.localStorage = localStorage;
            if (localStorage.GetItem<bool>("isUserLoggedIn"))
            { 
                IsUserLoggedIn = true;
                Token = localStorage.GetItem("token");
                TrySetAccessTokens();
                SetMyProfile();
            }
        }

        public async Task<UserView> GetMyProfile()
        {
            if (localStorage.GetItem<bool>("isUserLoggedIn"))
            {                
                if (userProfile == null)
                {
                    await SetMyProfile();
                }
                NotifyStateChanged();
                return userProfile;
            }
            else return new UserView();
        }

        public void TrySetAccessTokens()
        {
            if (localStorage.GetItem<bool>("isUserLoggedIn"))
            {
                if (!http.DefaultRequestHeaders.Contains("Authorization"))
                    http.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0} ", Token));
            }
            NotifyStateChanged();
        }

        private async Task SetMyProfile()
        {
            userProfile = await http.GetJsonAsync<UserView>("http://localhost:5000/api/users/myprofile");
            localStorage.SetItem("myprofile", userProfile);
            NotifyStateChanged();
        }

        public void LogUser(string token)
        {
            Token = token;
            localStorage.SetItem("token", Token);
            IsUserLoggedIn = true;
            localStorage.SetItem("isUserLoggedIn", IsUserLoggedIn);
            TrySetAccessTokens();
            NotifyStateChanged();
        }

        public void LogOutUser()
        {
            localStorage.RemoveItem("token");
            localStorage.RemoveItem("myprofile");
            localStorage.SetItem("isUserLoggedIn", false);
            userProfile = null;
            Token = null;
            IsUserLoggedIn = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
