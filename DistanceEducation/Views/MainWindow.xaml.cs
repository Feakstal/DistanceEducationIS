using DistanceEducation.Pages;
using DistanceEducation.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DistanceEducation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            winMain = this;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите закрыть программу?", "Завершение работы", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (question == MessageBoxResult.Yes) 
                Application.Current.Shutdown();
        }

        private void btnMaxWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void btnMinWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) winMain.DragMove();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void lvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            grdMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {             
                case "ItemReports":
                    usc = new ReportsUserControl();
                    grdMain.Children.Add(usc);
                    break;                
                case "ItemTimetable":
                    usc = new TimetableUserControl();
                    grdMain.Children.Add(usc);
                    break;                
                case "ItemTeachers":
                    usc = new EmployeesUserControl();
                    grdMain.Children.Add(usc);
                    break;
                case "ItemPupils":
                    usc = new PupilsUserControl();
                    grdMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

        private void lvExit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemExit":
                    var question = MessageBox.Show("Вы действительно хотите выйти из профиля?", "Смена пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (question == MessageBoxResult.Yes)
                    {
                        AuthWindow authWindow = new AuthWindow();
                        authWindow.Show();
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
        }

        private void LvMenu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
