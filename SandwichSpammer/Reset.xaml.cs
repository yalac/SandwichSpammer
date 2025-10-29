using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandwichSpammer.ViewModels;

namespace SandwichSpammer;

public partial class Reset : ContentPage
{
    public Reset(AccompanimentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}