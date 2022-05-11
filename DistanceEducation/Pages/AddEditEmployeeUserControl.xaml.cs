using DistanceEducation.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditEmployeeUserControl.xaml
    /// </summary>
    public partial class AddEditEmployeeUserControl : UserControl
    {
        Entities Entities = new Entities();
        private bool CheckEditEmployee = false;
        private Employee EditEmployee = new Employee();

        public AddEditEmployeeUserControl()
        {
            InitializeComponent();
            cmbDiscipline.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();
            cmbPost.ItemsSource = Entities.Post.Select(i => i.PostName).ToList();
        }

        public AddEditEmployeeUserControl(Employee GetEmployee)
        {
            InitializeComponent();
            cmbDiscipline.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();
            cmbPost.ItemsSource = Entities.Post.Select(i => i.PostName).ToList();

            if (GetEmployee != null)
            {
                EditEmployee = GetEmployee;
                CheckEditEmployee = true;
                tboxSurname.Text = GetEmployee.Surname;
                tboxName.Text = GetEmployee.Name;
                tboxFatherName.Text = GetEmployee.FatherName;
                tboxJobExperience.Text = GetEmployee.JobExperience.ToString();
                cmbDiscipline.SelectedItem = Entities.Discipline.Where(i => i.DisciplineID == GetEmployee.DisciplineID).Select(i => i.DisciplineName).FirstOrDefault();
                cmbPost.SelectedItem = Entities.Post.Where(i => i.PostID == GetEmployee.PostID).Select(i => i.PostName).FirstOrDefault();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditEmployee)
            {
                Employee employee = Entities.Employee.Find(EditEmployee.EmployeeID);
                if (String.IsNullOrWhiteSpace(tboxSurname.Text) || String.IsNullOrWhiteSpace(tboxName.Text) || String.IsNullOrWhiteSpace(tboxJobExperience.Text) || cmbPost.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные?", "Обновлениие данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    employee.Surname = tboxSurname.Text;
                    employee.Name = tboxName.Text;
                    employee.FatherName = tboxFatherName.Text;
                    employee.JobExperience = Convert.ToByte(tboxJobExperience.Text);
                    if (cmbDiscipline.SelectedIndex != -1 || !String.IsNullOrWhiteSpace(cmbDiscipline.Text))
                    {
                        employee.DisciplineID = Entities.Discipline.Where(i => i.DisciplineName == cmbDiscipline.SelectedItem.ToString()).Select(i => i.DisciplineID).FirstOrDefault();
                    }
                    employee.PostID = Entities.Post.Where(i => i.PostName == cmbPost.SelectedItem.ToString()).Select(i => i.PostID).FirstOrDefault();

                    if (Convert.ToByte(tboxJobExperience.Text) > 255 || Convert.ToByte(tboxJobExperience.Text) < 0)
                    {
                        MessageBox.Show("Опыт работы не может быть больше 255 лет или меньше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || cmbDiscipline.Text.Length > 120)
                    {
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            if (!CheckEditEmployee)
            {
                if (String.IsNullOrWhiteSpace(tboxSurname.Text) || String.IsNullOrWhiteSpace(tboxName.Text) || String.IsNullOrWhiteSpace(tboxJobExperience.Text) || cmbPost.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить клиента?", "Добавление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (cmbDiscipline.SelectedIndex != -1 || !String.IsNullOrWhiteSpace(cmbDiscipline.Text))
                    {
                        Employee employee = new Employee
                        {
                            Surname = tboxSurname.Text,
                            Name = tboxName.Text,
                            FatherName = tboxFatherName.Text,
                            JobExperience = Convert.ToByte(tboxJobExperience.Text),
                            DisciplineID = Entities.Discipline.Where(i => i.DisciplineName == cmbDiscipline.SelectedItem.ToString()).Select(i => i.DisciplineID).FirstOrDefault(),
                            PostID = Entities.Post.Where(i => i.PostName == cmbPost.SelectedItem.ToString()).Select(i => i.PostID).FirstOrDefault()
                        };

                        if (Convert.ToByte(tboxJobExperience.Text) > 255 || Convert.ToByte(tboxJobExperience.Text) < 0)
                        {
                            MessageBox.Show("Опыт работы не может быть больше 255 лет или меньше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || cmbDiscipline.Text.Length > 120)
                        {
                            MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            Entities.Employee.Add(employee);
                            Entities.SaveChanges();
                            MessageBox.Show($"Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        Employee employee = new Employee
                        {
                            Surname = tboxSurname.Text,
                            Name = tboxName.Text,
                            FatherName = tboxFatherName.Text,
                            JobExperience = Convert.ToByte(tboxJobExperience.Text),
                            PostID = Entities.Post.Where(i => i.PostName == cmbPost.SelectedItem.ToString()).Select(i => i.PostID).FirstOrDefault()
                        };

                        if (Convert.ToByte(tboxJobExperience.Text) > 255 || Convert.ToByte(tboxJobExperience.Text) < 0)
                        {
                            MessageBox.Show("Опыт работы не может быть больше 255 лет или меньше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || cmbDiscipline.Text.Length > 120)
                        {
                            MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            Entities.Employee.Add(employee);
                            Entities.SaveChanges();
                            MessageBox.Show($"Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditEmployee.Children.Clear();
            grdAddEditEmployee.Children.Add(new EmployeesUserControl());
        }
    }
}