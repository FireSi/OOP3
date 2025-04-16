using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace OOP3
{
    public partial class Form1 : Form
    {
        /*ApplicationVariables*/
        private List<Student> _students = new List<Student>();
        private List<Work> _works = new List<Work>();


        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Форма №1 запущена");
            StudentCreation.Visible = false;
            StudentSettingsNavigation.Visible = false;
            StudentSelectJob.Visible = false;
            CompanySettingsNavigation.Visible = false;
            List<string> Blank = new List<string>
            {
                "Не выбрано",
                "Студенты",
                "Компании"
            };
            InitializeComboBox(comboBox1, Blank);
        }
        /*Navigation*/
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        /*SettingsNavigation*/
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Изменено состояние выборки настроек...");
            string text = comboBox1.Text;
            if (text == "Студенты")
            {
                Console.WriteLine("Выбраны настройки студентов;");
                CloseSettingsNavigation();
                StudentDisplay.Visible = true;
                StudentSettingsNavigation.Visible = true;
                StudentCreation.Visible = true;
                List<string> Blank = new List<string>
                {
                    "Все"
                };
                foreach (Student student in _students)
                {
                    Blank.Add(student.Name);
                }
                InitializeComboBox(comboBox2, Blank);
            } 
            else if (text == "Компании")
            {
                Console.WriteLine("Выбраны настройки компаний;");
                CloseSettingsNavigation();
                CompanySettingsNavigation.Visible = true;
                List<string> Blank = new List<string>
                {
                    "Все"
                };
                InitializeComboBox(comboBox3, Blank);
            }
            else
            {
                Console.WriteLine("Выбраны настройки \""+text+"\";");
                CloseSettingsNavigation();
            }
        }
        /*StudentSettingsNavigation*/
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Изменено состояние выборки студента...");
            Console.WriteLine("Очищаю ввод в создании студента...");
            ClearStudentInput();
            string text = comboBox2.Text;
            if (text == "Все")
            {
                StudentSelectJob.Visible = false;
                Console.WriteLine("Студент не выбран;");
                StudentDisplay.Visible = false;
                Console.WriteLine("Разрешаю ввод в создании студента;");
                StudentCreation.Enabled = true;
            } else
            {
                StudentSelectJobClear();
                StudentSelectJob.Visible = true;
                Console.WriteLine("Выбран студент \"" + text + "\";");
                StudentDisplay.Visible = true;
                Console.WriteLine("Запрещаю ввод в создании студента;");
                StudentCreation.Enabled = false;
                Student TempStudent = null;
                foreach (Student student in _students)
                {
                    if (student.Name == text)
                    {
                        TempStudent = student;
                    }
                }
                ShowStudentInfo(TempStudent);
            }
        }
        /*CompanySettingsNavigation*/
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = comboBox3.Text;
            if (text == "Все")
            {
                CompanyCreation.Visible = true;
            } else
            {
                CompanyCreation.Visible = false;
            }
        }
        /*StudentCreationSubmit*/
        private void button2_Click(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine("=== Консоль разработчика ===");
            Console.WriteLine("Клавиша создания студента нажата...");
            string name = textBox1.Text;
            DateTime birthdate = dateTimePicker1.Value;
            string profession = textBox2.Text;
            string homeAdress = textBox4.Text;
            string avgMark = textBox3.Text;
            string university = textBox5.Text;
            string gender = "none";
            int course = Convert.ToInt32(numericUpDown1.Value);
            int group = Convert.ToInt32(numericUpDown2.Value);
            if (radioButton1.Checked == true)
            {
                gender = "Мужской";
            }
            else if (radioButton2.Checked == true)
            {
                gender = "Женский";
            }

            Student newStudent = new Student(name,birthdate,profession,homeAdress,avgMark,university,gender,course,group);


            newStudent.NameValidationFailed += OnNameValidationFailed;
            newStudent.ProffesionValidationFailed += OnProffesionValidationFailed;
            newStudent.HomeAddressValidationFailed += OnHomeAddressValidationFailed;
            newStudent.AvgMarkValidationFailed += OnAvgMarkValidationFailed;
            newStudent.UniversityValidationFailed += OnUniversityValidationFailed;
            newStudent.FirstValidationAccepted += OnValidationAccepted;

            newStudent.Validate();


        }

        private void InitializeComboBox(ComboBox SelectedComboBox, List<string> Content)
        {
            ComboBox comboBox = SelectedComboBox;
            comboBox.Items.Clear(); // Очистка старых значений
            comboBox.Items.AddRange(Content.ToArray());
            comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1; // Установка первого элемента, если есть элементы
            if (Content.Count() <= 1)
            {
                comboBox.Enabled = false;
            }
            else
            {
                comboBox.Enabled = true;
            }
            comboBox.KeyPress += comboBox_KeyPress;
        }
        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void CloseSettingsNavigation()
        {

            CompanyCreation.Visible = false;
            StudentSelectJob.Visible = false;
            StudentDisplay.Visible = false;
            StudentSettingsNavigation.Visible = false;
            CompanySettingsNavigation.Visible = false;
            StudentCreation.Visible = false;
            ClearStudentInput();
        }
        private void ClearStudentInput()
        {
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            textBox4.BackColor = Color.White;
            textBox5.BackColor = Color.White;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            DateTime DateCorrect = DateTime.Today;
            DateCorrect = new DateTime(DateCorrect.Year - 16, DateCorrect.Month, DateCorrect.Day);
            dateTimePicker1.MaxDate = DateCorrect;
            dateTimePicker1.MinDate = DateTime.Parse("01.01.1980");
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton1.Checked = true;
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
        }
        private void OnNameValidationFailed(string message)
        {
            Console.WriteLine("Ошибка имени: " + message);
            MessageBox.Show("Ошибка имени: " + message);
            textBox1.BackColor = Color.Firebrick;
        }
        private void OnProffesionValidationFailed(string message)
        {
            Console.WriteLine("Ошибка профессии: " + message);
            MessageBox.Show("Ошибка профессии: " + message);
            textBox2.BackColor = Color.Firebrick;
        }
        private void OnHomeAddressValidationFailed(string message)
        {
            Console.WriteLine("Ошибка адреса: " + message);
            MessageBox.Show("Ошибка адреса: " + message);
            textBox4.BackColor = Color.Firebrick;
        }
        private void OnAvgMarkValidationFailed(string message)
        {
            Console.WriteLine("Ошибка средней оценки: " + message);
            MessageBox.Show("Ошибка средней оценки: " + message);
            textBox3.BackColor = Color.Firebrick;
        }
        private void OnUniversityValidationFailed(string message)
        {
            Console.WriteLine("Ошибка университета: " + message);
            MessageBox.Show("Ошибка университета: " + message);
            textBox5.BackColor = Color.Firebrick;
        }

        private void OnValidationAccepted(string message)
        {
            Console.WriteLine("Успешная валидация: " + message);
            MessageBox.Show(message);
            Console.Clear();
            Console.WriteLine("=== Консоль разработчика ===");
            Console.WriteLine("Клавиша создания студента нажата...");
            string name = textBox1.Text;
            DateTime birthdate = dateTimePicker1.Value;
            string profession = textBox2.Text;
            string homeAdress = textBox4.Text;
            string avgMark = textBox3.Text;
            string university = textBox5.Text;
            string gender = "none";
            int course = Convert.ToInt32(numericUpDown1.Value);
            int group = Convert.ToInt32(numericUpDown2.Value);
            if (radioButton1.Checked == true)
            {
                gender = "Мужской";
            }
            else if (radioButton2.Checked == true)
            {
                gender = "Женский";
            }

            Student newStudent = new Student(name, birthdate, profession, homeAdress, avgMark, university, gender, course, group);

            ClearStudentInput();
            List<string> Blank = new List<string>
            {
                "Все"
            };
            foreach (Student student in _students)
            {
                Blank.Add(student.Name);
            }
            Blank.Add(newStudent.Name);
            _students.Add(newStudent);
            InitializeComboBox(comboBox2, Blank);
            comboBox2.Text = newStudent.Name;
        }
        private void ShowStudentInfo(Student student)
        {
            label14.Text = student.Name;
            label16.Text = student.Birthdate.ToString("dd MMMM yyyy", new CultureInfo("ru-RU"));
            label18.Text = student.Profession;
            label20.Text = student.HomeAddress;
            label26.Text = student.University;
            label30.Text = student.GetGender();
            label29.Text = student.Course.ToString();
            label28.Text = student.Group.ToString();
            label27.Text = student.AvgMark;
        }
        /*StudentDelete*/
        private void button5_Click(object sender, EventArgs e)
        {
            string text = comboBox2.Text;
            Student TempStudent = null;
            foreach(Student student in _students)
            {
                if (text == student.Name)
                {
                    TempStudent = student;
                }
            }
            _students.Remove(TempStudent);
            List<string> Blank = new List<string>
            {
                "Все"
            };
            foreach (Student student in _students)
            {
                Blank.Add(student.Name);
            }
            InitializeComboBox(comboBox2, Blank);
            comboBox2.Text = "Все";
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void StudentSelectJobClear()
        {
            List<string> Blank = new List<string>
            {
                "Не выбрана"
            };
            List<string> Blank1 = new List<string>
            {
                "Безработный"
            };
            foreach (Work work in _works)
            {
                Blank.Add(work.Company);
            }
            InitializeComboBox(comboBox4, Blank);
            InitializeComboBox(comboBox5, Blank1);
        }
        private void ClearCompanyCreation()
        {
            textBox6.Text = string.Empty;
            textBox6.BackColor = Color.White;
        }
        /*
        CompanyCreationSubmit
        */
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Работа успешно создана");
            string text = textBox6.Text;
            Work work = new Work(text);
            ClearCompanyCreation();
            _works.Add(work);
            List<string> Blank = new List<string>
            {
                "Не выбрана"
            };
            foreach (Work work1 in _works)
            {
                Blank.Add(work1.Company);
            }
            InitializeComboBox(comboBox4, Blank);
            comboBox2.Text = work.Company;
        }

        /*
        StudentSelectWork 
        */
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
