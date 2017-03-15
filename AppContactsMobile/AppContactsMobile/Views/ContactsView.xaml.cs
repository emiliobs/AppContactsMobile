using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContactsMobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppContactsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsView : ContentPage
    {
        public ContactsView()
        {
            InitializeComponent();


            //Aqui refrezco el lisview de forma autonoma:
            var mainViewModel = MainViewModel.GetInstance();

            base.Appearing += (sender, args) =>
            {
                mainViewModel.RefreshCommand.Execute(this);
            };

        }

     
    }
}
