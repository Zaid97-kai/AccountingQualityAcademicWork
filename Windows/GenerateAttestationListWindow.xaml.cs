using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AccountingQualityAcademicWork.Windows
{
    public class Item
    {
        public string Name { get; set; }
        public List<int> Scores { get; set; } = new List<int>();
    }
    /// <summary>
    /// Логика взаимодействия для GenerateAttestationListWindow.xaml
    /// </summary>
    public partial class GenerateAttestationListWindow : Window
    {
        private MainWindow _mainWindow;
        private Models.Group _group;
        private List<Models.Student> _students;
        private List<Models.StudentInReportCard> _studentInReportCards;
        private ObservableCollection<Item> _items;
        public GenerateAttestationListWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _students = new List<Models.Student>();
            _studentInReportCards = new List<Models.StudentInReportCard>();
            _items = new ObservableCollection<Item>();
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

        List<T> ConvertArrayList<T>(ArrayList data)
        {
            List<T> result = new List<T>(data.Count);
            foreach (T item in data)
                result.Add(item);
            return result;
        }

        private void CbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DgAttestationList.Columns.Clear();
            this._items.Clear();
            this._students.Clear();
            this._studentInReportCards.Clear();

            this._group = CbGroups.SelectedItem as Models.Group;
            this._students = Models.JournalDBEntities.GetContext().Student.Where(s => s.Group.Id == _group.Id).ToList();

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

            DgAttestationList.Columns.Add(new DataGridTextColumn() { Header = "Ф.И.О.", Binding = new Binding("Name") });

            for (int i = 0; i < elements.Count(); i++)
            {
                DgAttestationList.Columns.Add(new DataGridTextColumn() { Header = elements[i].Key, Binding = new Binding("Scores[" + i + "]") });
            }

            foreach (var student in _students)
            {
                _items.Add(new Item() { Name = student.FullName });
                foreach (var element in elements)
                {
                    foreach (var item in element)
                    {
                        if (item.FullName == student.FullName)
                        {
                            _items[_items.Count - 1].Scores.Add(item.Score);
                        }
                    }
                }
            }

            DgAttestationList.ItemsSource = _items;
        }
    }
}