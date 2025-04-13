using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    internal class Position
    {
        /*
         
        Поля
         
        */
        private string _name;
        private float _sellary;
        /*
         
        Свойства
         
        */
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public float Sellary
        {
            get { return _sellary; }
            set { _sellary = value; }
        }
        /*
        Конструктор 
        */
        public Position(string name, float sellary)
        {
            this.Name = name;
            this.Sellary = sellary;
        }
    }
}
