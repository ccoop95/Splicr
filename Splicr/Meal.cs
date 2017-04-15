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
        public int Id { get; set; }
        public double? MealCost { get; set; }
        public int People { get; set; }
        public double? TaxPercent { get; set; }
        public double? TipPercent { get; set; }
        public double? TotalCost { get; set; }
        public int UserId { get; set; }
    }
}