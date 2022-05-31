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
    /// Логика взаимодействия для GradesUserControl.xaml
    /// </summary>
    public partial class GradesUserControl : UserControl
    {
        Entities Entities = new Entities();

        List<Grade> listGrade = new List<Grade>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Название класса (по возрастанию)",
            "Название класса (по убыванию)",
            "Количество учеников (по возрастанию)",
            "Количество учеников (по убыванию)"
        };

        List<string> listFiltrGradeProfile = new List<string>();

        public GradesUserControl()
        {
            InitializeComponent();

            LvGrades.ItemsSource = Entities.Grade.ToList();

            List<GradeProfile> gradeProfiles = Entities.GradeProfile.ToList();

            foreach (GradeProfile item in gradeProfiles)
            {
                listFiltrGradeProfile.Add(item.GradeProfileName);
            }

            listFiltrGradeProfile.Insert(0, "Все категории");
            cboxFiltrGradeProfile.ItemsSource = listFiltrGradeProfile;
            cboxFiltrGradeProfile.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtration()
        {
            listGrade = Entities.Grade.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listGrade = listGrade.OrderBy(i => i.GradeID).ToList();
                    break;
                case 1:
                    listGrade = listGrade.OrderByDescending(i => i.GradeID).ToList();
                    break;
                case 2:
                    listGrade = listGrade.OrderBy(i => i.GradeName).ToList();
                    break;
                case 3:
                    listGrade = listGrade.OrderByDescending(i => i.GradeName).ToList();
                    break;
                case 4:
                    listGrade = listGrade.OrderBy(i => i.GradeProfileID).ToList();
                    break;
                case 5:
                    listGrade = listGrade.OrderByDescending(i => i.GradeProfileID).ToList();
                    break;
                default:
                    listGrade = listGrade.OrderBy(i => i.GradeID).ToList();
                    break;
            }

            if (cboxFiltrGradeProfile.SelectedIndex != 0)
            {
                listGrade = listGrade.Where(i => i.GradeProfileID == cboxFiltrGradeProfile.SelectedIndex).ToList();
            }

            listGrade = listGrade.Where(i => i.GradeName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvGrades.ItemsSource = listGrade;

            tblockItemsCount.Text = $"Текущее количество записей: {LvGrades.Items.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            grdGrade.Children.Clear();
            grdGrade.Children.Add(new AddEditGradeUserControl());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            grdGrade.Children.Clear();
            grdGrade.Children.Add(new AddEditGradeUserControl((sender as Button).DataContext as Grade));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (question == MessageBoxResult.Yes)
            {
                Entities.Grade.Remove((sender as Button).DataContext as Grade);
                Entities.SaveChanges();
                LvGrades.ItemsSource = Entities.Grade.ToList();
                tblockItemsCount.Text = $"Текущее количество записей: {LvGrades.Items.Count}";
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void cboxFiltrGradeProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdGrade.Children.Clear();
            grdGrade.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }
    }
}
