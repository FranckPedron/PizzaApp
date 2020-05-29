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
            pizzas.Add(new Pizza { nom = "Végétarienne", prix = 7, ingredients = new string[] { "Tomate", "poivrons", "oignons" } });
            pizzas.Add(new Pizza { nom = "Montagnarde", prix = 11, ingredients = new string[] { "Reblochon", "pommes de terre", "oignons","crème" } });
            pizzas.Add(new Pizza { nom = "Carnivore", prix = 14, ingredients = new string[] { "Tomate", "viande hachée", "Mozzarella" } });

        }
    }
}
