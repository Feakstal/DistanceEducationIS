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
    }
}
