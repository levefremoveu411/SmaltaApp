using SmaltaApp.Model;
using SmaltaApp.View;
using SmaltaApp.View.Windows;
using System.Windows;

namespace SmaltaApp.ViewModel
{
    public class ProjectVM
    {
        //Объявление методов
        private RelayCommand? openCommandBasicData;
        private RelayCommand? openCommandAllProjects;
        private RelayCommand? openCommandBuildingCharacteristics;
        private RelayCommand? openCommandLandAndFence;
        private RelayCommand? openCommandEstimate;
        private RelayCommand? openCommandEngineeringSystems;
        private RelayCommand? openCommanConstructiveSolution;
        private RelayCommand? openCommandRationale;

        readonly Project Project;
        //Конструктор
        public ProjectVM(Project project)
        {
            Project = project;
            ProjectPage.MyProjectFrame!.Content = new BasicData(Project);
            MainWindow.myTitle!.Text = "Основные исходные данные по проекту";
        }

        //Команда открытия окна с списком всех проектов
        public RelayCommand? OpenCommandAllProjects
        {
            get
            {
                return openCommandAllProjects ??= new RelayCommand((obj) =>
                {
                    MainWindow.myFrame!.NavigationService.RemoveBackEntry();
                    MainWindow.myFrame!.Content = new AllProjects();
                    MainWindow.myTitle!.Text = "Справочно-аналитическая база объектов архитектурно-строительного проектирования";
                });
            }
        }

        //Команда открытия окна с основными данными по проекту
        public RelayCommand? OpenCommandBasicData
        {
            get
            {
                return openCommandBasicData ??= new RelayCommand((obj) =>
                {
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new BasicData(Project);
                    MainWindow.myTitle!.Text = "Основные исходные данные по проекту";
                });
            }
        }

        //Команда открытия окна с характеристикой здания по проекту
        public RelayCommand? OpenCommandBuildingCharacteristics
        {
            get
            {
                return openCommandBuildingCharacteristics ??= new RelayCommand((obj) =>
                {
                    if(Project.Id == 0)
                    {
                        ShowMess("Для дальнейшей работы необходимо добавить и сохранить информацию по исходных данным, расположенным на текущей вкладке!");
                        return;
                    }
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new BuildingСharacteristics(Project.Id);
                    MainWindow.myTitle!.Text = "Характеристика здания по проекту";
                });
            }
        }

        //Команда открытия окна с данными по показателями земли и ограждающих конструкций 
        public RelayCommand? OpenCommandLandAndFence
        {
            get
            {
                return openCommandLandAndFence ??= new RelayCommand((obj) =>
                {
                    if (Project.Id == 0)
                    {
                        ShowMess("Для дальнейшей работы необходимо добавить и сохранить информацию по исходных данным, расположенным на текущей вкладке!");
                        return;
                    }
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new View.LandAndFence(Project.Id);
                    MainWindow.myTitle!.Text = "Показатели земельного участка, эффективности ограждающих конструкций и огнестойкости несущих конструкций";
                });
            }
        }


        //Команда открытия окна с данными по конструктивному решению
        public RelayCommand? OpenCommanConstructiveSolution
        {
            get
            {
                return openCommanConstructiveSolution ??= new RelayCommand((obj) =>
                {
                    if (Project.Id == 0)
                    {
                        ShowMess("Для дальнейшей работы необходимо добавить и сохранить информацию по исходных данным, расположенным на текущей вкладке!");
                        return;
                    }
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new ConstructiveSolution(Project.Id);
                    MainWindow.myTitle!.Text = "Конструктивное решение по проекту";
                });
            }
        }

        //Команда открытия окна с 
        public RelayCommand? OpenCommandRationale
        {
            get
            {
                return openCommandRationale ??= new RelayCommand((obj) =>
                {
                    if (Project.Id == 0)
                    {
                        ShowMess("Для дальнейшей работы необходимо добавить и сохранить информацию по исходных данным, расположенным на текущей вкладке!");
                        return;
                    }
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new Rationale(Project.Id);
                    MainWindow.myTitle!.Text = "Обоснование конструктивного решения";
                });
            }
        }


        //Команда открытия окна с данными инженерным системам
        public RelayCommand? OpenCommandEngineeringSystems
        {
            get
            {
                return openCommandEngineeringSystems ??= new RelayCommand((obj) =>
                {
                    if (Project.Id == 0)
                    {
                        ShowMess("Для дальнейшей работы необходимо добавить и сохранить информацию по исходных данным, расположенным на текущей вкладке!");
                        return;
                    }
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new EngineeringSystems(Project.Id);
                    MainWindow.myTitle!.Text = "Инженерные системы";
                });
            }
        }

        //Команда открытия окна с данными по смете 
        public RelayCommand? OpenCommandEstimate
        {
            get
            {
                return openCommandEstimate ??= new RelayCommand((obj) =>
                {
                    if (Project.Id == 0)
                    {
                        ShowMess("Для дальнейшей работы необходимо добавить и сохранить информацию по исходных данным, расположенным на текущей вкладке!");
                        return;
                    }
                    ProjectPage.MyProjectFrame!.NavigationService.RemoveBackEntry();
                    ProjectPage.MyProjectFrame!.Content = new View.Estimate(Project.Id);
                    MainWindow.myTitle!.Text = "Смета по проекту";
                });
            }
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
    }
}
