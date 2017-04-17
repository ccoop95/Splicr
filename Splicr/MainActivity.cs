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
        string username;
        int userId;
        int numPeople;
        int mealId;
        double mealsCost;
        double tipPercent;
        double tipAmount;
        double taxPercent;
        double taxAmount;
        double totalCost;
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Login);
            Button login = FindViewById<Button>(Resource.Id.loginButton);
            Button newAccount = FindViewById<Button>(Resource.Id.newAccount);
           /* Button register = FindViewById<Button>(Resource.Id.register);
            Button newSplice = FindViewById<Button>(Resource.Id.newSplice);
            Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
            Button viewSplices = FindViewById<Button>(Resource.Id.viewSplices);
            EditText mealCost = FindViewById<EditText>(Resource.Id.mealCost);
            EditText people = FindViewById<EditText>(Resource.Id.numPeople);
            EditText tax = FindViewById<EditText>(Resource.Id.Tax);
            EditText tip = FindViewById<EditText>(Resource.Id.Tip);*/
            


            login.Click += LoginButton_Click;

        }
        /*private async Task<JsonValue> AddMeal(string url)
        {

        }*/
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string loggedIn = await LoginASync();
            if (loggedIn.Equals("success"))
            {
                SetContentView(Resource.Layout.Main);

                Button newSplice = FindViewById<Button>(Resource.Id.newSplice);
                newSplice.Click += createNewSplice;
            }
            else
            {
                SetContentView(Resource.Layout.Login);
                Button login = FindViewById<Button>(Resource.Id.loginButton);
                Button newAccount = FindViewById<Button>(Resource.Id.newAccount);
                login.Click += LoginButton_Click;
            }
        }
        public void createNewSplice(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.NewSplice);
            EditText mealCost = FindViewById<EditText>(Resource.Id.mealCost);
            EditText people = FindViewById<EditText>(Resource.Id.numPeople);
            EditText tax = FindViewById<EditText>(Resource.Id.Tax);
            EditText tip = FindViewById<EditText>(Resource.Id.Tip);
            Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
            mealsCost = double.Parse(mealCost.Text);
            numPeople = int.Parse(people.Text);
            taxPercent = double.Parse(tax.Text);
            tipPercent = double.Parse(tip.Text);

        }
        public async Task<String> LoginASync()
        {
            EditText loginUsername = FindViewById<EditText>(Resource.Id.loginUsername);
            EditText loginPassword = FindViewById<EditText>(Resource.Id.loginPassword);
            JObject loginDetails = new JObject();
            loginDetails.Add("username", loginUsername.Text);
            loginDetails.Add("password", loginPassword.Text);
            JObject successObject = new JObject();
            successObject.Add("status", "success");
            HttpClient loginClient = new HttpClient();
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SplicrContractResolver();
            var loginJson = JsonConvert.SerializeObject(loginDetails, Formatting.Indented);
            var successJson = JsonConvert.SerializeObject(successObject, Formatting.Indented);
            StringContent jsonContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await loginClient.PostAsync(loginString, jsonContent);
            if (response != null || response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if(!content.Equals("{\"status\":\"failed\"}"))
                {
                    username = loginUsername.Text;
                    JObject userIdObject = JObject.Parse(content);
                    userId = (int)userIdObject["id"];

                    return "success";
                }
                else
                {
                    return "failure";
                }
            }
            return null;
        }
        public class SplicrContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }
    }
}

