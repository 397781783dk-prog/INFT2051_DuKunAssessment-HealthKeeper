using Microsoft.Maui.Controls;
using System;

namespace HealthKeeper;

public partial class SymptomLogPage : ContentPage
{
    private SymptomLogViewModel _viewModel;

    public SymptomLogPage()
    {
        InitializeComponent();

        // 初始化 ViewModel 并绑定上下文
        _viewModel = new SymptomLogViewModel();
        BindingContext = _viewModel;
    }

    // 点击加号添加的逻辑
    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        // 弹出输入框
        string userInput = await DisplayPromptAsync(
            title: "New Symptom",
            message: "What are you feeling today?",
            accept: "Save",
            cancel: "Cancel",
            placeholder: "e.g., Headache, Fever");

        // 如果用户输入了内容，则添加到列表中
        if (!string.IsNullOrWhiteSpace(userInput))
        {
            _viewModel.AddSymptom(userInput);
        }
    }

    // 点击 ❌ 号删除的逻辑
    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // 获取触发点击的按钮
        var button = sender as Button;

        // 获取绑定在这个按钮上的具体症状文字
        var symptomToDelete = button?.CommandParameter as string;

        // 执行删除
        if (!string.IsNullOrEmpty(symptomToDelete))
        {
            _viewModel.RemoveSymptom(symptomToDelete);
        }
    }
}