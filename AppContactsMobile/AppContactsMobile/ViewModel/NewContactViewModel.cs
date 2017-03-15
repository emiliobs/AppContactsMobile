using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppContactsMobile.Annotations;
using AppContactsMobile.Classes;
using AppContactsMobile.Models;
using AppContactsMobile.Services;
using GalaSoft.MvvmLight.Command;

namespace AppContactsMobile.ViewModel
{
    public class NewContactViewModel : Contact , INotifyPropertyChanged
    {

        #region Attributes

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        private bool isRunning;
        private bool isEnabled;

        #endregion


        #region Properties

        public bool IsRunning
        {
            get { return isRunning; }

            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }

            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

       

        #endregion


        #region Contructor

        public NewContactViewModel()
        {
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            IsEnabled = true;
        }

        #endregion


        #region Methods

        #endregion

        #region Commands

        public ICommand NewContactCommand
        {
            get { return  new RelayCommand(NewContact);}
        }

        private async void NewContact()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await dialogService.ShowMessage("Error", "You must enter a Firts Name.");
                return;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await dialogService.ShowMessage("Error", "You must enter a Last Name.");
                return;
            }


            var response = await apiService.Post("http://contactsbackend.azurewebsites.net", "/api", "/Contacts", this);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error",response.Message);

                return;
            }

            await navigationService.Back();

        }

        #endregion


        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

       

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion     
    }
}
