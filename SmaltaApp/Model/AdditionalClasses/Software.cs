using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model.AdditionalClasses
{
    public class Software : INotifyPropertyChanged
    {
        //Код проекта
        public int Id { get; set; }

        //Наименование программного обеспечения
        string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        //Разработчик
        string? developer;
        public string? Developer
        {
            get { return developer; }
            set
            {
                developer = value;
                OnPropertyChanged("Developer");
            }
        }

        //Сертификат
        string? certificate;
        public string? Сertificate
        {
            get { return certificate; }
            set
            {
                certificate = value;
                OnPropertyChanged("Сertificate");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
