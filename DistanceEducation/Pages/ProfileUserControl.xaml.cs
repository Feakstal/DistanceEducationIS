using DistanceEducation.Classes;
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
using System.Windows.Threading;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileUserControl.xaml
    /// </summary>
    public partial class ProfileUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Employee EditEmployee = new Employee();

        public ProfileUserControl()
        {
            InitializeComponent();
        }

        public ProfileUserControl(Employee GetEmployee)
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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxSurname.Text) || String.IsNullOrWhiteSpace(tboxName.Text)
                || cboxDiscipline.SelectedItem == null || cboxPost.SelectedItem == null)
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

        private void cboxPost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { tboxCurrentDateTime.Text = DateTime.Now.ToString(); };
            timer.Start();
        }
    }
}
