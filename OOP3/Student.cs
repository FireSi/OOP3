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
        private string _profession;
        private string _homeAddress;
        private string _avgMark;
        private string _gender;
        private string _university;
        private DateTime _birthdate;
        private int _course;
        private int _group;
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
        [Required(ErrorMessage = "Поле профессии обязательно для заполнения")]
        [RegularExpression(
            @"^[А-ЯЁа-яё]+([ -][А-ЯЁа-яё]+)*$",
            ErrorMessage = "Поле профессии должно содержать только русские буквы")]
        public string Profession
        {
            get { return _profession; }
            set { _profession = value; }
        }
        [Required(ErrorMessage = "Поле адреса обязательно для заполнения")]
        [RegularExpression(
            @"^[А-ЯЁ][а-яё]+,\sул\.\s[А-ЯЁ][а-яё]+,\s\d+(-\d+)?(,\s\d+)?$",
            ErrorMessage = "Неверный формат адреса. Примеры: \"Минск, ул. Рощи, 10\", \"Минск, ул. Рощи, 10-2, 34\"")]
        public string HomeAddress
        {
            get { return _homeAddress; }
            set { _homeAddress = value; }
        }
        [Required(ErrorMessage = "Поле средней оценки обязательно для заполнения")]
        [RegularExpression(
             @"^(10|[0-9](?:[.,][0-9]{1,2})?)$",
             ErrorMessage = "Допустимо значение от 0 до 10, до двух знаков после запятой или точки")]
        [StringLength(4, ErrorMessage = "Максимальная длина — 4 символа")]
        public string AvgMark
        {
            get { return _avgMark; }
            set { _avgMark = value; }
        }
        [Required(ErrorMessage = "Поле университета обязательно для заполнения")]
        [RegularExpression(
            @"^[А-ЯЁ]{3,4}$",
            ErrorMessage = "Поле должно содержать 3–4 заглавные русские буквы")]
        public string University
        {
            get { return _university; }
            set { _university = value; }
        }
        public DateTime Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }
        public int Course
        {
            get { return _course; }
            set { _course = value; }
        }
        public int Group
        {
            get { return _group; }
            set { _group = value; }
        }

        /*
         
        СОБЫТИЯ
        
        */
        public event Action<string> NameValidationFailed;
        public event Action<string> ProffesionValidationFailed;
        public event Action<string> HomeAddressValidationFailed;
        public event Action<string> AvgMarkValidationFailed;
        public event Action<string> UniversityValidationFailed;
        public event Action<string> FirstValidationAccepted;
        /*
        
        КОНСТРУКТОР БЕЗ РАБОТЫ

        */
        public Student(string name, DateTime birthdate, string profession, string homeAdress, string avgMark, string university, string gender, int course, int group)
        {
            /*
            Присваиваю работу безработным 
            */
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
            this.Profession = profession;
            this.HomeAddress = homeAdress;
            this._gender = gender;
            this.AvgMark = avgMark;
            this.University = university;
            this.Birthdate = birthdate;
            this.Course = course;
            this.Group = group;
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
                    if (validationResult.MemberNames.Contains(nameof(Profession)))
                        ProffesionValidationFailed?.Invoke(validationResult.ErrorMessage);
                    if (validationResult.MemberNames.Contains(nameof(HomeAddress)))
                        HomeAddressValidationFailed?.Invoke(validationResult.ErrorMessage);
                    if (validationResult.MemberNames.Contains(nameof(AvgMark)))
                        AvgMarkValidationFailed?.Invoke(validationResult.ErrorMessage);
                    if (validationResult.MemberNames.Contains(nameof(University)))
                        UniversityValidationFailed?.Invoke(validationResult.ErrorMessage);
                }
            }
            else
            {
                FirstValidationAccepted?.Invoke("Все данные корректны. Запускаю протокол внедрения студента в базу...");
            }
        }
        /*
         
        Получение гендера
         
        */
        public string GetGender()
        {
            if (this._gender == "Мужской")
            {
                return "Муж.";
            } else if (this._gender == "Женский")
            {
                return "Жен.";
            } else
            {
                return "";
            }
        }
    }
}
