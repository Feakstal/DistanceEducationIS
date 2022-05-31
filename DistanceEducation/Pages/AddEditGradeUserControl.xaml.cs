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
    /// Логика взаимодействия для AddEditGradeUserControl.xaml
    /// </summary>
    public partial class AddEditGradeUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Grade EditGrade = new Grade();
        private byte val;

        public AddEditGradeUserControl()
        {
            InitializeComponent();
            cboxProfile.ItemsSource = Entities.GradeProfile.Select(i => i.GradeProfileName).ToList();
        }

        public AddEditGradeUserControl(Grade GetGrade)
        {
            InitializeComponent();
            cboxProfile.ItemsSource = Entities.GradeProfile.Select(i => i.GradeProfileName).ToList();

            if (GetGrade != null)
            {
                CheckEdit = true;
                EditGrade = GetGrade;

                tboxGradeName.Text = GetGrade.GradeName;
                tboxPupilCount.Text = GetGrade.PupilCount.ToString();
                cboxProfile.SelectedItem = Entities.GradeProfile.Where(i => i.GradeProfileID == GetGrade.GradeProfileID).Select(i => i.GradeProfileName).FirstOrDefault();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxGradeName.Text) || String.IsNullOrWhiteSpace(tboxPupilCount.Text) || String.IsNullOrWhiteSpace(cboxProfile.Text))
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите обновить запись?.", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    UpdateGrade();
                }
            }
            else if (!CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите обновить запись?.", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    AddGrade();
                }
            }
        }

        private void UpdateGrade()
        {
            Grade grade = Entities.Grade.Find(EditGrade.GradeID);

            if (tboxGradeName.Text.Length < 5 && byte.TryParse(tboxPupilCount.Text, out val) && val <= byte.MaxValue && val >= 0 && cboxProfile.Text.Length < 80)
            {
                grade.GradeName = tboxGradeName.Text;
                grade.PupilCount = Convert.ToByte(tboxPupilCount.Text);
                if (Entities.GradeProfile.Where(i => i.GradeProfileID == grade.GradeProfileID).FirstOrDefault() == null)
                {
                    GradeProfile NewGradeProfile = new GradeProfile
                    {
                        GradeProfileName = cboxProfile.Text
                    };

                    Entities.GradeProfile.Add(NewGradeProfile);
                    Entities.SaveChanges();
                }
                else
                {
                    grade.GradeProfileID = Entities.GradeProfile.Where(i => i.GradeProfileName == cboxProfile.SelectedItem.ToString()).Select(i => i.GradeProfileID).FirstOrDefault();
                }

                Entities.SaveChanges();
                MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddGrade()
        {
            if (tboxGradeName.Text.Length < 5 && byte.TryParse(tboxPupilCount.Text, out val) && val <= byte.MaxValue && val >= 0 && cboxProfile.Text.Length < 80)
            {
                if (Entities.GradeProfile.Where(i => i.GradeProfileName == cboxProfile.Text).FirstOrDefault() == null)
                {
                    GradeProfile NewGradeProfile = new GradeProfile
                    {
                        GradeProfileName = cboxProfile.Text
                    };

                    Entities.GradeProfile.Add(NewGradeProfile);
                    Entities.SaveChanges();

                    Grade NewGrade = new Grade
                    {
                        GradeName = tboxGradeName.Text,
                        PupilCount = Convert.ToByte(tboxPupilCount.Text),
                        GradeProfileID = Entities.GradeProfile.Where(i => i.GradeProfileName == NewGradeProfile.GradeProfileName).Select(i => i.GradeProfileID).FirstOrDefault()
                    };

                    Entities.Grade.Add(NewGrade);
                    Entities.SaveChanges();

                    MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Grade NewGrade = new Grade
                    {
                        GradeName = tboxGradeName.Text,
                        PupilCount = Convert.ToByte(tboxPupilCount.Text),
                        GradeProfileID = Entities.GradeProfile.Where(i => i.GradeProfileName == cboxProfile.SelectedItem.ToString()).Select(i => i.GradeProfileID).FirstOrDefault()
                    };

                    Entities.Grade.Add(NewGrade);
                    Entities.SaveChanges();

                    MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditGrade.Children.Clear();
            grdAddEditGrade.Children.Add(new GradesUserControl());
        }

        private void tboxGradeName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlSpec(sender, e);
        }

        private void tboxPupilCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlLetters(sender, e);
        }

        private void cboxProfile_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }
    }
}
