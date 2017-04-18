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
        string registerString = "http://149.56.141.74/index.php/api/v1/register";
        string loginString = "http://149.56.141.74/index.php/api/v1/login";
        string getMealsString = "http://149.56.141.74/index.php/api/v1/getmeals";
        string addMealString = "http://149.56.141.74/index.php/api/v1/meal";
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
        double eachCost;
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


            newAccount.Click += newAccountButton_Click;
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

        private void newAccountButton_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Register);
            Button registerAccount = FindViewById<Button>(Resource.Id.registerAccount);
            registerAccount.Click += registerAccountButton_Click;
        }
            
        private async void registerAccountButton_Click(object sender, EventArgs e)
        {
 
            
            string registered = await RegisterAsync();
            if (registered.Equals("success"))
            {
                SetContentView(Resource.Layout.Login);

                Button login = FindViewById<Button>(Resource.Id.loginButton);
                login.Click += LoginButton_Click;
            }
            else
            {
                Button registerAccount = FindViewById<Button>(Resource.Id.registerAccount);
                registerAccount.Click += registerAccountButton_Click;
            }
        }

        public void createNewSplice(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.NewSplice);
            Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
            newMeal.Click += newMealButton_Click;

        }
        public async void newMealButton_Click(object sender, EventArgs e)
        {
            EditText mealCost = FindViewById<EditText>(Resource.Id.mealCost);
            EditText people = FindViewById<EditText>(Resource.Id.numPeople);
            EditText tax = FindViewById<EditText>(Resource.Id.Tax);
            EditText tip = FindViewById<EditText>(Resource.Id.Tip);
            mealsCost = double.Parse(mealCost.Text);
            numPeople = int.Parse(people.Text);
            taxPercent = double.Parse(tax.Text);
            tipPercent = double.Parse(tip.Text);
            string mealAdded = await AddMealAsync();
            if (mealAdded.Equals("success"))
            {
                SetContentView(Resource.Layout.ViewAllSplices);
            }
            else
            {
                SetContentView(Resource.Layout.NewSplice);
                Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
                newMeal.Click += newMealButton_Click;
            }
        }

        public async Task<String> RegisterAsync()
        {
            var settings = new JsonSerializerSettings();
            EditText registerUsername = FindViewById<EditText>(Resource.Id.registerUsername);
            EditText registerPassword = FindViewById<EditText>(Resource.Id.registerPassword);
            EditText registerEmail = FindViewById<EditText>(Resource.Id.registerEmail);
            JObject registerDetails = new JObject();
            registerDetails.Add("username", registerUsername.Text);
            registerDetails.Add("password", registerPassword.Text);
            registerDetails.Add("email", registerEmail.Text);
            HttpClient loginClient = new HttpClient();
            settings.ContractResolver = new SplicrContractResolver();
            var loginJson = JsonConvert.SerializeObject(registerDetails, Formatting.Indented);
            StringContent jsonContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await loginClient.PostAsync(registerString, jsonContent);
            if (response != null || response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (!content.Equals("{\"status\":\"failed\"}"))
                {
                    return "success";
                }
                else
                {
                    return "failure";
                }
            }
            return null;
        }

        public async Task<String> AddMealAsync()
        {
            tipAmount = (mealsCost * (tipPercent / 100 + 1));
            taxAmount = (mealsCost * (taxPercent / 100 + 1));
            totalCost = mealsCost + tipAmount + taxAmount;
            eachCost = totalCost / numPeople;
            var settings = new JsonSerializerSettings();
            JObject mealDetails = new JObject();
            mealDetails.Add("userid", userId);
            mealDetails.Add("mealcost", mealsCost);
            mealDetails.Add("people", numPeople);
            mealDetails.Add("tax", taxAmount);
            mealDetails.Add("tip", tipAmount);
            mealDetails.Add("totalcost", totalCost);
            HttpClient loginClient = new HttpClient();
            settings.ContractResolver = new SplicrContractResolver();
            var mealJson = JsonConvert.SerializeObject(mealDetails, Formatting.Indented);
            StringContent jsonContent = new StringContent(mealJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await loginClient.PostAsync(addMealString, jsonContent);
            if (response != null || response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if (content.Equals("{\"status\":\"success\"}"))
                {
                    return "success";
                }
                else
                {
                    return "failure";
                }
            }
            return null;
        }

        public async Task<String> LoginASync()
        {
            var settings = new JsonSerializerSettings();
            EditText loginUsername = FindViewById<EditText>(Resource.Id.loginUsername);
            EditText loginPassword = FindViewById<EditText>(Resource.Id.loginPassword);
            JObject loginDetails = new JObject();
            loginDetails.Add("username", loginUsername.Text);
            loginDetails.Add("password", loginPassword.Text);
            HttpClient loginClient = new HttpClient();
            settings.ContractResolver = new SplicrContractResolver();
            var loginJson = JsonConvert.SerializeObject(loginDetails, Formatting.Indented);
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

