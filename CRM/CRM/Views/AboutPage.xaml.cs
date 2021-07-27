using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            Enter.Clicked += EnterClick;

            try
            {
                if ((string)Application.Current.Properties["token"] == "Невалидный токен")
                {
                    Profile.Text = "";
                    Profile.IsEnabled = false;
                }
            }
            catch (Exception e)
            {
                Profile.Text = "";
                Profile.IsEnabled = false;
            }
        }

        void EnterClick(object sender, System.EventArgs e)
        {
            if (Login.Text == null || Login.Text == "" || Password.Text == null || Password.Text == "")
            {
                LoginNotification.Text = "Введите Все Поля !";
                return;
            }

            try
            {
                string token;
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create("https://uxcandy.com/~shapoval/test-task-backend/v2/login?developer=TestUser");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    streamWriter.Write($"username={Login.Text}&password={Password.Text}");
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    token = JsonConvert.DeserializeObject<LoginResponse>(streamReader.ReadToEnd()).message.token;
                Application.Current.Properties["token"] = token;
                if (token != null)
                {
                    Profile.Text = "Exit";
                    Profile.IsEnabled = true;
                    LoginNotification.Text = "Ok !";
                }
                else
                    LoginNotification.Text = "Не Верные Данные !";
            }
            catch(Exception ex)
            {
                LoginNotification.Text = "Check Out Your Internet !";
            }
        }

        void Exit(object sender, System.EventArgs e)
        {
            Profile.Text = "";
            Profile.IsEnabled = false;
            LoginNotification.Text = "";
            Application.Current.Properties["token"] = "Невалидный токен";
        }
    }
}