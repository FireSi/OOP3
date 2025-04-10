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
        /*
         
        ПОЛЯ
        
        */
        private string _name;
        /*
         
        СВОЙСТВА
        
        */
        [Required(ErrorMessage = "Поле ФИО обязательно для заполнения")]
        [RegularExpression(
            @"^([А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?)\s((([А-ЯЁ][а-яё]+)|([А-ЯЁ]\.))(\s([А-ЯЁ][а-яё]+|[А-ЯЁ]\.))?)$",
            ErrorMessage = "Неверный формат ФИО")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Gender { get; set; }
        /*
         
        СОБЫТИЯ
        
        */
        public event Action<string> NameValidationFailed;
        public event Action<string> GenderValidationFailed;
        public event Action<string> FirstValidationAccepted;
        /*
        
        КОНСТРУКТОР БЕЗ РАБОТЫ

        */
        public Student(string name, DateTime birthdate, string profession, string homeAdress, string avgMark, string university, string gender, int course, int group)
        {
            /*
            Пробежка по всем значениям в DEBUG консоли. 
            */
            Console.WriteLine("Вызываю конструктор Student, передавая в него");
            Console.WriteLine("Name = \"" + name + "\"");
            Console.WriteLine("Birthday = \"" + birthdate.ToString() + "\"");
            Console.WriteLine("Profession = \"" + profession + "\"");
            Console.WriteLine("HomeAdress = \"" + homeAdress + "\"");
            Console.WriteLine("AvgMark = \"" + avgMark + "\"");
            Console.WriteLine("University = \"" + university + "\"");
            Console.WriteLine("Gender = \"" + gender + "\"");
            Console.WriteLine("Course = \"" + course.ToString() + "\"");
            Console.WriteLine("Group = \"" + group.ToString() + "\"");


            /*
            Передача свойствам значений
            */
            this.Name = name;
            this.Gender = gender;
        }
        /*
         
        ВАЛИДАЦИЯ
         
        */
        public void Validate()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, context, results, true);

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    if (validationResult.MemberNames.Contains(nameof(Name)))
                        NameValidationFailed?.Invoke(validationResult.ErrorMessage);

                    if (validationResult.MemberNames.Contains(nameof(Gender)))
                        GenderValidationFailed?.Invoke(validationResult.ErrorMessage);
                }
            }
            else
            {
                FirstValidationAccepted?.Invoke("Все данные корректны. Запускаю протокол внедрения студента в базу...");
            }
        }
    }
}
