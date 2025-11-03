using SandwichSpammer.ViewModels;
using SandwichSpammer.Models;

namespace SandwichSpammer;

public partial class DetailPage : ContentPage
{
    private readonly DetailViewModel _vm;

    // page générale (tap sur calories)
    public DetailPage(AccompanimentViewModel accompVm, BoutiqueViewModel boutiqueVm)
    {
        InitializeComponent();
        _vm = new DetailViewModel(new Database.DatabaseService(), accompVm, boutiqueVm);
        BindingContext = _vm;
    }
}