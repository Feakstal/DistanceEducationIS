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
    /// Логика взаимодействия для PupilsUserControl.xaml
    /// </summary>
    public partial class PupilsUserControl : UserControl
    {
        Entities Entities = new Entities();

        List<Pupil> listPupil = new List<Pupil>();
        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Фамилия (по возрастанию)",
            "Фамилия (по убыванию)",
            "Класс (по возрастанию)",
            "Класс (по убыванию)"
        };
        List<string> listFiltrGrade = new List<string>();

        public PupilsUserControl()
        {
            InitializeComponent();
            LvPupils.ItemsSource = Entities.Pupil.ToList();

            List<Grade> grades = Entities.Grade.ToList();

            foreach (Grade item in grades)
            {
                listFiltrGrade.Add(item.GradeName);
            }

            listFiltrGrade.Insert(0, "Все категории");
            cboxFiltrGrade.ItemsSource = listFiltrGrade;
            cboxFiltrGrade.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtration()
        {
            listPupil = Entities.Pupil.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listPupil = listPupil.OrderBy(i => i.PupilID).ToList();
                    break;
                case 1:
                    listPupil = listPupil.OrderByDescending(i => i.PupilID).ToList();
                    break;
                case 2:
                    listPupil = listPupil.OrderBy(i => i.Surname).ToList();
                    break;
                case 3:
                    listPupil = listPupil.OrderByDescending(i => i.Surname).ToList();
                    break;
                case 4:
                    listPupil = listPupil.OrderBy(i => i.GradeID).ToList();
                    break;
                case 5:
                    listPupil = listPupil.OrderByDescending(i => i.GradeID).ToList();
                    break;
                default:
                    listPupil = listPupil.OrderBy(i => i.PupilID).ToList();
                    break;
            }

            if (cboxFiltrGrade.SelectedIndex != 0)
            {
                listPupil = listPupil.Where(i => i.Grade.GradeName == cboxFiltrGrade.SelectedItem.ToString()).ToList();
            }

            listPupil = listPupil.Where(i => i.Surname.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvPupils.ItemsSource = listPupil;

            tblockItemsCount.Text = $"Текущее количество записей: {LvPupils.Items.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            grdPupil.Children.Clear();
            grdPupil.Children.Add(new AddEditPupilUserControl());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            grdPupil.Children.Clear();
            grdPupil.Children.Add(new AddEditPupilUserControl((sender as Button).DataContext as Pupil));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (question == MessageBoxResult.Yes)
            {
                Entities.Pupil.Remove((sender as Button).DataContext as Pupil);
                Entities.SaveChanges();
                LvPupils.ItemsSource = Entities.Pupil.ToList();
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void cboxFiltrGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdPupil.Children.Clear();
            grdPupil.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }
    }
}
