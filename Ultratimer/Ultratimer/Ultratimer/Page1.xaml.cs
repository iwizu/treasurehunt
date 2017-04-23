using Java.Lang;
using NdefLibrary.Ndef;
using Plugin.Settings;
using Poz1.NFCForms.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultratimer.Data;
using Xamarin.Forms;

namespace Ultratimer
{
    public partial class Page1 : ContentPage
    {
        private readonly INfcForms device;
        bool first = true;
        public static GameManager gameManager { get; set; }
       
        public Page1()
        {
            InitializeComponent();
            device = DependencyService.Get<INfcForms>();
            device.NewTag += HandleNewTag;
            device.TagConnected += device_TagConnected;
            device.TagDisconnected += device_TagDisconnected;

            string selName = CrossSettings.Current.GetValueOrDefault("SelectedName", "");
            if (selName == "")
            {
                CrossSettings.Current.AddOrUpdateValue("SelectedName", "");
            }
            else
            {
                Name.Text = CrossSettings.Current.GetValueOrDefault("SelectedName", "");
            }

            RefreshGames();
            int sentOption = CrossSettings.Current.GetValueOrDefault("SelectedGame", -1);
            if (sentOption == -1)
            {
                CrossSettings.Current.AddOrUpdateValue("SelectedGame", -1);
            }
            else
            {
                RefreshClues();
            }

        }
        async void RefreshGames()
        {
            try
            {
                SelectGame.Items.Clear();                
                var lst = await App.gameManager.GetGamesAsync();
                if (lst != null)
                {
                    Global.Games = lst;
                    SelectGame.Items.Add("---Select Game---");
                    foreach (var i in lst)
                    {
                        SelectGame.Items.Add(i.Name);
                    }
                    int selGame = CrossSettings.Current.GetValueOrDefault("SelectedGame", -1);
                    if (selGame != -1)
                    {
                        var gm = Global.Games[selGame-1];
                        if (gm != null)
                        {
                            Global.AGame = gm;
                            SelectGame.SelectedIndex = selGame;

                        }
                    }
                }
                if(SelectGame.Items.Count<2)
                {
                    await DisplayAlert("No Internet", "It seems there is no internet connection. Click OK to try again!", "OK");
                    RefreshGames();
                }
            }
            catch(System.Exception ex)
            {
            }
        }
        async void OnGameChanged(object sender, EventArgs e)
        {
            if (SelectGame.Items[SelectGame.SelectedIndex]!= "---Select Game---")
            {
                if(first)
                {
                    first = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(Name.Text))
                    {
                        await DisplayAlert("Please fill in your Name", "You cannot select a game before you complete your name", "OK");
                        SelectGame.SelectedIndex = 0;
                    }
                    else
                    {
                        var answer = await DisplayAlert("Reset Games", "If you change your game the App will reset all your settings. Are you sure?", "Yes", "No");
                        if (answer)
                        {
                            CrossSettings.Current.AddOrUpdateValue("SelectedGame", SelectGame.SelectedIndex);
                            CrossSettings.Current.AddOrUpdateValue("SelectedName", Name.Text);
                            Global.AGame = Global.Games[SelectGame.SelectedIndex - 1];
                            App.Database.DeleteAllClues();
                            var cl = await App.gameManager.GetFirstClueAndStoreNameAsync(Global.AGame.Id, Name.Text);
                            if (string.IsNullOrEmpty(cl.Titlos)==false&& App.Database.GetClues().Any(i => i.IsFirst) == false)
                            {
                                Clues c = new Clues();
                                c.HFCode = "";
                                c.IsFinal = cl.IsFinal;
                                c.IsFirst = true;
                                c.Keimeno = cl.Keimeno;
                                c.Picture = cl.Picture;
                                c.Titlos = cl.Titlos;
                                App.Database.SaveClue(c);
                            }
                            RefreshClues();

                        }
                    }
                }
            }
        }
        public void RefreshClues()
        {
            SelectClue.Items.Clear();
            SelectClue.Items.Add("-----Select a Clue------");
            SelectClue.SelectedIndex = 0;
            var cl=App.Database.GetClues();
            foreach(var i in cl)
            {
                SelectClue.Items.Add(i.Titlos);
            }
        }
        async void OnClueChanged(object sender, EventArgs e)
        {
            var cl=App.Database.GetClues();
            if(SelectClue.SelectedIndex!=-1&&SelectClue.SelectedIndex< cl.Count())
            {
                CTitle.Text = "";
                CDetails.Text = "";
                CPicture.Source = "";
                Clues c = cl.ElementAt(SelectClue.SelectedIndex);
                CTitle.Text = c.Titlos;
                CDetails.Text = c.Keimeno;
                CPicture.Source = c.Picture;
            }
        }
        void device_TagDisconnected(object sender, NfcFormsTag e)
        {
#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                IsConnected.IsToggled = false;
            });
#else
#endif

        }
        void device_TagConnected(object sender, NfcFormsTag e)
        {

#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                IsConnected.IsToggled = true;
            });
#else
#endif

        }
        void HandleClicked(object sender, EventArgs e)
        {
            var spRecord = new NdefSpRecord
            {
                Uri = "www.poz1.com",
                NfcAction = NdefSpActRecord.NfcActionType.DoAction,
            };
            spRecord.AddTitle(new NdefTextRecord
            {
                Text = "NFCForms - XamarinForms - Poz1.com",
                LanguageCode = "en"
            });
            // Add record to NDEF message
            var msg = new NdefMessage { spRecord };
            try
            {
                device.WriteTag(msg);
            }
            catch (System.Exception excp)
            {
                DisplayAlert("Error", excp.Message, "OK");
            }
        }
        private ObservableCollection<string> readNDEFMEssage(NdefMessage message)
        {

            ObservableCollection<string> collection = new ObservableCollection<string>();
            foreach (NdefRecord record in message)
            {
                // Go through each record, check if it's a Smart Poster
                if (record.CheckSpecializedType(false) == typeof(NdefSpRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefSpRecord(record);
                    collection.Add("URI: " + spRecord.Uri);
                    collection.Add("Titles: " + spRecord.TitleCount());
                    collection.Add("1. Title: " + spRecord.Titles[0].Text);
                    collection.Add("Action set: " + spRecord.ActionInUse());
                }

                if (record.CheckSpecializedType(false) == typeof(NdefUriRecord))
                {
                    // Convert and extract Smart Poster info
                    var spRecord = new NdefUriRecord(record);
                    collection.Add("Text: " + spRecord.Uri);
                }
            }
            return collection;
        }
        async void DisplayTag(string tg)
        {
            var cl=await App.gameManager.GetClueFromNFCAsync(Name.Text, tg);
            if(cl!=null)
            {
                await DisplayAlert("Yeah you found a Clue", "Well done! You have just found a clue, please wait to download the clue", "OK");
                if(App.Database.GetClue(cl.Id)==null)
                App.Database.SaveClue(cl);                
                RefreshClues();
                SelectClue.SelectedIndex = SelectClue.Items.Count - 1;
                CTitle.Text = cl.Titlos;
                CDetails.Text = cl.Keimeno;
                CPicture.Source = cl.Picture;

                if(cl.IsFinal)
                {
                    await DisplayAlert("You found the treasure!", "Finally! The myth comes to an end, you are the owner of the ancient treasure!", "Thank you");
                }
            }
        
        }
        private string getDec(byte[] bytes)
        {
            long result = 0;
            long factor = 1;
            for (int i = 0; i < bytes.Length; ++i)
            {
                long value = bytes[i] & 0xffL;
                result += value * factor;
                factor *= 256L;
            }
            return result.ToString();
        }
        void HandleNewTag(object sender, NfcFormsTag e)
        {

#if SILVERLIGHT
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                welcomePanel.IsVisible = false;

                IsWriteable.IsToggled = e.IsWriteable;
                IsNDEFSupported.IsToggled = e.IsNdefSupported;

                if (TechList != null)
                    TechList.ItemsSource = e.TechList;

                if (e.IsNdefSupported)
                    NDEFMessage.ItemsSource = readNDEFMEssage(e.NdefMessage);
            });
#else
            byte[] btt = { e.Id[0], e.Id[1], e.Id[2], e.Id[3] };
            string tg = getDec(btt);
            DisplayTag(tg);
#endif
        }
        private string bytesToHexString(byte[] src)
        {
            Java.Lang.StringBuilder stringBuilder = new Java.Lang.StringBuilder("0x");
            if (src == null || src.Length <= 0)
            {
                return null;
            }

            char[] buffer = new char[2];
            for (int i = 0; i < src.Length; i++)
            {
                buffer[0] = Character.ForDigit((src[i] >> 4) & 0x0F, 16);
                buffer[1] = Character.ForDigit(src[i] & 0x0F, 16);

                stringBuilder.Append(buffer);
            }

            return stringBuilder.ToString();
        }      
    }
}
