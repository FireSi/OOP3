using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string Sellary
        {
            get { return _sellary.ToString(); }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _sellary = 0;
                    return;
                }

                // 1. Оставляем только цифры и первую точку или запятую
                var cleaned = new System.Text.StringBuilder();
                bool decimalFound = false;

                foreach (char c in value)
                {
                    if (char.IsDigit(c))
                    {
                        cleaned.Append(c);
                    }
                    else if ((c == '.' || c == ',') && !decimalFound)
                    {
                        cleaned.Append('.');
                        decimalFound = true;
                    }
                }

                string result = cleaned.ToString();

                // 2. Удаляем ведущие нули перед числом (до точки, если есть)
                if (decimalFound)
                {
                    var parts = result.Split('.');
                    parts[0] = parts[0].TrimStart('0');
                    if (parts[0] == "") parts[0] = "0"; // если осталась пустота
                    result = parts[0] + "." + parts[1];
                }
                else
                {
                    result = result.TrimStart('0');
                    if (result == "") result = "0";
                }

                // 3. Преобразуем в float
                if (!float.TryParse(result, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _sellary))
                {
                    _sellary = 0;
                }
            }
        }
        /*
        Конструктор 
        */
        public Position(string name, string sellary)
        {
            this.Name = name;
            this.Sellary = sellary;
        }
    }
}
