using CRM.Models;
using CRM.Services;
using CRM.ViewModels;
using CRM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemsViewModel(Previous, Next);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Filter(object sender, EventArgs e)
        {
            MockDataStore.sort_field = Picker.Items[Picker.SelectedIndex];
            _viewModel.ExecuteLoadItemsCommand();
        }
        
        private void Ranges(object sender, EventArgs e)
        {
            MockDataStore.range = Range.Items[Range.SelectedIndex];
            _viewModel.ExecuteLoadItemsCommand();
        }
    }
}