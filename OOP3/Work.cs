using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OOP3
{
    public class Work
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
            set
            {
                _company = Regex.Replace(value ?? string.Empty, @"[^a-zA-Zа-яА-ЯёЁ]", "");
                _company = _company.Trim();
            }
        }
        public List<Position> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }
        /*
        Конструктор без должностей 
        */
        public Work(string company)
        {
            if (string.IsNullOrWhiteSpace(company))
            {
                throw new ArgumentException("Название компании не может быть пустым или состоять только из пробела.");
            }

            Position Workless = new Position("Безработный", "0");
            this._positions.Add(Workless);
            this.Company = company;
        }

        public Work() { }
    }
}
