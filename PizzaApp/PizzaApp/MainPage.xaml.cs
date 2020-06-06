using Newtonsoft.Json;
using PizzaApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
       
        List<string> pizzasFav=new List<string>();

        enum e_tri
        {
            TRI_AUCUN, TRI_NOM, TRI_PRIX, TRI_FAV
        };

        e_tri tri = e_tri.TRI_AUCUN;

        const string KEY_TRI = "tri";
        const string KEY_FAV = "fav";

        string jsonFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"pizzas.json");
        string tempFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp");


        public MainPage()
        {
            InitializeComponent();


            LoadFavList();

            if (Application.Current.Properties.ContainsKey(KEY_TRI))
            {
                tri = (e_tri)Application.Current.Properties[KEY_TRI];
                sortButton.Source = GetImageSourceFromTri(tri);
            }

            listView.RefreshCommand = new Command((obj) =>
            {
                DownloadData((pizzas) =>
                {
                    if (pizzas != null)
                    {
                        listView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas),pizzasFav);
                    }
                    listView.IsRefreshing = false;
                });

            });

            listView.IsVisible = false;
            waitLayout.IsVisible = true;
            if (File.Exists(jsonFileName))
            {
                string pizzaJson=File.ReadAllText(jsonFileName);
                if (!string.IsNullOrEmpty(pizzaJson))
                {
                    pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzaJson);
                    listView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas), pizzasFav);
                    listView.IsVisible = true;
                    waitLayout.IsVisible = false;
                }
            }

            DownloadData((pizzas) =>
            {
              if (pizzas != null) { 
                listView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas), pizzasFav);
                }
                listView.IsVisible = true;
                waitLayout.IsVisible = false;

            });

        }
        public void DownloadData(Action<List<Pizza>> action)
        {
            const string URL = "https://drive.google.com/uc?export=download&id=1iGfsZabkvtiHtr8ZDjcgrPQLF3KSU2Rc";
            using (var webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                {

                    Exception ex = e.Error;
                    if (ex == null)
                    {
                        File.Copy(tempFileName, jsonFileName, true);
                        string pizzasJson=File.ReadAllText(jsonFileName);
                        pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            action.Invoke(pizzas);

                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async() =>
                        {
                            await DisplayAlert("Erreur !", "Une erreur s'est produite: " + ex.Message, "OK");
                            action.Invoke(null);
                        });
                    }
                };
                webClient.DownloadFileAsync(new Uri(URL),tempFileName);
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
                tri = e_tri.TRI_FAV;
            }
            else if (tri == e_tri.TRI_FAV)
            {
                tri = e_tri.TRI_AUCUN;
            }

            sortButton.Source= GetImageSourceFromTri(tri);
            listView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas), pizzasFav); ;

            Application.Current.Properties[KEY_TRI] = (int)tri;
            Application.Current.SavePropertiesAsync();
        }
       
        private string GetImageSourceFromTri(e_tri t)
        {
            switch (t) {
                case e_tri.TRI_NOM: 
                    return "sort_nom.png";

                case e_tri.TRI_PRIX:
                    return "sort_prix.png";

                case e_tri.TRI_FAV:
                    return "sort_fav.png";
            }
            return "sort_none.png";
        }

        private List<Pizza> GetPizzasFromTri(e_tri t, List<Pizza> l)
        {
            switch (t)
            {
                case e_tri.TRI_NOM:
                case e_tri.TRI_FAV:
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

        private void OnFavPizzaChanged(PizzaCell pizzaCell)
        {
            bool isInFavList = pizzasFav.Contains(pizzaCell.pizza.nom);
           
            if (pizzaCell.isFavorite && !isInFavList)
            {
                pizzasFav.Add(pizzaCell.pizza.nom);
                SaveFavList();
            }
            else if(!pizzaCell.isFavorite && isInFavList){
                pizzasFav.Remove(pizzaCell.pizza.nom);
                SaveFavList();
            }
        }

        private List<PizzaCell>GetPizzaCells(List<Pizza> p,List<string> f)
        {
            List<PizzaCell> ret = new List<PizzaCell>();
            
            if(p == null)
            {
                return ret;
            }

            foreach(Pizza pizza in p)
            {
                            
                bool isFav = f.Contains(pizza.nom);

                if (tri == e_tri.TRI_FAV)
                {
                    if (isFav)
                    {
                        ret.Add(new PizzaCell { pizza = pizza, isFavorite = isFav, favChangedAction = OnFavPizzaChanged });
                    }
                }
                else
                {
                    ret.Add(new PizzaCell { pizza = pizza, isFavorite = isFav, favChangedAction = OnFavPizzaChanged });
                }
            }
            return ret;
        }

        private void SaveFavList() {
            string json = JsonConvert.SerializeObject(pizzasFav);
            Application.Current.Properties[KEY_FAV] = json;
            Application.Current.SavePropertiesAsync();
        }

        private void LoadFavList()
        {
            if (Application.Current.Properties.ContainsKey(KEY_FAV))
            {
                string json = Application.Current.Properties[KEY_FAV].ToString();
                pizzasFav = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }
    }
}
    


