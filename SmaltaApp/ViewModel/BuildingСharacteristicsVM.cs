using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model;
using SmaltaApp.Model.AdditionalClasses;
using SmaltaApp.Model.DataBase;
using SmaltaApp.Model.HelperClasses;
using SmaltaApp.View;
using SmaltaApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace SmaltaApp.ViewModel
{
    public class BuildingСharacteristicsVM : INotifyPropertyChanged
    {

        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? editCommandCharecteristic;

        private RelayCommand? addCommandAppointment;
        private RelayCommand? addCommandFloorType;
        private RelayCommand? addCommandFunctionalPurpose;
        private RelayCommand? deleteCommandFunctionalPurpose;
        private RelayCommand? saveCommandFunctionalPurpose;
        private RelayCommand? cancelCommandFunctionalPurpose;


        //Таблицы
        public ObservableCollection<FloorType>? FloorTypes { get; set; }
        public ObservableCollection<Appointment>? Appointments { get; set; }

        //Список функциональных назначений здания
        List<FunctionalPurpose>? functionalPurposes;
        public List<FunctionalPurpose>? FunctionalPurposes
        {
            get { return functionalPurposes; }
            set
            {
                functionalPurposes = value;
                OnPropertyChanged("FunctionalPurposes");
            }
        }

        public FunctionalPurpose? defalutFunctional;

        public Charecteristic? Charecteristic;
        public BuildingVolume? BuildingVolume;
        public BuildingArea? BuildingArea;
        public BuildingHeight? BuildingHeight;
        //Конструктор
        public BuildingСharacteristicsVM(int projectID)
        {
            db.Database.EnsureCreated();
            db.FloorTypes.Load();
            FloorTypes = db.FloorTypes.Local.ToObservableCollection();

            db.Appointments.Load();
            Appointments = db.Appointments.Local.ToObservableCollection();

            Charecteristic = db.Charecteristics.FirstOrDefault(x => x.ProjectId == projectID);
            Charecteristic ??= new() { ProjectId = projectID };

            BuildingVolume = db.BuildingVolumes.FirstOrDefault(x => x.ProjectId == projectID);
            BuildingVolume ??= new() { ProjectId = projectID };

            BuildingArea = db.BuildingAreas.FirstOrDefault(x => x.ProjectId == projectID);
            BuildingArea ??= new() { ProjectId = projectID };

            BuildingHeight = db.BuildingHeights.FirstOrDefault(x => x.ProjectId == projectID);
            BuildingHeight ??= new() { ProjectId = projectID };

            FunctionalPurposes = db.FunctionalPurposes.Where(c => c.ProjectId == projectID).ToList();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);

            defalutFunctional = new() { ProjectId = projectID };
        }

        #region Свойства для интерфейса 
        //Свойство для доступности полей при редактировании
        bool isEnabled = false;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        SolidColorBrush colorGroup = Brushes.Transparent;
        public SolidColorBrush ColorGroup
        {
            get { return colorGroup; }
            set
            {
                colorGroup = value;
                OnPropertyChanged("ColorGroup");

            }
        }
        #endregion

        #region Свойства для данных подраздела "Характеристки здания"
        //Выбранная запись из списка категорий
        public string? SelectedCategory
        {
            get { return Charecteristic!.Category; }
            set
            {
                Charecteristic!.Category = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        //Выбранная запись из списка сроков эксплуатации
        public string? SelectedLifeTime
        {
            get { return Charecteristic!.LifeTime; }
            set
            {
                Charecteristic!.LifeTime = value;
                OnPropertyChanged("SelectedLifeTime");
            }
        }

        //Выбранная запись из списка огнестойкостей
        public string? SelectedFireResistance
        {
            get
            {
                if (Charecteristic!.FireResistance == null)
                    return string.Empty;
                else
                    return Charecteristic!.FireResistance.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Charecteristic!.FireResistance = Convert.ToInt32(value);
                OnPropertyChanged("SelectedFireResistance");
            }
        }

        //Площадь застройки
        public string? BuiltUpArea
        {
            get
            {
                if (Charecteristic!.BuiltUpArea == null)
                    return string.Empty;
                else
                    return Charecteristic!.BuiltUpArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Charecteristic!.BuiltUpArea = Convert.ToInt32(value);
                else
                    Charecteristic!.BuiltUpArea = null;
                OnPropertyChanged("BuiltUpArea");
            }
        }
        #endregion

        #region Свойства для данных подраздела "объемы здания"
        //Объем наземной части
        public string? GroundPart
        {
            get
            {
                return BuildingVolume!.GroundPart.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingVolume!.GroundPart = Convert.ToDouble(value);
                else
                    BuildingVolume!.GroundPart = 0;
                SummVolume = BuildingVolume!.GroundPart + BuildingVolume!.Basement + BuildingVolume!.EntrancePart;
                OnPropertyChanged("GroundPart");
            }
        }

        //Объем цокольной части
        public string? Basement
        {
            get
            {
                return BuildingVolume!.Basement.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingVolume!.Basement = Convert.ToDouble(value);
                else
                    BuildingVolume!.Basement = 0;
                SummVolume = BuildingVolume!.GroundPart + BuildingVolume!.Basement + BuildingVolume!.EntrancePart;
                OnPropertyChanged("Basement");
            }
        }

        //Объем входной части
        public string? EntrancePart
        {
            get
            {
                return BuildingVolume!.EntrancePart.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingVolume!.EntrancePart = Convert.ToDouble(value);
                else
                    BuildingVolume!.EntrancePart = 0;
                SummVolume = BuildingVolume!.GroundPart + BuildingVolume!.Basement + BuildingVolume!.EntrancePart;
                OnPropertyChanged("EntrancePart");
            }
        }
        #endregion

        #region Свойства для данных подраздела "площади здания"
        //Площадь магазинов
        public string? ShopsArea
        {
            get
            {
                return BuildingArea!.ShopsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.ShopsArea = Convert.ToDouble(value);
                else
                    BuildingArea!.ShopsArea = 0;
                SummArea = BuildingArea!.ShopsArea + BuildingArea!.OfficesArea + BuildingArea!.ApartmentsArea + BuildingArea!.OtherArea;
                OnPropertyChanged("ShopsArea");
            }
        }
        //Площадь офисов
        public string? OfficesArea
        {
            get
            {
                return BuildingArea!.OfficesArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.OfficesArea = Convert.ToDouble(value);
                else
                    BuildingArea!.OfficesArea = 0;
                SummArea = BuildingArea!.ShopsArea + BuildingArea!.OfficesArea + BuildingArea!.ApartmentsArea + BuildingArea!.OtherArea;
                OnPropertyChanged("OfficesArea");
            }
        }
        //Площадь квартир
        public string? ApartmentsArea
        {
            get
            {
                return BuildingArea!.ApartmentsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.ApartmentsArea = Convert.ToDouble(value);
                else
                    BuildingArea!.ApartmentsArea = 0;
                SummArea = BuildingArea!.ShopsArea + BuildingArea!.OfficesArea + BuildingArea!.ApartmentsArea + BuildingArea!.OtherArea;
                OnPropertyChanged("ApartmentsArea");
            }
        }
        //Площадь прочих помещений
        public string? OtherArea
        {
            get
            {
                return BuildingArea!.OtherArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.OtherArea = Convert.ToDouble(value);
                else
                    BuildingArea!.OtherArea = 0;
                SummArea = BuildingArea!.ShopsArea + BuildingArea!.OfficesArea + BuildingArea!.ApartmentsArea + BuildingArea!.OtherArea;
                OnPropertyChanged("OtherArea");
            }
        }


        //Полезная площадь магазинов
        public string? EffectiveShopsArea
        {
            get
            {
                return BuildingArea!.EffectiveShopsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.EffectiveShopsArea = Convert.ToDouble(value);
                else
                    BuildingArea!.EffectiveShopsArea = 0;
                SummEffectiveArea = BuildingArea!.EffectiveShopsArea + BuildingArea!.EffectiveOfficesArea + BuildingArea!.EffectiveApartmentsArea + BuildingArea!.EffectiveOtherArea;
                OnPropertyChanged("EffectiveShopsArea");
            }
        }
        //Полезная площадь офисов
        public string? EffectiveOfficesArea
        {
            get
            {
                return BuildingArea!.EffectiveOfficesArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.EffectiveOfficesArea = Convert.ToDouble(value);
                else
                    BuildingArea!.EffectiveOfficesArea = 0;
                SummEffectiveArea = BuildingArea!.EffectiveShopsArea + BuildingArea!.EffectiveOfficesArea + BuildingArea!.EffectiveApartmentsArea + BuildingArea!.EffectiveOtherArea;
                OnPropertyChanged("EffectiveOfficesArea");
            }
        }
        //Полезная площадь квартир
        public string? EffectiveApartmentsArea
        {
            get
            {
                return BuildingArea!.EffectiveApartmentsArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.EffectiveApartmentsArea = Convert.ToDouble(value);
                else
                    BuildingArea!.EffectiveApartmentsArea = 0;
                SummEffectiveArea = BuildingArea!.EffectiveShopsArea + BuildingArea!.EffectiveOfficesArea + BuildingArea!.EffectiveApartmentsArea + BuildingArea!.EffectiveOtherArea;
                OnPropertyChanged("EffectiveApartmentsArea");
            }
        }
        //Полезная площадь прочих помещений
        public string? EffectiveOtherArea
        {
            get
            {
                return BuildingArea!.EffectiveOtherArea.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingArea!.EffectiveOtherArea = Convert.ToDouble(value);
                else
                    BuildingArea!.EffectiveOtherArea = 0;
                SummEffectiveArea = BuildingArea!.EffectiveShopsArea + BuildingArea!.EffectiveOfficesArea + BuildingArea!.EffectiveApartmentsArea + BuildingArea!.EffectiveOtherArea;
                OnPropertyChanged("EffectiveOtherArea");
            }
        }
        #endregion

        #region Свойства для данных подраздела "высота здания"
        //Количество наземных этажей
        public string? CountGroundFloors
        {
            get
            {
                return BuildingHeight!.CountGroundFloors.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.CountGroundFloors = Convert.ToInt32(value);
                else
                    BuildingHeight!.CountGroundFloors = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("CountGroundFloors");
            }
        }

        //Количество цокольных этажей
        public string? CountBasements
        {
            get
            {
                return BuildingHeight!.CountBasements.ToString();
            }
            set
            {

                if (value != string.Empty)
                    BuildingHeight!.CountBasements = Convert.ToInt32(value);
                else
                    BuildingHeight!.CountBasements = 0;
                OnPropertyChanged("CountBasements");
            }
        }

        //Высота первого этажа
        public string? FirstFloor
        {
            get
            {
                return BuildingHeight!.FirstFloor.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.FirstFloor = Convert.ToDouble(value);
                else
                    BuildingHeight!.FirstFloor = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("FirstFloor");
            }
        }

        //Высота второго этажа
        public string? SecondFloor
        {
            get
            {
                return BuildingHeight!.SecondFloor.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.SecondFloor = Convert.ToDouble(value);
                else
                    BuildingHeight!.SecondFloor = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("SecondFloor");
            }
        }

        //Высота типового этажа
        public string? TypicalFloor
        {
            get
            {
                return BuildingHeight!.TypicalFloor.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.TypicalFloor = Convert.ToDouble(value);
                else
                    BuildingHeight!.TypicalFloor = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("TypicalFloor");
            }
        }

        //Высота цокольного этажа
        public string? GroundFloor
        {
            get
            {
                return BuildingHeight!.GroundFloor.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.GroundFloor = Convert.ToDouble(value);
                else
                    BuildingHeight!.GroundFloor = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("GroundFloor");
            }
        }

        //Высота технического этажа
        public string? TechnicalFloor
        {
            get
            {
                return BuildingHeight!.TechnicalFloor.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.TechnicalFloor = Convert.ToDouble(value);
                else
                    BuildingHeight!.TechnicalFloor = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("TechnicalFloor");
            }
        }

        //Высота нулевого этажа
        public string? NullFloor
        {
            get
            {
                return BuildingHeight!.NullFloor.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingHeight!.NullFloor = Convert.ToDouble(value);
                else
                    BuildingHeight!.NullFloor = 0;
                SummHeight = BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor;
                OnPropertyChanged("NullFloor");
            }
        }
        #endregion

        #region Свойства подсчета сумм значений
        //Сумма объемов здания
        public double? SummVolume
        {
            get
            {
                return Math.Round(Convert.ToDouble(BuildingVolume!.GroundPart + BuildingVolume!.Basement + BuildingVolume!.EntrancePart), 2);
            }
            set
            {
                OnPropertyChanged("SummVolume");
            }
        }
        //Сумма общей площади
        public double? SummArea
        {
            get
            {
                return Math.Round(Convert.ToDouble(BuildingArea!.ShopsArea + BuildingArea!.OfficesArea + BuildingArea!.ApartmentsArea + BuildingArea!.OtherArea), 2);
            }
            set
            {
                OnPropertyChanged("SummArea");
            }
        }
        //Сумма полезной площади
        public double? SummEffectiveArea
        {
            get
            {
                return Math.Round(Convert.ToDouble(BuildingArea!.EffectiveShopsArea + BuildingArea!.EffectiveOfficesArea + BuildingArea!.EffectiveApartmentsArea + BuildingArea!.EffectiveOtherArea), 2);
            }
            set
            {
                OnPropertyChanged("SummEffectiveArea");
            }
        }
        //Сумма высот здания
        public double? SummHeight
        {
            get
            {
                return Math.Round(Convert.ToDouble(BuildingHeight!.FirstFloor + BuildingHeight!.SecondFloor + BuildingHeight!.TechnicalFloor +
                    BuildingHeight!.NullFloor + (BuildingHeight!.CountGroundFloors - 2) * BuildingHeight!.TypicalFloor), 2);
            }
            set
            {
                OnPropertyChanged("SummHeight");
            }
        }
        #endregion

        //Редактирование данных раздела "техно-экономические характеристики"
        public RelayCommand? EditCommandCharecteristic
        {
            get
            {
                return editCommandCharecteristic ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу характеристика
                    if (Charecteristic!.Id != 0)
                        db.Entry(Charecteristic).State = EntityState.Modified;
                    else
                        db.Charecteristics.Add(Charecteristic);
                    //db.SaveChanges();

                    //Сохранение данных в таблицу объемы здания
                    if (BuildingVolume!.Id != 0)
                        db.Entry(BuildingVolume).State = EntityState.Modified;
                    else
                        db.BuildingVolumes.Add(BuildingVolume);

                    //Сохранение данных в таблицу площади здания
                    if (BuildingArea!.Id != 0)
                        db.Entry(BuildingArea).State = EntityState.Modified;
                    else

                        db.BuildingAreas.Add(BuildingArea);

                    //Сохранение данных в таблицу высоты здания
                    if (BuildingHeight!.Id != 0)
                        db.Entry(BuildingHeight).State = EntityState.Modified;
                    else
                        db.BuildingHeights.Add(BuildingHeight);


                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные раздела технико-экономичекие характеристики были успешно сохранены(обновлены)";
                    dispatcherTimer.Start();

                });
            }
        }

        #region Команды редактирования данных по функциональным назначениям здания 
        //Выбор записи из списка
        private FunctionalPurpose? selectedFunctionalPurposes;
        public FunctionalPurpose? SelectedFunctionalPurposes
        {
            get { return selectedFunctionalPurposes; }
            set
            {
                selectedFunctionalPurposes = value;
                OnPropertyChanged("SelectedFunctionalPurposes");
            }
        }

        #region Свойства для полей редактирования
        public Appointment? SelectedAppointment
        {
            get { return Appointments!.FirstOrDefault(x => x.Id == defalutFunctional!.AppointmentId); }
            set
            {
                defalutFunctional!.AppointmentId = value!.Id;
                OnPropertyChanged("SelectedAppointment");
            }
        }

        public FloorType? SelectedFloorType
        {
            get { return FloorTypes!.FirstOrDefault(x => x.Id == defalutFunctional!.FloorTypeId); }
            set
            {
                defalutFunctional!.FloorTypeId = value!.Id;
                OnPropertyChanged("SelectedFloorType");
            }
        }
        #endregion

        //Добавление записи
        public RelayCommand? AddCommandFunctionalPurpose
        {
            get
            {
                return addCommandFunctionalPurpose ??= new RelayCommand((obj) =>
                {
                    IsEnabled = true;
                    ColorGroup = Brushes.White;
                });
            }
        }
        //Удаление записи
        public RelayCommand? DeleteCommandFunctionalPurpose
        {
            get
            {
                return deleteCommandFunctionalPurpose ??= new RelayCommand((obj) =>
                {
                    if (SelectedFunctionalPurposes != null)
                    {

                        db.FunctionalPurposes.Remove(SelectedFunctionalPurposes);
                        db.SaveChanges();
                        FunctionalPurposes = db.FunctionalPurposes.Where(c => c.ProjectId == defalutFunctional!.ProjectId).ToList();
                    }
                    else { ShowMess("Удаление невозможно. Запись для удаления не выбрана."); }
                });
            }
        }
        //Сохранить
        public RelayCommand? SaveCommandFunctionalPurpose
        {
            get
            {
                return saveCommandFunctionalPurpose ??= new RelayCommand((obj) =>
                {
                    if (defalutFunctional!.AppointmentId != null && defalutFunctional!.FloorTypeId != null)
                    {

                        bool checkIsExist = db.FunctionalPurposes.Any(el => el.ProjectId == defalutFunctional.ProjectId &&
                                el.AppointmentId == defalutFunctional.AppointmentId && el.FloorTypeId == defalutFunctional.FloorTypeId);
                        if (!checkIsExist)
                        {
                            db.FunctionalPurposes.Add(new()
                            {
                                ProjectId = defalutFunctional.ProjectId,
                                AppointmentId = defalutFunctional.AppointmentId,
                                FloorTypeId = defalutFunctional.FloorTypeId,

                            });
                        }
                        else ShowMess("Добавление невозможно. Данное функциональное назначение уже отражено в текущем списке.");


                        db.SaveChanges();
                        FunctionalPurposes = db.FunctionalPurposes.Where(c => c.ProjectId == defalutFunctional.ProjectId).ToList();

                        IsEnabled = false; ColorGroup = Brushes.Transparent;
                    }
                    else ShowMess("Для сохранения данных необходимо заполнить все поля.");
                });
            }
        }
        //Отменить
        public RelayCommand? CancelCommandFunctionalPurpose
        {
            get
            {
                return cancelCommandFunctionalPurpose ??= new RelayCommand((obj) =>
                {

                    IsEnabled = false; ColorGroup = Brushes.Transparent;

                });
            }
        }
        #endregion

        #region Команды добавления новых данных в таблицы типы этажей, функциональные назначения 
        //Добавлание типа этажа
        public RelayCommand? AddCommandFloorType
        {
            get
            {
                return addCommandFloorType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа этажа"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        FloorType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.FloorTypes.Any(el => el.Name! == type.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.FloorTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        //Добавление функционального назначения
        public RelayCommand? AddCommandAppointment
        {
            get
            {
                return addCommandAppointment ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование назначения этажа"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        Appointment appointment = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.Appointments.Any(el => el.Name! == appointment.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.Appointments.Add(appointment);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //Вывод оповещения
        static void ShowMess(string mess)
        {
            Warning window = new(mess)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.ShowDialog();
        }

        //Очистка статуса
        void ClearStatus(object sender, EventArgs e)
        {
            MainWindow.myChangeStatus!.Text = String.Empty;
        }
    }
}
