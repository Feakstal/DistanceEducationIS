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
using DistanceEducation.Classes;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPupilUserControl.xaml
    /// </summary>
    public partial class AddEditPupilUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Pupil EditPupil = new Pupil();

        public AddEditPupilUserControl()
        {
            InitializeComponent();
            cboxGrade.ItemsSource = Entities.Grade.Select(i => i.GradeName).ToList();

            tblockTitle.Text = "Добавление";
        }

        public AddEditPupilUserControl(Pupil GetPupil)
        {
            InitializeComponent();
            cboxGrade.ItemsSource = Entities.Grade.Select(i => i.GradeName).ToList();

            if (GetPupil != null)
            {
                EditPupil = GetPupil;
                CheckEdit = true;

                tboxSurname.Text = GetPupil.Surname;
                tboxName.Text = GetPupil.Name;
                tboxFatherName.Text = GetPupil.FatherName;
                tboxPupilStatusDesc.Text = GetPupil.PupilStatusDesc;
                cboxGrade.SelectedItem = Entities.Grade.Where(i => i.GradeID == GetPupil.GradeID).Select(i => i.GradeName).FirstOrDefault();
            }

            tblockTitle.Text = "Редактирование";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tboxSurname.Text) || String.IsNullOrWhiteSpace(tboxName.Text) || cboxGrade.SelectedItem == null)
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                if (MessageBox.Show("Вы действительно хотите обновить данные?", "Обновлениие данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pupil pupil = Entities.Pupil.Find(EditPupil.PupilID);

                    pupil.Surname = tboxSurname.Text;
                    pupil.Name = tboxName.Text;
                    pupil.FatherName = tboxFatherName.Text;

                    if (!String.IsNullOrWhiteSpace(tboxPupilStatusDesc.Text))
                    {
                        pupil.PupilStatusDesc = tboxPupilStatusDesc.Text;
                    }

                    pupil.GradeID = Entities.Grade.Where(i => i.GradeName == cboxGrade.SelectedItem.ToString()).Select(i => i.GradeID).FirstOrDefault();

                    if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || tboxPupilStatusDesc.Text.Length > 100 || cboxGrade.Text.Length > 5)
                    {
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else if (!CheckEdit)
            {
                if (MessageBox.Show("Вы действительно хотите добавить запись?", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pupil NewPupil = new Pupil
                    {
                        Surname = tboxSurname.Text,
                        Name = tboxName.Text,
                        FatherName = tboxFatherName.Text,
                        PupilStatusDesc = tboxPupilStatusDesc.Text,
                        GradeID = Entities.Grade.Where(i => i.GradeName == cboxGrade.SelectedItem.ToString()).Select(i => i.GradeID).FirstOrDefault()
                    };

                    if (tboxSurname.Text.Length > 40 | tboxName.Text.Length > 40 || tboxFatherName.Text.Length > 40 || tboxPupilStatusDesc.Text.Length > 100 || cboxGrade.Text.Length > 5)
                    {
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Entities.Pupil.Add(NewPupil);
                        Entities.SaveChanges();
                        MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void tboxSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxFatherName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void tboxPupilStatusDesc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlSpec(sender, e);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditPupil.Children.Clear();
            grdAddEditPupil.Children.Add(new PupilsUserControl());
        }
    }
}
