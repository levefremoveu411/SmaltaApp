using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class Developer : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Полное наименование исполнителя
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

        //Аббревиатура
        string? abbreviation;
        public string? Abbreviation
        {
            get { return abbreviation; }
            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
