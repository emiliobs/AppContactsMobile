using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContactsMobile.Views;

namespace AppContactsMobile.Services
{
    public class NavigationService
    {
        public async Task Navigate(string viewName)
        {
            switch (viewName)
            {
                case "NewContactView":
                    await App.Current.MainPage.Navigation.PushAsync(new NewContactView());
                    break;
                default:
                    break;
            }
        }

        public async Task Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
