using DistanceEducation.DataBase;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DistanceEducation.Classes;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditEmployeeUserControl.xaml
    /// </summary>
    public partial class AddEditEmployeeUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Employee EditEmployee = new Employee();

        public AddEditEmployeeUserControl()
        {
            InitializeComponent();
            cboxDiscipline.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();
            cboxPost.ItemsSource = Entities.Post.Select(i => i.PostName).ToList();

            if (cboxDiscipline.Items != null)
            {
                cboxDiscipline.SelectedIndex = 0;
            }

            tblockTitle.Text = "Добавление";
        }

        public AddEditEmployeeUserControl(Employee GetEmployee)
        {
            InitializeComponent();
            cboxDiscipline.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();
            cboxPost.ItemsSource = Entities.Post.Select(i => i.PostName).ToList();

            if (GetEmployee != null)
            {
                EditEmployee = GetEmployee;
                CheckEdit = true;

                tboxSurname.Text = GetEmployee.Surname;
                tboxName.Text = GetEmployee.Name;
                tboxFatherName.Text = GetEmployee.FatherName;
                tboxJobExperience.Text = GetEmployee.JobExperience.ToString();
                cboxDiscipline.SelectedItem = Entities.Discipline.Where(i => i.DisciplineID == GetEmployee.DisciplineID).Select(i => i.DisciplineName).FirstOrDefault();
                cboxPost.SelectedItem = Entities.Post.Where(i => i.PostID == GetEmployee.PostID).Select(i => i.PostName).FirstOrDefault();
            }

            tblockTitle.Text = "Редактирование";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(tboxSurname.Text) || String.IsNullOrWhiteSpace(tboxName.Text)
                || cboxPost.SelectedItem == null || cboxDiscipline.SelectedItem == null)
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                if (MessageBox.Show("Вы действительно хотите обновить данные?", "Обновлениие данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Employee employee = Entities.Employee.Find(EditEmployee.EmployeeID);

                    employee.Surname = tboxSurname.Text;
                    employee.Name = tboxName.Text;
                    employee.FatherName = tboxFatherName.Text;
                    employee.JobExperience = Convert.ToByte(tboxJobExperience.Text);
                    employee.DisciplineID = Entities.Discipline.Where(i => i.DisciplineName == cboxDiscipline.SelectedItem.ToString()).Select(i => i.DisciplineID).FirstOrDefault();
                    employee.PostID = Entities.Post.Where(i => i.PostName == cboxPost.SelectedItem.ToString()).Select(i => i.PostID).FirstOrDefault();

                    if (Convert.ToByte(tboxJobExperience.Text) > 255 || Convert.ToByte(tboxJobExperience.Text) < 0)
                    {
                        MessageBox.Show("Опыт работы не может быть больше 255 лет или меньше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || cboxDiscipline.Text.Length > 120)
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
            if (!CheckEdit)
            {
                if (MessageBox.Show("Вы действительно хотите добавить запись?", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Employee NewEmployee = new Employee
                    {
                        Surname = tboxSurname.Text,
                        Name = tboxName.Text,
                        FatherName = tboxFatherName.Text,
                        JobExperience = Convert.ToByte(tboxJobExperience.Text),
                        DisciplineID = Entities.Discipline.Where(i => i.DisciplineName == cboxDiscipline.SelectedItem.ToString()).Select(i => i.DisciplineID).FirstOrDefault(),
                        PostID = Entities.Post.Where(i => i.PostName == cboxPost.SelectedItem.ToString()).Select(i => i.PostID).FirstOrDefault()
                    };

                    if (Convert.ToByte(tboxJobExperience.Text) > 255 || Convert.ToByte(tboxJobExperience.Text) < 0)
                    {
                        MessageBox.Show("Опыт работы не может быть больше 255 лет или меньше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || cboxDiscipline.Text.Length > 120)
                    {
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Entities.Employee.Add(NewEmployee);
                        Entities.SaveChanges();
                        MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditEmployee.Children.Clear();
            grdAddEditEmployee.Children.Add(new EmployeesUserControl());
        }

        private void tboxSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxFatherName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxJobExperience_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlLetters(sender, e);
        }

        private void cboxDiscipline_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }
    }
}