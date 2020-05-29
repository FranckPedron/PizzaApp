using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApp.Model
{
    public class Pizza
    {
        public string nom { get; set; }
        public int prix { get; set; }
        public string[] ingredients { get; set; }

        public Pizza()
        {
        }

    }
}
