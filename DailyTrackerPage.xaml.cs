using Microsoft.Maui.Controls;
using System;
using Microsoft.Maui.ApplicationModel; 

namespace HealthKeeper;

public partial class DailyTrackerPage : ContentPage
{
    private DailyTrackerViewModel _viewModel;

    public DailyTrackerPage()
    {
        InitializeComponent();
        _viewModel = new DailyTrackerViewModel();
        BindingContext = _viewModel;
    }

    private async void OnReminderClicked(object sender, EventArgs e)
    {
        try
        {
          
            TimeSpan selectedTime = ReminderTimePicker.Time.GetValueOrDefault();

            await _viewModel.ScheduleReminder(selectedTime);

            string timeString = selectedTime.ToString(@"hh\:mm");
            await DisplayAlert("Settings saved", $"The system will remind you to check in on time at {timeString} every day!", "OK");
        }
        catch (Exception ex)
        {
       
            await DisplayAlert("Setup failed", ex.Message, "I see");
        }
    }

 
    private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var parentGrid = checkBox?.Parent as Grid;
        if (parentGrid != null)
        {
            var textGrid = parentGrid.Children[1] as Grid;
            var taskLabel = textGrid?.Children[0] as Label;
            var strikeLine = textGrid?.Children[1] as BoxView;

            if (taskLabel != null && strikeLine != null)
            {
                if (e.Value)
                {
                    taskLabel.FadeTo(0.5, 250);
                    await strikeLine.ScaleXTo(1, 250, Easing.CubicOut);
                }
                else
                {
                    taskLabel.FadeTo(1, 250);
                    await strikeLine.ScaleXTo(0, 250, Easing.CubicIn);
                }
            }
        }
    }

   
    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        string userInput = await DisplayPromptAsync(
            "New Habit",
            "What habit do you want to track?",
            "Add", "Cancel",
            placeholder: "e.g., Read 10 pages");

        if (!string.IsNullOrWhiteSpace(userInput))
        {
            _viewModel.AddTask(userInput);
        }
    }

    private void OnDeleteTaskClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var task = button?.BindingContext as HealthTask;

        if (task != null)
        {
            _viewModel.RemoveTask(task);
        }
    }

    
    private void OnEditTaskTapped(object sender, TappedEventArgs e)
    {
        var label = sender as Label;
        var task = label?.BindingContext as HealthTask;

        if (task != null)
        {

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    string newName = await DisplayPromptAsync(
                        "Edit Habit",
                        "Update the name of your habit:",
                        "Save", "Cancel",
                        initialValue: task.Name);

                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        task.Name = newName;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred in the pop-up window: {ex.Message}");
                }
            });
        }
    }
}