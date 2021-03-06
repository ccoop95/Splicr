﻿using Android.App;
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
        string registerString = "http://imcoop.ca/index.php/api/v1/register";
        string loginString = "http://imcoop.ca/index.php/api/v1/login";
        string getMealsString = "http://imcoop.ca/index.php/api/v1/getmeals";
        string addMealString = "http://imcoop.ca/index.php/api/v1/meal";
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
        List<Meal> mealListData = new List<Meal>();
        JObject testData = new JObject();
        String testString;
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
                Button viewSplices = FindViewById<Button>(Resource.Id.viewSplices);
                newSplice.Click += createNewSplice;

                viewSplices.Click += viewSplicesButton_Click;
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
                viewAddedSplice();
            }
            else
            {
                SetContentView(Resource.Layout.NewSplice);
                Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
                newMeal.Click += newMealButton_Click;
            }
        }

        public void viewAddedSplice()
        {
            SetContentView(Resource.Layout.ViewSplice);
            TextView totalMealCost = FindViewById<TextView>(Resource.Id.totalMealCost);
            TextView totalTaxCost = FindViewById<TextView>(Resource.Id.totalTaxCost);
            TextView totalTipCost = FindViewById<TextView>(Resource.Id.totalTipCost);
            TextView pricePerPerson = FindViewById<TextView>(Resource.Id.costPerPerson);
            totalMealCost.Text = totalCost.ToString();
            totalTaxCost.Text = taxAmount.ToString();
            totalTipCost.Text = tipAmount.ToString();
            pricePerPerson.Text = eachCost.ToString();
            Button returnHome = FindViewById<Button>(Resource.Id.mainScreen);
            returnHome.Click += mainPageButton_Clicked;
        }

        public void viewSplicesButton_Click(object sender, EventArgs e)
        {
            viewAllTheSplices();
        }

        public async void viewAllTheSplices()
        {
            string mealsRetrieved = await getAllTheSplices();
             if (mealsRetrieved.Equals("success"))
             {
                 SetContentView(Resource.Layout.ViewAllSplices);
                 TextView testDataText = FindViewById<TextView>(Resource.Id.testViewData);
                 testDataText.Text = testString;
                Button mainPageButton = FindViewById<Button>(Resource.Id.mainPage);
                mainPageButton.Click += mainPageButton_Clicked;

             }
             else
             {
                 SetContentView(Resource.Layout.NewSplice);
                 Button newMeal = FindViewById<Button>(Resource.Id.spliceBill);
                 newMeal.Click += newMealButton_Click;
             }
        }

        public void mainPageButton_Clicked(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Main);

            Button newSplice = FindViewById<Button>(Resource.Id.newSplice);
            Button viewSplices = FindViewById<Button>(Resource.Id.viewSplices);
            newSplice.Click += createNewSplice;

            viewSplices.Click += viewSplicesButton_Click;
        }

        public async Task<String> getAllTheSplices()
        {
            var settings = new JsonSerializerSettings();
            JObject userIdDetails = new JObject();
            userIdDetails.Add("userid", 1);
            HttpClient getMealsCleint = new HttpClient();
            settings.ContractResolver = new SplicrContractResolver();
            var getMealsJson = JsonConvert.SerializeObject(userIdDetails, Formatting.Indented);
            StringContent jsonContent = new StringContent(getMealsJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await getMealsCleint.PostAsync(getMealsString, jsonContent);
            if (response != null || response.IsSuccessStatusCode)
            {
                SetContentView(Resource.Layout.ViewAllSplices);
                string content = await response.Content.ReadAsStringAsync();
                if (content != null)
                {
                    string trimmedContent = content.Trim(new Char[] { '[', ']' });
                    var count = trimmedContent.Count(x => x == '{');
                    
                   // testData = JObject.Parse(trimmedContent);
                    testString = trimmedContent;
                    //testData = JObject.Parse(content);
                    /*dynamic meals = JsonConvert.DeserializeObject(content);
                    foreach(var meal in meals)
                    {
                        mealListData.Add(meal.id);
                        mealListData.Add(meal.mealcost);
                        mealListData.Add(meal.people);
                        mealListData.Add(meal.taxpercent);
                        mealListData.Add(meal.tippercent);
                        mealListData.Add(meal.totalcost);
                        mealListData.Add(meal.userid);
                    }*/
                    return "success";
                    }
                    else
                    {
                        return "failure";
                    }
            }
            return null;
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

