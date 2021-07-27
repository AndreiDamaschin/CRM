using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CRM.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        public Label label;
        private string userName;
        private string email;
        private string text;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(userName)
                && !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(email);
        }

        public string Username
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }
        
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Description
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                label.Text = "Введите Коректный Email !";
                return;
            }

            Item newItem = new Item()
            {
                id = Guid.NewGuid().ToString(),
                username = Username,
                text = Description,
                email = Email
            };

            await DataStore.AddItemAsync(newItem);

            try
            {
                string status;
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create("https://uxcandy.com/~shapoval/test-task-backend/v2/create?developer=TestUser");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    streamWriter.Write($"username={userName}&email={email}&text={text}");
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    status = JsonConvert.DeserializeObject<LoginResponse>(streamReader.ReadToEnd()).status;

                if (status.Contains("ok"))
                {
                    label.Text = "Done !";
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch(Exception ex)
            {
                label.Text = "Check Your Internet Connection !";
            }
        }
    }
}
