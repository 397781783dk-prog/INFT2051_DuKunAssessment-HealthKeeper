using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HealthKeeper;

// 任务数据模型
public class HealthTask : INotifyPropertyChanged
{
    private string _name;
    public string Name // 任务名称
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isCompleted;
    public bool IsCompleted // 是否已勾选
    {
        get => _isCompleted;
        set
        {
            if (_isCompleted != value)
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// 页面逻辑模型
public class DailyTrackerViewModel : INotifyPropertyChanged
{
    public ObservableCollection<HealthTask> Tasks { get; set; }

    // 计算进度
    public string ProgressText => Tasks.Count == 0 ? "0/0" : $"{Tasks.Count(t => t.IsCompleted)}/{Tasks.Count}";
    public double ProgressValue => Tasks.Count == 0 ? 0 : (double)Tasks.Count(t => t.IsCompleted) / Tasks.Count;

    public DailyTrackerViewModel()
    {
        Tasks = new ObservableCollection<HealthTask>
        {
            new HealthTask { Name = "Morning Medicine" },
            new HealthTask { Name = "Noon Medicine" },
            new HealthTask { Name = "Evening Medicine" },
            new HealthTask { Name = "Drink 2000ml Water" },
            new HealthTask { Name = "Blood Pressure Check" }
        };

        foreach (var task in Tasks)
        {
            task.PropertyChanged += Task_PropertyChanged;
        }
    }

    //添加任务
    public void AddTask(string name)
    {
        var newTask = new HealthTask { Name = name };
        newTask.PropertyChanged += Task_PropertyChanged; // 给新任务也绑上监听
        Tasks.Add(newTask);
        UpdateProgress();
    }

    //删除任务
    public void RemoveTask(HealthTask task)
    {
        if (Tasks.Contains(task))
        {
            task.PropertyChanged -= Task_PropertyChanged; // 移除监听
            Tasks.Remove(task);
            UpdateProgress();
        }
    }

    // 当任务被勾选时触发
    private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HealthTask.IsCompleted))
        {
            UpdateProgress();
        }
    }

    // 更新进度条
    private void UpdateProgress()
    {
        OnPropertyChanged(nameof(ProgressText));
        OnPropertyChanged(nameof(ProgressValue));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}