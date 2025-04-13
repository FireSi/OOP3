using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    internal class Work
    {
        /*
        
        Поля 
        
        */
        private string _company;
        public List<Position> _positions = new List<Position>();
        /*
         
        Свойства
        
        */
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }
        /*
        Конструктор без должностей 
        */
        public Work(string company)
        {
            Position Workless = new Position("Безработный", 0);
            this._positions.Add(Workless);
            this.Company = company;
        }
    }
}
