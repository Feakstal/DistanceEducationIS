using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DistanceEducation.DataBase;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesUserControl.xaml
    /// </summary>
    public partial class EmployeesUserControl : UserControl
    {
        Entities Entities = new Entities();

        List<Employee> listEmployee = new List<Employee>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Фамилия (по возрастанию)",
            "Фамилия (по убыванию)",
            "Опыт работы (по возрастанию)",
            "Опыт работы (по убыванию)"
        };

        List<string> listFiltrDiscipline = new List<string>();
        List<string> listFiltrPost = new List<string>();

        public EmployeesUserControl()
        {
            InitializeComponent();

            LvEmployees.ItemsSource = Entities.Employee.ToList();

            List<Post> posts = Entities.Post.ToList();
            List<Discipline> disciplines = Entities.Discipline.ToList();

            foreach (Discipline item in disciplines)
            {
                listFiltrDiscipline.Add(item.DisciplineName);
            }

            foreach (Post item in posts)
            {
                listFiltrPost.Add(item.PostName);
            }

            listFiltrDiscipline.Insert(0, "Все категории");
            cboxFiltrDiscipline.ItemsSource = listFiltrDiscipline;
            cboxFiltrDiscipline.SelectedIndex = 0;

            listFiltrPost.Insert(0, "Все категории");
            cboxFiltrPost.ItemsSource = listFiltrPost;
            cboxFiltrPost.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        private void Filtration()
        {
            listEmployee = Entities.Employee.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listEmployee = listEmployee.OrderBy(i => i.EmployeeID).ToList();
                    break;
                case 1:
                    listEmployee = listEmployee.OrderByDescending(i => i.EmployeeID).ToList();
                    break;
                case 2:
                    listEmployee = listEmployee.OrderBy(i => i.Surname).ToList();
                    break;
                case 3:
                    listEmployee = listEmployee.OrderByDescending(i => i.Surname).ToList();
                    break;
                case 4:
                    listEmployee = listEmployee.OrderBy(i => i.JobExperience).ToList();
                    break;
                case 5:
                    listEmployee = listEmployee.OrderByDescending(i => i.JobExperience).ToList();
                    break;
                default:
                    listEmployee = listEmployee.OrderBy(i => i.EmployeeID).ToList();
                    break;
            }

            if (cboxFiltrPost.SelectedIndex != 0)
            {
                listEmployee = listEmployee.Where(i => i.PostID == cboxFiltrPost.SelectedIndex).ToList();
            }

            if (cboxFiltrDiscipline.SelectedIndex != 0)
            {
                listEmployee = listEmployee.Where(i => i.Discipline.DisciplineName == cboxFiltrDiscipline.SelectedItem.ToString()).ToList();
            }

            listEmployee = listEmployee.Where(i => i.Surname.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvEmployees.ItemsSource = listEmployee;

            tblockItemsCount.Text = $"Текущее количество записей: {LvEmployees.Items.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            grdEmployee.Children.Clear();
            grdEmployee.Children.Add(new AddEditEmployeeUserControl());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            grdEmployee.Children.Clear();
            grdEmployee.Children.Add(new AddEditEmployeeUserControl((sender as Button).DataContext as Employee));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var question = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (question == MessageBoxResult.Yes)
            {
                Entities.Employee.Remove((sender as Button).DataContext as Employee);
                Entities.SaveChanges();
                LvEmployees.ItemsSource = Entities.Employee.ToList();
                Filtration();
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

        private void cboxFiltrDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void cboxFiltrPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtration();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Filtration();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdEmployee.Children.Clear();
            grdEmployee.Children.Add(new ProfileUserControl(Views.AuthWindow.EmployeeFound));
        }
    }
}
