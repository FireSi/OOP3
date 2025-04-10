using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    public class Student
    {
        private string _name;

        public Student(string fullName)
        {
            // Назначаем значение для поля Name
            this.Name = fullName;

            // Создаем контекст валидации
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();

            // Проводим валидацию объекта
            bool isValid = Validator.TryValidateObject(this, context, results, true);

            // Если объект не валиден, выводим ошибки
            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    // Выводим каждое сообщение об ошибке в консоль
                    Console.WriteLine(validationResult.ErrorMessage);
                }
            }
            else
            {
                Console.WriteLine("Все данные корректны.");
            }
        }

        [Required(ErrorMessage = "Поле ФИО обязательно для заполнения")]
        [RegularExpression(
            @"^([А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?)\s((([А-ЯЁ][а-яё]+)|([А-ЯЁ]\.))(\s([А-ЯЁ][а-яё]+|[А-ЯЁ]\.))?)$",
            ErrorMessage = "Неверный формат ФИО")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
