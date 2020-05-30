using System;
using System.Collections.Generic;
using System.Text;
using PizzaApp.extensions;

namespace PizzaApp.Model
{
    public class Pizza
    {
        public string nom { get; set; }
        public int prix { get; set; }
        public string[] ingredients { get; set; }
        public string imageUrl { get; set; }

        public string PrixEuros { get { return prix + "€"; } }
        public string IngredientsStr { get { return String.Join(", ", ingredients); } }
        public string Titre{ get { return nom.PremiereLettreMajuscule(); } }
        
        
        public Pizza()
        {
        }

    }
}
