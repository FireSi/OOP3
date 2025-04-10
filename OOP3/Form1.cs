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

namespace OOP3
{
    public partial class Form1 : Form
    {
        /*ApplicationVariables*/
        private List<Student> _students = null;


        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Форма №1 запущена");
            StudentCreation.Visible = false;
            StudentSettingsNavigation.Visible = false;
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
                    "Все",
                    "ds"
                };
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
                Console.WriteLine("Студент не выбран;");
                StudentDisplay.Visible = false;
                Console.WriteLine("Разрешаю ввод в создании студента;");
                StudentCreation.Enabled = true;
            } else
            {
                Console.WriteLine("Выбран студент \"" + text + "\";");
                StudentDisplay.Visible = true;
                Console.WriteLine("Запрещаю ввод в создании студента;");
                StudentCreation.Enabled = false;
            }
        }
        /*CompanySettingsNavigation*/
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            newStudent.GenderValidationFailed += OnGenderValidationFailed;
            newStudent.FirstValidationAccepted += OnValidationAccepted;

            // Запуск валидации
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
            StudentDisplay.Visible = false;
            StudentSettingsNavigation.Visible = false;
            CompanySettingsNavigation.Visible = false;
            StudentCreation.Visible = false;
            ClearStudentInput();
        }
        private void ClearStudentInput()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            DateTime DateCorrect = DateTime.Today;
            DateCorrect = new DateTime(DateCorrect.Year - 16, DateCorrect.Month, DateCorrect.Day);
            dateTimePicker1.MaxDate = DateCorrect;
            dateTimePicker1.MinDate = DateTime.Parse("01.01.1980");
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
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
        }

        private void OnGenderValidationFailed(string message)
        {
            Console.WriteLine("Ошибка пола: " + message);
            MessageBox.Show("Ошибка пола: " + message);
        }

        private void OnValidationAccepted(string message)
        {
            Console.WriteLine("Успешная валидация: " + message);
            MessageBox.Show(message);
        }
    }
}
