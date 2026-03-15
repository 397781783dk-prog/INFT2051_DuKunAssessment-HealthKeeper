namespace HealthKeeper;

public partial class EmergencyCardPage : ContentPage
{
    public EmergencyCardPage()
    {
        InitializeComponent();

        BindingContext = new EmergencyCardViewModel();
    }
}