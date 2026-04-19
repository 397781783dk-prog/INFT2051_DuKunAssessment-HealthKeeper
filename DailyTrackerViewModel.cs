using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;
using SQLite; // 1. Introduce the database namespace
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using NotificationRequest = Plugin.LocalNotification.Core.Models.NotificationRequest;

namespace HealthKeeper;

// 2. Add a "database identity tag" to the task model
[Table("health_tasks")]
public class HealthTask : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }

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
    // 3. Declare a database administrator
    private DatabaseService _dbService;

    public ObservableCollection<HealthTask> Tasks { get; set; } = new();

    public string ProgressText => Tasks.Count == 0 ? "0/0" : $"{Tasks.Count(t => t.IsCompleted)}/{Tasks.Count}";
    public double ProgressValue => Tasks.Count == 0 ? 0 : (double)Tasks.Count(t => t.IsCompleted) / Tasks.Count;

    public DailyTrackerViewModel()
    {
        _dbService = new DatabaseService();
        LoadTasksFromDatabase(); //  4. Instead of using dummy data on startup, it will fetch data from the database.
    }

    
    private async void LoadTasksFromDatabase()
    {
        var items = await _dbService.GetTasksAsync();
        Tasks.Clear();
        foreach (var item in items)
        {
            item.PropertyChanged += Task_PropertyChanged;
            Tasks.Add(item);
        }
        UpdateProgress();
    }

    // 5. When adding a task, save it to the hard disk synchronously
    public async void AddTask(string name)
    {
        var newTask = new HealthTask { Name = name };
        await _dbService.SaveTaskAsync(newTask); 

        newTask.PropertyChanged += Task_PropertyChanged;
        Tasks.Add(newTask);
        UpdateProgress();
    }

    //  6. When deleting a task, erase it from the hard drive simultaneously
    public async void RemoveTask(HealthTask task)
    {
        if (Tasks.Contains(task))
        {
            await _dbService.DeleteTaskAsync(task); 
            task.PropertyChanged -= Task_PropertyChanged;
            Tasks.Remove(task);
            UpdateProgress();
        }
    }

    // 7. Update the status on the hard disk when checking or renaming a task
    private async void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HealthTask.IsCompleted) || e.PropertyName == nameof(HealthTask.Name))
        {
            var task = sender as HealthTask;
            await _dbService.SaveTaskAsync(task); // Update the database
            UpdateProgress();
        }
    }

    private void UpdateProgress()
    {
        OnPropertyChanged(nameof(ProgressText));
        OnPropertyChanged(nameof(ProgressValue));
    }

    //
    public async Task ScheduleReminder(TimeSpan selectedTime)
    {
        try
        {
            // 1. Check notification permissions: request user authorization if not enabled (mandatory for iOS/Android)
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }
            // 2. Calculate notification time: Today + selected time (e.g. 20:00)
            DateTime notifyTime = DateTime.Today.Add(selectedTime);
            // 3.Cross - day processing: If the current time has passed, automatically set to the same time the next day
            if (notifyTime < DateTime.Now)
            {
                notifyTime = notifyTime.AddDays(1);
            }
            // 4. Construct the notification request (title, content, trigger time)
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
            // 6. Exception catching: permission denied, system errors, etc.
            throw new Exception($"The system rejected the notification request or an error occurred: {ex.Message}");
        }
    }
    // MVVM Property Change Notification(Unrelated to Notification Functionality, Used for UI Binding)
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}