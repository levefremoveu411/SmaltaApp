using Microsoft.EntityFrameworkCore;
using SmaltaApp.Model.AdditionalClasses;

namespace SmaltaApp.Model.DataBase
{
    class ApplicationContext : DbContext
    {
        //Раздел исходные данные
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Developer> Developers { get; set; } = null!;
        public DbSet<Project> AllProjects { get; set; } = null!;
       
        //Раздел климатические особенности местности
        public DbSet<ClimaticCondition> ClimaticConditions { get; set; } = null!;
        public DbSet<SnowyRegion> SnowyRegions { get; set; } = null!;
        public DbSet<WindRegion> WindRegions { get; set; } = null!;
        public DbSet<Responsibility> Responsibilities { get; set; } = null!;

        //Раздел технико-экономичекие характеристики
        public DbSet<Charecteristic> Charecteristics { get; set; } = null!;
        public DbSet<BuildingArea> BuildingAreas { get; set; } = null!;
        public DbSet<BuildingVolume> BuildingVolumes { get; set; } = null!;
        public DbSet<BuildingHeight> BuildingHeights { get; set; } = null!;

        //Раздел функциональные назначения здания
        public DbSet<FloorType> FloorTypes { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<FunctionalPurpose> FunctionalPurposes { get; set; } = null!;

        //Раздел показатели земельного участка и огрождений
        public DbSet<FireResistance> FireResistances { get; set; } = null!;
        public DbSet<LandAndFence> LandAndFences { get; set; } = null!;

        //Раздел консруктивное решение
        //часть 1
        public DbSet<BuildingScheme> BuildingSchemes { get; set; } = null!;

        public DbSet<ConstructiveType> ConstructiveTypes { get; set; } = null!;
        public DbSet<SchemeType> SchemeTypes { get; set; } = null!;
        public DbSet<StabilityType> StabilityTypes { get; set; } = null!;

        //часть 2
        public DbSet<BuildingFoundation> BuildingFoundations { get; set; } = null!;

        public DbSet<FoundationType> FoundationTypes { get; set; } = null!;
        public DbSet<TechnologyType> TechnologyTypes { get; set; } = null!;

        //часть 3
        public DbSet<BuildingStructure> BuildingStructures { get; set; } = null!;

        public DbSet<ConstructionType> ConstructionTypes { get; set; } = null!;
        public DbSet<StairType> StairTypes { get; set; } = null!;
        public DbSet<LiftType> LiftTypes { get; set; } = null!;
        public DbSet<HouseTopType> HouseTopTypes { get; set; } = null!;
        public DbSet<RoofType> RoofTypes { get; set; } = null!;
        public DbSet<RoofingMaterial> RoofingMaterials { get; set; } = null!;
        public DbSet<AirShaftType> AirShaftTypes { get; set; } = null!;
        public DbSet<ExteriorFinish> ExteriorFinishes { get; set; } = null!;
        public DbSet<InsulationMaterial> InsulationMaterials { get; set; } = null!;

        //Раздел обоснования конструктивного решения
        public DbSet<ProjectFluctuation> ProjectFluctuations { get; set; } = null!;
        public DbSet<IgeWorking> IgeWorkings { get; set; } = null!;
        public DbSet<GeologicalWork> GeologicalWorks { get; set; } = null!;
        public DbSet<Software> Softwares { get; set; } = null!;
        public DbSet<RationaleInfo> RationaleInfos { get; set; } = null!;

        //Раздел инженерных систем
        public DbSet<WaterParameter> WaterParameters { get; set; } = null!;
        public DbSet<ElectricityParameter> ElectricityParameters { get; set; } = null!;
        public DbSet<HeatParameter> HeatParameters { get; set; } = null!;

        //Раздел сметы 
        public DbSet<Chapter> Chapters { get; set; } = null!;
        public DbSet<Estimate> Estimates { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SmaltaDB.db");
        }
    }
}
