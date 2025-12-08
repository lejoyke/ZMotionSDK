using System.Windows.Controls;
using ZMotionTest.ViewModels;

namespace ZMotionTest.Pages;

/// <summary>
/// 连接管理页面
/// </summary>
public partial class ConnectionPage : Page
{
    public ConnectionPage()
    {
        InitializeComponent();
        DataContext = new ConnectionViewModel();
    }
} 