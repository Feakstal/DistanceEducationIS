using DistanceEducation.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для LessonsUserControl.xaml
    /// </summary>
    public partial class LessonsUserControl : UserControl
    {
        Entities Entities = new Entities();

        List<Lesson> listLesson = new List<Lesson>();
        List<string> listSortLesson = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Название урока (по возрастанию)",
            "Название урока (по убыванию)"
        };
        List<string> listFiltrDiscipline = new List<string>();

        public LessonsUserControl()
        {
            InitializeComponent();
            lvLessons.ItemsSource = Entities.Lesson.ToList();

            List<Discipline> disciplines = Entities.Discipline.ToList();

            foreach (Discipline item in disciplines)
            {
                listFiltrDiscipline.Add(item.DisciplineName);
            }

            listFiltrDiscipline.Insert(0, "Все категории");
            cboxFiltrLDiscipline.ItemsSource = listFiltrDiscipline;
            cboxFiltrLDiscipline.SelectedIndex = 0;

            cboxSortLesson.ItemsSource = listSortLesson;
            cboxSortLesson.SelectedIndex = 0;
        }

        private void FiltrationLesson()
        {
            listLesson = Entities.Lesson.ToList();

            int selectSort = cboxSortLesson.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listLesson = listLesson.OrderBy(i => i.LessonID).ToList();
                    break;
                case 1:
                    listLesson = listLesson.OrderByDescending(i => i.LessonID).ToList();
                    break;
                case 2:
                    listLesson = listLesson.OrderBy(i => i.LessonName).ToList();
                    break;
                case 3:
                    listLesson = listLesson.OrderByDescending(i => i.LessonName).ToList();
                    break;
                default:
                    listLesson = listLesson.OrderBy(i => i.LessonID).ToList();
                    break;
            }

            if (cboxFiltrLDiscipline.SelectedIndex != 0)
            {
                listLesson = listLesson.Where(i => i.Discipline.DisciplineName == cboxFiltrLDiscipline.SelectedItem.ToString()).ToList();
            }

            listLesson = listLesson.Where(i => i.LessonName.ToLower().Contains(tboxSearchLesson.Text.ToLower())).ToList();
            lvLessons.ItemsSource = listLesson;

            tblockItemsCountL.Text = $"Текущее количество записей: {lvLessons.Items.Count}";
        }

        private void btnAddL_Click(object sender, RoutedEventArgs e)
        {
            grdLessonsMain.Children.Clear();
            grdLessonsMain.Children.Add(new AddEditLessonUserControl());
        }

        private void btnEditL_Click(object sender, RoutedEventArgs e)
        {
            grdLessonsMain.Children.Clear();
            grdLessonsMain.Children.Add(new AddEditLessonUserControl((sender as Button).DataContext as Lesson));
        }

        private void btnDeleteL_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes)
            {
                Entities.Lesson.Remove((sender as Button).DataContext as Lesson);
                Entities.SaveChanges();
                lvLessons.ItemsSource = Entities.Lesson.ToList();
                tblockItemsCountL.Text = $"Текущее количество записей: {lvLessons.Items.Count}";
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdLessonsMain.Children.Clear();
            grdLessonsMain.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FiltrationLesson();
        }

        private void tboxSearchLesson_SelectionChanged(object sender, RoutedEventArgs e)
        {
            FiltrationLesson();
        }

        private void cboxSortLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrationLesson();
        }

        private void cboxFiltrLDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrationLesson();
        }
    }
}
