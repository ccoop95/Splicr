using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Android.Net;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Splicr
{
    [Activity(Label = "Splicr", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string loginString = "http://149.56.141.74/index.php/api/v1/login";
        string getMealsString = "http://149.56.141.74/index.php/api/v1/getmeals";
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            Button login = FindViewById<Button>(Resource.Id.loginButton);
            Button newAccount = FindViewById<Button>(Resource.Id.newAccount);
            Button register = FindViewById<Button>(Resource.Id.register);
            Button newSplice = FindViewById<Button>(Resource.Id.newSplice);
            Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
            Button viewSplices = FindViewById<Button>(Resource.Id.viewSplices);
            EditText mealCost = FindViewById<EditText>(Resource.Id.mealCost);
            EditText people = FindViewById<EditText>(Resource.Id.numPeople);
            EditText tax = FindViewById<EditText>(Resource.Id.Tax);
            EditText tip = FindViewById<EditText>(Resource.Id.Tip);


            newSplice.Click += delegate {
                SetContentView(Resource.Layout.NewSplice);
            };
            /*newMeal.Click += delegate {
                string url = "ADD URL HERE" +
                 mealCost.Text +
                 "&lng=" +
                 longitude.Text +
                 "&username=demo";
                JsonValue json = await AddMeal(url);

            };*/
        }
        /*private async Task<JsonValue> AddMeal(string url)
        {

        }*/
    }
}

