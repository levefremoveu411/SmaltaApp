using Excel = Microsoft.Office.Interop.Excel ;
using Microsoft.Win32;
using System;

namespace SmaltaApp.Documentation
{
    public class ExcelHelper : IDisposable
    {
        public Excel.Application? application;
        private Excel.Workbook? workbook;

        public ExcelHelper(string fileName)
        {
            application = new Excel.Application();
            workbook = (Excel.Workbook)application.Workbooks.Open(fileName);
            application.Visible = true;
        }

        public void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel документ (*.xlsx)|.xlsx|Все файлы (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                object fileName = saveFileDialog.FileName;
                workbook!.SaveAs(fileName);
             
            }
        }

        public void Dispose()
        {
            try
            {
                application!.Quit();
            }
            catch { }
        }
    }
}
