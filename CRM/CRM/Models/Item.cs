using System;
using System.Drawing;

namespace CRM.Models
{
    public class Item
    {
        public string id { get; set; }
        public string username { get; set; }
        public string text { get; set; }
        public string status { get; set; }
        public string email { get; set; }
        public string image_path { get; set; }
        public Color color { get { if (status == "10" || status == "11") return Color.Gray; else return Color.White; } set { } }
    }
}