using CRM.Models;
using CRM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    public partial class NewItemPage : ContentPage
    {
        NewItemViewModel newItemViewModel;

        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = newItemViewModel = new NewItemViewModel();
            newItemViewModel.label = EmmailNotification;
        }
    }
}