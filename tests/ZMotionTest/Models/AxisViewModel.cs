using CommunityToolkit.Mvvm.ComponentModel;

namespace ZMotionTest.Models;

public partial class AxisViewModel : ObservableObject
{
    [ObservableProperty]
    private int axisIndex;

    [ObservableProperty]
    private bool isRunning;

    [ObservableProperty]
    private float dPosition;

    [ObservableProperty]
    private float mPosition;

    [ObservableProperty]
    private float speed;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    public string IsRunningText => IsRunning ? "运行中" : "停止";

    partial void OnIsRunningChanged(bool value)
    {
        OnPropertyChanged(nameof(IsRunningText));
    }
} 