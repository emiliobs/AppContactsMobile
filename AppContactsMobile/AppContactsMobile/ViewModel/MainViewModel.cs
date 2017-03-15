using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AppContactsMobile.Views;
using GalaSoft.MvvmLight.Command;

namespace AppContactsMobile.ViewModel
{
    public class MainViewModel  : INotifyPropertyChanged
    {
        #region Attributes

        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;

        private bool isRefreshing;


        #endregion

        #region Properties

        public ObservableCollection<ContactItemViewModel> Contacts { get; set; }

        public NewContactViewModel NewContactViewModel { get; set; }

        public EditContactViewModel EditContact { get; set; }

        public bool IsRefresh
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();

                }
            }

            get { return isRefreshing; }
        }



        #endregion

        #region Contructor

        public MainViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();    
            navigationService = new NavigationService();

            Contacts = new ObservableCollection<ContactItemViewModel>();

          //Intance singleton:
            instance = this;
        }


        #endregion



        #region Command

        public ICommand RefreshCommand
        {
            get { return  new  RelayCommand(Refresh);}
        }

        private void Refresh()
        {
            IsRefresh = true;
            LoadContact();
            isRefreshing = false;
        }

        public ICommand NewContactCommand
        {
            get { return new RelayCommand(NewContact); }
        }

        

        private async void NewContact()
        {
            //Llamo a la clase NewCpntactviewModel 
            //solo en el momento que se necesita. no en el contructor para evitar memoria inecesaria:
           NewContactViewModel = new NewContactViewModel();

           await navigationService.Navigate("NewContactView");
        }

        #endregion

        #region Method

        private async void LoadContact()
        {
            var response = await apiService.Get<Contact>("http://contactsbackend.azurewebsites.net","/api", "/Contacts");

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
            }

            //Aqui casteo a contact el onjeto result:
            ReloadContacts((List<Contact>)response.Result);
        }

        private void ReloadContacts(List<Contact> contacts)
        {
           Contacts.Clear();

            foreach (var contact in contacts.OrderBy(c => c.FirstName).ThenBy(c => c.LastName))
            {
              Contacts.Add(new ContactItemViewModel()
              {
                  Image = contact.Image,
                  ContactId = contact.ContactId,
                  FirstName = contact.FirstName,
                  LastName = contact.LastName,
                  EmailAddress = contact.EmailAddress,
                  PhoneNumber = contact.PhoneNumber,
              });  
            }
        }

        #endregion


        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            return instance ?? (instance = new MainViewModel());
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
