﻿using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SmaltaApp.Model.AdditionalClasses
{
    public class ExteriorFinish : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Наименование внешней отделки
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

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
