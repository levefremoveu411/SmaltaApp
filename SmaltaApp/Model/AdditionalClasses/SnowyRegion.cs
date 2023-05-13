using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SmaltaApp.Model.AdditionalClasses
{
    public class SnowyRegion : INotifyPropertyChanged
    {
        //Код
        public int Id { get; set; }

        //Наименование региона
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

        //Числовое значение 
        float? valueR;
        public float? Value
        {
            get { return valueR; }
            set
            {
                valueR = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
