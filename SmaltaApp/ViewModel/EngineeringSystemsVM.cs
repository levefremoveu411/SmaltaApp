using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model;
using SmaltaApp.Model.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SmaltaApp.ViewModel
{
    public class EngineeringSystemsVM : INotifyPropertyChanged
    {
        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? editCommandEngineeringSystems;

        public WaterParameter? WaterParameter;
        public ElectricityParameter? ElectricityParameter;
        public HeatParameter? HeatParameter;
        //Конструктор
        public EngineeringSystemsVM(int projectID)
        {
            db.Database.EnsureCreated();

            WaterParameter = db.WaterParameters.FirstOrDefault(x => x.ProjectId == projectID);
            WaterParameter ??= new() { ProjectId = projectID };

            ElectricityParameter = db.ElectricityParameters.FirstOrDefault(x => x.ProjectId == projectID);
            ElectricityParameter ??= new() { ProjectId = projectID };

            HeatParameter = db.HeatParameters.FirstOrDefault(x => x.ProjectId == projectID);
            HeatParameter ??= new() { ProjectId = projectID };


            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
        }

        #region Свойства для данных по водоснабжению
        //Водоканал
        public string? Vodokanal
        {
            get { return WaterParameter!.Vodokanal; }
            set
            {
                WaterParameter!.Vodokanal = value;
                OnPropertyChanged("Vodokanal");
            }
        }

        //Договор
        public string? Treaty
        {
            get { return WaterParameter!.Treaty; }
            set
            {
                WaterParameter!.Treaty = value;
                OnPropertyChanged("Treaty");
            }
        }

        //Улица
        public string? Street
        {
            get { return WaterParameter!.Street; }
            set
            {
                WaterParameter!.Street = value;
                OnPropertyChanged("Street");
            }
        }

        //Диаметр вырезки
        public string? Diameter
        {
            get
            {
                if (WaterParameter!.Diameter == null)
                    return string.Empty;
                else
                    return WaterParameter!.Diameter.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.Diameter = Convert.ToInt32(value);
                OnPropertyChanged("Diameter");
            }
        }

        #region Расход в секунду
        //Расход холодной воды/сек
        public string? SecondCold
        {
            get
            {
                if (WaterParameter!.SecondCold == null)
                    return string.Empty;
                else
                    return WaterParameter!.SecondCold.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.SecondCold = Convert.ToDouble(value);
                else
                    WaterParameter!.SecondCold = 0;
                WaterSecond = Convert.ToDouble(WaterParameter!.SecondCold + WaterParameter!.SecondHot);
                OnPropertyChanged("SecondCold");
            }
        }
        //Расход горячей воды/сек
        public string? SecondHot
        {
            get
            {
                if (WaterParameter!.SecondHot == null)
                    return string.Empty;
                else
                    return WaterParameter!.SecondHot.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.SecondHot = Convert.ToDouble(value);
                else
                    WaterParameter!.SecondHot = 0;
                WaterSecond = Convert.ToDouble(WaterParameter!.SecondCold + WaterParameter!.SecondHot);
                OnPropertyChanged("SecondHot");
            }
        }
        //Сумма расхода в секунду
        public double? WaterSecond
        {
            get
            {
                return Math.Round(Convert.ToDouble(WaterParameter!.SecondCold + WaterParameter!.SecondHot), 2);
            }
            set
            {
                OnPropertyChanged("WaterSecond");
            }
        }
        #endregion

        #region Расход в час
        //Расход холодной воды/час
        public string? HourCold
        {
            get
            {
                if (WaterParameter!.HourCold == null)
                    return string.Empty;
                else
                    return WaterParameter!.HourCold.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.HourCold = Convert.ToDouble(value);
                else
                    WaterParameter!.HourCold = 0;
                WaterHour = Convert.ToDouble(WaterParameter!.HourCold + WaterParameter!.HourHot);
                OnPropertyChanged("HourCold");
            }
        }
        //Расход горячей воды/час
        public string? HourHot
        {
            get
            {
                if (WaterParameter!.HourHot == null)
                    return string.Empty;
                else
                    return WaterParameter!.HourHot.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.HourHot = Convert.ToDouble(value);
                else
                    WaterParameter!.HourHot = 0;
                WaterHour = Convert.ToDouble(WaterParameter!.HourCold + WaterParameter!.HourHot);
                OnPropertyChanged("HourHot");
            }
        }
        //Сумма расхода в час
        public double? WaterHour
        {
            get
            {
                return Math.Round(Convert.ToDouble(WaterParameter!.HourCold + WaterParameter!.HourHot), 2);
            }
            set
            {
                OnPropertyChanged("WaterHour");
            }
        }
        #endregion

        #region Расход в день
        //Расход холодной воды/день
        public string? DayCold
        {
            get
            {
                if (WaterParameter!.DayCold == null)
                    return string.Empty;
                else
                    return WaterParameter!.DayCold.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.DayCold = Convert.ToDouble(value);
                else
                    WaterParameter!.DayCold = 0;
                WaterDay = Convert.ToDouble(WaterParameter!.DayCold + WaterParameter!.DayHot);
                OnPropertyChanged("DayCold");
            }
        }
        //Расход горячей воды/день
        public string? DayHot
        {
            get
            {
                if (WaterParameter!.DayHot == null)
                    return string.Empty;
                else
                    return WaterParameter!.DayHot.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.DayHot = Convert.ToDouble(value);
                else
                    WaterParameter!.DayHot = 0;
                WaterDay = Convert.ToDouble(WaterParameter!.DayCold + WaterParameter!.DayHot);
                OnPropertyChanged("DayHot");
            }
        }
        //Сумма расхода в день
        public double? WaterDay
        {
            get
            {
                return Math.Round(Convert.ToDouble(WaterParameter!.DayCold + WaterParameter!.DayHot), 2);
            }
            set
            {
                WaterYear = WaterDay * 365;
                OnPropertyChanged("WaterDay");
            }
        }
        #endregion

        //Расход во время пожаротушения
        public string? FireFighting
        {
            get
            {
                if (WaterParameter!.FireFighting == null)
                    return string.Empty;
                else
                    return WaterParameter!.FireFighting.ToString();
            }
            set
            {
                if (value != string.Empty)
                    WaterParameter!.FireFighting = Convert.ToDouble(value);
                else
                    WaterParameter!.FireFighting = 0;
                OnPropertyChanged("FireFighting");
            }
        }

        //Сумма расхода в год
        public double? WaterYear
        {
            get
            {
                return Math.Round(Convert.ToDouble(WaterDay * 365), 2);
            }
            set
            {
                OnPropertyChanged("WaterYear");
            }
        }

        #endregion

        #region Свойства для данных по электроснабжению
        //Электроснабжение магазинов
        public string? ShopsAreaElectricity
        {
            get
            {
                return ElectricityParameter!.ShopsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    ElectricityParameter!.ShopsArea = Convert.ToDouble(value);
                else
                    ElectricityParameter!.ShopsArea = 0;
                AllElectricity = Convert.ToDouble(ElectricityParameter!.ShopsArea + ElectricityParameter!.OfficesArea +
                    ElectricityParameter!.ApartmentsArea + ElectricityParameter!.OtherArea);
                OnPropertyChanged("ShopsAreaElectricity");
            }
        }

        //Электроснабжение офисов
        public string? OfficesAreaElectricity
        {
            get
            {
                return ElectricityParameter!.OfficesArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    ElectricityParameter!.OfficesArea = Convert.ToDouble(value);
                else
                    ElectricityParameter!.OfficesArea = 0;
                AllElectricity = Convert.ToDouble(ElectricityParameter!.ShopsArea + ElectricityParameter!.OfficesArea +
                    ElectricityParameter!.ApartmentsArea + ElectricityParameter!.OtherArea);
                OnPropertyChanged("OfficesAreaElectricity");
            }
        }

        //Электроснабжение квартир
        public string? ApartmentsAreaElectricity
        {
            get
            {
                return ElectricityParameter!.ApartmentsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    ElectricityParameter!.ApartmentsArea = Convert.ToDouble(value);
                else
                    ElectricityParameter!.ApartmentsArea = 0;
                AllElectricity = Convert.ToDouble(ElectricityParameter!.ShopsArea + ElectricityParameter!.OfficesArea +
                    ElectricityParameter!.ApartmentsArea + ElectricityParameter!.OtherArea);
                OnPropertyChanged("ApartmentsAreaElectricity");
            }
        }

        //Электроснабжение прочих помещений
        public string? OtherAreaElectricity
        {
            get
            {
                return ElectricityParameter!.OtherArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    ElectricityParameter!.OtherArea = Convert.ToDouble(value);
                else
                    ElectricityParameter!.OtherArea = 0;
                AllElectricity = Convert.ToDouble(ElectricityParameter!.ShopsArea + ElectricityParameter!.OfficesArea +
                    ElectricityParameter!.ApartmentsArea + ElectricityParameter!.OtherArea);
                OnPropertyChanged("OtherAreaElectricity");
            }
        }

        //Суммарное теплоснабжение
        public double? AllElectricity
        {
            get
            {
                return Math.Round(Convert.ToDouble(ElectricityParameter!.ShopsArea + ElectricityParameter!.OfficesArea +
                    ElectricityParameter!.ApartmentsArea + ElectricityParameter!.OtherArea), 2);
            }
            set
            {
                OnPropertyChanged("AllElectricity");
            }
        }
        #endregion

        #region Свойства для данных по теплоснабжению
        //Теплоснабжение магазинов
        public string? ShopsAreaHeat
        {
            get
            {
                return HeatParameter!.ShopsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    HeatParameter!.ShopsArea = Convert.ToDouble(value);
                else
                    HeatParameter!.ShopsArea = 0;
                AllHeat = Convert.ToDouble(HeatParameter!.ShopsArea + HeatParameter!.OfficesArea +
                    HeatParameter!.ApartmentsArea + HeatParameter!.OtherArea);
                OnPropertyChanged("ShopsAreaHeat");
            }
        }

        //Теплоснабжение офисов
        public string? OfficesAreaHeat
        {
            get
            {
                return HeatParameter!.OfficesArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    HeatParameter!.OfficesArea = Convert.ToDouble(value);
                else
                    HeatParameter!.OfficesArea = 0;
                AllHeat = Convert.ToDouble(HeatParameter!.ShopsArea + HeatParameter!.OfficesArea +
                    HeatParameter!.ApartmentsArea + HeatParameter!.OtherArea);
                OnPropertyChanged("OfficesAreaHeat");
            }
        }

        //Теплоснабжение квартир
        public string? ApartmentsAreaHeat
        {
            get
            {
                return HeatParameter!.ApartmentsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    HeatParameter!.ApartmentsArea = Convert.ToDouble(value);
                else
                    HeatParameter!.ApartmentsArea = 0;
                AllHeat = Convert.ToDouble(HeatParameter!.ShopsArea + HeatParameter!.OfficesArea +
                    HeatParameter!.ApartmentsArea + HeatParameter!.OtherArea);
                OnPropertyChanged("ApartmentsAreaHeat");
            }
        }

        //Теплоснабжение прочих помещений
        public string? OtherAreaHeat
        {
            get
            {
                return HeatParameter!.OtherArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    HeatParameter!.OtherArea = Convert.ToDouble(value);
                else
                    HeatParameter!.OtherArea = 0;
                AllHeat = Convert.ToDouble(HeatParameter!.ShopsArea + HeatParameter!.OfficesArea +
                    HeatParameter!.ApartmentsArea + HeatParameter!.OtherArea);
                OnPropertyChanged("OtherAreaHeat");
            }
        }

        //Суммарное теплоснабжение
        public double? AllHeat
        {
            get
            {
                return Math.Round(Convert.ToDouble(HeatParameter!.ShopsArea + HeatParameter!.OfficesArea +
                    HeatParameter!.ApartmentsArea + HeatParameter!.OtherArea), 2);
            }
            set
            {
                OnPropertyChanged("AllHeat");
            }
        }
        #endregion

        //Редактирование данных раздела "инженерные системы"
        public RelayCommand? EditCommandEngineeringSystems
        {
            get
            {
                return editCommandEngineeringSystems ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу водоснабжение
                    if (WaterParameter!.Id != 0)
                        db.Entry(WaterParameter).State = EntityState.Modified;
                    else
                        db.WaterParameters.Add(WaterParameter);

                    //Сохранение данных в таблицу электроснабжение
                    if (ElectricityParameter!.Id != 0)
                        db.Entry(ElectricityParameter).State = EntityState.Modified;
                    else
                        db.ElectricityParameters.Add(ElectricityParameter);

                    //Сохранение данных в таблицу теплоснабжение
                    if (HeatParameter!.Id != 0)
                        db.Entry(HeatParameter).State = EntityState.Modified;
                    else
                        db.HeatParameters.Add(HeatParameter);

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные раздела инженерные системы были успешно сохранены(обновлены)";
                    dispatcherTimer.Start();

                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //Очистка статуса
        void ClearStatus(object sender, EventArgs e)
        {
            MainWindow.myChangeStatus!.Text = String.Empty;
        }
    }
}
