using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model;
using SmaltaApp.Model.DataBase;
using SmaltaApp.View;
using SmaltaApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using M = System.Windows.Media;

namespace SmaltaApp.ViewModel
{
    public class AnalyticsVM : INotifyPropertyChanged
    {

        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление методов
        private RelayCommand? backCommandAllProject;
        private RelayCommand? clearDataCommand;
        private RelayCommand? createDataCommand;
        private RelayCommand? create2DataCommand;
        private RelayCommand? detailDataCommand;

        //Таблицы из базы
        public List<Model.Estimate>? Estimates { get; set; }


        List<Project>? allProjects;
        public List<Project>? AllProjects { get { return allProjects; } set { allProjects = value; OnPropertyChanged("AllProjects"); } }

        List<YearProject>? yearProjects;
        public List<YearProject>? YearProjects { get { return yearProjects; } set { yearProjects = value; OnPropertyChanged("YearProjects"); } }


        //График сравнения трудоемкости проектов
        public SeriesCollection LineChartSeriesCollection { get; }
        public List<int> DefaultCollection { get; set; }

        //График столбчатый для динамики проектов
        public SeriesCollection ColumnChartCollection { get; set; }
        public SeriesCollection PieChartCollection1 { get; set; }
        public SeriesCollection PieChartCollection2 { get; set; }

        //График выручки по проектам (по годам)
        public SeriesCollection BarChartSeriesCollection { get; set; }

        //Форматирование данных
        public Func<double, string> Formatter { get; set; }
        //Пояснение 
        public string[] BarLabels { get; set; }
        public List<string> BarLabels2 { get; set; }
        public List<string> BarLabels3 { get; set; }

        public AnalyticsVM()
        {
            db.Database.EnsureCreated();
            db.AllProjects.Load();
            AllProjects = db.AllProjects.Local.ToObservableCollection().ToList();

            #region 1 вкладка диаграмм
            ColumnChartCollection = new SeriesCollection();
            PieChartCollection1 = new SeriesCollection();
            PieChartCollection2 = new SeriesCollection();
            BarLabels2 = new List<string>();
            #endregion

            #region 2 вкладка диаграмм
            LineChartSeriesCollection = new SeriesCollection();
            DefaultCollection = new List<int>();
            ChartLaborCosts = new List<List<double>>();
            averageWorkings = new double[15];
            BarLabels = new string[] { "ПЗ", "АР", "ИР", "КР", "ОВ", "ВК", "ЭО", "ТХ", "ПЗУ", "СМ", "ПОС", "СС", "ООС", "ПБ", "ДИ" };
            Formatter = value => value.ToString("N0");
            #endregion

            #region 3 вкладка
            BarChartSeriesCollection = new SeriesCollection();
            BarLabels3 = new List<string>();
            YearProjects = new List<YearProject>();
            #endregion

        }

        #region Сравнение трудозатрат
        //Выбранный элемент из списка документаций
        string? documentation;
        public string? SelectedDocumentation
        {
            get { return documentation; }
            set
            {
                documentation = value;
                ProjectSelection();
                OnPropertyChanged("SelectedDocumentation");
            }
        }

        //Учитывать документацию или нет
        bool accounting = true;
        public bool Accounting
        {
            get { return accounting; }
            set
            {
                accounting = value;
                ProjectSelection();
                OnPropertyChanged("Accounting");
            }
        }

        //Данные рассмотренных проектов
        List<List<double>> ChartLaborCosts { get; set; }
        //Таблица для средних выработок
        double[] averageWorkings;

        //Отбор проектов
        void ProjectSelection()
        {
            if (accounting)
            {
                if (SelectedDocumentation != null)
                {
                    AllProjects = db.AllProjects.Where(x => x.Documentation == SelectedDocumentation).ToList();
                }
            }
            else AllProjects = db.AllProjects.Local.ToObservableCollection().ToList();
            ClearData();
        }

        //Построение графика расчет средних данных
        void BuildGraph(int selectedProject)
        {
            Estimates = db.Estimates.Where(x => x.ProjectId == selectedProject).ToList();
            ChartValues<double> laborCosts = new() { };
            List<double> laborCosts1 = new() { };
            foreach (var item in Estimates)
            {
                if (item.LaborCosts != null)
                {
                    laborCosts.Add((int)item.LaborCosts);
                    laborCosts1.Add((int)item.LaborCosts);
                }
                else { laborCosts.Add(0); laborCosts1.Add(0); }
            }
            LineChartSeriesCollection.Add(new LineSeries
            {

                Title = selectedProjectOne!.Name,
                Values = laborCosts
            });
            ChartLaborCosts.Add(laborCosts1);
            DefaultCollection.Add(selectedProjectOne.Id);


            averageWorkings = new double[15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < ChartLaborCosts.Count; j++)
                {
                    averageWorkings[i] += ChartLaborCosts[j][i];
                }
                averageWorkings[i] = Math.Round(averageWorkings[i] / ChartLaborCosts.Count, 2);
            }
            Assign(averageWorkings);
        }

        //Выбранный проект
        private Project? selectedProjectOne = new();
        public Project? SelectedProjectOne
        {
            get { return selectedProjectOne; }
            set
            {
                selectedProjectOne = value;
                if (selectedProjectOne != null)
                {
                    bool checkIsExist = DefaultCollection.Any(el => el == selectedProjectOne.Id);
                    if (!checkIsExist)
                    {
                        BuildGraph(selectedProjectOne.Id);
                    }
                    else ShowMess("Информация данного проекта ранее была отражена в текущем графике.");

                }

                OnPropertyChanged("SelectedProjectOne");
            }
        }

        //Присвоение значений
        void Assign(double[] averageWorkings)
        {
            AverageWorkings_1 = averageWorkings[0];
            AverageWorkings_2 = averageWorkings[1];
            AverageWorkings_3 = averageWorkings[2];
            AverageWorkings_4 = averageWorkings[3];
            AverageWorkings_5 = averageWorkings[4];
            AverageWorkings_6 = averageWorkings[5];
            AverageWorkings_7 = averageWorkings[6];
            AverageWorkings_8 = averageWorkings[7];
            AverageWorkings_9 = averageWorkings[8];
            AverageWorkings_10 = averageWorkings[9];
            AverageWorkings_11 = averageWorkings[10];
            AverageWorkings_12 = averageWorkings[11];
            AverageWorkings_13 = averageWorkings[12];
            AverageWorkings_14 = averageWorkings[13];
            AverageWorkings_15 = averageWorkings[14];
        }


        //Очистить данные 
        void ClearData()
        {
            SelectedProjectOne = null;
            averageWorkings = new double[15];
            Assign(averageWorkings);
            LineChartSeriesCollection.Clear();
            DefaultCollection.Clear();
        }
        public RelayCommand? ClearDataCommand
        {
            get
            {
                return clearDataCommand ??= new RelayCommand((obj) =>
                {
                    ClearData();
                });
            }
        }

        #region Свойства для полей среднего значения
        double averageWorkings_1;
        public double AverageWorkings_1
        {
            get
            {
                return averageWorkings_1;
            }
            set
            {
                averageWorkings_1 = value;
                OnPropertyChanged("AverageWorkings_1");
            }
        }

        double averageWorkings_2;
        public double AverageWorkings_2
        {
            get
            {
                return averageWorkings_2;
            }
            set
            {
                averageWorkings_2 = value;
                OnPropertyChanged("AverageWorkings_2");
            }
        }

        double averageWorkings_3;
        public double AverageWorkings_3
        {
            get
            {
                return averageWorkings_3;
            }
            set
            {
                averageWorkings_3 = value;
                OnPropertyChanged("AverageWorkings_3");
            }
        }

        double averageWorkings_4;
        public double AverageWorkings_4
        {
            get
            {
                return averageWorkings_4;
            }
            set
            {
                averageWorkings_4 = value;
                OnPropertyChanged("AverageWorkings_4");
            }
        }

        double averageWorkings_5;
        public double AverageWorkings_5
        {
            get
            {
                return averageWorkings_5;
            }
            set
            {
                averageWorkings_5 = value;
                OnPropertyChanged("AverageWorkings_5");
            }
        }

        double averageWorkings_6;
        public double AverageWorkings_6
        {
            get
            {
                return averageWorkings_6;
            }
            set
            {
                averageWorkings_6 = value;
                OnPropertyChanged("AverageWorkings_6");
            }
        }

        double averageWorkings_7;
        public double AverageWorkings_7
        {
            get
            {
                return averageWorkings_7;
            }
            set
            {
                averageWorkings_7 = value;
                OnPropertyChanged("AverageWorkings_7");
            }
        }

        double averageWorkings_8;
        public double AverageWorkings_8
        {
            get
            {
                return averageWorkings_8;
            }
            set
            {
                averageWorkings_8 = value;
                OnPropertyChanged("AverageWorkings_8");
            }
        }

        double averageWorkings_9;
        public double AverageWorkings_9
        {
            get
            {
                return averageWorkings_9;
            }
            set
            {
                averageWorkings_9 = value;
                OnPropertyChanged("AverageWorkings_9");
            }
        }

        double averageWorkings_10;
        public double AverageWorkings_10
        {
            get
            {
                return averageWorkings_10;
            }
            set
            {
                averageWorkings_10 = value;
                OnPropertyChanged("AverageWorkings_10");
            }
        }

        double averageWorkings_11;
        public double AverageWorkings_11
        {
            get
            {
                return averageWorkings_11;
            }
            set
            {
                averageWorkings_11 = value;
                OnPropertyChanged("AverageWorkings_11");
            }
        }

        double averageWorkings_12;
        public double AverageWorkings_12
        {
            get
            {
                return averageWorkings_12;
            }
            set
            {
                averageWorkings_12 = value;
                OnPropertyChanged("AverageWorkings_12");
            }
        }

        double averageWorkings_13;
        public double AverageWorkings_13
        {
            get
            {
                return averageWorkings_13;
            }
            set
            {
                averageWorkings_13 = value;
                OnPropertyChanged("AverageWorkings_13");
            }
        }

        double averageWorkings_14;
        public double AverageWorkings_14
        {
            get
            {
                return averageWorkings_14;
            }
            set
            {
                averageWorkings_14 = value;
                OnPropertyChanged("AverageWorkings_14");
            }
        }

        double averageWorkings_15;
        public double AverageWorkings_15
        {
            get
            {
                return averageWorkings_15;
            }
            set
            {
                averageWorkings_15 = value;
                OnPropertyChanged("AverageWorkings_15");
            }
        }

        double averageWorkings_16;
        public double AverageWorkings_16
        {
            get
            {
                return averageWorkings_16;
            }
            set
            {
                averageWorkings_16 = value;
                OnPropertyChanged("AverageWorkings_16");
            }
        }
        #endregion
        #endregion

        #region Динамика проектов
        //Года начала и продолжение
        int yearS = 1992;
        public int YearS
        {
            get { return yearS; }
            set
            {
                if (value < 1992)
                {
                    ShowMess("Компания ведет рабочую деятельность с 1992 года. Год начала изменен на минимально доступное значение.");
                    yearS = 1992;
                }
                else
                    yearS = value;
                OnPropertyChanged("YearS");
            }
        }

        int yearF = DateTime.Now.Year;
        public int YearF
        {
            get { return yearF; }
            set
            {
                if (value > DateTime.Now.Year)
                {
                    ShowMess("Вы ввели значение, которое превышает значение текущего года. Год окончания изменен на максимально доступное значение.");
                    yearF = DateTime.Now.Year;
                }
                else
                    yearF = value;
                OnPropertyChanged("YearF");
            }
        }

        //Рассмотреть документации или нет
        bool сheckDoc1 = true;
        public bool CheckDoc1
        {
            get { return сheckDoc1; }
            set
            {
                сheckDoc1 = value;
                OnPropertyChanged("CheckDoc1");
            }
        }

        bool сheckDoc2 = true;
        public bool CheckDoc2
        {
            get { return сheckDoc2; }
            set
            {
                сheckDoc2 = value;
                OnPropertyChanged("CheckDoc2");
            }
        }

        bool сheckDoc3 = true;
        public bool CheckDoc3
        {
            get { return сheckDoc3; }
            set
            {
                сheckDoc3 = value;
                OnPropertyChanged("CheckDoс3");
            }
        }

        bool сheckDoc4 = true;
        public bool CheckDoc4
        {
            get { return сheckDoc4; }
            set
            {
                сheckDoc4 = value;
                OnPropertyChanged("CheckDoc4");
            }
        }

        //Выбранный элемент из списка документаций
        string? documentation2;
        public string? SelectedDocumentation2
        {
            get { return documentation2; }
            set
            {
                documentation2 = value;
                OnPropertyChanged("SelectedDocumentation2");
            }
        }

        //Формирование графика по годам 
        void DetailPie(string documentation, int YearS, int YearF, SeriesCollection pie)
        {
            pie.Clear();
            for (int i = YearS; i <= YearF; i++)
            {
                int count = db.AllProjects.Where(p => p.Year == i && p.Documentation == documentation).ToList().Count;
                if (count > 0)
                    pie.Add(new PieSeries
                    {
                        Title = i.ToString(),
                        Values = new ChartValues<int> { count },
                        Fill = M.Brushes.DodgerBlue,
                        DataLabels = true

                    });
            }
        }

        //Нахождение количества проектов в год
        ChartValues<int> CountNumberProjects(String documentation, int YearS, int YearF)
        {
            ChartValues<int> CNP = new();

            for (int i = YearS; i <= YearF; i++)
            {
                CNP.Add(db.AllProjects.Where(p => p.Year == i && p.Documentation == documentation).ToList().Count);
            }
            return CNP;

        }
        //Нахождение сумм по ролям в проекте
        int CountRoleProjects(String role, int YearS, int YearF)
        {
            int count = 0;

            count += db.AllProjects.Where(p => p.Year >= YearS && p.Year <= YearF && p.Role == role).ToList().Count;
            return count;

        }

        //Формирование диаграмм
        public RelayCommand? СreateDataCommand
        {
            get
            {
                return createDataCommand ??= new RelayCommand((obj) =>
                {
                    //Основной график
                    ColumnChartCollection.Clear();
                    BarLabels2.Clear();

                    for (int i = YearS; i <= YearF; i++)
                        BarLabels2.Add(i.ToString());

                    if (CheckDoc1)
                        ColumnChartCollection.Add(new StackedColumnSeries
                        {
                            Title = "Выборочный капитальный ремонт",
                            Values = CountNumberProjects("Выборочный капитальный ремонт", YearS, YearF)
                        });
                    if (CheckDoc2)
                        ColumnChartCollection.Add(new StackedColumnSeries
                        {
                            Title = "Комплексный капитальный ремонт",
                            Values = CountNumberProjects("Комплексный капитальный ремонт", YearS, YearF)
                        });

                    if (CheckDoc3)
                        ColumnChartCollection.Add(new StackedColumnSeries
                        {
                            Title = "Реконструкция",
                            Values = CountNumberProjects("Реконструкция", YearS, YearF)
                        });
                    if (CheckDoc4)
                        ColumnChartCollection.Add(new StackedColumnSeries
                        {
                            Title = "Строительство",
                            Values = CountNumberProjects("Строительство", YearS, YearF)
                        });

                    //Дополнпительный график
                    PieChartCollection2.Clear();
                    PieChartCollection2.Add(new PieSeries
                    {
                        Title = "Генпроектная",
                        Values = new ChartValues<int> { CountRoleProjects("Генпроектная", YearS, YearF) },
                        Fill = M.Brushes.LimeGreen,
                        DataLabels = true

                    });
                    PieChartCollection2.Add(new PieSeries
                    {
                        Title = "Субподрядная",
                        Values = new ChartValues<int> { CountRoleProjects("Субподрядная", YearS, YearF) },
                        Fill = M.Brushes.Orange,
                        DataLabels = true

                    });

                    //Детальный график
                    if (SelectedDocumentation2 != null)
                        DetailPie(SelectedDocumentation2, YearS, yearF, PieChartCollection1);
                });
            }
        }
        #endregion

        #region Выручка с проектов
        //Года начала и продолжение
        int yearS2 = 1992;
        public int YearS2
        {
            get { return yearS2; }
            set
            {
                if (value < 1992)
                {
                    ShowMess("Компания ведет рабочую деятельность с 1992 года. Год начала изменен на минимально доступное значение.");
                    yearS = 19922;
                }
                else
                    yearS2 = value;
                OnPropertyChanged("YearS2");
            }
        }

        int yearF2 = DateTime.Now.Year;
        public int YearF2
        {
            get { return yearF2; }
            set
            {
                if (value > DateTime.Now.Year)
                {
                    ShowMess("Вы ввели значение, которое превышает значение текущего года. Год окончания изменен на максимально доступное значение.");
                    yearF2 = DateTime.Now.Year;
                }
                else
                    yearF2 = value;
                OnPropertyChanged("YearF2");
            }
        }

        //Год детализации
        int? yearDetal = null;
        public int? YearDetal
        {
            get { return yearDetal; }
            set
            {
                if (value > DateTime.Now.Year || value < 1992)
                    ShowMess("Вы ввели значение, выходящее за рамки доступных значений. Значение должно лежать в диапазоне 1992-текущий год.");
                else
                    yearDetal = value;
                OnPropertyChanged("YearDetal");
            }
        }

        #region Максимальная минимальная выручка
        double? minPay = null;
        public double? MinPay
        {
            get { return minPay; }
            set
            {
                minPay = value;
                OnPropertyChanged("MinPay");
            }
        }

        int? minPayYear = null;
        public int? MinPayYear
        {
            get { return minPayYear; }
            set
            {
                minPayYear = value;
                OnPropertyChanged("MinPayYear");
            }
        }

        int? minPayProject = null;
        public int? MinPayProject
        {
            get { return minPayProject; }
            set
            {
                minPayProject = value;
                OnPropertyChanged("MinPayProject");
            }
        }


        double? maxPay = null;
        public double? MaxPay
        {
            get { return maxPay; }
            set
            {
                maxPay = value;
                OnPropertyChanged("MaxPay");
            }
        }

        int? maxPayYear = null;
        public int? MaxPayYear
        {
            get { return maxPayYear; }
            set
            {
                maxPayYear = value;
                OnPropertyChanged("MaxPayYear");
            }
        }

        int? maxPayProject = null;
        public int? MaxPayProject
        {
            get { return maxPayProject; }
            set
            {
                maxPayProject = value;
                OnPropertyChanged("MaxPayProject");
            }
        }
        #endregion

        //Нахождение выручки с проектов в год
        ChartValues<double>? PayDocProjects(String documentation, int YearS2, int YearF2)
        {
            ChartValues<double> CNP = new();

            for (int i = YearS2; i <= YearF2; i++)
            {
                List<Project> projects = db.AllProjects.Where(p => p.Year == i && p.Documentation == documentation).ToList();
                if (projects.Count == 0) CNP.Add(0);
                else
                {
                    foreach (var item in projects)
                    {
                        List<Model.Estimate> estimate = db.Estimates.Where(p => p.ProjectId == item.Id).ToList();
                        if (estimate.Count != 0)
                        {
                            double? sum = 0;
                            foreach (var item1 in estimate)
                            {
                                sum += item1.Pay ?? 0;
                            }
                            if (sum != null) CNP.Add((double)sum);
                            else CNP.Add(0);
                        }
                    }
                }
            }
            return CNP;
        }

        //Нахождение максимального и минимального значение выработок
        List<List<double>> DetailingPay(int YearS2, int YearF2)
        {
            List<List<double>> detail = new();

            for (int year = YearS2; year <= YearF2; year++)
            {
                List<Project> projects = db.AllProjects.Where(p => p.Year == year).ToList();
                if (projects.Count != 0)
                {
                    double? sum = 0;
                    foreach (var item in projects)
                    {
                        List<Model.Estimate> estimate = db.Estimates.Where(p => p.ProjectId == item.Id).ToList();
                        if (estimate.Count != 0)
                        {

                            sum += estimate.Sum(e => e.Pay);

                        }
                    }
                    detail.Add(new List<double>() { (double)sum, year, projects.Count });
                }

            }
            return detail;
        }

        //Формирование диаграмм
        public RelayCommand? Сreate2DataCommand
        {
            get
            {
                return create2DataCommand ??= new RelayCommand((obj) =>
                {
                    //Основной график
                    BarChartSeriesCollection.Clear();
                    BarLabels3.Clear();

                    for (int i = YearS2; i <= YearF2; i++)
                        BarLabels3.Add(i.ToString());

                    BarChartSeriesCollection.Add(new StackedRowSeries
                    {
                        Title = "Строительство",
                        Values = PayDocProjects("Строительство", YearS2, YearF2)
                    });

                    BarChartSeriesCollection.Add(new StackedRowSeries
                    {
                        Title = "Выборочный капитальный ремонт",
                        Values = PayDocProjects("Выборочный капитальный ремонт", YearS2, YearF2)
                    });

                    BarChartSeriesCollection.Add(new StackedRowSeries
                    {
                        Title = "Комплексный капитальный ремонт",
                        Values = PayDocProjects("Комплексный капитальный ремонт", YearS2, YearF2)
                    });

                    BarChartSeriesCollection.Add(new StackedRowSeries
                    {
                        Title = "Реконструкция",
                        Values = PayDocProjects("Реконструкция", YearS2, YearF2)
                    });

                    //Мнимальные и максимальные значения
                    List<List<double>> detailPay = DetailingPay(YearS2, YearF2);
                    if (detailPay.Count > 0)
                    {
                        int idMax = 0;
                        int idMin = 0;
                        if (detailPay.Count == 1)
                        {
                            MaxPay = detailPay[idMax][0]; MaxPayYear = (int)detailPay[idMax][1]; MaxPayProject = (int)detailPay[idMax][2];
                            MinPay = null; MinPayYear = null; MinPayProject = null;
                        }
                        else
                        {
                            for (int i = 0; i < detailPay.Count; i++)
                            {
                                if (detailPay[i][0] > detailPay[idMax][0])
                                    idMax = i;
                                if (detailPay[i][0] < detailPay[idMin][0])
                                    idMin = i;
                            }
                            MaxPay = detailPay[idMax][0]; MaxPayYear = (int)detailPay[idMax][1]; MaxPayProject = (int)detailPay[idMax][2];
                            MinPay = detailPay[idMin][0]; MinPayYear = (int)detailPay[idMin][1]; MinPayProject = (int)detailPay[idMin][2];
                        }
                    }
                    else
                    {
                        MinPay = null; MinPayYear = null; MinPayProject = null;
                        MaxPay = null; MaxPayYear = null; MaxPayProject = null;
                    }
                });
            }
        }

        //Детализация проектов
        public RelayCommand? DetailDataCommand
        {
            get
            {
                return detailDataCommand ??= new RelayCommand((obj) =>
                {
                    if (YearDetal == null)
                        ShowMess("Год для детализации данных не указан. Введите данные и повторите действия.");
                    else{
                        List<Project> projects = db.AllProjects.Where(p => p.Year == yearDetal).ToList();
                        List<YearProject> newList = new ();
                        foreach (var project in projects)
                        {
                            newList!.Add(new YearProject
                            {
                                Name = project.Name,
                                Pay = db.Estimates.Where(p => p.ProjectId == project.Id).ToList().Sum(p => p.Pay),
                            });
                        }
                        YearProjects = newList;
                    }
                });
            }
        }
        #endregion



        //Команда возвращение на вкладку во всеми проектами
        public RelayCommand? BackCommandAllProject
        {
            get
            {
                return backCommandAllProject ??= new RelayCommand((obj) =>
                {
                    MainWindow.myFrame!.NavigationService.RemoveBackEntry();
                    MainWindow.myFrame!.Content = new AllProjects();
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

        //Дополнительный класс
        public class YearProject : INotifyPropertyChanged
        {
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


            double? pay;
            public double? Pay
            {
                get { return pay; }
                set
                {
                    pay = value;
                    OnPropertyChanged("Pay");
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
