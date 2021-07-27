using CRM.Models;
using CRM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        public static ItemsViewModel itemsViewModel;
        public static string name = "TestUser";
        public static int page = 1;
        public static string sort_field = "username";
        public static int pages = 1;
        public static string range = "asc";
        const string URL = "https://uxcandy.com/~shapoval/test-task-backend/v2/";
        List<Item> items;

        public MockDataStore()
        {

        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.id == item.id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            Message message = new Message();
            HttpClient client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("?developer={0}&page={1}&sort_field={2}&sort_direction={3}", name, page.ToString(), sort_field, range)).Result;
                if (response.IsSuccessStatusCode)
                {
                    message = JsonConvert.DeserializeObject<Items>(response.Content.ReadAsStringAsync().Result).message;
                    items = message.tasks;
                    int count = Int32.Parse(message.total_task_count);
                    pages = count % 3 == 0 ? count / 3 : count / 3 + 1;
                    itemsViewModel.Title = $"Task List {pages} Pages";
                }
            }
            catch(Exception ex)
            {
                if (items != null)
                    items.Clear();
                itemsViewModel.Title = "Check Out Your Internet !";
            }
            finally
            {
                client.Dispose();
            }
            return await Task.FromResult(items);
        }
    }
}