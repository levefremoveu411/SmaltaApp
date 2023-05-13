using Word = Microsoft.Office.Interop.Word;
using System;
using Microsoft.Win32;
using SmaltaApp.Model;
using System.Windows;
using SmaltaApp.View;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using SmaltaApp.Model.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SmaltaApp.ViewModel;
using System.Collections.Generic;
using SmaltaApp.Model.AdditionalClasses;
using Microsoft.Office.Interop.Word;
using System.Data;

namespace SmaltaApp.Documentation
{
    public class WordHelper : IDisposable
    {
        readonly ApplicationContext db = new();

        public Word.Application? application;
        readonly private Document? document;

        public WordHelper(string fileName, Project project)
        {
            application = new Word.Application();
            document = application.Documents.Open(fileName);
            application.Visible = true;
            CreateDocument(project);
        }

        public void CreateDocument(Project project)
        {
            db.Database.EnsureCreated();


            #region ОСНОВНЫЕ ДАННЫЕ ПО ПРОЕКТУ
            //Наименование
            document!.Bookmarks["Наименование"].Range.Text = project.Name;
            //Фото
            if (project.Image != null)
            {
                MemoryStream memory = new(project.Image);
                BitmapImage bitmap = new();
                bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                Clipboard.SetImage(bitmap);
                document!.Bookmarks["ФотоПроекта"].Range.Paste();
            }
            else document!.Bookmarks["ФотоПроекта"].Range.Text = "(ФОТО ОТСУТСТВУЕТ)";
            //Город
            db.Cities.Load();
            document!.Bookmarks["Город"].Range.Text = project.City!.Name ?? "ГОРОД";
            //Год
            document!.Bookmarks["Год"].Range.Text = project.Year.ToString();
            //Разработчик
            db.Developers.Load();
            document!.Bookmarks["Разработчик"].Range.Text = project.Developer!.Abbreviation ?? "РАЗРАБОТЧИК";
            //Роль
            document!.Bookmarks["Роль"].Range.Text = project.Role;
            //Заказчик
            db.Customers.Load();
            document!.Bookmarks["Заказчик"].Range.Text = project.Customer!.Abbreviation ?? "ЗАКАЗЧИК";
            //Договор
            document!.Bookmarks["Договор"].Range.Text = project.Treaty;
            //Дата
            document!.Bookmarks["Дата"].Range.Text = project.DateOfConclusion!.Value.ToShortDateString();
            //Документация
            document!.Bookmarks["Документация"].Range.Text = project.Documentation;
            #endregion

            #region КЛИМАТИЧЕСКИЕ ОСОБЕННОСТИ МЕСТНОСТИ
            ClimaticCondition? climatic = db.ClimaticConditions.FirstOrDefault(c => c.ProjectId == project.Id);
            if (climatic != null)
            {
                //Район
                document!.Bookmarks["Район"].Range.Text = String.Format("{0} {1}", climatic.Region, climatic.SubRegion);
                //Температура холодной пятидневки
                document!.Bookmarks["ТемператураП"].Range.Text = climatic.TemperatureColdW.ToString();
                //Температура наиболее холодных суток
                document!.Bookmarks["ТемператураС"].Range.Text = climatic.TemperatureColdD.ToString();
                //Снеговой район
                db.SnowyRegions.Load();
                document!.Bookmarks["Снеговой"].Range.Text = climatic.SnowyRegion?.Name!.ToString();
                //Числовое значение
                document!.Bookmarks["ЗначениеС"].Range.Text = climatic.SnowyRegion?.Value!.ToString();
                //Ветровой район
                db.WindRegions.Load();
                document!.Bookmarks["Ветровой"].Range.Text = climatic.WindRegion?.Name!.ToString();
                //Числовое значение
                document!.Bookmarks["ЗначениеВ"].Range.Text = climatic.WindRegion?.Value!.ToString();
                //Уровень ответсвенности
                db.Responsibilities.Load();
                document!.Bookmarks["Уровень"].Range.Text = climatic.Responsibility?.Level;
                //Класс соотружения
                document!.Bookmarks["Класс"].Range.Text = climatic.Responsibility?.Class;
                //Коэффициент надежности
                document!.Bookmarks["Коэффициент"].Range.Text = climatic.Responsibility?.Coefficient!.ToString();
                //Карта
                document!.Bookmarks["Карта"].Range.Text = climatic.Map;
                //Результат тестирования
                document!.Bookmarks["Микрорайонирование"].Range.Text = climatic.SeismicityTest!.ToString();
                //Сейсмичность по проекту
                document!.Bookmarks["Сейсмичность"].Range.Text = climatic.SeismicityProject!.ToString();
            }
            #endregion

            #region ХАРАКТЕРИСТИКА ЗДАНИЯ ПО ПРОЕКТУ
            //Основные характеристики
            Charecteristic? charecteristic = db.Charecteristics.FirstOrDefault(x => x.ProjectId == project.Id);
            if (charecteristic != null)
            {
                document!.Bookmarks["Категория"].Range.Text = charecteristic.Category;
                document!.Bookmarks["Срок"].Range.Text = charecteristic.LifeTime;
                document!.Bookmarks["Огнестойкость"].Range.Text = charecteristic.FireResistance.ToString();
                document!.Bookmarks["ПлощадьЗастройки"].Range.Text = charecteristic.BuiltUpArea.ToString();
                document!.Bookmarks["ПлощадьЗастройки2"].Range.Text = charecteristic.BuiltUpArea.ToString();
            }
            //Объемы здания
            BuildingVolume? buildingVolume = db.BuildingVolumes.FirstOrDefault(x => x.ProjectId == project.Id);
            if (buildingVolume != null)
            {
                document!.Bookmarks["НаземныйО"].Range.Text = buildingVolume.GroundPart.ToString();
                document!.Bookmarks["ЦокольныйО"].Range.Text = buildingVolume.Basement.ToString();
                document!.Bookmarks["ВходнойО"].Range.Text = buildingVolume.EntrancePart.ToString();
                document!.Bookmarks["Объем"].Range.Text = (buildingVolume.GroundPart + buildingVolume.Basement + buildingVolume.EntrancePart).ToString();
            }
            //Площади здания
            BuildingArea? buildingArea = db.BuildingAreas.FirstOrDefault(x => x.ProjectId == project.Id);
            if (buildingArea != null)
            {
                document!.Bookmarks["ТОП"].Range.Text = buildingArea.ShopsArea.ToString();
                document!.Bookmarks["АОП"].Range.Text = buildingArea.OfficesArea.ToString();
                document!.Bookmarks["ЖОП"].Range.Text = buildingArea.ApartmentsArea.ToString();
                document!.Bookmarks["ПОП"].Range.Text = buildingArea.OtherArea.ToString();
                document!.Bookmarks["ОбщаяПлощадь"].Range.Text = (buildingArea.ShopsArea + buildingArea.ApartmentsArea + buildingArea.OfficesArea + buildingArea.OtherArea).ToString();

                document!.Bookmarks["ТПП"].Range.Text = buildingArea.EffectiveShopsArea.ToString();
                document!.Bookmarks["АПП"].Range.Text = buildingArea.EffectiveOfficesArea.ToString();
                document!.Bookmarks["ЖПП"].Range.Text = buildingArea.EffectiveApartmentsArea.ToString();
                document!.Bookmarks["ППП"].Range.Text = buildingArea.EffectiveOtherArea.ToString();
                document!.Bookmarks["ПолезнаяПлощадь"].Range.Text = (buildingArea.EffectiveShopsArea + buildingArea.EffectiveApartmentsArea + buildingArea.EffectiveOfficesArea + buildingArea.EffectiveOtherArea).ToString();

            }
            //Высота здания
            BuildingHeight? buildingHeight = db.BuildingHeights.FirstOrDefault(x => x.ProjectId == project.Id);
            if (buildingHeight != null)
            {
                document!.Bookmarks["Типовые"].Range.Text = buildingHeight.CountGroundFloors.ToString();
                document!.Bookmarks["Типовые2"].Range.Text = buildingHeight.CountGroundFloors.ToString();
                document!.Bookmarks["Цокольные"].Range.Text = buildingHeight.CountBasements.ToString();
                document!.Bookmarks["Цокольные2"].Range.Text = buildingHeight.CountBasements.ToString();

                document!.Bookmarks["ПервыйВ"].Range.Text = buildingHeight.FirstFloor.ToString();
                document!.Bookmarks["ВторойВ"].Range.Text = buildingHeight.SecondFloor.ToString();
                document!.Bookmarks["ТиповойВ"].Range.Text = buildingHeight.TypicalFloor.ToString();
                document!.Bookmarks["ЦокольныйВ"].Range.Text = buildingHeight.GroundFloor.ToString();
                document!.Bookmarks["ЦокольныйВ2"].Range.Text = buildingHeight.GroundFloor.ToString();
                document!.Bookmarks["ТехническийВ"].Range.Text = buildingHeight.TechnicalFloor.ToString();
                document!.Bookmarks["НулевойВ"].Range.Text = buildingHeight.NullFloor.ToString();

                double? height = buildingHeight!.FirstFloor + buildingHeight!.SecondFloor + buildingHeight!.TechnicalFloor +
                    buildingHeight!.NullFloor + (buildingHeight!.CountGroundFloors - 2) * buildingHeight!.TypicalFloor;
                document!.Bookmarks["ОбщаяВысота"].Range.Text = height.ToString();
                document!.Bookmarks["ВысотаЗдания"].Range.Text = height.ToString();
                document!.Bookmarks["ВысотаЗдания2"].Range.Text = height.ToString();

            }
            #endregion

            #region ФУНКЦИОНАЛЬНЫЕ НАЗНАЧЕНИЯ ЗДАНИЯ
            db.Appointments.Load();
            db.FloorTypes.Load();
            List<FunctionalPurpose>? functionalPurposes = db.FunctionalPurposes.Where(c => c.ProjectId == project.Id).ToList();
            if (functionalPurposes.Count != 0)
            {
                Word.Table table1 = document.Tables.Add(document.Bookmarks["ТаблицаНазначений"].Range, functionalPurposes.Count + 1, 2);
                table1.Select();
                table1.Borders.Enable = 1;
                table1.Cell(1, 1).Range.Text = "Назначение этажа";
                table1.Cell(1, 2).Range.Text = "Тип этажа";

                for (int i = 0; i < functionalPurposes.Count; i++)
                {
                    table1.Cell(i + 2, 1).Range.Text = functionalPurposes[i].Appointment?.Name;
                    table1.Cell(i + 2, 2).Range.Text = functionalPurposes[i].FloorType?.Name;
                }
            }
            else
            {
                document.Bookmarks["ТаблицаНазначений"].Range.Text = "Данные отсутсвуют";
            }
            #endregion

            #region ЗЕМЕЛЬНЫЙ УЧАСТОК И ОГРАЖДАЮЩИЕ КОНСТРУКЦИИ
            //Земельный участок и ограждающие конструкции
            Model.LandAndFence? landAndFence = db.LandAndFences.FirstOrDefault(x => x.ProjectId == project.Id);
            if (landAndFence != null)
            {
                document!.Bookmarks["ПлощадьУчастка"].Range.Text = landAndFence.LandArea.ToString();
                document!.Bookmarks["ПлощадьАсфальта"].Range.Text = landAndFence.AsphaltArea.ToString();
                document!.Bookmarks["ПлощадьОзеленения"].Range.Text = landAndFence.GreenArea.ToString();

                document!.Bookmarks["СТстен"].Range.Text = landAndFence.WallsST.ToString();
                document!.Bookmarks["СТпокрытия"].Range.Text = landAndFence.CoatingsST.ToString();
                document!.Bookmarks["СТокон"].Range.Text = landAndFence.WindowsST.ToString();
            }

            //Огнестойкость несущих конструкций
            FireResistance? fireResistance = db.FireResistances.FirstOrDefault(x => x.ProjectId == project.Id);
            if (fireResistance != null)
            {
                if (fireResistance.Section600 != null)
                {
                    document!.Bookmarks["ОС600"].Range.Text = fireResistance.Section600.ToString();
                    if (fireResistance.Section600 >= 120) document!.Bookmarks["А600"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["А600"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Section500 != null)
                {
                    document!.Bookmarks["ОС500"].Range.Text = fireResistance.Section500.ToString();
                    if (fireResistance.Section500 >= 120) document!.Bookmarks["А500"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["А500"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Section400 != null)
                {
                    document!.Bookmarks["ОС400"].Range.Text = fireResistance.Section400.ToString();
                    if (fireResistance.Section400 >= 120) document!.Bookmarks["А400"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["А400"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Beams != null)
                {
                    document!.Bookmarks["ОР"].Range.Text = fireResistance.Beams.ToString();
                    if (fireResistance.Beams >= 120) document!.Bookmarks["АР"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["АР"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Walls != null)
                {
                    document!.Bookmarks["ОСН"].Range.Text = fireResistance.Walls.ToString();
                    if (fireResistance.Walls >= 30) document!.Bookmarks["АСН"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["АСН"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Coatings != null)
                {
                    document!.Bookmarks["ОМП"].Range.Text = fireResistance.Coatings.ToString();
                    if (fireResistance.Coatings >= 60) document!.Bookmarks["АМП"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["АМП"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.FlightsOfStairs != null)
                {
                    document!.Bookmarks["ОЛМ"].Range.Text = fireResistance.FlightsOfStairs.ToString();
                    if (fireResistance.FlightsOfStairs >= 60) document!.Bookmarks["АЛМ"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["АЛМ"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Partitions != null)
                {
                    document!.Bookmarks["ОВП"].Range.Text = fireResistance.Partitions.ToString();
                    if (fireResistance.Partitions >= 120) document!.Bookmarks["АВП"].Range.Text = "Соответствует  БП!";
                    else document!.Bookmarks["АВП"].Range.Text = "Не соответствует БП!";
                }

                if (fireResistance.Connections != null)
                {
                    document!.Bookmarks["ОВС"].Range.Text = fireResistance.Connections.ToString();
                    if (fireResistance.Connections >= 15) document!.Bookmarks["АВС"].Range.Text = "Соответсвует БП!";
                    else document!.Bookmarks["АВС"].Range.Text = "Не соответсвует БП!";
                }
            }

            #endregion

            #region ИНЖЕНЕРНЫЕ СИСТЕМЫ
            //Водоснабжение и водоотведение
            WaterParameter? waterParameter = db.WaterParameters.FirstOrDefault(x => x.ProjectId == project.Id);
            if (waterParameter != null)
            {
                //холодная вода
                document!.Bookmarks["ВодоснабжениеДоговор"].Range.Text = waterParameter.Treaty;
                document!.Bookmarks["Огранизация"].Range.Text = waterParameter.Vodokanal;
                document!.Bookmarks["Врезка"].Range.Text = waterParameter.Street;
                document!.Bookmarks["Диаметр"].Range.Text = waterParameter.Diameter.ToString();

                document!.Bookmarks["ХВсут"].Range.Text = waterParameter.DayCold.ToString();
                document!.Bookmarks["ХВсут2"].Range.Text = waterParameter.DayCold.ToString();

                document!.Bookmarks["ХВчас"].Range.Text = waterParameter.HourCold.ToString();
                document!.Bookmarks["ХВчас2"].Range.Text = waterParameter.HourCold.ToString();

                document!.Bookmarks["ХВсек"].Range.Text = waterParameter.SecondCold.ToString();
                document!.Bookmarks["ХВсек2"].Range.Text = waterParameter.SecondCold.ToString();

                document!.Bookmarks["ХВпож"].Range.Text = waterParameter.FireFighting.ToString();

                //горячая вода
                document!.Bookmarks["ГВсут"].Range.Text = waterParameter.DayHot.ToString();
                document!.Bookmarks["ГВсут2"].Range.Text = waterParameter.DayHot.ToString();

                document!.Bookmarks["ГВчас"].Range.Text = waterParameter.HourHot.ToString();
                document!.Bookmarks["ГВчас2"].Range.Text = waterParameter.HourHot.ToString();

                document!.Bookmarks["ГВсек"].Range.Text = waterParameter.SecondHot.ToString();
                document!.Bookmarks["ГВсек2"].Range.Text = waterParameter.SecondHot.ToString();

                //водоотведение
                document!.Bookmarks["ОбщСут"].Range.Text = (waterParameter.DayCold + waterParameter.DayHot).ToString();
                document!.Bookmarks["ОбщЧас"].Range.Text = (waterParameter.HourCold + waterParameter.HourHot).ToString();
                document!.Bookmarks["ОбщСек"].Range.Text = (waterParameter.SecondCold + waterParameter.SecondHot).ToString();

                document!.Bookmarks["ГодовоеПотребление"].Range.Text = Math.Round(Convert.ToDouble((waterParameter.DayCold + waterParameter.DayHot) * 365), 2).ToString();
            }

            //Электроснабжение
            ElectricityParameter? electricityParameter = db.ElectricityParameters.FirstOrDefault(x => x.ProjectId == project.Id);
            if (electricityParameter != null)
            {
                document!.Bookmarks["ТПЭ"].Range.Text = electricityParameter.ShopsArea.ToString();
                document!.Bookmarks["АПЭ"].Range.Text = electricityParameter.OfficesArea.ToString();
                document!.Bookmarks["ЖПЭ"].Range.Text = electricityParameter.ApartmentsArea.ToString();
                document!.Bookmarks["ППЭ"].Range.Text = electricityParameter.OtherArea.ToString();

                document!.Bookmarks["СуммаЭлектро"].Range.Text = (electricityParameter.ShopsArea + electricityParameter.OfficesArea + electricityParameter.ApartmentsArea + electricityParameter.OtherArea).ToString();
            }

            //Отопление и вентиляция
            HeatParameter? heatParameter = db.HeatParameters.FirstOrDefault(x => x.ProjectId == project.Id);
            if (heatParameter != null)
            {
                document!.Bookmarks["ТПО"].Range.Text = heatParameter.ShopsArea.ToString();
                document!.Bookmarks["АПО"].Range.Text = heatParameter.OfficesArea.ToString();
                document!.Bookmarks["ЖПО"].Range.Text = heatParameter.ApartmentsArea.ToString();
                document!.Bookmarks["ППО"].Range.Text = heatParameter.OtherArea.ToString();

                document!.Bookmarks["СуммаТепло"].Range.Text = (heatParameter.ShopsArea + heatParameter.OfficesArea + heatParameter.ApartmentsArea + heatParameter.OtherArea).ToString();
            }

            #endregion

            #region КОНСТРУКТИВНОЕ РЕШЕНИЕ
            db.FoundationTypes.Load();
            db.TechnologyTypes.Load();
            db.ConstructionTypes.Load();
            db.ConstructiveTypes.Load();
            db.SchemeTypes.Load();
            db.StabilityTypes.Load();
            db.StairTypes.Load();
            db.LiftTypes.Load();
            db.HouseTopTypes.Load();
            db.RoofTypes.Load();
            db.RoofingMaterials.Load();
            db.AirShaftTypes.Load();
            db.ExteriorFinishes.Load();
            db.InsulationMaterials.Load();

            //Характеристики конструктивной схемы здания (сооружения)
            BuildingScheme? buildingScheme = db.BuildingSchemes.FirstOrDefault(x => x.ProjectId == project.Id);
            if (buildingScheme != null)
            {
                document!.Bookmarks["Форма"].Range.Text = buildingScheme.Form;
                document!.Bookmarks["Сложность"].Range.Text = buildingScheme.Complexity;
                document!.Bookmarks["Ширина"].Range.Text = buildingScheme.Width.ToString();
                document!.Bookmarks["Длина"].Range.Text = buildingScheme.Length.ToString();
                document!.Bookmarks["КонструктивныйТип"].Range.Text = buildingScheme.ConstructiveType?.Name;

                document!.Bookmarks["ВертикальнаяУст"].Range.Text = db.StabilityTypes.FirstOrDefault(x => x.Id == buildingScheme!.VerticalStabilityId)?.Name;
                document!.Bookmarks["ГоризонтальнаяУст"].Range.Text = db.StabilityTypes.FirstOrDefault(x => x.Id == buildingScheme!.HorizontalStabilityId)?.Name;
                document!.Bookmarks["ПоперечнаяУст"].Range.Text = db.StabilityTypes.FirstOrDefault(x => x.Id == buildingScheme!.LateralStabilityId)?.Name;
                document!.Bookmarks["ПродольнаяУст"].Range.Text = db.StabilityTypes.FirstOrDefault(x => x.Id == buildingScheme!.LongitudinalStabilityId)?.Name;


                document!.Bookmarks["ПоперечнаяСхема"].Range.Text = db.SchemeTypes!.FirstOrDefault(x => x.Id == buildingScheme!.LateralSchemeId)?.Name; ;
                document!.Bookmarks["ПоперПролеты"].Range.Text = buildingScheme.CountLateralSpan.ToString();
                document!.Bookmarks["ШагиШирина"].Range.Text = buildingScheme.LateralSteps;

                document!.Bookmarks["ПродольнаяСхема"].Range.Text = db.SchemeTypes!.FirstOrDefault(x => x.Id == buildingScheme!.LongitudinalStabilityId)?.Name;
                document!.Bookmarks["ПродПролеты"].Range.Text = buildingScheme.CountLongitudinalSpan.ToString();
                document!.Bookmarks["ШагиДлина"].Range.Text = buildingScheme.LongitudinalSteps;
            }

            //Конструкция здания (сооружения) ниже отметки 0.00
            BuildingFoundation? buildingFoundation = db.BuildingFoundations.FirstOrDefault(x => x.ProjectId == project.Id);
            if (buildingFoundation != null)
            {
                document!.Bookmarks["ТипФундамента"].Range.Text = buildingFoundation.FoundationType?.Name;
                document!.Bookmarks["ТехнологияФундамента"].Range.Text = buildingFoundation.TechnologyType?.Name;

                document!.Bookmarks["ТолщинаПлиты"].Range.Text = buildingFoundation.ThicknessSlab.ToString();
                document!.Bookmarks["МатериалПлиты"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.Slab)?.Name;
                document!.Bookmarks["ТолщинаНадбалка"].Range.Text = buildingFoundation.ThicknessSlabTwo.ToString();
                document!.Bookmarks["МатериалНадбалки"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.SlabTwo)?.Name;

                document!.Bookmarks["РазмерностьПодкол"].Range.Text = buildingFoundation.DimensionUnderСolumns;
                document!.Bookmarks["МатериалПодколонников"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.UnderСolumns)?.Name;
                document!.Bookmarks["РазмерностьКолонны"].Range.Text = buildingFoundation.DimensionСolumns;
                document!.Bookmarks["МатериалКолонны"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.Сolumns)?.Name;

                document!.Bookmarks["ТолщинаСтены"].Range.Text = buildingFoundation.ThicknessWalls.ToString();
                document!.Bookmarks["МатериалСтены"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.Walls)?.Name;

                document!.Bookmarks["ТолщинаРигели"].Range.Text = buildingFoundation.ThicknessСrossbars.ToString();
                document!.Bookmarks["МатериалРигели"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.Сrossbars)?.Name;
                document!.Bookmarks["ТолщинаПерекрытия"].Range.Text = buildingFoundation.ThicknessFloors.ToString();
                document!.Bookmarks["МатериалПерекрытия"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingFoundation!.Floors)?.Name;

                document!.Bookmarks["БетонНиже0"].Range.Text = string.Format("Бетон класса В{0}, F{1}, W{2}", buildingFoundation.ConcreteB, buildingFoundation.ConcreteF, buildingFoundation.ConcreteW);
                document!.Bookmarks["СтальНиже0"].Range.Text = buildingFoundation.SteelGrade;
            }
            //Конструкция здания (сооружения) выше отметки 0.00
            BuildingStructure? buildingStructure = db.BuildingStructures.FirstOrDefault(x => x.ProjectId == project.Id);
            if (buildingStructure != null)
            {
                document!.Bookmarks["НесущиеКострукции"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingStructure!.BearingStructuresId)?.Name;
                document!.Bookmarks["КолонныВыше0"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingStructure!.ColumnsId)?.Name;
                document!.Bookmarks["РигелиВыше0"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingStructure!.СrossbarsId)?.Name;
                document!.Bookmarks["СтеныВыше0"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingStructure!.WallsId)?.Name;
                document!.Bookmarks["ПерекрытияВыше0"].Range.Text = db.ConstructionTypes!.FirstOrDefault(x => x.Id == buildingStructure!.FloorsId)?.Name;

                document!.Bookmarks["БетонВыше0"].Range.Text = string.Format("Бетон класса В{0}, F{1}, W{2}", buildingStructure.ConcreteB, buildingStructure.ConcreteF, buildingStructure.ConcreteW);
                document!.Bookmarks["СтальВыше0"].Range.Text = buildingStructure.SteelGrade;

                document!.Bookmarks["Лестница"].Range.Text = buildingStructure.StairType?.Name;
                document!.Bookmarks["Косоуры"].Range.Text = buildingStructure.Beam;

                document!.Bookmarks["Лифт"].Range.Text = buildingStructure.LiftType?.Name;
                document!.Bookmarks["ТолщинаЛифта"].Range.Text = buildingStructure.ThicknessLift.ToString();

                document!.Bookmarks["Крыша"].Range.Text = buildingStructure.HouseTopType?.Name;
                document!.Bookmarks["Водосток"].Range.Text = buildingStructure.Gully;
                document!.Bookmarks["ТипКровли"].Range.Text = buildingStructure.RoofType?.Name;
                document!.Bookmarks["МатериалКровли"].Range.Text = buildingStructure.RoofingMaterial?.Name;

                document!.Bookmarks["ВысотаПарапет"].Range.Text = buildingStructure.ParapetHeight.ToString();
                document!.Bookmarks["ПримечаниеПарапет"].Range.Text = buildingStructure.ParapetNote;

                document!.Bookmarks["МатериалОтмостки"].Range.Text = string.Format("Бетон класса В{0}, F{1}, W{2}", buildingStructure.ConcreteBb, buildingStructure.ConcreteFf, buildingStructure.ConcreteWw);
                document!.Bookmarks["ТолщинаБетона"].Range.Text = buildingStructure.ThicknessConcrete.ToString();
                document!.Bookmarks["Подготовка"].Range.Text = buildingStructure.Preparation;
                document!.Bookmarks["ТолщинаПодготовки"].Range.Text = buildingStructure.ThicknessPreparation.ToString();

                document!.Bookmarks["МатериалУтеплителя"].Range.Text = buildingStructure.InsulationMaterial?.Name;
                document!.Bookmarks["ПлотностьУтеплителя"].Range.Text = buildingStructure.Density;
                document!.Bookmarks["ТолщинаУтеплителя"].Range.Text = buildingStructure.ThicknessInsulation.ToString();

                document!.Bookmarks["ВентШахта"].Range.Text = buildingStructure.AirShaftType?.Name;
                document!.Bookmarks["ОтделкаФасада"].Range.Text = buildingStructure.ExteriorFinish?.Name;
            }

            #endregion

            #region РАСЧЕТНЫЕ ОБОСНОВАНИЯ КОНСТРУКТИВНОГО РЕШЕНИЯ
            //Основные данные
            RationaleInfo? rationaleInfo = db.RationaleInfos.FirstOrDefault(x => x.ProjectId == project.Id);
            if (rationaleInfo != null)
            {
                db.Softwares.Load();
                document!.Bookmarks["НаименованиеПО"].Range.Text = rationaleInfo.Software?.Name;
                document!.Bookmarks["РазработчикПО"].Range.Text = rationaleInfo.Software?.Developer;
                document!.Bookmarks["Сертификат"].Range.Text = rationaleInfo.Software?.Сertificate;

                if (rationaleInfo.ImageOne != null)
                {
                    MemoryStream memory = new(rationaleInfo.ImageOne);
                    BitmapImage bitmap = new();
                    bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                    Clipboard.SetImage(bitmap);
                    document!.Bookmarks["РМД"].Range.Paste();
                }
                else document!.Bookmarks["РМД"].Range.Text = "(РМД-ПРОЕКТ ОТСУТСТВУЕТ)";
                if (rationaleInfo.ImageTwo != null)
                {
                    MemoryStream memory = new(rationaleInfo.ImageTwo);
                    BitmapImage bitmap = new();
                    bitmap.BeginInit(); bitmap.StreamSource = memory; bitmap.EndInit();
                    Clipboard.SetImage(bitmap);
                    document!.Bookmarks["Эпюра"].Range.Paste();
                }
                else document!.Bookmarks["Эпюра"].Range.Text = "(ЭПЮРА ОТСУТСТВУЕТ)";
            }

            //Сведения о выработках
            IgeWorking? igeWorking = db.IgeWorkings.FirstOrDefault(x => x.ProjectId == project.Id);
            if (igeWorking != null)
            {
                document!.Bookmarks["Шифр"].Range.Text = igeWorking.Report;
                document!.Bookmarks["ГодВыработки"].Range.Text = igeWorking.Year.ToString();
                document!.Bookmarks["ОрганизацияВыработки"].Range.Text = igeWorking.Organization;

                List<GeologicalWork>? geologicalWorks = db.GeologicalWorks.Where(c => c.IgeWorkingId == igeWorking.Id).ToList();
                if (geologicalWorks.Count != 0)
                {
                    Word.Table table3 = document.Tables.Add(document.Bookmarks["ТаблицаВыработки"].Range, geologicalWorks.Count * 2 + 1, 6);
                    table3.Select();

                    table3.Borders.Enable = 1;
                    table3.Cell(1, 1).Range.Text = "Объемный вес";
                    table3.Cell(1, 2).Range.Text = "Модуль деформации";
                    table3.Cell(1, 3).Range.Text = "Удельное сцепление";
                    table3.Cell(1, 4).Range.Text = "Угол внутр-го трения";
                    table3.Cell(1, 5).Range.Text = "Показатель текучести";
                    table3.Cell(1, 6).Range.Text = "Примечание";

                    for (int i = 0; i < geologicalWorks.Count; i++)
                    {
                        table3.Rows[i * 2 + 2].Cells[1].Merge(table3.Rows[i * 2 + 2].Cells[6]);
                        table3.Rows[i * 2 + 2].Cells[1].Range.Text = geologicalWorks[i].Name + "; мощность (м) - " + geologicalWorks[i].Power.ToString();

                        table3.Cell(i * 2 + 3, 1).Range.Text = geologicalWorks[i].Weight!.ToString();
                        table3.Cell(i * 2 + 3, 2).Range.Text = geologicalWorks[i].Deformation!.ToString();
                        table3.Cell(i * 2 + 3, 3).Range.Text = geologicalWorks[i].Clutch!.ToString();
                        table3.Cell(i * 2 + 3, 4).Range.Text = geologicalWorks[i].Friction.ToString();
                        table3.Cell(i * 2 + 3, 5).Range.Text = geologicalWorks[i].Fluidity.ToString();
                        table3.Cell(i * 2 + 3, 6).Range.Text = geologicalWorks[i].Note;
                    }

                }
            }

            //Сведения о колебаниях здания
            List<ProjectFluctuation>? projectFluctuations = db.ProjectFluctuations.Where(c => c.ProjectId == project.Id).ToList();
            if (projectFluctuations.Count != 0)
            {
                Word.Table table4 = document.Tables.Add(document.Bookmarks["ТаблицаКолебания"].Range, projectFluctuations.Count + 1, 4);
                table4.Select();
                table4.Columns[1].Width = 190;
                table4.Columns[2].Width = 95;
                table4.Columns[3].Width = 95;
                table4.Columns[4].Width = 95;


                table4.Borders.Enable = 1;
                table4.Cell(1, 1).Range.Text = "Кол-во учтенных форм";
                table4.Cell(1, 2).Range.Text = "W, рад/с";
                table4.Cell(1, 3).Range.Text = "f, Гц";
                table4.Cell(1, 4).Range.Text = "T, с";

                for (int i = 0; i < projectFluctuations.Count; i++)
                {
                    table4.Cell(i + 2, 1).Range.Text = projectFluctuations[i].CountForm!.ToString();
                    table4.Cell(i + 2, 2).Range.Text = projectFluctuations[i].W!.ToString();
                    table4.Cell(i + 2, 3).Range.Text = projectFluctuations[i].F!.ToString();
                    table4.Cell(i + 2, 4).Range.Text = projectFluctuations[i].T.ToString();
                }
            }
            #endregion

            #region СМЕТА ПО ПРОЕКТУ
            db.Chapters.Load();
            List<Model.Estimate>? estimates = db.Estimates.Where(c => c.ProjectId == project.Id).ToList();
            if (estimates.Count != 0)
            {
                Word.Table table4 = document.Tables.Add(document.Bookmarks["ТаблицаСмета"].Range, estimates.Count + 1, 5);
                table4.Select();
                table4.Columns[1].Width = 30;
                table4.Columns[2].Width = 50;
                table4.Columns[3].Width = 225;
                table4.Columns[4].Width = 80;
                table4.Columns[5].Width = 95;


                table4.Borders.Enable = 1;
                table4.Cell(1, 1).Range.Text = "№";
                table4.Cell(1, 2).Range.Text = "Марка";
                table4.Cell(1, 3).Range.Text = "Наименование раздела";
                table4.Cell(1, 4).Range.Text = "Выплата з.п. (руб)";
                table4.Cell(1, 5).Range.Text = "Трудозатраты (чел/день)";

                for (int i = 0; i < estimates.Count; i++)
                {
                    table4.Cell(i + 2, 1).Range.Text = (i + 1).ToString();
                    table4.Cell(i + 2, 2).Range.Text = estimates[i].Chapter!.Abbreviation;
                    table4.Cell(i + 2, 3).Range.Text = estimates[i].Chapter!.Name;
                    table4.Cell(i + 2, 4).Range.Text = estimates[i].Pay.ToString();
                    table4.Cell(i + 2, 5).Range.Text = estimates[i].LaborCosts.ToString();
                }

                document!.Bookmarks["СуммаФОТ"].Range.Text = (Math.Round(Convert.ToDouble(estimates!.Sum(p => p.Pay) / 0.87), 2)).ToString();
            }
            #endregion

            //SaveAs();

        }
        public void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Word документ (*.docx)|.docx|Все файлы (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                object fileName = saveFileDialog.FileName;
                document!.SaveAs(fileName);
            }
        }

        public void Dispose()
        {
            try
            {
                application!.Quit();
            }
            catch { }
        }
    }
}
