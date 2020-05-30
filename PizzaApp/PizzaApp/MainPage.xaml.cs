using PizzaApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

            pizzas = new List<Pizza>();
            pizzas.Add(new Pizza { nom = "végétarienne", prix = 7, ingredients = new string[] { "Tomate", "poivrons", "oignons" },imageUrl= "https://www.maxi-mag.fr/sites/default/files/media/recipe/2016-01/pizza-vegetarienne.jpg" });
            pizzas.Add(new Pizza { nom = "MONTAGNARDE", prix = 11, ingredients = new string[] { "Reblochon", "pommes de terre", "oignons","crème" },imageUrl= "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSPOb_6xyauj5NA5AqNGHBvbMIinpYnn4twDSf-5b2mRK7SNKyF&usqp=CAU" });
            pizzas.Add(new Pizza { nom = "Carnivore", prix = 14, ingredients = new string[] { "Tomate", "viande hachée", "Mozzarella" },imageUrl= "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQiPP3u8N97UiPgNdZzPoJy5mOXh00vTaiu9vJr_x4Dg9pxT8mG&usqp=CAU" });


            listView.ItemsSource = pizzas;
            {
               
            }
        }
    }
}
