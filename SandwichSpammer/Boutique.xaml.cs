using SandwichSpammer.ViewModels;

namespace SandwichSpammer;

public partial class Boutique : ContentPage
{
    public Boutique(BoutiqueViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}