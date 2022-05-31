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
using DistanceEducation.DataBase;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportsUserControl.xaml
    /// </summary>
    public partial class ReportsUserControl : UserControl
    {
        Entities Entities = new Entities();

        List<Report> listReport = new List<Report>();
        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Фамилия ученика (по возрастанию)",
            "Фамилия ученика (по убыванию)",            
            "Фамилия сотрудника (по возрастанию)",
            "Фамилия сотрудника (по убыванию)",
            "Тема урока (по возрастанию)",
            "Тема урока (по убыванию)",
            "Дата (по возрастанию)",
            "Дата (по убыванию)",
            "Тип урока (по возрастанию)",
            "Тип урока (по убыванию)"
        };
        List<string> listFiltrIsAttend = new List<string>();
        List<string> listFiltrLessonType = new List<string>();

        public ReportsUserControl()
        {
            InitializeComponent();

            LvReports.ItemsSource = Entities.Report.ToList();

            List<IsAttend> reports = Entities.IsAttend.ToList();

            foreach (IsAttend item in reports)
            {
                listFiltrIsAttend.Add(item.IsAttendName);
            }

            listFiltrIsAttend.Insert(0, "Все категории");
            cboxFiltrIsAttend.ItemsSource = listFiltrIsAttend;
            cboxFiltrIsAttend.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtration()
        {
            listReport = Entities.Report.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listReport = listReport.OrderBy(i => i.ReportID).ToList();
                    break;
                case 1:
                    listReport = listReport.OrderByDescending(i => i.ReportID).ToList();
                    break;
                case 2:
                    listReport = listReport.OrderBy(i => i.PupilID).ToList();
                    break;
                case 3:
                    listReport = listReport.OrderByDescending(i => i.PupilID).ToList();
                    break;
                case 4:
                    listReport = listReport.OrderBy(i => i.EmployeeID).ToList();
                    break;
                case 5:
                    listReport = listReport.OrderByDescending(i => i.EmployeeID).ToList();
                    break;
                case 6:
                    listReport = listReport.OrderBy(i => i.LessonTopicID).ToList();
                    break;
                case 7:
                    listReport = listReport.OrderByDescending(i => i.LessonTopicID).ToList();
                    break;
                case 8:
                    listReport = listReport.OrderBy(i => i.TimetableID).ToList();
                    break;
                case 9:
                    listReport = listReport.OrderByDescending(i => i.TimetableID).ToList();
                    break;
                case 10:
                    listReport = listReport.OrderBy(i => i.LessonTypeID).ToList();
                    break;
                case 11:
                    listReport = listReport.OrderByDescending(i => i.LessonTypeID).ToList();
                    break;
                default:
                    listReport = listReport.OrderBy(i => i.EmployeeID).ToList();
                    break;
            }

            if (cboxFiltrIsAttend.SelectedIndex != 0)
            {
                listReport = listReport.Where(i => i.IsAttendID == cboxFiltrIsAttend.SelectedIndex).ToList();
            }

            listReport = listReport.Where(i => i.Pupil.Surname.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvReports.ItemsSource = listReport;

            tblockItemsCount.Text = $"Текущее количество записей: {LvReports.Items.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            grdReport.Children.Clear();
            grdReport.Children.Add(new AddEditReportUserControl());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            grdReport.Children.Clear();
            grdReport.Children.Add(new AddEditReportUserControl((sender as Button).DataContext as Report));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (question == MessageBoxResult.Yes)
            {
                Entities.Report.Remove((sender as Button).DataContext as Report);
                Entities.SaveChanges();
                LvReports.ItemsSource = Entities.Report.ToList();
                tblockItemsCount.Text = $"Текущее количество записей: {LvReports.Items.Count}";
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void cboxFiltrLessonType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void cboxFiltrIsAttend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdReport.Children.Clear();
            grdReport.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }
    }
}
