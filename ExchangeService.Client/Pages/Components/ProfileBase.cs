using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeService.Client.Pages.Components
{
    public class ProfileBase : BlazorComponent
    {
        public UserView UserProfile { get; set; } = new UserView();
        public List<string> Errors = new List<string>();
        public bool IsValid = false;

        public void ValidateInputs()
        {
            if (string.IsNullOrEmpty(UserProfile.Name))
                Errors.Add("Please fill your name.");
            if (string.IsNullOrEmpty(UserProfile.Location))
                Errors.Add("Please fill your location.");

            if (string.IsNullOrEmpty(UserProfile.PhoneNumber) && string.IsNullOrEmpty(UserProfile.ContactEmail))
                Errors.Add("Please fill at least one contact detail.");
            else
            {
                if (!string.IsNullOrEmpty(UserProfile.PhoneNumber))
                {
                    string phonePattern = @"\d{3}-? *\d{3}-? *-?\d{3}";
                    Regex rgx = new Regex(phonePattern);
                    if (!rgx.IsMatch(UserProfile.PhoneNumber))
                        Errors.Add("Phone number has invalid format.");
                }
                if (!string.IsNullOrEmpty(UserProfile.ContactEmail))
                {
                    if (!UserProfile.ContactEmail.Contains("@"))
                        Errors.Add("Email has invalid format.");
                }
            }
            IsValid = Errors.Count() == 0;
            StateHasChanged();
        }
    }
}
