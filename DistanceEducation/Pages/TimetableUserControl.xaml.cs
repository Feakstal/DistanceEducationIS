using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DistanceEducation.DataBase;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для TimetableUserControl.xaml
    /// </summary>
    public partial class TimetableUserControl : UserControl
    {
        Entities Entities = new Entities();

        List<Timetable> listTimetable = new List<Timetable>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Номер урока (по возрастанию)",
            "Номер урока (по убыванию)",
            "Дата (по возрастанию)",
            "Дата (по убыванию)",
            "Название класса (по возрастанию)",
            "Название класса (по убыванию)"
        };

        List<string> listFiltrLesson = new List<string>();

        public TimetableUserControl()
        {
            InitializeComponent();
            LvTimetable.ItemsSource = Entities.Timetable.ToList();

            List<Lesson> lessons = Entities.Lesson.ToList();

            foreach (Lesson item in lessons)
            {
                listFiltrLesson.Add(item.LessonName);
            }

            listFiltrLesson.Insert(0, "Все категории");
            cboxFiltrLesson.ItemsSource = listFiltrLesson;
            cboxFiltrLesson.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        private void Filtration()
        {
            listTimetable = Entities.Timetable.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listTimetable = listTimetable.OrderBy(i => i.TimetableID).ToList();
                    break;
                case 1:
                    listTimetable = listTimetable.OrderByDescending(i => i.TimetableID).ToList();
                    break;
                case 2:
                    listTimetable = listTimetable.OrderBy(i => i.LessonNumber).ToList();
                    break;
                case 3:
                    listTimetable = listTimetable.OrderByDescending(i => i.LessonNumber).ToList();
                    break;
                case 4:
                    listTimetable = listTimetable.OrderBy(i => i.DateEnd).ToList();
                    break;
                case 5:
                    listTimetable = listTimetable.OrderByDescending(i => i.DateEnd).ToList();
                    break;
                case 6:
                    listTimetable = listTimetable.OrderBy(i => i.GradeID).ToList();
                    break;
                case 7:
                    listTimetable = listTimetable.OrderByDescending(i => i.GradeID).ToList();
                    break;
                default:
                    listTimetable = listTimetable.OrderBy(i => i.TimetableID).ToList();
                    break;
            }

            if (cboxFiltrLesson.SelectedIndex != 0)
            {
                listTimetable = listTimetable.Where(i => i.Lesson.LessonName == cboxFiltrLesson.SelectedItem.ToString()).ToList();
            }

            listTimetable = listTimetable.Where(i => i.DateEnd.ToString().Contains(tboxSearch.Text)).ToList();
            LvTimetable.ItemsSource = listTimetable;

            tblockItemsCount.Text = $"Текущее количество записей: {LvTimetable.Items.Count}";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdTimetable.Children.Clear();
            grdTimetable.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            grdTimetable.Children.Clear();
            grdTimetable.Children.Add(new AddEditTimetableUserControl());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            grdTimetable.Children.Clear();
            grdTimetable.Children.Add(new AddEditTimetableUserControl((sender as Button).DataContext as Timetable));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes)
            {
                Entities.Timetable.Remove((sender as Button).DataContext as Timetable);
                Entities.SaveChanges();
                LvTimetable.ItemsSource = Entities.Timetable.ToList();
                Filtration();
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void cboxFiltrGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void cboxFiltrLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
