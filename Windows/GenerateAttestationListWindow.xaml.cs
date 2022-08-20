using System;
using System.Collections;
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
    public struct AttestationListItem
    {
        public string Name;
        public string Discipline;
        public int Score;
    }
    /// <summary>
    /// Логика взаимодействия для GenerateAttestationListWindow.xaml
    /// </summary>
    public partial class GenerateAttestationListWindow : Window
    {
        private MainWindow _mainWindow;
        private Models.Group _group;
        public GenerateAttestationListWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            CbGroups.ItemsSource = Models.JournalDBEntities.GetContext().Group.ToList().OrderBy(g => g.GroupNumber);
        }

        private void BnGenerate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this._mainWindow.Show();
            this.Hide();
        }

        private void CbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DgAttestationList.Columns.Add(new DataGridTextColumn()
            {
                Header = "Ф.И.О",
                Binding = new Binding("FullName")
            });

            List<AttestationListItem> arrayList = new List<AttestationListItem>();
            this._group = CbGroups.SelectedItem as Models.Group;
            var students = Models.JournalDBEntities.GetContext().Student.ToList().Where(s => s.Group == this._group).ToList();
            for(int i = 0; i < Models.JournalDBEntities.GetContext().StudentInReportCard.ToList().Count; i++)
            {
                if (Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].Student.Group == this._group)
                {
                    AttestationListItem temp = new AttestationListItem()
                    {
                        Name = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].Student.FullName,
                        Discipline = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].ReportCard.NameDiscipline,
                        Score = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].Scores
                    };
                    arrayList.Add(temp);
                }
            }

            var groupList = arrayList.GroupBy(g => g.Name);
            var groupListByDiscipline = arrayList.GroupBy(g => g.Discipline);

            IEnumerable<AttestationListItem> smths = groupList.SelectMany(group => group);
            List<AttestationListItem> newList = smths.ToList();

            for (int i = 0; i < groupListByDiscipline.Count(); i++)
            {
                DgAttestationList.Columns.Add(new DataGridTextColumn()
                {
                    Header = groupListByDiscipline.ToList()[i].Key,
                    Binding = new Binding("Discipline" + i)
                });
            }

            foreach (var g in groupList)
            {
                List<AttestationListItem> attestationListItems = newList.Where(a => a.Name == g.Key).ToList();
                
                DgAttestationList.Items.Add(new { FullName = g.Key, Discipline0 = attestationListItems[0].Score, Discipline1 = attestationListItems[1].Score });
            }
        }
    }
}   
