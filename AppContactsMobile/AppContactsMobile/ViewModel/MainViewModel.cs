using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContactsMobile.Classes;
using AppContactsMobile.Models;

namespace AppContactsMobile.ViewModel
{
    public class MainViewModel
    {
        #region Attributes

        private ApiService apiService;
        private DialogService dialogService;

        #endregion

        #region Properties

        public ObservableCollection<ContactItemViewModel> Contacts { get; set; }

        #endregion

        #region Contructor

        public MainViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();       

            Contacts = new ObservableCollection<ContactItemViewModel>();

            LoadContact();
        }


        #endregion


        #region Command

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

        #region Events

        #endregion
    }
}
