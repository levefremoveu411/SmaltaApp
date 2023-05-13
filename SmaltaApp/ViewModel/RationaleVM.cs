using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SmaltaApp.ViewModel
{
    public class RationaleVM : INotifyPropertyChanged
    {
        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? addCommandSoftware;

        private RelayCommand? editCommandImageOne;
        private RelayCommand? editCommandImageTwo;

        private RelayCommand? editCommandWorking;
        private RelayCommand? editCommandRationaleInfo;

        private RelayCommand? addCommandGeologicalWork;
        private RelayCommand? saveCommandGeologicalWork;
        private RelayCommand? cancelCommandGeologicalWork;
        private RelayCommand? deleteCommandGeologicalWork;

        private RelayCommand? addCommandProjectFluctuation;
        private RelayCommand? saveCommandProjectFluctuation;
        private RelayCommand? cancelCommandProjectFluctuation;
        private RelayCommand? deleteCommandProjectFluctuation;

        //Таблицы
        public ObservableCollection<Software>? Softwares { get; set; }

        //Список колебаний фундамента 
        List<ProjectFluctuation>? projectFluctuations;
        public List<ProjectFluctuation>? ProjectFluctuations
        {
            get { return projectFluctuations; }
            set
            {
                projectFluctuations = value;
                OnPropertyChanged("ProjectFluctuations");
            }
        }

        //Список геологических выработок
        List<GeologicalWork>? geologicalWorks;
        public List<GeologicalWork>? GeologicalWorks
        {
            get { return geologicalWorks; }
            set
            {
                geologicalWorks = value;
                OnPropertyChanged("GeologicalWorks");
            }
        }

        //Конструктор
        public RationaleInfo? RationaleInfo;
        public IgeWorking? IgeWorking;
        public GeologicalWork? defalutGeologicalWork; //для добавления выработки
        public ProjectFluctuation? defalutProjectFluctuation; //для добавления колебаний
        public RationaleVM(int projectId)
        {
            db.Database.EnsureCreated();
            db.Softwares.Load();
            Softwares = db.Softwares.Local.ToObservableCollection();

            RationaleInfo = db.RationaleInfos.FirstOrDefault(x => x.ProjectId == projectId);
            RationaleInfo ??= new() { ProjectId = projectId };
            if (RationaleInfo.ImageOne != null)
            {
                MemoryStream memory = new(RationaleInfo.ImageOne);
                BitmapImage bitmap = new();
                bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                Rationale.myImageOne!.Source = bitmap;
            }
            if (RationaleInfo.ImageTwo != null)
            {
                MemoryStream memory = new(RationaleInfo.ImageTwo);
                BitmapImage bitmap = new();
                bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                Rationale.myImageTwo!.Source = bitmap;
            }

            //Заполнение списка колебаний
            defalutProjectFluctuation = new() { ProjectId = projectId };
            ProjectFluctuations = db.ProjectFluctuations.Where(c => c.ProjectId == projectId).ToList();


            //Заполнение данных по выработкам
            defalutGeologicalWork = new();
            IgeWorking = db.IgeWorkings.FirstOrDefault(x => x.ProjectId == projectId);
            if (IgeWorking == null) IgeWorking = new() { ProjectId = projectId };
            else
            {
                IsVisibilityButton = Visibility.Visible;
                GeologicalWorks = db.GeologicalWorks.Where(c => c.IgeWorkingId == IgeWorking.Id).ToList();
            }



            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);

        }

        #region Свойства для интерфейса 

        //Свойство для доступноcnb кнопок
        Visibility isVisibilityButton = Visibility.Hidden;
        public Visibility IsVisibilityButton
        {
            get { return isVisibilityButton; }
            set
            {
                isVisibilityButton = value;
                OnPropertyChanged("IsVisibilityButton");
            }
        }

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


        //Свойство для доступности полей при редактировании
        bool isEnabled_1 = false;
        public bool IsEnabled_1
        {
            get { return isEnabled_1; }
            set
            {
                isEnabled_1 = value;
                OnPropertyChanged("IsEnabled_1");
            }
        }

        SolidColorBrush colorGroup_1 = Brushes.Transparent;
        public SolidColorBrush ColorGroup_1
        {
            get { return colorGroup_1; }
            set
            {
                colorGroup_1 = value;
                OnPropertyChanged("ColorGroup_1");

            }
        }
        #endregion

        #region Основные данные

        //Выбранный программный продукт
        public Software? SelectedSoftware
        {
            get
            {
                if (RationaleInfo!.Software != null)
                {
                    Deleloper = RationaleInfo!.Software.Developer;
                    Сertificate = RationaleInfo!.Software.Сertificate;
                }
                return RationaleInfo!.Software;
            }
            set
            {
                RationaleInfo!.Software = value;
                Deleloper = value!.Developer;
                Сertificate = value!.Сertificate;
                OnPropertyChanged("SelectedSoftware");
            }
        }

        string? developer;
        public string? Deleloper
        {
            get
            {
                return developer;
            }
            set
            {
                developer = value;
                OnPropertyChanged("Deleloper");
            }
        }

        string? certificate;
        public string? Сertificate
        {
            get
            {
                return certificate;
            }
            set
            {
                certificate = value;
                OnPropertyChanged("Сertificate");
            }
        }


        //Добавление(обновление) картинки 
        public RelayCommand? EditCommandImageOne
        {
            get
            {
                return editCommandImageOne ??= new RelayCommand((obj) =>
                {
                    OpenFileDialog openFileDialog = new()
                    {
                        Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
                    };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        Rationale.myImageOne!.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                        FileStream fileStream = new(openFileDialog.FileName.ToString(), FileMode.Open, FileAccess.Read);
                        BinaryReader binaryReader = new(fileStream);
                        RationaleInfo!.ImageOne = binaryReader.ReadBytes((int)fileStream.Length);
                    }
                });
            }
        }

        //Добавление(обновление) картинки 
        public RelayCommand? EditCommandImageTwo
        {
            get
            {
                return editCommandImageTwo ??= new RelayCommand((obj) =>
                {
                    OpenFileDialog openFileDialog = new()
                    {
                        Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
                    };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        Rationale.myImageTwo!.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                        FileStream fileStream = new(openFileDialog.FileName.ToString(), FileMode.Open, FileAccess.Read);
                        BinaryReader binaryReader = new(fileStream);
                        RationaleInfo!.ImageTwo = binaryReader.ReadBytes((int)fileStream.Length);
                    }
                });
            }
        }

        //Редактирование основных данных
        public RelayCommand? EditCommandRationaleInfo
        {
            get
            {
                return editCommandRationaleInfo ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу характеристика
                    if (RationaleInfo!.Id != 0)
                        db.Entry(RationaleInfo).State = EntityState.Modified;
                    else
                    {
                        db.RationaleInfos.Add(RationaleInfo);
                    }

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Основная информация была успешно сохранена(обновлена)";
                    dispatcherTimer.Start();
                });
            }
        }

        //Добавление города
        public RelayCommand? AddCommandSoftware
        {
            get
            {
                return addCommandSoftware ??= new RelayCommand((obj) =>
                {
                    AddSoftware window = new(new Software())
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        Software software = new()
                        {
                            Name = window.NewSoftware.Name,
                            Developer = window.NewSoftware.Developer,
                            Сertificate = window.NewSoftware.Сertificate,
                        };
                        bool checkIsExist = db.Softwares.Any(el => el.Name!.ToLower() == software.Name!.ToLower());
                        if (!checkIsExist)
                        {
                            db.Softwares.Add(software);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        #endregion

        #region Сведения по колебаниям
        //Выбор записи из списка
        private ProjectFluctuation? selectedProjectFluctuation;
        public ProjectFluctuation? SelectedProjectFluctuation
        {
            get { return selectedProjectFluctuation; }
            set
            {
                selectedProjectFluctuation = value;
                OnPropertyChanged("SelectedProjectFluctuation");
            }
        }

        //Колиечство форм
        public string? CountForm
        {
            get
            {
                if (defalutProjectFluctuation!.CountForm == null)
                    return string.Empty;
                else
                    return defalutProjectFluctuation!.CountForm.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutProjectFluctuation!.CountForm = Convert.ToInt32(value);
                else
                    defalutProjectFluctuation!.CountForm = null;
                OnPropertyChanged("CountForm");
            }
        }
        //W
        public string? W
        {
            get
            {
                if (defalutProjectFluctuation!.W == null)
                    return string.Empty;
                else
                    return defalutProjectFluctuation!.W.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutProjectFluctuation!.W = Convert.ToDouble(value);
                else
                    defalutProjectFluctuation!.W = null;
                OnPropertyChanged("W");
            }
        }
        //T
        public string? T
        {
            get
            {
                if (defalutProjectFluctuation!.T == null)
                    return string.Empty;
                else
                    return defalutProjectFluctuation!.T.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutProjectFluctuation!.T = Convert.ToDouble(value);
                else
                    defalutProjectFluctuation!.T = null;
                OnPropertyChanged("T");
            }
        }
        //F
        public string? F
        {
            get
            {
                if (defalutProjectFluctuation!.F == null)
                    return string.Empty;
                else
                    return defalutProjectFluctuation!.F.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutProjectFluctuation!.F = Convert.ToDouble(value);
                else
                    defalutProjectFluctuation!.F = null;
                OnPropertyChanged("F");
            }
        }

        //Добавление записи
        public RelayCommand? AddCommandProjectFluctuation
        {
            get
            {
                return addCommandProjectFluctuation ??= new RelayCommand((obj) =>
                {
                    IsEnabled_1 = true;
                    ColorGroup_1 = Brushes.White;
                });
            }
        }
        //Cохранение данных
        public RelayCommand? SaveCommandProjectFluctuation
        {
            get
            {
                return saveCommandProjectFluctuation ??= new RelayCommand((obj) =>
                {
                    if (defalutProjectFluctuation!.CountForm != null && defalutProjectFluctuation.W != null && defalutProjectFluctuation.T != null && defalutProjectFluctuation.F != null)
                    {
                        bool checkIsExist = db.ProjectFluctuations.Any(el => el.ProjectId == defalutProjectFluctuation!.ProjectId && el.CountForm == defalutProjectFluctuation.CountForm);
                        if (!checkIsExist)
                        {
                            db.ProjectFluctuations.Add(new()
                            {
                                ProjectId = defalutProjectFluctuation!.ProjectId,
                                CountForm = defalutProjectFluctuation!.CountForm,
                                W = defalutProjectFluctuation!.W,
                                T = defalutProjectFluctuation!.T,
                                F = defalutProjectFluctuation!.F
                            });
                            db.SaveChanges();
                            IsEnabled_1 = false;
                            ColorGroup_1 = Brushes.Transparent;
                            ProjectFluctuations = db.ProjectFluctuations.Where(c => c.ProjectId == defalutProjectFluctuation!.ProjectId).ToList();
                            CountForm = string.Empty; W = string.Empty; F = string.Empty; T = string.Empty;
                        }
                        else ShowMess("Добавление невозможно. Колебание с текущим числом учтенных форм уже отражены в текущем списке.");
                    }
                    else ShowMess("Для сохранения данных необходимо заполнить все поля.");
                });
            }
        }
        //Удаление
        public RelayCommand? DeleteCommandProjectFluctuation
        {
            get
            {
                return deleteCommandProjectFluctuation ??= new RelayCommand((obj) =>
                {
                    if (SelectedProjectFluctuation!.Id != 0)
                    {

                        db.ProjectFluctuations.Remove(SelectedProjectFluctuation);
                        db.SaveChanges();
                        ProjectFluctuations = db.ProjectFluctuations.Where(c => c.ProjectId == defalutProjectFluctuation!.ProjectId).ToList();

                    }
                    else { ShowMess("Удаление невозможно. Запись для удаления не выбрана."); }
                });
            }
        }
        //Отмена
        public RelayCommand? CancelCommandProjectFluctuation
        {
            get
            {
                return cancelCommandProjectFluctuation ??= new RelayCommand((obj) =>
                {
                    IsEnabled_1 = false;
                    ColorGroup_1 = Brushes.Transparent;
                    CountForm = string.Empty; W = string.Empty; F = string.Empty; T = string.Empty;
                });
            }
        }
        #endregion

        #region Сведения по инженерно-геологическим выработкам основания фундамента

        #region Свойства для основных полей
        //Отчет
        public string? Report
        {
            get { return IgeWorking!.Report; }
            set
            {
                IgeWorking!.Report = value;
                OnPropertyChanged("Report");
            }
        }
        //Год
        public string? Year
        {
            get
            {
                if (IgeWorking!.Year == null)
                    return string.Empty;
                else
                    return IgeWorking!.Year.ToString();
            }
            set
            {
                if (value != string.Empty)
                    IgeWorking!.Year = Convert.ToInt32(value);
                OnPropertyChanged("Year");
            }
        }
        //Организация
        public string? Organization
        {
            get
            {
                if (IgeWorking!.Organization == null)
                    IgeWorking!.Organization ="ОАО «СахалинТИСИЗ»";
                return IgeWorking!.Organization;
            }
            set
            {
                IgeWorking!.Organization = value;
                OnPropertyChanged("Organization");
            }
        }
        #endregion

        #region Свойства для полей редактирования выработки 
        //Выбор записи из списка
        private GeologicalWork? selectedGeologicalWork = new();
        public GeologicalWork? SelectedGeologicalWork
        {
            get { return selectedGeologicalWork; }
            set
            {
                selectedGeologicalWork = value;
                NameG = selectedGeologicalWork?.Name;
                Power = selectedGeologicalWork?.Power.ToString();
                Weight = selectedGeologicalWork?.Weight.ToString();
                Deformation = selectedGeologicalWork?.Deformation.ToString();
                Clutch = selectedGeologicalWork?.Clutch.ToString();
                Friction = selectedGeologicalWork?.Friction.ToString();
                Fluidity = selectedGeologicalWork?.Fluidity.ToString();
                Note = selectedGeologicalWork?.Note;

                OnPropertyChanged("SelectedGeologicalWork");
            }
        }

        //Наименование 
        public string? NameG
        {
            get { return defalutGeologicalWork!.Name; }
            set
            {
                defalutGeologicalWork!.Name = value;
                OnPropertyChanged("NameG");
            }
        }
        //Мощность
        public string? Power
        {
            get
            {
                if (defalutGeologicalWork!.Power == null)
                    return string.Empty;
                else
                    return defalutGeologicalWork!.Power.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutGeologicalWork!.Power = Convert.ToDouble(value);
                else
                    defalutGeologicalWork!.Power = null;
                OnPropertyChanged("Power");
            }
        }
        //Вес
        public string? Weight
        {
            get
            {
                if (defalutGeologicalWork!.Weight == null)
                    return string.Empty;
                else
                    return defalutGeologicalWork!.Weight.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutGeologicalWork!.Weight = Convert.ToDouble(value);
                else
                    defalutGeologicalWork!.Weight = null;
                OnPropertyChanged("Weight");
            }
        }
        //Модуль деформации
        public string? Deformation
        {
            get
            {
                if (defalutGeologicalWork!.Deformation == null)
                    return string.Empty;
                else
                    return defalutGeologicalWork!.Deformation.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutGeologicalWork!.Deformation = Convert.ToDouble(value);
                else
                    defalutGeologicalWork!.Deformation = null;
                OnPropertyChanged("Deformation");
            }
        }
        //Удельное сцепление
        public string? Clutch
        {
            get
            {
                if (defalutGeologicalWork!.Clutch == null)
                    return string.Empty;
                else
                    return defalutGeologicalWork!.Clutch.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutGeologicalWork!.Clutch = Convert.ToDouble(value);
                else
                    defalutGeologicalWork!.Clutch = null;
                OnPropertyChanged("Clutch");
            }
        }
        //Угол внутреннего трения
        public string? Friction
        {
            get
            {
                if (defalutGeologicalWork!.Friction == null)
                    return string.Empty;
                else
                    return defalutGeologicalWork!.Friction.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutGeologicalWork!.Friction = Convert.ToInt32(value);
                else
                    defalutGeologicalWork!.Friction = null;
                OnPropertyChanged("Friction");
            }
        }
        //Показатель текучести
        public string? Fluidity
        {
            get
            {
                if (defalutGeologicalWork!.Fluidity == null)
                    return string.Empty;
                else
                    return defalutGeologicalWork!.Fluidity.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutGeologicalWork!.Fluidity = Convert.ToInt32(value);
                else
                    defalutGeologicalWork!.Fluidity = null;
                OnPropertyChanged("Fluidity");
            }
        }
        //Примечание 
        public string? Note
        {
            get { return defalutGeologicalWork!.Note; }
            set
            {
                defalutGeologicalWork!.Note = value;
                OnPropertyChanged("Note");
            }
        }
        #endregion

        #region Команды для работы с выработками
        //Редактирование основных данных выработок
        public RelayCommand? EditCommandWorking
        {
            get
            {
                return editCommandWorking ??= new RelayCommand((obj) =>
                {
                    //Сохранение данных в таблицу характеристика
                    if (IgeWorking!.Id != 0)
                        db.Entry(IgeWorking).State = EntityState.Modified;
                    else
                    {
                        db.IgeWorkings.Add(IgeWorking);
                        isVisibilityButton = Visibility.Visible;
                    }

                    db.SaveChanges();
                    MainWindow.myChangeStatus!.Text = "Основная информация по выработкам была успешно сохранена(обновлена)";
                    dispatcherTimer.Start();

                });
            }
        }
        //Добавление записи
        public RelayCommand? AddCommandGeologicalWork
        {
            get
            {
                return addCommandGeologicalWork ??= new RelayCommand((obj) =>
                {
                    IsEnabled = true;
                    ColorGroup = Brushes.White;
                    SelectedGeologicalWork = new() { IgeWorkingId = IgeWorking!.Id };
                });
            }
        }
        //Cохранение данных
        public RelayCommand? SaveCommandGeologicalWork
        {
            get
            {
                return saveCommandGeologicalWork ??= new RelayCommand((obj) =>
                {


                    db.GeologicalWorks.Add(new()
                    {
                        IgeWorkingId = IgeWorking!.Id,
                        Name = defalutGeologicalWork?.Name,
                        Power = defalutGeologicalWork?.Power,
                        Weight = defalutGeologicalWork?.Weight,
                        Deformation = defalutGeologicalWork?.Deformation,
                        Clutch = defalutGeologicalWork?.Clutch,
                        Friction = defalutGeologicalWork?.Friction,
                        Fluidity = defalutGeologicalWork?.Fluidity,
                        Note = defalutGeologicalWork?.Note,
                    });
                    db.SaveChanges();
                    IsEnabled = false;
                    ColorGroup = Brushes.Transparent;
                    SelectedGeologicalWork = new() { IgeWorkingId = IgeWorking!.Id };
                    GeologicalWorks = db.GeologicalWorks.Where(c => c.IgeWorkingId == IgeWorking.Id).ToList();
                });
            }
        }
        //Отмена
        public RelayCommand? CancelCommandGeologicalWork
        {
            get
            {
                return cancelCommandGeologicalWork ??= new RelayCommand((obj) =>
                {
                    IsEnabled = false;
                    ColorGroup = Brushes.Transparent;
                    SelectedGeologicalWork = new() { IgeWorkingId = IgeWorking!.Id };
                });
            }
        }
        //Удаление
        public RelayCommand? DeleteCommandGeologicalWork
        {
            get
            {
                return deleteCommandGeologicalWork ??= new RelayCommand((obj) =>
                {
                    if (SelectedGeologicalWork!.Id != 0)
                    {

                        db.GeologicalWorks.Remove(SelectedGeologicalWork);
                        db.SaveChanges();
                        GeologicalWorks = db.GeologicalWorks.Where(c => c.IgeWorkingId == IgeWorking!.Id).ToList();

                    }
                    else { ShowMess("Удаление невозможно. Запись для удаления не выбрана."); }
                });
            }
        }
        #endregion

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
