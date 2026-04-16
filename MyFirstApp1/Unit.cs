using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstApp1
{
    public class Unit
    {
        private float _health;
        private float _armor;
       

        public float Health => _health;

        public string Name { get; }


        public Unit() : this(name: " Unknown Unit ")
        {
        }
        public Unit(string name)
        {
            Name = name;
        }

        public float RealHealth()
        {
            return Health * (1 + Armour);
        }

        public float Armour

        {
            get { return (float)Math.Round(_armor, 2);}
            set
            {

            }
        }

       
    }
}
