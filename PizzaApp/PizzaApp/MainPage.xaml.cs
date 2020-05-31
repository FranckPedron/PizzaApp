using Newtonsoft.Json;
using PizzaApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PizzaApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        
        
        public MainPage()
        {
            InitializeComponent();

            const string URL = "https://drive.google.com/uc?export=download&id=1iGfsZabkvtiHtr8ZDjcgrPQLF3KSU2Rc";

            listView.IsVisible = false;
            waitLayout.IsVisible = true;
            
            using (var webClient = new WebClient())
            {
                try {
                    webClient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
                      {
                          
                          string pizzasJson = e.Result;
                          List<Pizza> pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

                          Device.BeginInvokeOnMainThread(() =>
                          {
                              listView.ItemsSource = pizzas;
                              listView.IsVisible = true;
                              waitLayout.IsVisible = false;
                          });
                      };

                    webClient.DownloadStringAsync(new Uri(URL));
                    }
           
                catch (Exception e){
                        Device.BeginInvokeOnMainThread(() => {
                            DisplayAlert("Erreur !", "Une erreur s'est produite: " + e.Message, "OK");
                        });
                    return;
                }
            }
        }
    }
}
