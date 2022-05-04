using DistanceEducation.Pages;
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
            var question = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (question == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void btnMaxWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winMain.DragMove();
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
                    usc = new TeachersUserControl();
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
    }
}
