using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultratimer.Data;
using Xamarin.Forms;

namespace Ultratimer
{
    public partial class App : Application
    {
        static GameDatabase database;
        public static GameManager gameManager { get; set; }
        public static GameDatabase Database
        {
            get
            {
                database = database ?? new GameDatabase();
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new Ultratimer.Page1();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
