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
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;


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
            LoadDataFromJson();
            Console.WriteLine("Форма №1 запущена");
            StudentCreation.Visible = false;
            CompanyDisplay.Visible = false;
            StudentSettingsNavigation.Visible = false;
            StudentSelectJob.Visible = false;
            SearchPanel.Visible = false;
            CompanySettingsNavigation.Visible = false;
            PositionCreation.Visible = false;
            List<string> Blank = new List<string>
            {
                "Не выбрано",
                "Студенты",
                "Компании"
            };
            InitializeComboBox(comboBox1, Blank);
        }
        /*Settings*/
        private void button3_Click(object sender, EventArgs e)
        {
            CloseSearch();
            SettingsNavigation.Visible = true;
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
                    "Не выбрана"
                };
                foreach (Work work in _works)
                {
                    Blank.Add(work.Company);
                }
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
            if (text == "Не выбрана")
            {
                PositionCreation.Visible = false;
                CompanyDisplay.Visible = false;
                CompanyCreation.Visible = true;
            } else
            {
                PositionCreation.Visible = false;
                CompanyDisplay.Visible = false;
                CompanyCreation.Visible = false;
                foreach (Work work in _works)
                {
                    if (comboBox3.Text == work.Company)
                    {
                        PositionCreation.Visible = true;
                        CompanyDisplay.Visible = true;
                        label70.Text = work.Company;
                        List<string> Blank = new List<string>();
                        foreach (Position position in work._positions)
                        {
                            Blank.Add(position.Name);
                        }
                        InitializeComboBox(comboBox6, Blank);
                        comboBox6.Text = "Безработный";
                    }
                }
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
            CompanyDisplay.Visible = false;
            PositionCreation.Visible = false;
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
            label36.Text = student._company.Company;
            label35.Text = student._position.Name;
            label37.Text = student._position.Sellary;
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

        /*
        StudentCompanySet 
        */
        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Не выбрана")
            {
                MessageBox.Show("Выберите профессию");
                label36.Text = "Не выбрана";
                label35.Text = "Безработный";
                label37.Text = "0";
                Student tempStudent = null;
                foreach (Student student in _students)
                {
                    if (comboBox2.Text == student.Name)
                    {
                        tempStudent = student;
                    }
                }
                if (tempStudent != null)
                {
                    _students.Remove(tempStudent);
                    tempStudent._company = new Work("Не выбрана");
                    tempStudent._position = new Position("Безработный", "0");
                    _students.Add(tempStudent);
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
            }
            else
            {
                Work tempWork = null;
                Position tempPos = null;
                foreach (Work work in _works)
                {
                    if (work.Company == comboBox4.Text)
                    {
                        tempWork = work;
                    }
                }
                if (tempWork != null)
                {
                    if (comboBox5.Text == "Безработный")
                    {
                        MessageBox.Show("Выберите должность");
                        label36.Text = tempWork.Company;
                        label35.Text = "Безработный";
                        label37.Text = "0";
                        Student tempStudent = null;
                        foreach (Student student in _students)
                        {
                            if (comboBox2.Text == student.Name)
                            {
                                tempStudent = student;
                            }
                        }
                        if (tempStudent != null)
                        {
                            _students.Remove(tempStudent);
                            tempStudent._company = tempWork;
                            tempStudent._position = new Position("Безработный","0");
                            _students.Add(tempStudent);
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
                    } 
                    else
                    {
                        foreach (Position position in tempWork._positions)
                        {
                            if (comboBox5.Text == position.Name)
                            {
                                tempPos = position;
                            }
                        }
                        if (tempPos != null)
                        {
                            label36.Text = tempWork.Company;
                            label35.Text = tempPos.Name;
                            label37.Text = tempPos.Sellary;
                            Student tempStudent = null;
                            foreach (Student student in _students)
                            {
                                if (comboBox2.Text == student.Name)
                                {
                                    tempStudent = student;
                                }
                            }
                            if (tempStudent != null)
                            {
                                _students.Remove(tempStudent);
                                tempStudent._company = tempWork;
                                tempStudent._position = tempPos;
                                _students.Add(tempStudent);
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
                        }
                    }
                }
            }
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
            string text = textBox6.Text;
            Work work = null;
            try
            {
                work = new Work(text);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            if (work != null)
            {
                MessageBox.Show("Работа успешно создана");
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
                InitializeComboBox(comboBox3, Blank);
                comboBox3.Text = work.Company;
            }
            else
            {
                MessageBox.Show("Работа не создана");
            }
        }

        /*
        StudentSelectWork 
        */
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Work tempWork = null;
            foreach (Work work in _works)
            {
                if (comboBox4.Text == work.Company)
                {
                    tempWork = work;
                }
            }
            if (tempWork != null)
            {
                List<string> Blank = new List<string>();
                foreach (Position position in tempWork._positions)
                {
                    Blank.Add(position.Name);
                }
                InitializeComboBox(comboBox5, Blank);
            } else
            {
                List<string> Blank = new List<string>
                {
                    "Безработный"
                };
                InitializeComboBox(comboBox5, Blank);
            }
        }
        /*
         PositionCreationSubmit
        */
        private void button6_Click(object sender, EventArgs e)
        {
            string name = textBox7.Text;
            string sellary = textBox8.Text;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            Work tempWork = null;
            foreach (Work work in _works)
            {
                if (comboBox3.Text == work.Company)
                {
                    tempWork = work;
                }
            }
            Position newPosition = new Position(name, sellary);
            _works.Remove(tempWork);
            tempWork._positions.Add(newPosition);
            _works.Add(tempWork);
            List<string> Blank = new List<string>
            {
                "Не выбрана"
            };
            foreach (Work work1 in _works)
            {
                Blank.Add(work1.Company);
            }
            InitializeComboBox(comboBox4, Blank);
            InitializeComboBox(comboBox3, Blank);
            comboBox3.Text = tempWork.Company;
        }
        /*
        PositionSelection 
        */
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Work work in _works)
            {
                if (comboBox3.Text == work.Company)
                {
                    foreach (Position position in work._positions)
                    {
                        if (comboBox6.Text == position.Name)
                        {
                            label46.Text = position.Name;
                            label49.Text = position.Sellary;
                        }
                    }
                }
            }
        }
        /*
        PositionDelete 
        */
        private void button9_Click(object sender, EventArgs e)
        {
            Work tempWork = null;
            Position tempPos = null;
            foreach (Work work in _works) {
                if (work.Company == comboBox3.Text)
                {
                    tempWork = work;
                    foreach (Position position in work._positions)
                    {
                        if (position.Name == comboBox6.Text)
                        {
                            tempPos = position;
                        }
                    }
                }
            }
            if (tempWork != null)
            {
                if (tempPos != null)
                {
                    if (tempPos.Name != "Безработный")
                    {
                        _works.Remove(tempWork);
                        tempWork._positions.Remove(tempPos);
                        _works.Add(tempWork);
                        List<string> Blank = new List<string>
                        {
                            "Не выбрана"
                        };
                        foreach (Work work1 in _works)
                        {
                            Blank.Add(work1.Company);
                        }
                        InitializeComboBox(comboBox4, Blank);
                        InitializeComboBox(comboBox3, Blank);
                        comboBox3.Text = tempWork.Company;

                    }
                }
            }
        }
        /*
        CompanyDelete 
        */
        private void button8_Click(object sender, EventArgs e)
        {
            Work tempWork = null;
            foreach (Work work in _works)
            {
                if (work.Company == comboBox3.Text)
                {
                    tempWork = work;
                }
            }
            if (tempWork != null)
            {
                _works.Remove(tempWork);
                List<string> Blank = new List<string>
                {
                    "Не выбрана"
                };
                foreach (Work work1 in _works)
                {
                    Blank.Add(work1.Company);
                }
                InitializeComboBox(comboBox4, Blank);
                InitializeComboBox(comboBox3, Blank);
                comboBox3.Text = "Не выбрана";
            }
        }
        public void CloseCreation()
        {
            StudentDisplay.Visible = false;
            SettingsNavigation.Visible = false;
            StudentSettingsNavigation.Visible = false;
            CompanyDisplay.Visible = false;
            CompanySettingsNavigation.Visible = false;
            StudentCreation.Visible = false;
            CompanyCreation.Visible = false;
            StudentSelectJob.Visible = false;
            PositionCreation.Visible = false;
        }

        public void CloseSearch()
        {
            SearchPanel.Visible = false;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            CloseCreation();
            SearchPanel.Visible = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string nameInput = Regex.Replace(textBox9.Text ?? "", @"[^\w\sа-яА-ЯёЁ\-]", "").Trim(); // Оставляем буквы, дефисы и пробелы

            string salaryMinRaw = Regex.Replace(textBox10.Text ?? "", @"[^0-9,\.]", "").Trim();
            string salaryMaxRaw = Regex.Replace(textBox11.Text ?? "", @"[^0-9,\.]", "").Trim();

            // Удалим лишние точки и запятые: только одну оставим
            salaryMinRaw = Regex.Replace(salaryMinRaw, @"[.,](?=[^.,]*[.,])", "");
            salaryMaxRaw = Regex.Replace(salaryMaxRaw, @"[.,](?=[^.,]*[.,])", "");

            // Проверка на пустоту
            if (string.IsNullOrWhiteSpace(nameInput) && string.IsNullOrWhiteSpace(salaryMinRaw) && string.IsNullOrWhiteSpace(salaryMaxRaw))
            {
                MessageBox.Show("Поиск по пустым полям невозможен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            textBox12.Clear(); // Очистим вывод

            // Сначала ищем по имени
            if (!string.IsNullOrWhiteSpace(nameInput))
            {
                var studentsFound = SearchService.SearchByNameRegex(_students, nameInput);
                textBox12.AppendText("Найденные студенты:\r\n");

                foreach (var student in studentsFound)
                {
                    textBox12.AppendText($"Имя: {student.Name}, Университет: {student.University}, Курс: {student.Course}\r\n");
                }

                textBox12.AppendText("\r\n");
            }

            // Потом ищем по зарплате
            if (!string.IsNullOrWhiteSpace(salaryMinRaw) && !string.IsNullOrWhiteSpace(salaryMaxRaw))
            {
                if (float.TryParse(salaryMinRaw.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float min) &&
                    float.TryParse(salaryMaxRaw.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float max))
                {
                    var positionsFound = SearchService.SearchPositionBySellaryRange(_works, min, max);
                    textBox12.AppendText("Найденные должности:\r\n");

                    foreach (var pos in positionsFound)
                    {
                        textBox12.AppendText($"Должность: {pos.Name}, Зарплата: {pos.Sellary}\r\n");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка в формате чисел зарплаты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox12.Clear();

            var sortedStudents = SortService.SortByNameThenMark(_students);

            textBox12.AppendText("Студенты (сортировка по имени и среднему баллу):\r\n\r\n");

            foreach (var student in sortedStudents)
            {
                textBox12.AppendText(
                    $"Имя: {student.Name}, Средний балл: {student.AvgMark}, Курс: {student.Course}, Группа: {student.Group}\r\n"
                );
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox12.Clear();

            var sortedPositions = SortService.SortBySellary(_works);

            textBox12.AppendText("Должности (сортировка по зарплате):\r\n\r\n");

            foreach (var position in sortedPositions)
            {
                textBox12.AppendText(
                    $"Название: {position.Name}, Зарплата: {position.Sellary}\r\n"
                );
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // можно убрать, если хочешь сохранить точные имена
                PropertyNameCaseInsensitive = true
            };

            try
            {
                // Сохраняем студентов
                string studentsJson = JsonSerializer.Serialize(_students, options);
                File.WriteAllText("students.json", studentsJson);

                // Сохраняем работы
                string worksJson = JsonSerializer.Serialize(_works, options);
                File.WriteAllText("works.json", worksJson);

                MessageBox.Show("Данные успешно сохранены в JSON.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataFromJson()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // чтобы не было проблем с регистром
            };

            if (File.Exists("students.json"))
            {
                string studentsJson = File.ReadAllText("students.json");
                _students = JsonSerializer.Deserialize<List<Student>>(studentsJson, options) ?? new List<Student>();
            }

            if (File.Exists("works.json"))
            {
                string worksJson = File.ReadAllText("works.json");
                _works = JsonSerializer.Deserialize<List<Work>>(worksJson, options) ?? new List<Work>();
            }

            MessageBox.Show("Данные успешно загружены из JSON.", "Загрузка завершена", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа: Рассчет капитала\nРазработал: Луцевич Павел Алексеевич\nВерсия: 0,1,15");
        }
    }
}
