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
using System.Windows.Shapes;
using DistanceEducation.DataBase;
using DistanceEducation.Classes;

namespace DistanceEducation.Views
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        Entities Entities = new Entities();

        public RegWindow()
        {
            InitializeComponent();
            winReg = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxLogin.Text) || String.IsNullOrWhiteSpace(pboxPassword.Password)
                || String.IsNullOrWhiteSpace(tboxSurname.Text) || String.IsNullOrWhiteSpace(tboxName.Text))
            {
                MessageBox.Show("Некоторый поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Entities.User.FirstOrDefault(i => i.Login == tboxLogin.Text) != null)
            {
                MessageBox.Show("Профиль с данным логином уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var quesiton = MessageBox.Show("Завершить создание профиля?.", "Создание профиля", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (quesiton == MessageBoxResult.Yes)
            {
                Employee NewEmployee = new Employee
                {
                    Surname = tboxSurname.Text,
                    Name = tboxName.Text,
                    DisciplineID = 1,
                    PostID = 3
                };

                if (tboxSurname.Text.Length > 40 || tboxName.Text.Length > 40)
                {
                    MessageBox.Show("Некоторый поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Entities.Employee.Add(NewEmployee);
                    Entities.SaveChanges();
                }

                User NewUser = new User
                {
                    Login = tboxLogin.Text,
                    Password = pboxPassword.Password,
                    PostID = 3,
                    EmployeeID = NewEmployee.EmployeeID
                };

                if (tboxLogin.Text.Length > 30 || pboxPassword.Password.Length > 30)
                {
                    MessageBox.Show("Некоторый поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Entities.User.Add(NewUser);
                    Entities.SaveChanges();
                    MessageBox.Show("Вы успешно зарегистрировались.", "Создание профиля", MessageBoxButton.OK, MessageBoxImage.Information);

                    AuthWindow authWindow = new AuthWindow();
                    this.Close();
                    authWindow.ShowDialog();
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
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) winReg.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes) Application.Current.Shutdown();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите покинуть окно регистрации?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes)
            {
                AuthWindow authWindow = new AuthWindow();
                this.Close();
                authWindow.ShowDialog();
            }
        }
    }
}
