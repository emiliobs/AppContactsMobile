using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContactsMobile.Models;
using AppContactsMobile.ViewModel;
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

                case "EditContactView":
                    await App.Current.MainPage.Navigation.PushAsync(new EditContactView());
                    break;
                default:
                    break;
            }
        }


        //public async Task EditContact(Contact contact)
        //{
        //    var mainViewModel = MainViewModel.GetInstance();
        //    mainViewModel.EditContact = new  EditContactViewModel(contact);
        //    await App.Current.MainPage.Navigation.PushAsync(new EditContactView());
        //}

        public async Task Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
