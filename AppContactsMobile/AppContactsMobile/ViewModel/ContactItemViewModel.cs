using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppContactsMobile.Models;
using AppContactsMobile.Services;
using GalaSoft.MvvmLight.Command;

namespace AppContactsMobile.ViewModel
{
   public class ContactItemViewModel : Contact
    {
        #region Attributes

        private NavigationService navigationService;

        #endregion


        #region Constructor

        public ContactItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Command

        public ICommand EditContactCommand
        {
            get
            {
                return  new RelayCommand(EditContact);
            }
        }

        private async void EditContact()
        {

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditContact = new EditContactViewModel(this);

            await navigationService.Navigate("EditContactView");
        }

        #endregion
    }
}
