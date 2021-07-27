using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Models
{
    public class Message
    {
        public List<Item> tasks;
        public string total_task_count;
    }

    public class Items
    {
        public string status;
        public Message message;
    }
}
