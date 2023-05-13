using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model;
using SmaltaApp.Model.DataBase;
using SmaltaApp.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SmaltaApp.ViewModel
{
    public class LandAndFenceVM: INotifyPropertyChanged
    {

        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? editCommandLandAndFence;
        private RelayCommand? editCommandFireResistance;

        //Конструктор
        public LandAndFence? LandAndFence;
        public FireResistance? FireResistance;
        public LandAndFenceVM(int projectID)
        {
            db.Database.OpenConnection();

            LandAndFence = db.LandAndFences.FirstOrDefault(x => x.ProjectId == projectID);
            LandAndFence ??= new() { ProjectId = projectID};

            FireResistance = db.FireResistances.FirstOrDefault(x => x.ProjectId == projectID);
            FireResistance ??= new() { ProjectId = projectID };

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);

            Charecteristic? ch = db.Charecteristics.FirstOrDefault(x => x.ProjectId == projectID);
            if (ch != null) BuiltUpArea = ch.BuiltUpArea.ToString(); 

        }

        #region Свойства для данных подраздела "Показатели земельного участка"
        //Площадь участка
        public string? LandArea
        {
            get
            {
                if (LandAndFence!.LandArea == null)
                    return string.Empty;
                else
                    return LandAndFence!.LandArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    LandAndFence!.LandArea = Convert.ToDouble(value);
                else
                    LandAndFence!.LandArea = null;
                OnPropertyChanged("LandArea");
            }
        }

        //Площадь застройки
        double? builtUpArea;
        public string? BuiltUpArea
        {
            get
            {
                if (builtUpArea == null)
                    return string.Empty;
                else
                    return builtUpArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    builtUpArea = Convert.ToDouble(value);
                else
                    builtUpArea = null;
                OnPropertyChanged("BuiltUpArea");
            }
        }

        //Площадь асфальта
        public string? AsphaltArea
        {
            get
            {
                if (LandAndFence!.AsphaltArea == null)
                    return string.Empty;
                else
                    return LandAndFence!.AsphaltArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    LandAndFence!.AsphaltArea = Convert.ToDouble(value);
                else
                    LandAndFence!.AsphaltArea = null;
                OnPropertyChanged("AsphaltArea");
            }
        }

        //Площадь озеленения
        public string? GreenArea
        {
            get
            {
                if (LandAndFence!.GreenArea == null)
                    return string.Empty;
                else
                    return LandAndFence!.GreenArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    LandAndFence!.GreenArea = Convert.ToDouble(value);
                else
                    LandAndFence!.GreenArea = null;
                OnPropertyChanged("GreenArea");
            }
        }
        #endregion

        #region Свойства для данных подраздела "Показатели эффективности ограждающих конструкций"
        //Сопротивление теплопередачи внешних стен
        public string? WallsST
        {
            get
            {
                if (LandAndFence!.WallsST == null)
                    return string.Empty;
                else
                    return LandAndFence!.WallsST.ToString();
            }
            set
            {
                if (value != string.Empty)
                    LandAndFence!.WallsST = Convert.ToDouble(value);
                else
                    LandAndFence!.WallsST = null;
                OnPropertyChanged("WallsST");
            }
        }

        //Сопротивление теплопередачи покрытия
        public string? CoatingsST
        {
            get
            {
                if (LandAndFence!.CoatingsST == null)
                    return string.Empty;
                else
                    return LandAndFence!.CoatingsST.ToString();
            }
            set
            {
                if (value != string.Empty)
                    LandAndFence!.CoatingsST = Convert.ToDouble(value);
                else
                    LandAndFence!.CoatingsST = null;
                OnPropertyChanged("CoatingsST");
            }
        }

        //Сопротивление теплопередачи окон
        public string? WindowsST
        {
            get
            {
                if (LandAndFence!.WindowsST == null)
                    return string.Empty;
                else
                    return LandAndFence!.WindowsST.ToString();
            }
            set
            {
                if (value != string.Empty)
                    LandAndFence!.WindowsST = Convert.ToDouble(value);
                else
                    LandAndFence!.WindowsST = null;
                OnPropertyChanged("WindowsST");
            }
        }
        #endregion

        #region Свойства для данных подраздела "Показатели огнестойкости несущих конструкций"
        //Огнестойкость cечения_600
        public string? Section600
        {
            get
            {
                if (FireResistance!.Section600 == null)
                    return string.Empty;
                else
                    return FireResistance!.Section600.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Section600 = Convert.ToInt32(value);
                    if (FireResistance!.Section600 < 120) Section600Text = "НЕ соответствует ПБ!";
                    else Section600Text = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Section600 = null;
                    Section600Text = string.Empty;                   
                }
                OnPropertyChanged("Section600");
            }
        }
        public string? Section600Text
        {
            get
            {
               if (FireResistance!.Section600 == null)
                    return string.Empty;
               else if (FireResistance!.Section600 < 120)
                    return "НЕ соответствует ПБ!";
               else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("Section600Text");
            }
        }

        //Огнестойкость cечения_500
        public string? Section500
        {
            get
            {
                if (FireResistance!.Section500 == null)
                    return string.Empty;
                else
                    return FireResistance!.Section500.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Section500 = Convert.ToInt32(value);
                    if (FireResistance!.Section500 < 120) Section500Text = "НЕ соответствует ПБ!";
                    else Section500Text = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Section500 = null;
                    Section500Text = string.Empty;
                }
                OnPropertyChanged("Section500");
            }
        }
        public string? Section500Text
        {
            get
            {
                if (FireResistance!.Section500 == null)
                    return string.Empty;
                else if (FireResistance!.Section500 < 120)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("Section500Text");
            }
        }

        //Огнестойкость cечения_400
        public string? Section400
        {
            get
            {
                if (FireResistance!.Section400 == null)
                    return string.Empty;
                else
                    return FireResistance!.Section400.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Section400 = Convert.ToInt32(value);
                    if (FireResistance!.Section400 < 120) Section400Text = "НЕ соответствует ПБ!";
                    else Section400Text = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Section400 = null;
                    Section400Text = string.Empty;
                }
                OnPropertyChanged("Section400");
            }
        }
        public string? Section400Text
        {
            get
            {
                if (FireResistance!.Section400 == null)
                    return string.Empty;
                else if (FireResistance!.Section400 < 120)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("Section400Text");
            }
        }

        //Огнестойкость балок
        public string? Beams
        {
            get
            {
                if (FireResistance!.Beams == null)
                    return string.Empty;
                else
                    return FireResistance!.Beams.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Beams = Convert.ToInt32(value);
                    if (FireResistance!.Beams < 120) BeamsText = "НЕ соответствует ПБ!";
                    else BeamsText = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Beams = null;
                    BeamsText = string.Empty;
                }
                OnPropertyChanged("Beams");
            }
        }
        public string? BeamsText
        {
            get
            {
                if (FireResistance!.Beams == null)
                    return string.Empty;
                else if (FireResistance!.Beams < 120)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("BeamsText");
            }
        }

        //Огнестойкость стен
        public string? Walls
        {
            get
            {
                if (FireResistance!.Walls == null)
                    return string.Empty;
                else
                    return FireResistance!.Walls.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Walls = Convert.ToInt32(value);
                    if (FireResistance!.Walls < 30) WallsText = "НЕ соответствует ПБ!";
                    else WallsText = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Walls = null;
                    BeamsText = string.Empty;
                }
                OnPropertyChanged("Walls");
            }
        }
        public string? WallsText
        {
            get
            {
                if (FireResistance!.Walls == null)
                    return string.Empty;
                else if (FireResistance!.Walls < 30)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("WallsText");
            }
        }

        //Огнестойкость перекрытия
        public string? Coatings
        {
            get
            {
                if (FireResistance!.Coatings == null)
                    return string.Empty;
                else
                    return FireResistance!.Coatings.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Coatings = Convert.ToInt32(value);
                    if (FireResistance!.Coatings < 60) CoatingsText = "НЕ соответствует ПБ!";
                    else CoatingsText = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Coatings = null;
                    BeamsText = string.Empty;
                }
                OnPropertyChanged("Walls");
            }
        }
        public string? CoatingsText
        {
            get
            {
                if (FireResistance!.Coatings == null)
                    return string.Empty;
                else if (FireResistance!.Coatings < 60)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("CoatingsText");
            }
        }

        //Огнестойкость марш
        public string? FlightsOfStairs
        {
            get
            {
                if (FireResistance!.FlightsOfStairs == null)
                    return string.Empty;
                else
                    return FireResistance!.FlightsOfStairs.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.FlightsOfStairs = Convert.ToInt32(value);
                    if (FireResistance!.FlightsOfStairs < 60) FlightsOfStairsText = "НЕ соответствует ПБ!";
                    else FlightsOfStairsText = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.FlightsOfStairs = null;
                    BeamsText = string.Empty;
                }
                OnPropertyChanged("FlightsOfStairs");
            }
        }
        public string? FlightsOfStairsText
        {
            get
            {
                if (FireResistance!.FlightsOfStairs == null)
                    return string.Empty;
                else if (FireResistance!.FlightsOfStairs < 60)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("FlightsOfStairsText");
            }
        }

        //Огнестойкость перегородок
        public string? Partitions
        {
            get
            {
                if (FireResistance!.Partitions == null)
                    return string.Empty;
                else
                    return FireResistance!.Partitions.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Partitions = Convert.ToInt32(value);
                    if (FireResistance!.Partitions < 120) PartitionsText = "НЕ соответствует ПБ!";
                    else PartitionsText = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Partitions = null;
                    BeamsText = string.Empty;
                }
                OnPropertyChanged("Partitions");
            }
        }
        public string? PartitionsText
        {
            get
            {
                if (FireResistance!.Partitions == null)
                    return string.Empty;
                else if (FireResistance!.Partitions < 120)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("PartitionsText");
            }
        }

        //Огнестойкость связи
        public string? Connections
        {
            get
            {
                if (FireResistance!.Connections == null)
                    return string.Empty;
                else
                    return FireResistance!.Connections.ToString();
            }
            set
            {
                if (value != string.Empty)
                {
                    FireResistance!.Connections = Convert.ToInt32(value);
                    if (FireResistance!.Connections < 15) ConnectionsText = "НЕ соответствует ПБ!";
                    else ConnectionsText = "Cоответствует ПБ!";
                }
                else
                {
                    FireResistance!.Connections = null;
                    BeamsText = string.Empty;
                }
                OnPropertyChanged("Connections");
            }
        }
        public string? ConnectionsText
        {
            get
            {
                if (FireResistance!.Connections == null)
                    return string.Empty;
                else if (FireResistance!.Connections < 15)
                    return "НЕ соответствует ПБ!";
                else
                    return "Cоответствует ПБ!";
            }
            set
            {
                OnPropertyChanged("ConnectionsText");
            }
        }
        #endregion

        #region Команды редактирования данных по показателям земельного учатска, ограждающих конструкций и огнестойкости несущих конструкций
        //Сохранение данных по показателям земельного учатска и ограждающих конструкций
        public RelayCommand? EditCommandLandAndFence
        {
            get
            {
                return editCommandLandAndFence ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу 
                    if (LandAndFence!.Id != 0)
                        db.Entry(LandAndFence).State = EntityState.Modified;
                    else
                        db.LandAndFences.Add(LandAndFence);

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные по показателям земельного участка и эффективности ограждающих конструкций были успешно сохранены(обновлены)";
                    dispatcherTimer.Start();
                });
            }
        }

        //Сохранение данных по показателям огнестойкости несущих конструкций
        public RelayCommand? EditCommandFireResistance
        {
            get
            {
                return editCommandFireResistance ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу 
                    if (FireResistance!.Id != 0)
                        db.Entry(FireResistance).State = EntityState.Modified;
                    else
                        db.FireResistances.Add(FireResistance);

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные по показателям огнестойкости несущих конструкций были успешно сохранены(обновлены)";
                    dispatcherTimer.Start();
                });
            }
        }
        #endregion

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
