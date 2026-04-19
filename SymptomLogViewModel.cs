using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HealthKeeper;

public class SymptomLogViewModel : INotifyPropertyChanged
{
    
    public ObservableCollection<string> Symptoms { get; set; } = new ObservableCollection<string>();

    
    public bool HasNoRecords => Symptoms.Count == 0;

    
    public void AddSymptom(string newSymptom)
    {
        Symptoms.Add(newSymptom);
       
        OnPropertyChanged(nameof(HasNoRecords));
    }

    public void RemoveSymptom(string symptomToRemove)
    {
        if (Symptoms.Contains(symptomToRemove))
        {
            Symptoms.Remove(symptomToRemove);
            
            OnPropertyChanged(nameof(HasNoRecords));
        }
    }

    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}