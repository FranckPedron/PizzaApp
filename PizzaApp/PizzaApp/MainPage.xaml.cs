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
        List<Pizza> pizzas;
        
        public MainPage()
        {
            InitializeComponent();

            const string URL = "https://drive.google.com/uc?export=download&id=1iGfsZabkvtiHtr8ZDjcgrPQLF3KSU2Rc";
            string pizzasJson = "";
            
            using (var webClient = new WebClient())
            {
                try { 
                
                    pizzasJson = webClient.DownloadString(URL);
                    }
           
            catch (Exception e){
                    Device.BeginInvokeOnMainThread(() => {
                        DisplayAlert("Erreur !", "Une erreur s'est produite: " + e.Message, "OK");
                    });
                    return;
                }
            }
            pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);
            

            listView.ItemsSource = pizzas;
            {
               
            }
        }
    }
}
