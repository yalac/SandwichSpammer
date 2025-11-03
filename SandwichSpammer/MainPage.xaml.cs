using System.ComponentModel;
using Microsoft.Maui.Controls;
using SandwichSpammer.Models;
using SandwichSpammer.ViewModels;

namespace SandwichSpammer;

public partial class MainPage : ContentPage
{
    private readonly BoutiqueViewModel? _boutiqueVm;

    public MainPage(AccompanimentViewModel viewModel, BoutiqueViewModel boutiqueViewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        _boutiqueVm = boutiqueViewModel;

        if (_boutiqueVm != null)
        {
            // état initial des cheerleaders
            SetCheerleadersVisible(_boutiqueVm.CheerleadersVisible);
            _boutiqueVm.PropertyChanged += BoutiqueVm_PropertyChanged;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_boutiqueVm != null)
        {
            SetCheerleadersVisible(_boutiqueVm.CheerleadersVisible);
        }
    }

    private void BoutiqueVm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e?.PropertyName == nameof(BoutiqueViewModel.CheerleadersVisible))
        {
            Dispatcher.Dispatch(() => SetCheerleadersVisible(_boutiqueVm?.CheerleadersVisible ?? false));
        }
    }

    private void SetCheerleadersVisible(bool visible)
    {
        UpgradeCheerleader1.IsVisible = visible;
        UpgradeCheerleader2.IsVisible = visible;
        UpgradeCheerleader3.IsVisible = visible;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_boutiqueVm != null)
        {
            _boutiqueVm.PropertyChanged -= BoutiqueVm_PropertyChanged;
        }
    }

    // Tap sur le label des calories -> page détail générale
    private async void CaloriesLabel_Tapped(object sender, EventArgs e)
    {
        var accompanimentVm = BindingContext as AccompanimentViewModel;
        await Navigation.PushAsync(new DetailPage(accompanimentVm, _boutiqueVm));
    }
}
