using System.Windows.Controls;
using ZMotionTest.ViewModels;

namespace ZMotionTest.Pages;

/// <summary>
/// MotionBufferPage.xaml 的交互逻辑
/// </summary>
public partial class MotionBufferPage : Page
{
    public MotionBufferPage()
    {
        InitializeComponent();
        DataContext = new MotionBufferViewModel();
    }
} 