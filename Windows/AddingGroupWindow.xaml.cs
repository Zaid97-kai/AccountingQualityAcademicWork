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
using System.Windows.Shapes;

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddingGroupWindow.xaml
    /// </summary>
    public partial class AddingGroupWindow : Window
    {
        private MainWindow _mainWindow;
        public AddingGroupWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void BnAddingGroup_Click(object sender, RoutedEventArgs e)
        {
            Models.JournalDBEntities.GetContext().Group.Add(new Models.Group() { GroupNumber = Convert.ToInt32(TbNumberGroup.Text), Specialization = CbCodeSpecialization.SelectedItem as Models.Specialization });
            Models.JournalDBEntities.GetContext().SaveChanges();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow.Show();
            this.Hide();
        }
    }
}
