using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model.DataBase;
using SmaltaApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmaltaApp.View;
using System.IO;
using System.Windows.Media.Imaging;
using SmaltaApp.View.Windows;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SmaltaApp.ViewModel
{
    class AllProjectsVM : INotifyPropertyChanged
    {


        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление методов
        private RelayCommand? addCommandProject;
        private RelayCommand? editCommandProject;
        private RelayCommand? openCommandAnalytics;
        private RelayCommand? filterCommand;
        private RelayCommand? searchCommandName;

        //Таблицы
        List<Project>? allProjects;
        public List<Project>? AllProjects { get { return allProjects; } set { allProjects = value; OnPropertyChanged("AllProjects"); } }

        //Конструктор
        public AllProjectsVM()
        {
            db.Database.EnsureCreated();
            db.AllProjects.Load();
            AllProjects = db.AllProjects.Local.ToObservableCollection().ToList();
            db.Cities.Load();
            db.Customers.Load();
            db.Developers.Load();
        }

        //Выбранная запись из списка проектов
        private Project? selectedProject;
        public Project? SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                if (value != null)
                {
                    if (value?.Image != null)
                    {
                        MemoryStream memory = new(value.Image);
                        BitmapImage bitmap = new();
                        bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                        View.AllProjects.myMainImage!.Source = bitmap;
                    }
                    else
                    {
                        View.AllProjects.myMainImage!.Source = null;
                    }
                }
                OnPropertyChanged("SelectedProject");
            }
        }

        //Команда добавления проекта(открытие страницы проекта)
        public RelayCommand? AddCommandProject
        {
            get
            {
                return addCommandProject ??= new RelayCommand((obj) =>
                {
                    MainWindow.myFrame!.NavigationService.RemoveBackEntry();
                    MainWindow.myFrame!.Content = new ProjectPage(new Model.Project());
                });
            }
        }

        //Команда редактирования заявки(открытие страницы заявки)
        public RelayCommand? EditCommandProject
        {
            get
            {
                return editCommandProject ??= new RelayCommand((obj) =>
                {
                    Project? project = SelectedProject;
                    if (project != null)
                    {
                        MainWindow.myFrame!.NavigationService.RemoveBackEntry();
                        MainWindow.myFrame!.Content = new ProjectPage(SelectedProject!);
                    }
                });
            }
        }

        //Команда открытия страницы аналитики
        public RelayCommand? OpenCommandAnalytics
        {
            get
            {
                return openCommandAnalytics ??= new RelayCommand((obj) =>
                {
                    MainWindow.myFrame!.NavigationService.RemoveBackEntry();
                    MainWindow.myFrame!.Content = new Analytics();
                });
            }
        }

        #region Поиск проекта по наименованию
        //Состояние фильтра
        bool isSearch = false;
        public bool IsSearch
        {
            get { return isSearch; }
            set
            {
                isSearch = value;
                OnPropertyChanged("IsSearch");
            }
        }

        //Параметр для наименования
        string nameFilter = string.Empty;
        public string NameFilter
        {
            get { return nameFilter; }
            set
            {
                nameFilter = value;
                OnPropertyChanged("NameFilter");
            }
        }

        //Команда поиска проекта по имени
        public RelayCommand? SearchCommandName
        {
            get
            {
                return searchCommandName ??= new RelayCommand((obj) =>
                {

                    if (IsSearch)
                    {
                        if (NameFilter != string.Empty)
                            AllProjects = db.AllProjects.Where(x => EF.Functions.Like(x.Name!,
                                         string.Format("%{0}%", NameFilter))).ToList();
                        else IsSearch = !IsSearch;
                    }
                    else
                    {

                        AllProjects = db.AllProjects.Local.ToObservableCollection().ToList();
                        NameFilter = string.Empty;
                    }
                });
            }
        }
        #endregion

        #region Фильтрация списка проектов
        //Состояние фильтра
        bool isFilter = false;
        public bool IsFilter
        {
            get { return isFilter; }
            set
            {
                isFilter = value;
                OnPropertyChanged("IsFilter");
            }
        }

        //Параметр-текст для фильтрации
        string filterText = string.Empty;
        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                OnPropertyChanged("FilterText");
            }
        }

        //Критерий отбора
        int parametr;
        public int Parametr
        {
            get { return parametr; }
            set
            {
                parametr = value;
                OnPropertyChanged("Parametr");
            }
        }

        //Команда фильтрация списка проектов
        public RelayCommand? FilterCommand
        {
            get
            {
                return filterCommand ??= new RelayCommand((obj) =>
                {
                    if (IsFilter)
                    {
                        if (FilterText == string.Empty)
                        {
                            IsFilter = !IsFilter;
                            ShowMess("Для фильтрации данных необходимо заполнить поле критерием отбора данных.");
                        }
                        else
                        {
                            switch (parametr)
                            {
                                case 0:
                                    try
                                    {
                                        Convert.ToInt32(FilterText);
                                        AllProjects = db.AllProjects.Where(x => x.Year == Convert.ToInt32(FilterText)).ToList();
                                    }
                                    catch
                                    {
                                        IsFilter = !IsFilter;
                                        ShowMess("Некоректный ввод даты. Фильтрация невозможна. Для повторной попытки отбора исправте введенное значение.");
                                    }
                                    break;
                                case 1:
                                    AllProjects = db.AllProjects.Where(x => EF.Functions.Like(x.City!.Name!,
                                        string.Format("%{0}%", FilterText))).ToList();
                                    break;
                                case 2:
                                    AllProjects = db.AllProjects.Where(x => EF.Functions.Like(x.Documentation!,
                                        string.Format("%{0}%", FilterText))).ToList();
                                    break;
                                case 3:
                                    AllProjects = db.AllProjects.Where(x => EF.Functions.Like(x.Customer!.Name!,
                                        string.Format("%{0}%", FilterText))).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        AllProjects = db.AllProjects.Local.ToObservableCollection().ToList();
                        FilterText = string.Empty;
                        IsFilter = false;
                    }
                });
            }
        }
        #endregion

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

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
