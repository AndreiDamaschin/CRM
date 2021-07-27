using CRM.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        bool toggle;
        private string itemId;
        private string name;
        private string email;
        private string status;
        private string text;

        public Item item;

        public string Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
        
        public bool Toggle
        {
            get => toggle;
            set => SetProperty(ref toggle, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                item = await DataStore.GetItemAsync(itemId);
                Id = item.id;
                Name = item.username;
                Email = item.email;
                Status = item.status;
                Text = item.text;
                Toggle = Status == "10" || Status == "11" ? true : false;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
