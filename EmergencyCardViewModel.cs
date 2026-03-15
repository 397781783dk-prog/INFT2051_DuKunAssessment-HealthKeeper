using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage; // 引入 MAUI 的本地存储魔法

namespace HealthKeeper;

public class EmergencyCardViewModel : INotifyPropertyChanged
{
    // 姓名
    public string Name
    {
        // Get: 每次界面需要显示名字时，去本地小本本里找，找不到就返回空字符串 ""
        get => Preferences.Default.Get(nameof(Name), "");
        set
        {
            // Set: 只要用户在界面上打字修改了名字，立刻覆盖写进本地小本本！
            Preferences.Default.Set(nameof(Name), value);
            OnPropertyChanged();
        }
    }

    // 血型
    public string BloodType
    {
        get => Preferences.Default.Get(nameof(BloodType), "");
        set
        {
            Preferences.Default.Set(nameof(BloodType), value);
            OnPropertyChanged();
        }
    }

    // 药物过敏史
    public string DrugAllergies
    {
        get => Preferences.Default.Get(nameof(DrugAllergies), "");
        set
        {
            Preferences.Default.Set(nameof(DrugAllergies), value);
            OnPropertyChanged();
        }
    }

    // 紧急联系电话 (默认先给个 120)
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