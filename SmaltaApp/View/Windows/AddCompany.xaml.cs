using SmaltaApp.Model.HelperClasses;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SmaltaApp.View.Windows
{
    public partial class AddCompany : Window
    {
        public Company NewCompany { get; private set; }
        public AddCompany(Company c)
        {
            InitializeComponent();
            NewCompany = c;
            DataContext = NewCompany;
        }

        //Перемещение окна программы
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (NewCompany.Abbreviation != null && NewCompany.Abbreviation?.Replace(" ", "").Length != 0)
                DialogResult = true;
        }
    }


}
