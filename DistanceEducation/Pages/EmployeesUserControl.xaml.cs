using System.Linq;
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

        public EmployeesUserControl()
        {
            InitializeComponent();

            LvEmployee.ItemsSource = Entities.Employee.ToList();
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            grdEmployee.Children.Clear();
            grdEmployee.Children.Add(new AddEditEmployeeUserControl());
        }

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            grdEmployee.Children.Clear();
            grdEmployee.Children.Add(new AddEditEmployeeUserControl((sender as Button).DataContext as Employee));
        }
    }
}
