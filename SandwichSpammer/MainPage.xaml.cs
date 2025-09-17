using SandwichSpammer.ViewModels;

namespace SandwichSpammer;

public partial class MainPage : ContentPage
{
    public MainPage(AccompanimentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}