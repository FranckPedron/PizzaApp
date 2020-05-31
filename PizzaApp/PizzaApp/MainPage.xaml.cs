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

            listView.RefreshCommand = new Command((obj) =>
            {
                  DownloadData((pizzas) =>
                  {
                      listView.ItemsSource = pizzas;
                      listView.IsRefreshing = false;
                  });

            });

            listView.IsVisible = false;
            waitLayout.IsVisible = true;

            DownloadData((pizzas) =>
            {
                      listView.ItemsSource = pizzas;

                      listView.IsVisible = true;
                      waitLayout.IsVisible = false;

            });
              
        }
        public void DownloadData(Action<List<Pizza>> action)
        {
            const string URL = "https://drive.google.com/uc?export=download&id=1iGfsZabkvtiHtr8ZDjcgrPQLF3KSU2Rc";
            using (var webClient = new WebClient())
            {
                webClient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
                {
                    try
                    {
                        string pizzasJson = e.Result;
                        List<Pizza> pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            action.Invoke(pizzas);

                        });
                    }


                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Erreur !", "Une erreur s'est produite: " + ex.Message, "OK");
                            action.Invoke(null);
                        });
                    }
                };
                    webClient.DownloadStringAsync(new Uri(URL));
            }

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}

