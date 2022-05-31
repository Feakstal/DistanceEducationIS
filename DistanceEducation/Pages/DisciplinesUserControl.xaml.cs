using DistanceEducation.Classes;
using DistanceEducation.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для DisciplinesUserControl.xaml
    /// </summary>
    public partial class DisciplinesUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Discipline EditDiscipline = new Discipline();
        private short val;

        List<Discipline> listDiscipline = new List<Discipline>();
        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Название дисциплины (по возрастанию)",
            "Название дисциплины (по убыванию)",
            "Количество часов (по возрастанию)",
            "Количество часов (по убыванию)"
        };

        public DisciplinesUserControl()
        {
            InitializeComponent();

            LvDisciplines.ItemsSource = Entities.Discipline.ToList();

            cboxDiscipline.ItemsSource = Entities.Discipline.Select(i => i.DisciplineName).ToList();

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;

            tblockTitleMode.Text = "Добавление";
        }

        private void Filtration()
        {
            listDiscipline = Entities.Discipline.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listDiscipline = listDiscipline.OrderBy(i => i.DisciplineID).ToList();
                    break;
                case 1:
                    listDiscipline = listDiscipline.OrderByDescending(i => i.DisciplineID).ToList();
                    break;
                case 2:
                    listDiscipline = listDiscipline.OrderBy(i => i.DisciplineName).ToList();
                    break;
                case 3:
                    listDiscipline = listDiscipline.OrderByDescending(i => i.DisciplineName).ToList();
                    break;
                case 4:
                    listDiscipline = listDiscipline.OrderBy(i => i.HoursCount).ToList();
                    break;
                case 5:
                    listDiscipline = listDiscipline.OrderByDescending(i => i.HoursCount).ToList();
                    break;
                default:
                    listDiscipline = listDiscipline.OrderBy(i => i.DisciplineID).ToList();
                    break;
            }

            LvDisciplines.ItemsSource = listDiscipline;

            tblockItemsCount.Text = $"Текущее количество записей: {LvDisciplines.Items.Count}";
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void UpdateDiscipline()
        {
            Discipline discipline = Entities.Discipline.Find(EditDiscipline.DisciplineID);

            if (cboxDiscipline.Text.Length <= 120 && short.TryParse(tboxHoursCount.Text, out val) && val <= short.MaxValue && val >= 0)
            {
                discipline.DisciplineName = cboxDiscipline.Text;
                discipline.HoursCount = Convert.ToInt16(tboxHoursCount.Text);

                Entities.SaveChanges();
                LvDisciplines.ItemsSource = Entities.Discipline.ToList();

                MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDiscipline()
        {

            if (Entities.Discipline.FirstOrDefault(i => i.DisciplineName == cboxDiscipline.Text) != null)
            {
                MessageBox.Show("Данная дисциплина существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cboxDiscipline.Text.Length <= 120 && short.TryParse(tboxHoursCount.Text, out val) && val <= short.MaxValue && val >= 0)
            {
                Discipline NewDiscipline = new Discipline
                {
                    DisciplineName = cboxDiscipline.Text,
                    HoursCount = Convert.ToInt16(tboxHoursCount.Text)
                };

                Entities.Discipline.Add(NewDiscipline);
                Entities.SaveChanges();
                LvDisciplines.ItemsSource = Entities.Discipline.ToList();

                MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(cboxDiscipline.Text))
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите обновить запись?.", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (question == MessageBoxResult.Yes)
                {
                    UpdateDiscipline();
                }
            }
            else if (!CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите добавить запись?.", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    AddDiscipline();
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CheckEdit = false;

            cboxDiscipline.Text = null;
            tboxHoursCount.Text = null;
            tblockTitleMode.Text = "Добавление";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CheckEdit = true;

            Discipline GetDiscipline = (sender as Button).DataContext as Discipline;
            EditDiscipline = GetDiscipline;

            cboxDiscipline.Text = GetDiscipline.DisciplineName;
            tboxHoursCount.Text = GetDiscipline.HoursCount.ToString();
            tblockTitleMode.Text = "Редактирование";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (question == MessageBoxResult.Yes)
            {
                Entities.Discipline.Remove((sender as Button).DataContext as Discipline);
                Entities.SaveChanges();
                LvDisciplines.ItemsSource = Entities.Discipline.ToList();
                Filtration();
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdDiscipline.Children.Clear();
            grdDiscipline.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }

        private void tboxHoursCount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlLetters(sender, e);
        }

        private void cboxDiscipline_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }
    }
}