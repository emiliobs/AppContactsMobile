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
    public class NewContactViewModel : Contact , INotifyPropertyChanged
    {

        #region Attributes

        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        private ImageSource imageSource;
        private MediaFile file;

        private bool isRunning;
        private bool isEnabled;

        #endregion


        #region Properties


        public ImageSource Imagesource
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

        public ICommand TakePictureCommand
        {
            get { return  new RelayCommand(TakePicture);}
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await dialogService.ShowMessage("No Camara", ":(No Camera Available.)");
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsRunning = true;

            if (file != null)
            {
                 Imagesource = ImageSource.FromStream(() =>
                 {
                     var stream = file.GetStream();
                     return stream;
                 });
            }

            IsRunning = false;
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


            var imageArray = FilesHelper.ReadFully(file.GetStream());
            file.Dispose();

            var contact = new Contact()
            {
                LastName = LastName,
                FirstName = FirstName,
                ImageArray = imageArray,
               PhoneNumber = PhoneNumber,
               
            };

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.Post("http://contactsbackend.azurewebsites.net", "/api", "/Contacts", contact);

            IsRunning = false;
            IsEnabled = true;

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
