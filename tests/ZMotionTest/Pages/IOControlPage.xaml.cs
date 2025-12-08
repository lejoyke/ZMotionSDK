using System.Windows.Controls;
using ZMotionTest.ViewModels;

namespace ZMotionTest.Pages;

/// <summary>
/// IO控制页面
/// </summary>
public partial class IOControlPage : Page
{
    public IOControlPage()
    {
        InitializeComponent();
        DataContext = new IOControlViewModel();
    }
} 