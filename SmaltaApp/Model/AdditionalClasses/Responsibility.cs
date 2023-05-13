using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model.AdditionalClasses
{
    public class Responsibility : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Уровень ответственности
        string? level;
        public string? Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }

        //Класс ответственности
        string? classR;
        public string? Class
        {
            get { return classR; }
            set
            {
                classR = value;
                OnPropertyChanged("Class");
            }
        }

        //Коэффициент
        float? coefficient;
        public float? Coefficient
        {
            get { return coefficient; }
            set
            {
                coefficient = value;
                OnPropertyChanged("Coefficient");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
