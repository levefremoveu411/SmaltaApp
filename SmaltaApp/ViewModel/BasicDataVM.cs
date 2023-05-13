using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SmaltaApp.Documentation;
using SmaltaApp.Model;
using SmaltaApp.Model.AdditionalClasses;
using SmaltaApp.Model.DataBase;
using SmaltaApp.Model.HelperClasses;
using SmaltaApp.View;
using SmaltaApp.View.Windows;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SmaltaApp.ViewModel
{
    public class BasicDataVM : INotifyPropertyChanged
    {
        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? addCommandCustomer;
        private RelayCommand? addCommandDeveloper;
        private RelayCommand? addCommandCity;

        private RelayCommand? editCommandImage;
        private RelayCommand? editCommandProject;
        private RelayCommand? fillConditionCommand;

        private RelayCommand? createCommandWordDocumentation;

        //Таблицы
        public ObservableCollection<Customer>? Customers { get; set; }
        public ObservableCollection<Developer>? Developers { get; set; }
        public ObservableCollection<City>? Cities { get; set; }

        public ObservableCollection<SnowyRegion>? SnowyRegions { get; set; }
        public ObservableCollection<WindRegion>? WindRegions { get; set; }
        public ObservableCollection<Responsibility>? Responsibilities { get; set; }

        readonly Project? Project;
        readonly ClimaticCondition? Condition;
        //Конструктор
        public BasicDataVM(Project project)
        {
            Project = project;
            Condition = new();

            db.Database.EnsureCreated();
            db.Customers.Load();
            Customers = db.Customers.Local.ToObservableCollection();

            db.Developers.Load();
            Developers = db.Developers.Local.ToObservableCollection();

            db.Cities.Load();
            Cities = db.Cities.Local.ToObservableCollection();


            db.SnowyRegions.Load();
            SnowyRegions = db.SnowyRegions.Local.ToObservableCollection();

            db.WindRegions.Load();
            WindRegions = db.WindRegions.Local.ToObservableCollection();

            db.Responsibilities.Load();
            Responsibilities = db.Responsibilities.Local.ToObservableCollection();

            if (Project.Id != 0)
            {
                if (Project.Image != null)
                {
                    MemoryStream memory = new(Project.Image);
                    BitmapImage bitmap = new();
                    bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                    BasicData.myImageProject!.Source = bitmap;
                }
                Condition = db.ClimaticConditions.First(c => c.ProjectId == Project.Id);
            }

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
        }

        #region Свойства для основных данных
        //Наименование проекта
        public string? Name
        {
            get { return Project!.Name; }
            set
            {
                Project!.Name = value;
                OnPropertyChanged("Name");
            }
        }

        //Год 
        public string? Year
        {
            get
            {
                if (Project!.Year == null)
                    return string.Empty;
                else
                    return Project!.Year.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Project!.Year = Convert.ToInt32(value);
                OnPropertyChanged("Year");
            }
        }

        //Выбранный элемент из списка городов
        public City SelectedCity
        {
            get { return Project!.City!; }
            set
            {
                Project!.City = value;
                Project!.CityId = value.Id;
                OnPropertyChanged("SelectedCity");
            }
        }
        public string SelectedCityText
        {
            get
            {
                if (Project!.City != null)
                    return Project!.City!.Name!;
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedCityText");
            }

        }

        //Выбранный элемент из списка разработчиков
        public Developer SelectedDeveloper
        {
            get { return Project!.Developer!; }
            set
            {
                Project!.Developer = value;
                Project!.DeveloperId = value.Id;
                OnPropertyChanged("SelectedDeveloper");
            }
        }
        public string SelectedDeveloperText
        {
            get
            {
                if (Project!.Developer != null)
                    return Project!.Developer!.Abbreviation!;
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedDeveloperText");
            }

        }

        //Выбранный элемент из списка ролей
        public string SelectedRole
        {
            get { return Project!.Role!; }
            set
            {
                Project!.Role = value;
                OnPropertyChanged("SelectedRole");
            }
        }

        //Выбранный элемент из списка заказчиков
        public Customer SelectedCustomer
        {
            get { return Project!.Customer!; }
            set
            {
                Project!.Customer = value;
                Project!.CustomerId = value.Id;
                OnPropertyChanged("SelectedCustomer");
            }
        }
        public string SelectedCustomerText
        {
            get
            {
                if (Project!.Customer != null)
                    return Project!.Customer!.Abbreviation!;
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedCustomerText");
            }

        }

        //Номер договора
        public string? Treaty
        {
            get { return Project!.Treaty; }
            set
            {
                Project!.Treaty = value;
                OnPropertyChanged("Treaty");
            }
        }

        //Дата заключения
        string? data;
        public string? DateOfConclusion
        {
            get
            {
                data = Convert.ToDateTime(Project!.DateOfConclusion).ToShortDateString();
                if (data != "01.01.0001")
                    return data;
                else return null;
            }
            set
            {
                try
                {
                    Project!.DateOfConclusion = Convert.ToDateTime(value);
                }
                catch (Exception) { };
                OnPropertyChanged("DateOfConclusion");
            }
        }

        //Выбранный элемент из списка документаций
        public string SelectedDocumentation
        {
            get { return Project!.Documentation!; }
            set
            {
                Project!.Documentation = value;
                OnPropertyChanged("SelectedDocumentation");
            }
        }
        #endregion

        #region Свойства для данных по климатическим особенностям местности
        //Выбранный элемент из списка регионов
        public string? SelectedRegion
        {
            get { return Condition!.Region; }
            set
            {
                Condition!.Region = value;
                OnPropertyChanged("SelectedRegion");
            }
        }

        //Выбранный элемент из списка подрегионов
        public string? SelectedSubRegion
        {
            get { return Condition!.SubRegion; }
            set
            {
                Condition!.SubRegion = value;
                OnPropertyChanged("SelectedSubRegion");
            }
        }

        //Температура холодного дня
        public string? TemperatureColdD
        {
            get
            {
                if (Condition!.TemperatureColdD == null)
                    return string.Empty;
                else
                    return Condition!.TemperatureColdD.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Condition!.TemperatureColdD = Convert.ToInt32(value);
                else
                    Condition!.TemperatureColdD = null;
                OnPropertyChanged("TemperatureColdD");
            }
        }

        //Температура холодной пятивневки 
        public string? TemperatureColdW
        {
            get
            {
                if (Condition!.TemperatureColdW == null)
                    return string.Empty;
                else
                    return Condition!.TemperatureColdW.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Condition!.TemperatureColdW = Convert.ToInt32(value);
                else
                    Condition!.TemperatureColdW = null;
                OnPropertyChanged("TemperatureColdW");
            }
        }

        //Выбранный элемент из списка снеговых районов
        public SnowyRegion? SelectedSnowyRegion
        {
            get { return Condition!.SnowyRegion!; }
            set
            {
                Condition!.SnowyRegion = value;
                Condition!.SnowyRegionId = value!.Id;
                ValueSnowyRegion = value.Value.ToString();
                OnPropertyChanged("SelectedSnowyRegion");
            }
        }

        public string SelectedSnowyRegionText
        {
            get
            {
                if (Condition!.SnowyRegion != null)
                {
                    ValueSnowyRegion = Condition!.SnowyRegion.Value.ToString();
                    return Condition!.SnowyRegion!.Name!;
                }
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedSnowyRegionText");
            }
        }

        string? valueSnowyRegion;
        public string? ValueSnowyRegion
        {
            get
            {
                return valueSnowyRegion;
            }
            set
            {
                valueSnowyRegion = value;
                OnPropertyChanged("ValueSnowyRegion");
            }
        }

        //Выбранный элемент из списка ветровых районов
        public WindRegion? SelectedWindRegion
        {
            get { return Condition!.WindRegion!; }
            set
            {
                Condition!.WindRegion = value;
                Condition!.WindRegionId = value!.Id;
                ValueWindRegion = value.Value.ToString();
                OnPropertyChanged("SelectedWindRegion");
            }
        }

        public string SelectedWindRegionText
        {
            get
            {
                if (Condition!.WindRegion != null)
                {
                    ValueWindRegion = Condition!.WindRegion.Value.ToString();
                    return Condition!.WindRegion!.Name!;
                }
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedWindRegionText");
            }
        }

        string? valueWindRegion;
        public string? ValueWindRegion
        {
            get
            {
                return valueWindRegion;
            }
            set
            {
                valueWindRegion = value;
                OnPropertyChanged("ValueWindRegion");
            }
        }

        //Выбранный элемент из списка ответственности
        public Responsibility? SelectedResponsibility
        {
            get { return Condition!.Responsibility!; }
            set
            {
                Condition!.Responsibility = value;
                Condition!.ResponsibilityId = value!.Id;
                ValueClass = value.Class;
                ValueCoefficient = value.Coefficient!.ToString();
                OnPropertyChanged("SelectedResponsibility");
            }
        }
        public string SelectedResponsibilityText
        {
            get
            {
                if (Condition!.Responsibility != null)
                {
                    ValueClass = Condition!.Responsibility.Class;
                    ValueCoefficient = Condition!.Responsibility.Coefficient.ToString();
                    return Condition!.Responsibility!.Level!;
                }
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedResponsibilityText");
            }
        }

        string? valueClass;
        public string? ValueClass
        {
            get
            {
                return valueClass;
            }
            set
            {
                valueClass = value;
                OnPropertyChanged("ValueClass");
            }
        }

        string? valueCoefficient;
        public string? ValueCoefficient
        {
            get
            {
                return valueCoefficient;
            }
            set
            {
                valueCoefficient = value;
                OnPropertyChanged("ValueCoefficient");
            }
        }

        //Выбранный элемент из списка карт
        public string? SelectedMap
        {
            get { return Condition!.Map; }
            set
            {
                Condition!.Map = value;
                OnPropertyChanged("SelectedMap");
            }
        }

        //Сейсмичность по тесту
        public string? SeismicityTest
        {
            get
            {
                if (Condition!.SeismicityTest == null)
                    return string.Empty;
                else
                    return Condition!.SeismicityTest.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Condition!.SeismicityTest = Convert.ToInt32(value);
                else
                    Condition!.SeismicityTest = null;
                OnPropertyChanged("SeismicityTest");
            }
        }

        //Сейсмичность по проекту
        public string? SeismicityProject
        {
            get
            {
                if (Condition!.SeismicityProject == null)
                    return string.Empty;
                else
                    return Condition!.SeismicityProject.ToString();
            }
            set
            {
                if (value != string.Empty)
                    Condition!.SeismicityProject = Convert.ToInt32(value);
                else
                    Condition!.SeismicityProject = null;
                OnPropertyChanged("SeismicityProject");
            }
        }
        #endregion

        #region Команды добавления новых данных в таблицы города, заказчики, разработчики
        //Добавление заказчика
        public RelayCommand? AddCommandCustomer
        {
            get
            {
                return addCommandCustomer ??= new RelayCommand((obj) =>
                {
                    AddCompany window = new(new Company())
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        Customer customer = new()
                        {
                            Name = window.NewCompany.Name,
                            Abbreviation = window.NewCompany.Abbreviation,
                        };
                        bool checkIsExist = db.Customers.Any(el => el.Abbreviation! == customer.Abbreviation!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.Customers.Add(customer);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Добавление разработчика
        public RelayCommand? AddCommandDeveloper
        {
            get
            {
                return addCommandDeveloper ??= new RelayCommand((obj) =>
                {
                    AddCompany window = new(new Company())
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        Developer developer = new()
                        {
                            Name = window.NewCompany.Name,
                            Abbreviation = window.NewCompany.Abbreviation,
                        };
                        bool checkIsExist = db.Developers.Any(el => el.Abbreviation! == developer.Abbreviation!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.Developers.Add(developer);
                            db.SaveChanges();
                        }
                    }
                });
            }
        }

        //Добавление города
        public RelayCommand? AddCommandCity
        {
            get
            {
                return addCommandCity ??= new RelayCommand((obj) =>
                {
                    AddEntry window = new(new Entry("Наименование города"))
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        City city = new()
                        {
                            Name = window.NewEntry.Name,
                        };
                        bool checkIsExist = db.Cities.Any(el => el.Name! == city.Name!.ToLowerInvariant());
                        if (!checkIsExist)
                        {
                            db.Cities.Add(city);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }
        #endregion

        #region Команды добавления картинки и данных по проекту, заполнение данных по  местности
        //Добавление(обновление) картинки 
        public RelayCommand? EditCommandImage
        {
            get
            {
                return editCommandImage ??= new RelayCommand((obj) =>
                {
                    OpenFileDialog openFileDialog = new()
                    {
                        Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
                    };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        BasicData.myImageProject!.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                        FileStream fileStream = new(openFileDialog.FileName.ToString(), FileMode.Open, FileAccess.Read);
                        BinaryReader binaryReader = new(fileStream);
                        Project!.Image = binaryReader.ReadBytes((int)fileStream.Length);
                    }
                });
            }
        }

        //Добавление(обновление) данных по проекту
        public RelayCommand? EditCommandProject
        {
            get
            {
                return editCommandProject ??= new RelayCommand((obj) =>
                {
                    if (Project!.Treaty != null && Project.Name != null && Project.Customer != null && Project.Developer != null)
                    {
                        if (Project.Id != 0)
                        {
                            db.Entry(Project).State = EntityState.Modified;
                            db.Entry(Condition!).State = EntityState.Modified;
                            db.SaveChanges();
                            MainWindow.myChangeStatus!.Text = "Изменения основных данных по проекту успешно сохранены";
                            dispatcherTimer.Start();

                        }
                        else
                        {
                            bool checkIsExist = db.AllProjects.Any(el => (el.Treaty == Project.Treaty && el.Name == Project.Name));
                            if (!checkIsExist)
                            {
                                //Добавлание проекта
                                db.AllProjects.Add(Project);
                                //Добавление климатических условий
                                Condition!.Project = Project;
                                db.ClimaticConditions.Add(Condition);
                                db.SaveChanges();
                                MainWindow.myChangeStatus!.Text = "Основные данные по проекту успешно сохранены";
                                dispatcherTimer.Start();

                            }
                            else ShowMess("Текущий номер договора уже существует в базе. Повторное использование невозможно!");
                        }
                    }
                    else ShowMess("Для успешного сохранения необходимо запонить номер документа, дейтсвующие лиза (заказчик, исполнитель) и название проекта.");
                });
            }
        }

        //Заполнение данных по местности
        public RelayCommand? FillConditionCommand
        {
            get
            {
                return fillConditionCommand ??= new RelayCommand((obj) =>
                {
                    if (SelectedCity == null)
                        ShowMess("Поле, отвечающее за место расположение проекта пустое, необходимо выбрать город и повторить попытку.");
                    else
                    {
                        Project? defaultProject = db.AllProjects.FirstOrDefault(c => c.City == SelectedCity);
                        if (defaultProject == null)
                            ShowMess("Климатические особенности данного города не были отражены ранее в программе, автоматическое заполнение невозможно.");
                        else
                        {
                            ClimaticCondition condition = db.ClimaticConditions.First(c => c.ProjectId == defaultProject.Id);
                            SelectedRegion = condition.Region;
                            SelectedSubRegion = condition.SubRegion;
                            TemperatureColdD = condition.TemperatureColdD.ToString();
                            TemperatureColdW = condition.TemperatureColdW.ToString();
                            SelectedSnowyRegion = condition.SnowyRegion;
                            SelectedWindRegion = condition.WindRegion;
                            SelectedResponsibility = condition.Responsibility;
                            SelectedMap = condition.Map;
                            SeismicityTest = condition.SeismicityTest.ToString();
                            SeismicityProject = condition.SeismicityProject.ToString();

                        };
                    }
                });
            }
        }
        #endregion


        //Создание документа Word
        public RelayCommand? CreateCommandWordDocumentation
        {
            get
            {
                return createCommandWordDocumentation ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        if (Project!.Id != 0)
                        {
                            WordHelper word = new (Directory.GetCurrentDirectory() + @"\Данные по проекту.docx", Project);
                        }
                        else
                        {
                            ShowMess("Формирование данных по проекту невозможно. Недостаточность данных.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMess(ex.Message);
                    }
                });
            }
        }

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
