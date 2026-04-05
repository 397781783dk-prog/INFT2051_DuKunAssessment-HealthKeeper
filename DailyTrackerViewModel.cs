using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NotificationRequest = Plugin.LocalNotification.Core.Models.NotificationRequest; // 引入 Task

namespace HealthKeeper;

public class HealthTask : INotifyPropertyChanged
{
    private string _name;
    public string Name
    {
        get => _name;
        set { if (_name != value) { _name = value; OnPropertyChanged(); } }
    }

    private bool _isCompleted;
    public bool IsCompleted
    {
        get => _isCompleted;
        set { if (_isCompleted != value) { _isCompleted = value; OnPropertyChanged(); } }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


public class DailyTrackerViewModel : INotifyPropertyChanged
{
    public ObservableCollection<HealthTask> Tasks { get; set; }

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

    public void AddTask(string name)
    {
        var newTask = new HealthTask { Name = name };
        newTask.PropertyChanged += Task_PropertyChanged;
        Tasks.Add(newTask);
        UpdateProgress();
    }

    public void RemoveTask(HealthTask task)
    {
        if (Tasks.Contains(task))
        {
            task.PropertyChanged -= Task_PropertyChanged;
            Tasks.Remove(task);
            UpdateProgress();
        }
    }

    private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HealthTask.IsCompleted))
        {
            UpdateProgress();
        }
    }

    private void UpdateProgress()
    {
        OnPropertyChanged(nameof(ProgressText));
        OnPropertyChanged(nameof(ProgressValue));
    }


    public async Task ScheduleReminder(TimeSpan selectedTime)
    {
        try
        {
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            DateTime notifyTime = DateTime.Today.Add(selectedTime);

            if (notifyTime < DateTime.Now)
            {
                notifyTime = notifyTime.AddDays(1);
            }

            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "HealthKeeper Remainder",
                Description = "You still have unfinished health tasks today. Come and check in quickly！",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
        }
        catch (Exception ex)
        {
          
            throw new Exception($"The system rejected the notification request or an error occurred: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}