using CRM.Models;
using CRM.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Forms;

namespace CRM.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel itemDetailViewModel;

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = itemDetailViewModel = new ItemDetailViewModel();
            Submit.Clicked += SubmitClicked;
        }

        private void SubmitClicked(object sender, EventArgs e)
        {
            try
            {
                LoginResponse loginResponse;
                string status = Toggle.IsToggled ? Text.Text == itemDetailViewModel.item.text && itemDetailViewModel.item.status != "11" && itemDetailViewModel.item.status != "1" ? "10" : "11" : Text.Text == itemDetailViewModel.item.text && itemDetailViewModel.item.status != "1" && itemDetailViewModel.item.status != "11" ? "0" : "1";
                string token = Application.Current.Properties["token"].ToString();
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create("https://uxcandy.com/~shapoval/test-task-backend/v2/edit/" + itemDetailViewModel.ItemId + "?developer=TestUser");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    streamWriter.Write($"token={token}&status={status}&text={Text.Text}");
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(streamReader.ReadToEnd());
                TouchUpNotification.Text = "Ok !";
            }
            catch(Exception ex)
            {
                TouchUpNotification.Text = "Check Out Your Internet !";
            }
        }
    }
}