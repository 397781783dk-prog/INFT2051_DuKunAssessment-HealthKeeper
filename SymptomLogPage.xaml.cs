using Microsoft.Maui.Controls;
using System;

namespace HealthKeeper;

public partial class SymptomLogPage : ContentPage
{
    private SymptomLogViewModel _viewModel;

    public SymptomLogPage()
    {
        InitializeComponent();

       
        _viewModel = new SymptomLogViewModel();
        BindingContext = _viewModel;
    }

    
    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        
        string userInput = await DisplayPromptAsync(
            title: "New Symptom",
            message: "What are you feeling today?",
            accept: "Save",
            cancel: "Cancel",
            placeholder: "e.g., Headache, Fever");

        
        if (!string.IsNullOrWhiteSpace(userInput))
        {
            _viewModel.AddSymptom(userInput);
        }
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        
        var button = sender as Button;

        
        var symptomToDelete = button?.CommandParameter as string;

       
        if (!string.IsNullOrEmpty(symptomToDelete))
        {
            _viewModel.RemoveSymptom(symptomToDelete);
        }
    }
}