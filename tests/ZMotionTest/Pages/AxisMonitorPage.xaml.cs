using System.Windows.Controls;
using ZMotionTest.ViewModels;

namespace ZMotionTest.Pages;

/// <summary>
/// 轴状态监控页面
/// </summary>
public partial class AxisMonitorPage : Page
{
    public AxisMonitorPage()
    {
        InitializeComponent();
        DataContext = new AxisMonitorViewModel();
    }
} 