using System.Windows.Controls;
using ZMotionTest.ViewModels;

namespace ZMotionTest.Pages;

/// <summary>
/// ParameterTestPage.xaml 的交互逻辑
/// </summary>
public partial class ParameterTestPage : Page
{
    public ParameterTestPage()
    {
        InitializeComponent();
        DataContext = new ParameterTestViewModel();
    }
} 