using SmaltaApp.Model.AdditionalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class GeologicalWork :INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Внешний ключ таблицы IgeWorking
        int? igeWorkingId;
        public int? IgeWorkingId
        {
            get { return igeWorkingId; }
            set
            {
                igeWorkingId = value;
            }
        }

        //Ссылка на данные выработок
        IgeWorking? igeWorking;
        public IgeWorking? IgeWorking
        {
            get { return igeWorking; }
            set
            {
                igeWorking = value;
            }
        }

        //Наименование
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

        //Мощность
        double? power;
        public double? Power
        {
            get { return power; }
            set
            {
                power = value;
                OnPropertyChanged("Power");
            }
        }

        //Вес
        double? weight;
        public double? Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        //Модуль деформации
        double? deformation;
        public double? Deformation
        {
            get { return deformation; }
            set
            {
                deformation = value;
                OnPropertyChanged("Deformation");
            }
        }

        //Удельное сцепление
        double? clutch;
        public double? Clutch
        {
            get { return clutch; }
            set
            {
                clutch = value;
                OnPropertyChanged("Clutch");
            }
        }

        //Угол внутреннего трения
        int? friction;
        public int? Friction
        {
            get { return friction; }
            set
            {
                friction = value;
                OnPropertyChanged("Friction");
            }
        }

        //Показатель текучести
        double? fluidity;
        public double? Fluidity
        {
            get { return fluidity; }
            set
            {
                fluidity = value;
                OnPropertyChanged("Fluidity");
            }
        }

        //Примечание
        string? note;
        public string? Note
        {
            get { return note; }
            set
            {
                note = value;
                OnPropertyChanged("Note");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
