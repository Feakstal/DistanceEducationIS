using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DistanceEducation.Classes;
using DistanceEducation.DataBase;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditLessonUserControl.xaml
    /// </summary>
    public partial class AddEditLessonUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Lesson EditLesson = new Lesson();

        public AddEditLessonUserControl()
        {
            InitializeComponent();
            cboxDisciplineName.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();

            if (cboxDisciplineName.Items != null)
            {
                cboxDisciplineName.SelectedIndex = 0;
            }

            tblockTitle.Text = "Добавление";
        }

        public AddEditLessonUserControl(Lesson GetLesson)
        {
            InitializeComponent();
            cboxDisciplineName.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();

            if (GetLesson != null)
            {
                CheckEdit = true;
                EditLesson = GetLesson;

                tboxLessonName.Text = GetLesson.LessonName;
                cboxDisciplineName.SelectedItem = Entities.Discipline.Where(i => i.DisciplineID == GetLesson.DisciplineID).Select(i => i.DisciplineName).FirstOrDefault();
            }

            tblockTitle.Text = "Редактирование";
        }

        private void UpdateLesson()
        {
            Lesson lesson = Entities.Lesson.Find(EditLesson.LessonID);

            if (cboxDisciplineName.Text.Length <= 120 && tboxLessonName.Text.Length <= 100)
            {
                lesson.LessonName = tboxLessonName.Text;
                lesson.DisciplineID = Entities.Discipline.Where(i => i.DisciplineName == cboxDisciplineName.SelectedItem.ToString()).Select(i => i.DisciplineID).FirstOrDefault();

                Entities.SaveChanges();

                MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddLesson()
        {
            if (cboxDisciplineName.Text.Length <= 120 && tboxLessonName.Text.Length <= 100)
            {
                Lesson NewLesson = new Lesson
                {
                    LessonName = tboxLessonName.Text,
                    DisciplineID = Entities.Discipline.Where(i => i.DisciplineName == cboxDisciplineName.SelectedItem.ToString()).Select(i => i.DisciplineID).FirstOrDefault()
                };

                Entities.Lesson.Add(NewLesson);
                Entities.SaveChanges();

                MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxLessonName.Text) || String.IsNullOrWhiteSpace(cboxDisciplineName.Text))
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите обновить запись?.", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    UpdateLesson();
                }
            }
            else if (!CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите добавить запись?.", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    AddLesson();
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditLesson.Children.Clear();
            grdAddEditLesson.Children.Add(new LessonsUserControl());
        }

        private void cboxDisciplineName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxLessonName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }
    }
}
