using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            string text = comboBox1.Text;
            if (text == "Студенты")
            {
                CloseSettingsNavigation();
                StudentDisplay.Visible = true;
                StudentSettingsNavigation.Visible = true;
                StudentCreation.Visible = true;
                List<string> Blank = new List<string>
                {
                    "Все"
                };
                InitializeComboBox(comboBox2, Blank);
            } 
            else if (text == "Компании")
            {
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
                CloseSettingsNavigation();
            }
        }
        /*StudentSettingsNavigation*/
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if 
        }
        /*CompanySettingsNavigation*/
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*StudentCreation*/
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

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
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }
    }
}
