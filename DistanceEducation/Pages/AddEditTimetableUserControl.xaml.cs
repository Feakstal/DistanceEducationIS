using DistanceEducation.Classes;
using DistanceEducation.DataBase;
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

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditTimetableUserControl.xaml
    /// </summary>
    public partial class AddEditTimetableUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Timetable EditTimetable = new Timetable();

        public AddEditTimetableUserControl()
        {
            InitializeComponent();

            cboxGrade.ItemsSource = Entities.Grade.Select(i => i.GradeName).ToList();
            cboxLesson.ItemsSource = Entities.Lesson.Select(i => i.LessonName).ToList();

            tblockTitle.Text = "Добавление";
        }

        public AddEditTimetableUserControl(Timetable GetTimetable)
        {
            InitializeComponent();

            cboxGrade.ItemsSource = Entities.Grade.Select(i => i.GradeName).ToList();
            cboxLesson.ItemsSource = Entities.Lesson.Select(i => i.LessonName).ToList();

            if (GetTimetable != null)
            {
                CheckEdit = true;
                EditTimetable = GetTimetable;

                cboxGrade.SelectedItem = Entities.Grade.Where(i => i.GradeID == GetTimetable.GradeID).Select(i => i.GradeName).FirstOrDefault();
                cboxLesson.SelectedItem = Entities.Lesson.Where(i => i.LessonID == GetTimetable.LessonID).Select(i => i.LessonName).FirstOrDefault();
                tboxLessonNumber.Text = GetTimetable.LessonNumber.ToString();
                dtpDateStart.Text = GetTimetable.DateStart.ToString();
                dtpDateEnd.Text = GetTimetable.DateEnd.ToString();
            }

            tblockTitle.Text = "Редактирование";
        }

        private void AddTimetable()
        {
            Timetable NewTimetable = new Timetable
            {
                GradeID = Entities.Grade.Where(i => i.GradeName == cboxGrade.SelectedItem.ToString()).Select(i => i.GradeID).FirstOrDefault(),
                LessonID = Entities.Lesson.Where(i => i.LessonName == cboxLesson.SelectedItem.ToString()).Select(i => i.LessonID).FirstOrDefault(),
                LessonNumber = Convert.ToByte(tboxLessonNumber.Text),
                DateStart = Convert.ToDateTime(dtpDateStart.Text),
                DateEnd = Convert.ToDateTime(dtpDateEnd.Text)
            };

            Entities.Timetable.Add(NewTimetable);
            Entities.SaveChanges();

            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateTimetable()
        {
            Timetable timetable = Entities.Timetable.Find(EditTimetable.TimetableID);

            timetable.GradeID = Entities.Grade.Where(i => i.GradeName == cboxGrade.SelectedItem.ToString()).Select(i => i.GradeID).FirstOrDefault();
            timetable.LessonID = Entities.Lesson.Where(i => i.LessonName == cboxLesson.SelectedItem.ToString()).Select(i => i.LessonID).FirstOrDefault();
            timetable.LessonNumber = Convert.ToByte(tboxLessonNumber.Text);
            timetable.DateStart = Convert.ToDateTime(dtpDateStart.Text);
            timetable.DateEnd = Convert.ToDateTime(dtpDateEnd.Text);

            Entities.SaveChanges();

            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(cboxGrade.Text) || String.IsNullOrWhiteSpace(cboxLesson.Text)
                || String.IsNullOrWhiteSpace(tboxLessonNumber.Text) || String.IsNullOrWhiteSpace(dtpDateStart.Text)
                || String.IsNullOrWhiteSpace(dtpDateEnd.Text))
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cboxGrade.Text.Length >= 5 || cboxLesson.Text.Length >= 100 || (byte.TryParse(tboxLessonNumber.Text, out byte val) && (val >= byte.MaxValue || val <= 0)))
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Entities.Grade.FirstOrDefault(i => i.GradeName == cboxGrade.Text) == null || Entities.Lesson.FirstOrDefault(i => i.LessonName == cboxLesson.Text) == null)
            {
                MessageBox.Show("Данный объект не существует в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите обновить запись?.", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    UpdateTimetable();
                }
            }
            else if (!CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите добавить запись?.", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    AddTimetable();
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditTimetable.Children.Clear();
            grdAddEditTimetable.Children.Add(new TimetableUserControl());
        }

        private void cboxGrade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlSpec(sender, e);
        }

        private void cboxLesson_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxLessonNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlLetters(sender, e);
        }

        private void tboxDateEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlForDate(sender, e);
        }

        private void tboxDateStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlForDate(sender, e);
        }

        private void dtpDateStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlForDate(sender, e);
        }

        private void dtpDateEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlForDate(sender, e);
        }
    }
}
