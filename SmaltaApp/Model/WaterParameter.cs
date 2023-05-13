using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmaltaApp.Model
{
    public class WaterParameter : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Внешний ключ таблицы AllProjects
        int? projectId;
        public int? ProjectId
        {
            get { return projectId; }
            set
            {
                projectId = value;
                OnPropertyChanged("ProjectId");
            }
        }

        //Ссылка на проект
        Project? project;
        public Project? Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }

        //Водоканалы
        string? vodokanal;
        public string? Vodokanal
        {
            get { return vodokanal;
            }
            set
            {
                vodokanal = value;
                OnPropertyChanged("Vodokanal");
            }
        }

        //Договор
        string? treaty;
        public string? Treaty
        {
            get
            {
                return treaty;
            }
            set
            {
                treaty = value;
                OnPropertyChanged("Treaty");
            }
        }

        //Улица
        string? street;
        public string? Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
                OnPropertyChanged("Street");
            }
        }

        //Диаметр вырезки
        int? diameter;
        public int? Diameter
        {
            get { return diameter; }
            set
            {
                diameter = value;
                OnPropertyChanged("Diameter");
            }
        }

        #region Данные по холодной воде
        //Расход в секунду
        double? secondCold = 0;
        public double? SecondCold
        {
            get { return secondCold; }
            set
            {
                secondCold = value;
                OnPropertyChanged("SecondCold");
            }
        }

        //Расход в час
        double? hourCold = 0;
        public double? HourCold
        {
            get { return hourCold; }
            set
            {
                hourCold = value;
                OnPropertyChanged("HourCold");
            }
        }

        //Расход в день
        double? dayCold = 0;
        public double? DayCold
        {
            get { return dayCold; }
            set
            {
                dayCold = value;
                OnPropertyChanged("DayCold");
            }
        }

        //Расход во время пожаротушения
        double? fireFighting;
        public double? FireFighting
        {
            get { return fireFighting; }
            set
            {
                fireFighting = value;
                OnPropertyChanged("FireFighting");
            }
        }
        #endregion

        #region Данные по горячей воде
        //Расход в секунду
        double? secondHot = 0;
        public double? SecondHot
        {
            get { return secondHot; }
            set
            {
                secondHot = value;
                OnPropertyChanged("SecondHot");
            }
        }

        //Расход в час
        double? hourHot = 0;
        public double? HourHot
        {
            get { return hourHot; }
            set
            {
                hourHot = value;
                OnPropertyChanged("HourHot");
            }
        }

        //Расход в день
        double? dayHot = 0;
        public double? DayHot
        {
            get { return dayHot; }
            set
            {
                dayHot = value;
                OnPropertyChanged("DayHot");
            }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
