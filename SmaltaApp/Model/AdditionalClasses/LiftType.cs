﻿using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SmaltaApp.Model.AdditionalClasses
{
    public class LiftType : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Наименование типа конструкции шахты лифта
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
