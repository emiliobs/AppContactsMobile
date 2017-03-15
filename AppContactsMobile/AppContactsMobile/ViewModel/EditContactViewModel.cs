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
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppContactsMobile.ViewModel
{
    public class EditContactViewModel : Contact, INotifyPropertyChanged
    {
        #region Atrributes

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        private bool isRunning;
        private bool isEnabled;

        private ImageSource imageSource;
        private MediaFile file;


        #endregion

        #region Properties

        public bool IsRunning
        {


            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged();
                }
            }

            get { return isRunning; }
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

        public ImageSource ImageSource
        {
           

            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged();
                }
            }

            get { return imageSource; }
        }



        #endregion

        #region Constructor

        public EditContactViewModel(Contact contact)
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            ContactId = contact.ContactId;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Image = contact.Image;
            EmailAddress = contact.EmailAddress;
            PhoneNumber = contact.PhoneNumber;

            IsEnabled = true;

        }



        #endregion


        #region Commands     

        public ICommand TakePictureCommand
        {
            get { return  new RelayCommand(TakePicture);}
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {

                await dialogService.ShowMessage("No Camara", ":( NO Camara avalable.)");
                
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "sample",
                Name = "test,jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }

        public ICommand SaveContactCommand
        {
            get { return  new RelayCommand(SaveContact);}
        }

        private async void SaveContact()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await dialogService.ShowMessage("Error","You must enter a first name");
                return;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await dialogService.ShowMessage("Error", "You must enter a last name");
                return;

            }

           

            var imageArray = FilesHelper.ReadFully(file.GetStream());
            file.Dispose();

            var contact = new Contact()
            {
                Image = Image,
                LastName = LastName,
                FirstName = FirstName,
                ContactId = ContactId,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,
                ImageArray = imageArray,
            };

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Put("http://contactsbackend.azurewebsites.net", "/api", "/Contacts", contact);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);

                return;
            }

            await navigationService.Back();
        }

        public ICommand DeleteContactCommand
        {
            get { return new RelayCommand(DeleteContact); }
        }

       

        private async void DeleteContact()
        {
            var answer = await dialogService.ShowConfirm("Confirm", "Are you sure to delete this records.");

            if (!answer)
            {
                return;
            }

            var contact = new Contact()
            {
                Image = Image,
                LastName = LastName,
                FirstName = FirstName,
                ContactId = ContactId,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber,


            };

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Delete("http://contactsbackend.azurewebsites.net", "/api", "/Contacts", contact);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);

                return;
            }

            await navigationService.Back();

        }

        #endregion

        #region Methods

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
