using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Splicr
{
    class Meal
    {
        public int id { get; set; }
        public string mealCost { get; set; }
        public int people { get; set; }
        public string taxPercent { get; set; }
        public string tipPercent { get; set; }
        public string totalCost { get; set; }
        public int userId { get; set; }
    }
}