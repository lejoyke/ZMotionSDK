using System.Windows.Controls;
using ZMotionTest.ViewModels;

namespace ZMotionTest.Pages;

/// <summary>
/// 轴运动控制页面
/// </summary>
public partial class AxisControlPage : Page
{
    public AxisControlPage()
    {
        InitializeComponent();
        DataContext = new AxisControlViewModel();
    }
} 