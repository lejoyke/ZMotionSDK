using CommunityToolkit.Mvvm.ComponentModel;

namespace ZMotionTest.Models;

public partial class InverterConfigViewModel : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private bool _isInverted;

    [ObservableProperty]
    private string _description = string.Empty;
} 