using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HealthKeeper;

public class SymptomLogViewModel : INotifyPropertyChanged
{
    // 存放所有记录的症状列表
    public ObservableCollection<string> Symptoms { get; set; } = new ObservableCollection<string>();

    // 魔法开关：当列表数量为 0 时，返回 true（显示虚线框）；否则返回 false（隐藏虚线框）
    public bool HasNoRecords => Symptoms.Count == 0;

    // 添加新症状的方法
    public void AddSymptom(string newSymptom)
    {
        Symptoms.Add(newSymptom);
        // 通知界面刷新虚线框的显示状态
        OnPropertyChanged(nameof(HasNoRecords));
    }

    // 删除症状的方法
    public void RemoveSymptom(string symptomToRemove)
    {
        if (Symptoms.Contains(symptomToRemove))
        {
            Symptoms.Remove(symptomToRemove);
            // 通知界面刷新虚线框的显示状态
            OnPropertyChanged(nameof(HasNoRecords));
        }
    }

    // 触发界面更新的接口
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}