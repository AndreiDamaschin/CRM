using CRM.Models;
using CRM.Services;
using CRM.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; set; }
        public Button Previous { get; set; }
        public Button Next { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public Command<Item> ItemTapped { get; set; }

        public ItemsViewModel(Button Previous, Button Next)
        {
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            this.Previous = Previous;

            this.Next = Next;

            this.Previous.Command = new Command(() => PreviousAndNext(-1));

            this.Next.Command = new Command(() => PreviousAndNext(1));

            this.Previous.Text = $"Previous {(MockDataStore.page - 1)}";

            this.Next.Text = $"Next {(MockDataStore.page + 1)}";

            MockDataStore.itemsViewModel = this;
        }

        private void PreviousAndNext(int page)
        {
            MockDataStore.page += page;
            if (MockDataStore.page < 1)
                MockDataStore.page = 1;
            if (MockDataStore.page > MockDataStore.pages)
                MockDataStore.page = MockDataStore.pages;
            Previous.Text = $"Previous {(MockDataStore.page - 1)}";
            Next.Text = $"Next {(MockDataStore.page + 1)}";
            ExecuteLoadItemsCommand();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            try
            {
                if ((string)Application.Current.Properties["token"] == "Невалидный токен")
                    return;
            }
            catch(Exception e)
            {
                return;
            }

            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.id}");
        }
    }
}