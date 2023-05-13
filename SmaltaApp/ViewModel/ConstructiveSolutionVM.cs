using SmaltaApp.Model;
using SmaltaApp.Model.DataBase;
using SmaltaApp.Model.HelperClasses;
using SmaltaApp.View.Windows;
using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using SmaltaApp.Model.AdditionalClasses;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace SmaltaApp.ViewModel
{
    public class ConstructiveSolutionVM : INotifyPropertyChanged
    {

        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? addCommandStabilityType;
        private RelayCommand? addCommandSchemeType;
        private RelayCommand? addCommandConstructiveTypeBuiding;

        private RelayCommand? addCommandFoundationType;
        private RelayCommand? addCommandTechnologyType;

        private RelayCommand? addCommandConstructiveType;
        private RelayCommand? addCommandStairType;
        private RelayCommand? addCommandLiftType;
        private RelayCommand? addCommandHouseTopType;
        private RelayCommand? addCommandRoofType;
        private RelayCommand? addCommandRoofingMaterial;
        private RelayCommand? addCommandAirShaftType;
        private RelayCommand? addCommandExteriorFinish;
        private RelayCommand? addCommandInsulationMaterial;

        private RelayCommand? editCommanBuildingStructure;
        private RelayCommand? editCommanBuildingFoundation;
        private RelayCommand? editCommanBuildingSсheme;

        //Таблицы
        public ObservableCollection<ConstructiveType>? ConstructiveTypes { get; set; }
        public ObservableCollection<SchemeType>? SchemeTypes { get; set; }
        public ObservableCollection<StabilityType>? StabilityTypes { get; set; }

        public ObservableCollection<FoundationType>? FoundationTypes { get; set; }
        public ObservableCollection<TechnologyType>? TechnologyTypes { get; set; }

        public ObservableCollection<ConstructionType>? ConstructionTypes { get; set; }
        public ObservableCollection<StairType>? StairTypes { get; set; }
        public ObservableCollection<LiftType>? LiftTypes { get; set; }
        public ObservableCollection<HouseTopType>? HouseTopTypes { get; set; }
        public ObservableCollection<RoofType>? RoofTypes { get; set; }
        public ObservableCollection<RoofingMaterial>? RoofingMaterials { get; set; }
        public ObservableCollection<AirShaftType>? AirShaftTypes { get; set; }
        public ObservableCollection<ExteriorFinish>? ExteriorFinishes { get; set; }
        public ObservableCollection<InsulationMaterial>? InsulationMaterials { get; set; }

        public BuildingScheme? BuildingScheme;
        public BuildingFoundation? BuildingFoundation;
        public BuildingStructure? BuildingStructure;
        //Конструктор
        public ConstructiveSolutionVM(int projectId)
        {

            db.Database.EnsureCreated();

            db.FoundationTypes.Load();
            FoundationTypes = db.FoundationTypes.Local.ToObservableCollection();

            db.TechnologyTypes.Load();
            TechnologyTypes = db.TechnologyTypes.Local.ToObservableCollection();


            db.ConstructionTypes.Load();
            ConstructionTypes = db.ConstructionTypes.Local.ToObservableCollection();

            db.ConstructiveTypes.Load();
            ConstructiveTypes = db.ConstructiveTypes.Local.ToObservableCollection();

            db.SchemeTypes.Load();
            SchemeTypes = db.SchemeTypes.Local.ToObservableCollection();

            db.StabilityTypes.Load();
            StabilityTypes = db.StabilityTypes.Local.ToObservableCollection();

            db.StairTypes.Load();
            StairTypes = db.StairTypes.Local.ToObservableCollection();

            db.LiftTypes.Load();
            LiftTypes = db.LiftTypes.Local.ToObservableCollection();

            db.HouseTopTypes.Load();
            HouseTopTypes = db.HouseTopTypes.Local.ToObservableCollection();

            db.RoofTypes.Load();
            RoofTypes = db.RoofTypes.Local.ToObservableCollection();

            db.RoofingMaterials.Load();
            RoofingMaterials = db.RoofingMaterials.Local.ToObservableCollection();

            db.AirShaftTypes.Load();
            AirShaftTypes = db.AirShaftTypes.Local.ToObservableCollection();

            db.ExteriorFinishes.Load();
            ExteriorFinishes = db.ExteriorFinishes.Local.ToObservableCollection();

            db.InsulationMaterials.Load();
            InsulationMaterials = db.InsulationMaterials.Local.ToObservableCollection();

            //Данные из другой таблицы для 1 части
            BuildingHeight? bh = db.BuildingHeights.FirstOrDefault(x => x.ProjectId == projectId);
            if (bh != null)
            {
                CountBasements = bh.CountBasements;
                CountGroundFloors = bh.CountGroundFloors;
                GroundFloor = bh.GroundFloor;
                BuildingHeight = bh.FirstFloor + bh.SecondFloor + bh.TechnicalFloor + bh.NullFloor + (bh.CountGroundFloors - 2) * bh.TypicalFloor;
            }
            //Таблица для 1 части
            BuildingScheme = db.BuildingSchemes.FirstOrDefault(x => x.ProjectId == projectId);
            BuildingScheme ??= new() { ProjectId = projectId };

            //Таблица для 2 части
            BuildingFoundation = db.BuildingFoundations.FirstOrDefault(x => x.ProjectId == projectId);
            BuildingFoundation ??= new() { ProjectId = projectId };

            //Таблица для 3 части
            BuildingStructure = db.BuildingStructures.FirstOrDefault(x => x.ProjectId == projectId);
            BuildingStructure ??= new() { ProjectId = projectId };

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
        }


        #region Часть 1. Характеристики конструктивной схемы здания (сооружения)

        #region Свойства для полей 1 части

        #region Подраздел объемно-планировочные решения
        //Форма здания
        public string? SelectedForm
        {
            get { return BuildingScheme!.Form; }
            set
            {
                BuildingScheme!.Form = value;
                OnPropertyChanged("SelectedForm");
            }
        }
        //Сложность формы
        public string? SelectedComplexity
        {
            get { return BuildingScheme!.Complexity; }
            set
            {
                BuildingScheme!.Complexity = value;
                OnPropertyChanged("SelectedComplexity");
            }
        }
        //Длина здания
        public string? Length
        {
            get
            {
                if (BuildingScheme!.Length == null)
                    return string.Empty;
                else
                    return BuildingScheme!.Length.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingScheme!.Length = Convert.ToDouble(value);
                else
                    BuildingScheme!.Length = null;
                OnPropertyChanged("Length");
            }
        }
        //Ширина здания
        public string? Width
        {
            get
            {
                if (BuildingScheme!.Width == null)
                    return string.Empty;
                else
                    return BuildingScheme!.Width.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingScheme!.Width = Convert.ToDouble(value);
                else
                    BuildingScheme!.Width = null;
                OnPropertyChanged("Width");
            }
        }
        //Конструктивный тип здания
        public ConstructiveType? SelectedConstructiveType
        {
            get { return ConstructiveTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.ConstructiveTypeId); }
            set
            {
                BuildingScheme!.ConstructiveTypeId = value!.Id;
                OnPropertyChanged("SelectedConstructiveType");
            }
        }
        #endregion

        #region Подраздел конструктивная схема здания
        //Поперечная схема
        public SchemeType? SelectedLateralScheme
        {
            get { return SchemeTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.LateralSchemeId); }
            set
            {
                BuildingScheme!.LateralSchemeId = value!.Id;
                OnPropertyChanged("SelectedLateralScheme");
            }
        }
        //Количество поперечных пролетов
        public string? CountLateralSpan
        {
            get
            {
                if (BuildingScheme!.CountLateralSpan == null)
                    return string.Empty;
                else
                    return BuildingScheme!.CountLateralSpan.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingScheme!.CountLateralSpan = Convert.ToInt32(value);
                else
                    BuildingScheme!.CountLateralSpan = null;
                OnPropertyChanged("CountLateralSpan");
            }
        }
        //Шаги
        public string LateralSteps
        {
            get { return BuildingScheme!.LateralSteps!; }
            set
            {
                BuildingScheme!.LateralSteps = value;
                OnPropertyChanged("LateralSteps");
            }
        }

        //Продольная схема
        public SchemeType? SelectedLongitudinalScheme
        {
            get { return SchemeTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.LongitudinalStabilityId); }
            set
            {
                BuildingScheme!.LongitudinalStabilityId = value!.Id;
                OnPropertyChanged("SelectedLongitudinalScheme");
            }
        }
        //Количество продольных пролетов
        public string? CountLongitudinalSpan
        {
            get
            {
                if (BuildingScheme!.CountLongitudinalSpan == null)
                    return string.Empty;
                else
                    return BuildingScheme!.CountLongitudinalSpan.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingScheme!.CountLongitudinalSpan = Convert.ToInt32(value);
                else
                    BuildingScheme!.CountLongitudinalSpan = null;
                OnPropertyChanged("CountLongitudinalSpan");
            }
        }
        //Шаги
        public string LongitudinalSteps
        {
            get { return BuildingScheme!.LongitudinalSteps!; }
            set
            {
                BuildingScheme!.LongitudinalSteps = value;
                OnPropertyChanged("LongitudinalSteps");
            }
        }
        #endregion

        #region Подраздел пространсвенная устойчивость и неизменяемость
        //Вертикальная устойчивость
        public StabilityType? SelectedVerticalStability
        {
            get { return StabilityTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.VerticalStabilityId); }
            set
            {
                BuildingScheme!.VerticalStabilityId = value!.Id;
                OnPropertyChanged("SelectedVerticalStability");
            }
        }

        //Горизонтальная устойчивость
        public StabilityType? SelectedHorizontalStability
        {
            get { return StabilityTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.HorizontalStabilityId); }
            set
            {
                BuildingScheme!.HorizontalStabilityId = value!.Id;
                OnPropertyChanged("SelectedHorizontalStability");
            }
        }

        //Поперечная устойчивость
        public StabilityType? SelectedLateralStability
        {
            get { return StabilityTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.LateralStabilityId); }
            set
            {
                BuildingScheme!.LateralStabilityId = value!.Id;
                OnPropertyChanged("SelectedLateralStability");
            }
        }

        //Продольная устойчивость
        public StabilityType? SelectedLongitudinalStability
        {
            get { return StabilityTypes!.FirstOrDefault(x => x.Id == BuildingScheme!.LongitudinalStabilityId); }
            set
            {
                BuildingScheme!.LongitudinalStabilityId = value!.Id;
                OnPropertyChanged("SelectedLongitudinalStability");
            }
        }
        #endregion

        #region Подраздел высота здания
        //Количество наземных этажей
        int? countGroundFloors;
        public int? CountGroundFloors
        {
            get
            {
                if (countGroundFloors == null)
                    return null;
                else
                    return countGroundFloors;
            }
            set
            {
                if (value != null)
                    countGroundFloors = value;
                else
                    countGroundFloors = null;
                OnPropertyChanged("CountGroundFloors");
            }
        }

        //Количество цокольных этажей
        int? countBasements;
        public int? CountBasements
        {
            get
            {
                if (countBasements == null)
                    return null;
                else
                    return countBasements;
            }
            set
            {
                if (value != null)
                    countBasements = value;
                else
                    countBasements = null;
                OnPropertyChanged("CountBasements");
            }
        }

        //Высота наземной части
        double? buildingHeight;
        public double? BuildingHeight
        {
            get
            {
                if (buildingHeight == null)
                    return null;
                else
                    return buildingHeight;
            }
            set
            {
                if (value != null)
                    buildingHeight = value;
                else
                    buildingHeight = null;
                OnPropertyChanged("BuildingHeight");
            }
        }

        //Высота цокольного этажа
        double? groundFloor;
        public double? GroundFloor
        {
            get
            {
                if (groundFloor == null)
                    return null;
                else
                    return groundFloor;
            }
            set
            {
                if (value != null)
                    groundFloor = value;
                else
                    groundFloor = null;
                OnPropertyChanged("GroundFloor");
            }
        }
        #endregion

        #endregion

        #region Команды добавления новых данных в comboBox'ы
        //Добавлениетипа конструктивного типа здания
        public RelayCommand? AddCommandConstructiveTypeBuiding
        {
            get
            {
                return addCommandConstructiveTypeBuiding ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование конструктивного типа здания"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        ConstructiveType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.ConstructiveTypes.Any(el => el.Name! == type.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.ConstructiveTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        //Добавление конструктивной схемы здания
        public RelayCommand? AddCommandSchemeType
        {
            get
            {
                return addCommandSchemeType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование конструктивной схемы"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        SchemeType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.SchemeTypes.Any(el => el.Name! == type.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.SchemeTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        //Добавление  пространственной устойчивости 
        public RelayCommand? AddCommandStabilityType
        {
            get
            {
                return addCommandStabilityType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование конструкции, обес-щей устойчивость"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        StabilityType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.StabilityTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.StabilityTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        #endregion

        //Сохранение данных 1 части
        public RelayCommand? EditCommanBuildingSсheme
        {
            get
            {
                return editCommanBuildingSсheme ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу характеристика
                    if (BuildingScheme!.Id != 0)
                        db.Entry(BuildingScheme).State = EntityState.Modified;
                    else
                        db.BuildingSchemes.Add(BuildingScheme);

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные подраздела характеристика конструктивной схемы здания (сооружения) были успешно сохранены(обновлены)";
                    dispatcherTimer.Start();

                });
            }
        }
        #endregion

        #region Часть 2. Конструкция здания (сооружения) ниже отметки 0.00

        #region Свойства для полей 2 части

        #region Подраздел фундамент несущих конструкций
        //Тип фундамента
        public FoundationType? SelectedFoundationType
        {
            get { return FoundationTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.FoundationTypeId); }
            set
            {
                BuildingFoundation!.FoundationTypeId = value!.Id;
                OnPropertyChanged("SelectedFoundationType");
            }
        }
        //Технология СМР
        public TechnologyType? SelectedTechnologyType
        {
            get { return TechnologyTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.TechnologyTypeId); }
            set
            {
                BuildingFoundation!.TechnologyTypeId = value!.Id;
                OnPropertyChanged("SelectedTechnologyType");
            }
        }

        //Плита 
        public ConstructionType? SelectedSlab
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.Slab); }
            set
            {
                BuildingFoundation!.Slab = value!.Id;
                OnPropertyChanged("SelectedSlab");
            }
        }
        //Толщина плиты
        public string? ThicknessSlab
        {
            get
            {
                if (BuildingFoundation!.ThicknessSlab == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ThicknessSlab.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ThicknessSlab = Convert.ToDouble(value);
                else
                    BuildingFoundation!.ThicknessSlab = null;
                OnPropertyChanged("ThicknessSlab");
            }
        }

        //Плита с надбалкой
        public ConstructionType? SelectedSlabTwo
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.SlabTwo); }
            set
            {
                BuildingFoundation!.SlabTwo = value!.Id;
                OnPropertyChanged("SelectedSlabTwo");
            }
        }
        //Толщина плиты с надбалкой
        public string? ThicknessSlabTwo
        {
            get
            {
                if (BuildingFoundation!.ThicknessSlabTwo == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ThicknessSlabTwo.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ThicknessSlabTwo = Convert.ToDouble(value);
                else
                    BuildingFoundation!.ThicknessSlabTwo = null;
                OnPropertyChanged("ThicknessSlabTwo");
            }
        }
        #endregion

        #region Подраздел вертикальные конструкции ниже 0.00
        //Подколонники
        public ConstructionType? SelectedUnderСolumns
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.UnderСolumns); }
            set
            {
                BuildingFoundation!.UnderСolumns = value!.Id;
                OnPropertyChanged("SelectedUnderСolumns");
            }
        }
        //Размерность подколонников
        public string? DimensionUnderСolumns
        {
            get { return BuildingFoundation!.DimensionUnderСolumns!; }
            set
            {
                BuildingFoundation!.DimensionUnderСolumns = value;
                OnPropertyChanged("DimensionUnderСolumns");
            }
        }

        //Колонны
        public ConstructionType? SelectedСolumnsF
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.Сolumns); }
            set
            {
                BuildingFoundation!.Сolumns = value!.Id;
                OnPropertyChanged("SelectedСolumnsF");
            }
        }
        //Размерность колонн
        public string? DimensionСolumns
        {
            get { return BuildingFoundation!.DimensionСolumns!; }
            set
            {
                BuildingFoundation!.DimensionСolumns = value;
                OnPropertyChanged("DimensionСolumns");
            }
        }

        //Стены цоколя
        public ConstructionType? SelectedWallsF
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.Walls); }
            set
            {
                BuildingFoundation!.Walls = value!.Id;
                OnPropertyChanged("SelectedWallsF");
            }
        }
        //Толщина стен цоколя
        public string? ThicknessWallsF
        {
            get
            {
                if (BuildingFoundation!.ThicknessWalls == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ThicknessWalls.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ThicknessWalls = Convert.ToDouble(value);
                else
                    BuildingFoundation!.ThicknessWalls = null;
                OnPropertyChanged("ThicknessWallsF");
            }
        }
        #endregion

        #region Подраздел перекрытия на отметке 0.00
        //Перекрытия
        public ConstructionType? SelectedFloorF
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.Floors); }
            set
            {
                BuildingFoundation!.Floors = value!.Id;
                OnPropertyChanged("SelectedFloorF");
            }
        }
        //Толщина перекрытия
        public string? ThicknessFloorF
        {
            get
            {
                if (BuildingFoundation!.ThicknessFloors == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ThicknessFloors.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ThicknessFloors = Convert.ToDouble(value);
                else
                    BuildingFoundation!.ThicknessFloors = null;
                OnPropertyChanged("ThicknessFloorF");
            }
        }

        //Ригели
        public ConstructionType? SelectedСrossbarsF
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingFoundation!.Сrossbars); }
            set
            {
                BuildingFoundation!.Сrossbars = value!.Id;
                OnPropertyChanged("SelectedСrossbarsF");
            }
        }
        //Толщина ригели
        public string? ThicknessCrossbarsF
        {
            get
            {
                if (BuildingFoundation!.ThicknessСrossbars == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ThicknessСrossbars.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ThicknessСrossbars = Convert.ToDouble(value);
                else
                    BuildingFoundation!.ThicknessСrossbars = null;
                OnPropertyChanged("ThicknessСrossbars");
            }
        }
        #endregion

        #region Подраздел материалы ниже отметки 0.00
        //Бетон B
        public string? ConcreteBbb
        {
            get
            {
                if (BuildingFoundation!.ConcreteB == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ConcreteB.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ConcreteB = Convert.ToInt32(value);
                else
                    BuildingFoundation!.ConcreteB = null;
                OnPropertyChanged("ConcreteBbb");
            }
        }
        //Бетон F
        public string? ConcreteFff
        {
            get
            {
                if (BuildingFoundation!.ConcreteF == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ConcreteF.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ConcreteF = Convert.ToInt32(value);
                else
                    BuildingFoundation!.ConcreteF = null;
                OnPropertyChanged("ConcreteFff");
            }
        }
        //Бетон W
        public string? ConcreteWww
        {
            get
            {
                if (BuildingFoundation!.ConcreteW == null)
                    return string.Empty;
                else
                    return BuildingFoundation!.ConcreteW.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingFoundation!.ConcreteW = Convert.ToInt32(value);
                else
                    BuildingFoundation!.ConcreteW = null;
                OnPropertyChanged("ConcreteWww");
            }
        }
        //Марка стали
        public string SteelGradeF
        {
            get { return BuildingFoundation!.SteelGrade!; }
            set
            {
                BuildingFoundation!.SteelGrade = value;
                OnPropertyChanged("SteelGradeF");
            }
        }
        #endregion

        #endregion

        #region Команды добавления новых данных в comboBox'ы
        //Добавление типа фундамента
        public RelayCommand? AddCommandFoundationType
        {
            get
            {
                return addCommandFoundationType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа фундамента"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        FoundationType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.FoundationTypes.Any(el => el.Name! == type.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.FoundationTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление типа технологии смр
        public RelayCommand? AddCommandTechnologyType
        {
            get
            {
                return addCommandTechnologyType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа технологии СМР"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        TechnologyType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.TechnologyTypes.Any(el => el.Name! == type.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.TechnologyTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        #endregion

        //Сохранение данных 3 части
        public RelayCommand? EditCommanBuildingStructure
        {
            get
            {
                return editCommanBuildingStructure ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу характеристика
                    if (BuildingStructure!.Id != 0)
                        db.Entry(BuildingStructure).State = EntityState.Modified;
                    else
                        db.BuildingStructures.Add(BuildingStructure);

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные подраздела конструкции здания (сооружения) выше отметки 0.00 были успешно сохранены(обновлены)";
                    dispatcherTimer.Start();

                });
            }
        }
        #endregion

        #region Часть 3. Конструкция здания (сооружения) выше отметки 0.00

        #region Свойства для полей 3 части 

        #region Подраздел несущие конструкции
        //Несущие конструкции
        public ConstructionType? SelectedBearingStructures
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.BearingStructuresId); }
            set
            {
                BuildingStructure!.BearingStructuresId = value!.Id;
                OnPropertyChanged("SelectedBearingStructures");
            }
        }
        //Колонны
        public ConstructionType? SelectedColumn
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.ColumnsId); }
            set
            {
                BuildingStructure!.ColumnsId = value!.Id;
                OnPropertyChanged("SelectedColumn");
            }
        }
        //Перекрытия
        public ConstructionType? SelectedFloor
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.FloorsId); }
            set
            {
                BuildingStructure!.FloorsId = value!.Id;
                OnPropertyChanged("SelectedFloor");
            }
        }
        //Ригели
        public ConstructionType? SelectedСrossbars
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.СrossbarsId); }
            set
            {
                BuildingStructure!.СrossbarsId = value!.Id;
                OnPropertyChanged("SelectedСrossbars");
            }
        }
        //Стены
        public ConstructionType? SelectedWalls
        {
            get { return ConstructionTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.WallsId); }
            set
            {
                BuildingStructure!.WallsId = value!.Id;
                OnPropertyChanged("SelectedWalls");
            }
        }
        //Бетон B
        public string? ConcreteB
        {
            get
            {
                if (BuildingStructure!.ConcreteB == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ConcreteB.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ConcreteB = Convert.ToInt32(value);
                else
                    BuildingStructure!.ConcreteB = null;
                OnPropertyChanged("ConcreteB");
            }
        }
        //Бетон F
        public string? ConcreteF
        {
            get
            {
                if (BuildingStructure!.ConcreteF == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ConcreteF.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ConcreteF = Convert.ToInt32(value);
                else
                    BuildingStructure!.ConcreteF = null;
                OnPropertyChanged("ConcreteF");
            }
        }
        //Бетон W
        public string? ConcreteW
        {
            get
            {
                if (BuildingStructure!.ConcreteW == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ConcreteW.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ConcreteW = Convert.ToInt32(value);
                else
                    BuildingStructure!.ConcreteW = null;
                OnPropertyChanged("ConcreteW");
            }
        }
        //Марка стали
        public string SteelGrade
        {
            get { return BuildingStructure!.SteelGrade!; }
            set
            {
                BuildingStructure!.SteelGrade = value;
                OnPropertyChanged("SteelGrade");
            }
        }

        #endregion

        #region Подраздел лестницы и шахты лифта
        //Конструкция лестниц
        public StairType? SelectedStairType
        {
            get { return StairTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.StairId); }
            set
            {
                BuildingStructure!.StairId = value!.Id;
                OnPropertyChanged("SelectedStairType");
            }
        }
        //Косоуры
        public string? Beam
        {
            get
            {
                if (BuildingStructure!.Beam == null)
                    BuildingStructure!.Beam = "Из металлопроката";

                return BuildingStructure!.Beam;
            }
            set
            {
                BuildingStructure!.Beam = value;
                OnPropertyChanged("Beam");
            }
        }
        //Конструкция шахты лифта
        public LiftType? SelectedLiftType
        {
            get { return LiftTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.LiftId); }
            set
            {
                BuildingStructure!.LiftId = value!.Id;
                OnPropertyChanged("SelectedLiftType");
            }
        }
        //Толщина шахты лифта
        public string? ThicknessLift
        {
            get
            {
                if (BuildingStructure!.ThicknessLift == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ThicknessLift.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ThicknessLift = Convert.ToDouble(value);
                else
                    BuildingStructure!.ThicknessLift = null;
                OnPropertyChanged("ThicknessLift");
            }
        }
        #endregion

        #region Подраздел крыша/парапет/кровля
        //Конструкция крыши
        public HouseTopType? SelectedHouseTopType
        {
            get { return HouseTopTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.HouseTopId); }
            set
            {
                BuildingStructure!.HouseTopId = value!.Id;
                OnPropertyChanged("SelectedHouseTopType");
            }
        }
        //Водосток
        public string SelectedGully
        {
            get { return BuildingStructure!.Gully!; }
            set
            {
                BuildingStructure!.Gully = value;
                OnPropertyChanged("SelectedGully");
            }
        }
        //Конструкция кровли
        public RoofType? SelectedRoofType
        {
            get { return RoofTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.RoofTypeId); }
            set
            {
                BuildingStructure!.RoofTypeId = value!.Id;
                OnPropertyChanged("SelectedRoofType");
            }
        }
        //Материал кровли
        public RoofingMaterial? SelectedRoofingMaterial
        {
            get { return RoofingMaterials!.FirstOrDefault(x => x.Id == BuildingStructure!.RoofingMaterialId); }
            set
            {
                BuildingStructure!.RoofingMaterialId = value!.Id;
                OnPropertyChanged("SelectedRoofingMaterial");
            }
        }
        //Высота парапета
        public string? ParapetHeight
        {
            get
            {
                if (BuildingStructure!.ParapetHeight == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ParapetHeight.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ParapetHeight = Convert.ToDouble(value);
                else
                    BuildingStructure!.ParapetHeight = null;
                OnPropertyChanged("ParapetHeight");
            }
        }
        //Примечание по парапету
        public string? ParapetNote
        {
            get { return BuildingStructure!.ParapetNote; }
            set
            {
                BuildingStructure!.ParapetNote = value;
                OnPropertyChanged("ParapetNote");
            }
        }
        #endregion

        #region Подраздел отмостка/входы/пандусы/крыльцо
        //Бетон B
        public string? ConcreteBb
        {
            get
            {
                if (BuildingStructure!.ConcreteBb == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ConcreteBb.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ConcreteBb = Convert.ToInt32(value);
                else
                    BuildingStructure!.ConcreteBb = null;
                OnPropertyChanged("ConcreteBb");
            }
        }
        //Бетон F
        public string? ConcreteFf
        {
            get
            {
                if (BuildingStructure!.ConcreteFf == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ConcreteFf.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ConcreteFf = Convert.ToInt32(value);
                else
                    BuildingStructure!.ConcreteFf = null;
                OnPropertyChanged("ConcreteFf");
            }
        }
        //Бетон W
        public string? ConcreteWw
        {
            get
            {
                if (BuildingStructure!.ConcreteWw == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ConcreteWw.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ConcreteWw = Convert.ToInt32(value);
                else
                    BuildingStructure!.ConcreteWw = null;
                OnPropertyChanged("ConcreteWw");
            }
        }
        //Толщина бетона
        public string? ThicknessConcrete
        {
            get
            {
                if (BuildingStructure!.ThicknessConcrete == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ThicknessConcrete.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ThicknessConcrete = Convert.ToDouble(value);
                else
                    BuildingStructure!.ThicknessConcrete = null;
                OnPropertyChanged("ThicknessConcrete");
            }
        }

        //Подготовка
        public string? Preparation
        {
            get
            {
                if (BuildingStructure!.Preparation == null)
                    BuildingStructure!.Preparation = "Щебень";
                return BuildingStructure!.Preparation;
            }
            set
            {
                BuildingStructure!.Preparation = value;
                OnPropertyChanged("Preparation");
            }
        }
        //Толщина подготовки
        public string? ThicknessPreparation
        {
            get
            {
                if (BuildingStructure!.ThicknessPreparation == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ThicknessPreparation.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ThicknessPreparation = Convert.ToDouble(value);
                else
                    BuildingStructure!.ThicknessPreparation = null;
                OnPropertyChanged("ThicknessPreparation");
            }
        }
        #endregion

        #region Подраздел прочее 
        //Вентиляционные шахты
        public AirShaftType? SelectedAirShaft
        {
            get { return AirShaftTypes!.FirstOrDefault(x => x.Id == BuildingStructure!.AirShaftTypeId); }
            set
            {
                BuildingStructure!.AirShaftTypeId = value!.Id;
                OnPropertyChanged("SelectedAirShaft");
            }
        }
        //Наружная отделка фасада
        public ExteriorFinish? SelectedExteriorFinish
        {
            get { return ExteriorFinishes!.FirstOrDefault(x => x.Id == BuildingStructure!.ExteriorFinishId); }
            set
            {
                BuildingStructure!.ExteriorFinishId = value!.Id;
                OnPropertyChanged("SelectedExteriorFinish");
            }
        }
        //Материал утеплителя
        public InsulationMaterial? SelectedInsulation
        {
            get { return InsulationMaterials!.FirstOrDefault(x => x.Id == BuildingStructure!.InsulationMaterialId); }
            set
            {
                BuildingStructure!.InsulationMaterialId = value!.Id;
                OnPropertyChanged("SelectedInsulation");
            }
        }
        //Толщина утеплителя
        public string? ThicknessInsulation
        {
            get
            {
                if (BuildingStructure!.ThicknessInsulation == null)
                    return string.Empty;
                else
                    return BuildingStructure!.ThicknessInsulation.ToString();
            }
            set
            {
                if (value != string.Empty)
                    BuildingStructure!.ThicknessInsulation = Convert.ToDouble(value);
                else
                    BuildingStructure!.ThicknessInsulation = null;
                OnPropertyChanged("ThicknessInsulation");
            }
        }
        //Плотность утеплителя
        public string? Density
        {
            get { return BuildingStructure!.Density; }
            set
            {
                BuildingStructure!.Density = value;
                OnPropertyChanged("Density");
            }
        }
        #endregion

        #endregion

        #region Команды добавления новых данных в comboBox'ы

        //Добавление типа конструкции 
        public RelayCommand? AddCommandConstructiveType
        {
            get
            {
                return addCommandConstructiveType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа конструкции"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        ConstructionType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.ConstructionTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.ConstructionTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление типа конструкции лестницы
        public RelayCommand? AddCommandStairType
        {
            get
            {
                return addCommandStairType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа конструкции лестницы"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        StairType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.StairTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.StairTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление типа конструкции лифта
        public RelayCommand? AddCommandLiftType
        {
            get
            {
                return addCommandLiftType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа конструкции шахты лифта"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        LiftType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.LiftTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.LiftTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление типа конструкции крыши
        public RelayCommand? AddCommandHouseTopType
        {
            get
            {
                return addCommandHouseTopType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа конструкции крыши"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        HouseTopType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.HouseTopTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.HouseTopTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление типа кровли
        public RelayCommand? AddCommandRoofType
        {
            get
            {
                return addCommandRoofType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа кровли"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        RoofType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.RoofTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.RoofTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление материала кровли
        public RelayCommand? AddCommandRoofingMaterial
        {
            get
            {
                return addCommandRoofingMaterial ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование кровельного материала"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        RoofingMaterial roofing = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.RoofingMaterials.Any(el => el.Name! == roofing.Name!);
                        if (!checkIsExist)
                        {
                            db.RoofingMaterials.Add(roofing);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление типа вентиляционной шахты
        public RelayCommand? AddCommandAirShaftType
        {
            get
            {
                return addCommandAirShaftType ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование типа вентиляционных шахт"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        AirShaftType type = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.AirShaftTypes.Any(el => el.Name! == type.Name!);
                        if (!checkIsExist)
                        {
                            db.AirShaftTypes.Add(type);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление наружная отделки фасада
        public RelayCommand? AddCommandExteriorFinish
        {
            get
            {
                return addCommandExteriorFinish ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование наружной отделки фасада"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        ExteriorFinish finish = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.ExteriorFinishes.Any(el => el.Name! == finish.Name!);
                        if (!checkIsExist)
                        {
                            db.ExteriorFinishes.Add(finish);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление утеплителя
        public RelayCommand? AddCommandInsulationMaterial
        {
            get
            {
                return addCommandInsulationMaterial ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование утеплителя"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        InsulationMaterial insulation = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.InsulationMaterials.Any(el => el.Name! == insulation.Name!);
                        if (!checkIsExist)
                        {
                            db.InsulationMaterials.Add(insulation);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        #endregion

        //Сохранение данных 3 части
        public RelayCommand? EditCommanBuildingFoundation
        {
            get
            {
                return editCommanBuildingFoundation ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу характеристика фундамента 
                    if (BuildingFoundation!.Id != 0)
                        db.Entry(BuildingFoundation).State = EntityState.Modified;
                    else
                        db.BuildingFoundations.Add(BuildingFoundation);

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Данные подраздела конструкции здания (сооружения) ниже отметки 0.00 были успешно сохранены(обновлены)";
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
