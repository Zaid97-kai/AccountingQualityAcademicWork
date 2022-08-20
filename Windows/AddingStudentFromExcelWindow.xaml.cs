using Microsoft.Win32;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddingStudentFromExcelWindow.xaml
    /// </summary>
    public partial class AddingStudentFromExcelWindow : Window
    {
        private MainWindow _mainWindow;
        private OpenFileDialog _openFileDialog;
        public AddingStudentFromExcelWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            CbGroup.ItemsSource = Models.JournalDBEntities.GetContext().Group.ToList();
            _mainWindow = mainWindow;
        }
        /// <summary>
        /// Импорт файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _openFileDialog = new OpenFileDialog()
                {
                    DefaultExt = "*.xls;*.xlsx",
                    Filter = "файл Excel (Spisok.xlsx)|*.xlsx",
                    Title = "Выберите файл базы данных"
                };
                if (!(_openFileDialog.ShowDialog() == true))
                    return;
            }
            catch
            {
                BnOpenFile.Background = Brushes.Red;
            }
        }
        /// <summary>
        /// Сохранение результатов импорта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[,] list;
                Excel.Application ObjWorkExcel = new Excel.Application();
                Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(_openFileDialog.FileName);
                Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
                var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

                int _columns = (int)lastCell.Column;
                int _rows = (int)lastCell.Row;

                list = new string[_rows, _columns];

                for (int j = 0; j < _columns; j++)
                    for (int i = 0; i < _rows; i++)
                        list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text;
                ObjWorkBook.Close(false, Type.Missing, Type.Missing);
                ObjWorkExcel.Quit();
                GC.Collect();

                for (int i = 0; i < _rows; i++)
                {
                    Models.JournalDBEntities.GetContext().Student.Add(new Models.Student() { Surname = list[i, 0], Name = list[i, 1], Patronymic = list[i, 2], Group = CbGroup.SelectedItem as Models.Group }); ;
                }
                Models.JournalDBEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this._mainWindow.Show();
                this.Hide();
            }
        }
        /// <summary>
        /// Возвращение в главное меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnBack_Click(object sender, RoutedEventArgs e)
        {
            this._mainWindow.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this._mainWindow.Show();
            this.Hide();
        }
    }
}
