using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SmaltaApp.Model.HelperClasses
{
    public class Company : INotifyPropertyChanged
    {

        //Полное наименование компании
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

        //Аббревиатура
        string? abbreviation;
        public string? Abbreviation
        {
            get { return abbreviation; }
            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
