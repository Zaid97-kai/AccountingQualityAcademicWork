using AccountingQualityAcademicWork.PartialClasses;
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
    /// <summary>
    /// Логика взаимодействия для GenerateAttestationListWindow.xaml
    /// </summary>
    public partial class GenerateAttestationListWindow : Window
    {
        private MainWindow _mainWindow;
        private Models.Group _group;
        private List<Models.Student> _students;
        private List<Models.StudentInReportCard> _studentInReportCards;
        public GenerateAttestationListWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _studentInReportCards = new List<Models.StudentInReportCard>();
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
            this._group = CbGroups.SelectedItem as Models.Group;
            _students = Models.JournalDBEntities.GetContext().Student.Where(s => s.Group.Id == _group.Id).ToList();

            foreach (var inReportCard in Models.JournalDBEntities.GetContext().StudentInReportCard)
            {
                foreach (var student in _students)
                {
                    if (inReportCard.Student.Id == student.Id)
                    {
                        _studentInReportCards.Add(inReportCard);
                    }
                }
            }

            var elements = (from r in _studentInReportCards
                           select new 
                           { 
                               FullName = r.Student.FullName, 
                               Discipline = r.ReportCard.NameDiscipline, 
                               Score = r.Scores 
                           }).ToList().GroupBy(el => el.Discipline).ToList();
            var groupListByDiscipline = elements.SelectMany(group => group).GroupBy(s => s.Discipline);
            
            foreach (var group in elements.SelectMany(group => group).GroupBy(s => s.FullName))
            {
                var temp = new { FullName = group.Key };

            }



            DgAttestationList.ItemsSource = elements;
            

            //DgAttestationList.Columns.Add(new DataGridTextColumn()
            //{
            //    Header = "Ф.И.О",
            //    Binding = new Binding("FullName")
            //});

            //List<AttestationListItem> arrayList = new List<AttestationListItem>();
            //var students = Models.JournalDBEntities.GetContext().Student.ToList().Where(s => s.Group == this._group).ToList();
            //for(int i = 0; i < Models.JournalDBEntities.GetContext().StudentInReportCard.ToList().Count; i++)
            //{
            //    if (Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].Student.Group == this._group)
            //    {
            //        AttestationListItem temp = new AttestationListItem()
            //        {
            //            Name = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].Student.FullName,
            //            Discipline = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].ReportCard.NameDiscipline,
            //            Score = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList()[i].Scores
            //        };
            //        arrayList.Add(temp);
            //    }
            //}

            //var groupList = arrayList.GroupBy(g => g.Name);
            //var groupListByDiscipline = arrayList.GroupBy(g => g.Discipline);

            //IEnumerable<AttestationListItem> smths = groupList.SelectMany(group => group);
            //List<AttestationListItem> newList = smths.ToList();

            //for (int i = 0; i < groupListByDiscipline.Count(); i++)
            //{
            //    DgAttestationList.Columns.Add(new DataGridTextColumn()
            //    {
            //        Header = groupListByDiscipline.ToList()[i].Key,
            //        Binding = new Binding("Discipline" + i)
            //    });
            //}
        }
    }
}   
