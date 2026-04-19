using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage; 

namespace HealthKeeper;

public class EmergencyCardViewModel : INotifyPropertyChanged
{
   
    public string Name
    {
   
        get => Preferences.Default.Get(nameof(Name), "");
        set
        {
         
            Preferences.Default.Set(nameof(Name), value);
            OnPropertyChanged();
        }
    }

  
    public string BloodType
    {
        get => Preferences.Default.Get(nameof(BloodType), "");
        set
        {
            Preferences.Default.Set(nameof(BloodType), value);
            OnPropertyChanged();
        }
    }

 
    public string DrugAllergies
    {
        get => Preferences.Default.Get(nameof(DrugAllergies), "");
        set
        {
            Preferences.Default.Set(nameof(DrugAllergies), value);
            OnPropertyChanged();
        }
    }

    
    public string EmergencyPhone
    {
        get => Preferences.Default.Get(nameof(EmergencyPhone), "120");
        set
        {
            Preferences.Default.Set(nameof(EmergencyPhone), value);
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}