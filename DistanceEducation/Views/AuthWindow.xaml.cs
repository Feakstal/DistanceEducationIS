using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DistanceEducation.DataBase;

namespace DistanceEducation.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        Entities Entities = new Entities();

        private static User AuthUser;
        public static Employee EmployeeFound;

        public AuthWindow()
        {
            InitializeComponent();
            winAuth = this;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            AuthUser = Entities.User.FirstOrDefault(i => i.Login == tboxLogin.Text && i.Password == pboxPassword.Password);

            if (AuthUser != null)
            {
                EmployeeFound = Entities.Employee.Where(i => i.EmployeeID == AuthUser.EmployeeID).FirstOrDefault();

                if (EmployeeFound == null)
                {
                    MessageBox.Show("Идентификация не пройдена. Пользователь не найден.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (EmployeeFound != null)
                {
                    MessageBox.Show($"Здравствуйте, {EmployeeFound.Name}.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = new MainWindow(EmployeeFound);
                    mainWindow.Show();
                    Hide();
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(pboxPassword.Password) && !String.IsNullOrWhiteSpace(tboxLogin.Text))
                {
                    MessageBox.Show("Вы не ввели пароль.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (String.IsNullOrWhiteSpace(pboxPassword.Password) && String.IsNullOrWhiteSpace(tboxLogin.Text))
                {
                    MessageBox.Show("Вы не заполнили данные для авторизации.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!String.IsNullOrWhiteSpace(tboxLogin.Text) && !String.IsNullOrWhiteSpace(pboxPassword.Password))
                {
                    MessageBox.Show("В доступе отказано. Проверьте правильность введенных данных.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (String.IsNullOrWhiteSpace(tboxLogin.Text) && !String.IsNullOrWhiteSpace(pboxPassword.Password))
                {
                    MessageBox.Show("Вы не ввели логин.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes) Application.Current.Shutdown();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) winAuth.DragMove();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите перейти в окно регистрации?", "Авторизация", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes)
            {
                RegWindow regWindow = new RegWindow();
                this.Close();
                regWindow.ShowDialog();
            }
        }
    }
}
