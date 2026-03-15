using Microsoft.Maui.Controls;
using System;

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

    // 框状态改变时播放划线动画
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
                    // 划线从左向右长出来
                    await strikeLine.ScaleXTo(1, 250, Easing.CubicOut);
                }
                else // 取消勾选
                {
                    // 文字恢复原样
                    taskLabel.FadeTo(1, 250);
                    // 线条缩回去
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

    //修改任务名称
    private async void OnEditTaskTapped(object sender, TappedEventArgs e)
    {
        var label = sender as Label;
        var task = label?.BindingContext as HealthTask;

        if (task != null)
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
    }
}