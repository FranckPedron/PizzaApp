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

        enum e_tri
        {
            TRI_AUCUN, TRI_NOM, TRI_PRIX
        };

        e_tri tri = e_tri.TRI_AUCUN;

        public MainPage()
        {
            InitializeComponent();

            listView.RefreshCommand = new Command((obj) =>
            {
                DownloadData((pizzas) =>
                {
                    listView.ItemsSource = GetPizzasFromTri(tri,pizzas);
                    listView.IsRefreshing = false;
                });

            });

            listView.IsVisible = false;
            waitLayout.IsVisible = true;

            DownloadData((pizzas) =>
            {
                listView.ItemsSource = GetPizzasFromTri(tri, pizzas);

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
                        pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

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

        private void SortButton_Clicked(object sender, EventArgs e)
        {
            if (tri == e_tri.TRI_AUCUN)
            {
                tri = e_tri.TRI_NOM;
            }
            else if (tri == e_tri.TRI_NOM)
            {
                tri = e_tri.TRI_PRIX;
            }
            else if (tri == e_tri.TRI_PRIX)
            {
                tri = e_tri.TRI_AUCUN;
            }

            sortButton.Source= GetImageSourceFromTri(tri);
            listView.ItemsSource = GetPizzasFromTri(tri, pizzas);
        }
       
        private string GetImageSourceFromTri(e_tri t)
        {
            switch (t) {
                case e_tri.TRI_NOM: 
                    return "sort_nom.png";

                case e_tri.TRI_PRIX:
                    return "sort_prix.png";
            }
            return "sort_none.png";
        }

        private List<Pizza> GetPizzasFromTri(e_tri t, List<Pizza> l)
        {
            switch (t)
            {
                case e_tri.TRI_NOM:
                    {
                        List<Pizza> ret = new List<Pizza>(l);
                        ret.Sort((p1,p2)=> { return p1.Titre.CompareTo(p2.Titre); });
                        return ret;
                    }
                   

                case e_tri.TRI_PRIX:
                    {
                        List<Pizza> ret = new List<Pizza>(l);
                        ret.Sort((p1, p2) => { return p2.prix.CompareTo(p1.prix); });
                        return ret;
                    }
            }
            return l;

        }
    }
}
    


