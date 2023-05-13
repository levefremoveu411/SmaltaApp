using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model;
using SmaltaApp.Model.AdditionalClasses;
using SmaltaApp.Model.DataBase;
using SmaltaApp.Model.HelperClasses;
using SmaltaApp.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Windows.Controls;
using SmaltaApp.Documentation;
using System.IO;

namespace SmaltaApp.ViewModel
{
    public class EstimateVM : INotifyPropertyChanged
    {
        //Получение базы данных
        readonly ApplicationContext db = new();

        //Объявление таймера
        readonly DispatcherTimer dispatcherTimer;

        //Объявление методов
        private RelayCommand? addCommandChapter;

        private RelayCommand? addCommandEstimate;
        private RelayCommand? editCommandEstimate;
        private RelayCommand? saveCommandEstimate;
        private RelayCommand? cancelCommandEstimate;
        private RelayCommand? deleteCommandEstimate;
        private RelayCommand? createCommandExcelDocumentation;

        //Таблицы
        public ObservableCollection<Chapter>? Chapters { get; set; }
        List<Estimate>? estimates;
        public List<Estimate>? Estimates
        {
            get { return estimates; }
            set
            {
                estimates = value;

                OnPropertyChanged("Estimates");
            }
        }

        readonly Estimate? defalutEstimate;
        readonly int ProjectId;
        //Конструктор
        public EstimateVM(int projectID)
        {
            ProjectId = projectID;
            db.Database.EnsureCreated();
            db.Chapters.Load();
            Chapters = db.Chapters.Local.ToObservableCollection();

            Estimates = db.Estimates.Where(c => c.ProjectId == ProjectId).ToList();
            if (Estimates.Count == 0)
            {
                foreach (Chapter chapter in Chapters)
                {
                    db.Estimates.Add(new()
                    {
                        ProjectId = projectID,
                        Chapter = chapter,
                    });
                }
                db.SaveChanges();
                Estimates = db.Estimates.Where(c => c.ProjectId == ProjectId).ToList();
            }

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ClearStatus!);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);

            defalutEstimate = new() { ProjectId = projectID };
        }

        #region Свойства для интерфейса 
        //Свойство для доступности полей при редактировании товара
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
        //Цвет окантовки группбокса
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

        //Общая сумма работ
        public double? SummPay
        {
            get { return Math.Round(Convert.ToDouble(Estimates!.Sum(p => p.Pay) / 0.87), 2); }
            set
            {
                OnPropertyChanged("SummPay");
            }
        }
        //Общая сумма времени
        public int? SummLaborCost
        {
            get { return Estimates!.Sum(p => p.LaborCosts); }
            set
            {
                OnPropertyChanged("SummLaborCost");
            }
        }

        //Выделенная запить в списке
        private Estimate? selectedEstimates = new();
        public Estimate? SelectedEstimates
        {
            get { return selectedEstimates; }
            set
            {
                selectedEstimates = value;
                OnPropertyChanged("SelectedEstimates");
            }
        }


        #region Свойства для полей редактирования
        //Выбранный элемент из списка критериев
        public Chapter? NewSelectedChapter
        {
            get { return defalutEstimate!.Chapter!; }
            set
            {
                defalutEstimate!.Chapter = value;
                OnPropertyChanged("NewSelectedChapter");
            }
        }
        public string SelectedChapterText
        {
            get
            {
                if (defalutEstimate!.Chapter != null)
                    return defalutEstimate!.Chapter!.Name!;
                else return "";
            }
            set
            {
                OnPropertyChanged("SelectedChapterText");
            }

        }

        //Выплата
        public string? NewPay
        {
            get
            {
                if (defalutEstimate!.Pay == null)
                    return string.Empty;
                else
                    return defalutEstimate!.Pay.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutEstimate!.Pay = Convert.ToDouble(value);
                else
                    defalutEstimate!.Pay = null;
                OnPropertyChanged("NewPay");
            }
        }
        //Трудозатраты
        public string? NewLaborCosts
        {
            get
            {
                if (defalutEstimate!.LaborCosts == null)
                    return string.Empty;
                else
                    return defalutEstimate!.LaborCosts.ToString();
            }
            set
            {
                if (value != string.Empty)
                    defalutEstimate!.LaborCosts = Convert.ToInt32(value);
                else
                    defalutEstimate!.LaborCosts = null;
                OnPropertyChanged("NewLaborCosts");
            }
        }

        #endregion


        #region Команды редактирования записей сметы
        //Добавление
        public RelayCommand? AddCommandEstimate
        {
            get
            {
                return addCommandEstimate ??= new RelayCommand((obj) =>
                {
                    IsEnabled = true; ColorGroup = Brushes.White;
                   
                });
            }
        }

        //Изменение
        public RelayCommand? EditCommandEstimate
        {
            get
            {
                return editCommandEstimate ??= new RelayCommand((obj) =>
                {
                    if (SelectedEstimates!.Id != 0)
                    {
                        IsEnabled = true; ColorGroup = Brushes.SandyBrown;
                        //defalutEstimate!.Id = selectedEstimates!.Id;
                        NewPay = SelectedEstimates!.Pay.ToString();
                        NewLaborCosts = SelectedEstimates!.LaborCosts.ToString();
                        NewSelectedChapter = SelectedEstimates.Chapter;
                        
                    }
                    else { ShowMess("Изменение невозможно. Запись для изменения не выбрана."); }
                });
            }
        }

        //Сохранить изменения
        public RelayCommand? SaveCommandEstimate
        {
            get
            {
                return saveCommandEstimate ??= new RelayCommand((obj) =>
                {
                    if (defalutEstimate!.Pay != null && defalutEstimate!.LaborCosts != null)
                    {
                        //if (addition)
                        //{
                        //    bool checkIsExist = db.Estimates.Any(el => el.ProjectId == defalutEstimate.ProjectId && el.Chapter!.Id == defalutEstimate.Chapter.Id);
                        //    if (!checkIsExist)
                        //    {
                        //        db.Estimates.Add(new()
                        //        {
                        //            ProjectId = defalutEstimate.ProjectId,
                        //            Pay = defalutEstimate.Pay,
                        //            LaborCosts = defalutEstimate.LaborCosts,
                        //            Chapter = defalutEstimate.Chapter,
                        //        });
                        //    }
                        //    else ShowMess("Добавление невозможно.Данный критерий уже отражен в текущей смете. Вы можете произвости изменения данной записи.");
                        //}

                        //else
                        //{
                        selectedEstimates!.Pay = defalutEstimate.Pay;
                        selectedEstimates!.LaborCosts = defalutEstimate.LaborCosts;
                        selectedEstimates.Chapter = defalutEstimate.Chapter;
                        db.Entry(selectedEstimates!).State = EntityState.Modified;
                        // }


                        db.SaveChanges();
                        SummPay = Math.Round(Convert.ToDouble(Estimates!.Sum(p => p.Pay) / 0.87), 2);
                        SummLaborCost = Estimates!.Sum(p => p.LaborCosts);
                        //Estimates = db.Estimates.Where(c => c.ProjectId == ProjectId).ToList();

                        IsEnabled = false; ColorGroup = Brushes.Transparent;
                        NewPay = string.Empty; NewLaborCosts = string.Empty;
                        NewSelectedChapter = null;
                    }
                    else ShowMess("Для сохранения данных необходимо заполнить все поля.");
                });
            }
        }

        //Удаление
        public RelayCommand? CancelCommandEstimate
        {
            get
            {
                return cancelCommandEstimate ??= new RelayCommand((obj) =>
                {
                    IsEnabled = false; ColorGroup = Brushes.Transparent;
                    NewPay = string.Empty; NewLaborCosts = string.Empty;
                });
            }
        }

        //Удаление
        public RelayCommand? DeleteCommandEstimate
        {
            get
            {
                return deleteCommandEstimate ??= new RelayCommand((obj) =>
                {
                    if (SelectedEstimates!.Id != 0)
                    {

                        db.Estimates.Remove(SelectedEstimates);
                        db.SaveChanges();
                        Estimates = db.Estimates.Where(c => c.ProjectId == ProjectId).ToList();
                    }
                    else { ShowMess("Удаление невозможно. Запись для удаления не выбрана."); }
                });
            }
        }
        #endregion

        //Добавление критерия
        public RelayCommand? AddCommandChapter
        {
            get
            {
                return addCommandChapter ??= new RelayCommand((obj) =>
                {
                    AddCompany window = new(new Company())
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    if (window.ShowDialog() == true)
                    {
                        Chapter chapter = new()
                        {
                            Name = window.NewCompany.Name,
                            Abbreviation = window.NewCompany.Abbreviation,
                        };
                        bool checkIsExist = db.Chapters.Any(el => el.Abbreviation!.ToLower() == chapter.Abbreviation!.ToLower());
                        if (!checkIsExist)
                        {
                            db.Chapters.Add(chapter);
                            db.SaveChanges();
                        }

                    }
                });
            }
        }

        //Формирование документа excel
        public RelayCommand? CreateCommandExcelDocumentation
        {
            get
            {
                return createCommandExcelDocumentation ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        using ExcelHelper excel = new(Directory.GetCurrentDirectory() + @"\Расчетная форма проекта.xlsx");
                        var sheet = excel.application!.Workbooks[1].Worksheets[1];

                        sheet.Cells[2, 9].Value = DateTime.Now.ToShortDateString();
                        sheet.Cells[4, 4].Value = db.AllProjects.First(c => c.Id == ProjectId).Name;

                        for (int i = 0; i < Estimates!.Count; i++)
                        {
                            sheet.Cells[8 + i, 5].Value = Estimates[i].LaborCosts;
                            sheet.Cells[8 + i, 9].Value = Estimates[i].Pay;
                        }
                        excel.SaveAs();
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
