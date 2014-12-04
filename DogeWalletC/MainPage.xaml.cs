﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DogeWalletC.Resources;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;


namespace DogeWalletC
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static string FIRST_RUN_FLAG = "FIRST_RUN_FLAG";
        private static IsolatedStorageSettings firstRun = IsolatedStorageSettings.ApplicationSettings;
        private string dogeresult;
        private string bitresult;
        private string literesult;



        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set up first time start up to set api
            
            /*if (IsFirstRun())
            {
                NavigationService.Navigate(new Uri("/settings.xaml", UriKind.RelativeOrAbsolute));
            }
            
            else
            {*/
            if (Read_API("btc") == "NoAPI" || Read_API("doge") == "NoAPI" || Read_API("lite") == "NoAPI")
            {
                //DisplayMessage();
            }
            else
                Refresh();

            //Refresh();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();


        }

        private void Set_API()
        {
            string navTo = "/settings.xaml";
            NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
        }

        private bool IsFirstRun()
        {
            if (!firstRun.Contains(FIRST_RUN_FLAG))
            {
                //string navTo = "/settings.xaml";
                //NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));

                firstRun.Add(FIRST_RUN_FLAG, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void Refresh()
        {


            HttpClient client = new HttpClient();

            //https://block.io/api/v2/get_balance/?api_key=

            // set apikeys to local vars
            var dogeapiKey = Read_API("doge");
            var bitapiKey = Read_API("btc");
            var liteapiKey = Read_API("ltc");

            // setting urls
            string dogeurl = "https://block.io/api/v2/get_balance/" +
                "?api_key=" + dogeapiKey;
            string biturl = "https://block.io/api/v2/get_balance/" +
                "?api_key=" + bitapiKey;
            string liteurl = "https://block.io/api/v2/get_balance/" +
                "?api_key=" + liteapiKey;

            // try catch for each url

            //doge
            try
            {
                dogeresult = await client.GetStringAsync(dogeurl);
                setBalance("doge");
            }
            catch
            {
                DogecoinBalance.Text = "Set API Key";
            }
            try
            {
                bitresult = await client.GetStringAsync(biturl);
                setBalance("bit");
            }
            catch
            {
                BitcoinBalance.Text = "Set API Key";
            }
            try
            {
                literesult = await client.GetStringAsync(liteurl);
                setBalance("lite");
            }
            catch
            {
                LitecoinBalance.Text = "Set API Key";
            }

            
            
            
            
        }

        private void setBalance(string net)
        {
            // SET AVAILABLE and UNCONFIRMED

            if (net == "doge")
            {
                BlockDataResult DogeapiData = JsonConvert.DeserializeObject<BlockDataResult>(dogeresult);

                double doubleBal = Double.Parse(DogeapiData.data.available_balance);
                string bal = String.Concat(doubleBal);
                double unconBal = Double.Parse(DogeapiData.data.pending_received_balance);

                DogecoinBalance.Text = bal.Substring(0, bal.Length - 9) + " Ð";
                DogeUnconfirmedBalance.Text = unconBal + " Ð";
            } 
            else if (net == "bit")
            {
                BlockDataResult BitapiData = JsonConvert.DeserializeObject<BlockDataResult>(bitresult);

                double doubleBal = Double.Parse(BitapiData.data.available_balance);
                string bal = String.Concat(doubleBal);
                double unconBal = Double.Parse(BitapiData.data.pending_received_balance);

                BitcoinBalance.Text = bal + " ฿";
                BitUnconfirmedBalance.Text = unconBal + " ฿";
            }
            else if (net == "lite")
            {
                BlockDataResult LiteapiData = JsonConvert.DeserializeObject<BlockDataResult>(literesult);

                double doubleBal = Double.Parse(LiteapiData.data.available_balance);
                string bal = String.Concat(doubleBal);
                double unconBal = Double.Parse(LiteapiData.data.pending_received_balance);

                LitecoinBalance.Text = bal.Substring(0, bal.Length - 5) + " Ł";
                LiteUnconfirmedBalance.Text = unconBal + " Ł";
            }
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButtonSend = new ApplicationBarIconButton(
                new Uri("/AppBarIcons/upload.png", UriKind.Relative));
            appBarButtonSend.Text = "Send";
            appBarButtonSend.Click += SendClick;
            ApplicationBar.Buttons.Add(appBarButtonSend);

            ApplicationBarIconButton appBarButtonReceive = new ApplicationBarIconButton(
                new Uri("/AppBarIcons/download.png", UriKind.Relative));
            appBarButtonReceive.Text = "Receive";
            appBarButtonReceive.Click += ReceiveClick;
            ApplicationBar.Buttons.Add(appBarButtonReceive);

            ApplicationBarIconButton appBarButtonRefresh = new ApplicationBarIconButton(
                new Uri("/AppBarIcons/refresh.png", UriKind.Relative));
            appBarButtonRefresh.Text = "Refresh";
            appBarButtonRefresh.Click += RefreshClick;
            ApplicationBar.Buttons.Add(appBarButtonRefresh);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItemSettings = new ApplicationBarMenuItem("Settings");
            appBarMenuItemSettings.Click += MenuSettings;
            ApplicationBar.MenuItems.Add(appBarMenuItemSettings);

            ApplicationBarMenuItem appBarMenuItemCredits = new ApplicationBarMenuItem("Credits");
            appBarMenuItemCredits.Click += MenuCredits;
            ApplicationBar.MenuItems.Add(appBarMenuItemCredits);

            //Shapeshift.io page
            ApplicationBarMenuItem appBarMenuItemShape = new ApplicationBarMenuItem("Shift Coins");
            appBarMenuItemShape.Click += MenuShape;
            ApplicationBar.MenuItems.Add(appBarMenuItemShape);
        }

        private void MenuShape(object sender, EventArgs e)
        {
            string navTo = "/shift.xaml";
            NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
        }

        // Click event for Settings option in app Menu
        private void MenuSettings(object sender, EventArgs e)
        {
            string navTo = "/settings.xaml";
            NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
        }

        // Click event for Credits option in app Menu
        private void MenuCredits(object sender, EventArgs e)
        {
            string navTo = "/credits.xaml";
            NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
        }
        // Click event for Receive
        private void ReceiveClick(object sender, EventArgs e)
        {
            string navTo = "/receive.xaml";
            NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
        }
        // Click event for Refresh
        private void RefreshClick(object sender, EventArgs e)
        {
            if (Read_API("doge") == "NoAPI" && Read_API("btc") == "NoAPI" && Read_API("ltc") == "NoAPI")
            {
                DisplayMessage();
            }else{
                Refresh();
            }
            
            
        }

        private void DisplayMessage()
        {
            MessageBoxResult messageBox = MessageBox.Show("Error!", "Please set all 3 API Keys in Settings", MessageBoxButton.OK);
        }
        // Click event for Send
        private void SendClick(object sender, EventArgs e)
        {
            string navTo = "/send.xaml";
            NavigationService.Navigate(new Uri(navTo, UriKind.RelativeOrAbsolute));
        }
        private string Read_API(string net)
        {
            if (net == "doge")
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("DogecoinAPIKey"))
                {
                    var apiKey = IsolatedStorageSettings.ApplicationSettings["DogecoinAPIKey"].ToString();
                    return apiKey;
                }
            }
            else if (net == "btc")
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("BitcoinAPIKey"))
                {
                    var apiKey = IsolatedStorageSettings.ApplicationSettings["BitcoinAPIKey"].ToString();
                    return apiKey;
                }
            }
            else if (net == "ltc")
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("LitecoinAPIKey"))
                {
                    var apiKey = IsolatedStorageSettings.ApplicationSettings["LitecoinAPIKey"].ToString();
                    return apiKey;
                }
            }
            return "NoAPI";

        }

        /*private void Dogecoin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
        private void Bitcoin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
        private void Litecoin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }*/

    }


    public class Data
    {
        public string network { get; set; }
        public string available_balance { get; set; }
        public string pending_received_balance { get; set; }
        public string error_message { get; set; }
    }

    public class BlockDataResult
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
}
