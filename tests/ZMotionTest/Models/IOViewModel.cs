using CommunityToolkit.Mvvm.ComponentModel;

namespace ZMotionTest.Models;

public partial class IOViewModel : ObservableObject
{
    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private string description = string.Empty;

    public string IndexText => $"IO{Index:D2}";
    
    public Action<int, bool>? ValueChangedCallback { get; set; }

    partial void OnIndexChanged(int value)
    {
        OnPropertyChanged(nameof(IndexText));
    }
    
    partial void OnIsActiveChanged(bool value)
    {
        ValueChangedCallback?.Invoke(Index, value);
    }
} 