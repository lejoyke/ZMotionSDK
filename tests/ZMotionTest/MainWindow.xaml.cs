using System.Windows;
using System.Windows.Controls;
using ZMotionTest.Pages;
using ZMotionTest.ViewModels;

namespace ZMotionTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; } = null!;
    public MainWindowViewModel ViewModel { get; private set; } = null!;

    public MainWindow()
    {
        Instance = this;
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;
        
        // 默认导航到连接管理页面
        NavigateToConnectionPage();
    }

    private void NavigateToPage(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string pageTag)
        {
            switch (pageTag)
            {
                case "Connection":
                    NavigateToConnectionPage();
                    break;
                case "AxisMonitor":
                    NavigateToAxisMonitorPage();
                    break;
                case "AxisControl":
                    NavigateToAxisControlPage();
                    break;
                case "IOControl":
                    NavigateToIOControlPage();
                    break;
                case "ParameterTest":
                    NavigateToParameterTestPage();
                    break;
                case "MotionBuffer":
                    NavigateToMotionBufferPage();
                    break;
            }
        }
    }

    private void NavigateToConnectionPage()
    {
        ContentFrame.Navigate(new ConnectionPage());
    }

    private void NavigateToAxisMonitorPage()
    {
        ContentFrame.Navigate(new AxisMonitorPage());
    }

    private void NavigateToAxisControlPage()
    {
        ContentFrame.Navigate(new AxisControlPage());
    }

    private void NavigateToIOControlPage()
    {
        ContentFrame.Navigate(new IOControlPage());
    }

    private void NavigateToParameterTestPage()
    {
        ContentFrame.Navigate(new ParameterTestPage());
    }

    private void NavigateToMotionBufferPage()
    {
        ContentFrame.Navigate(new MotionBufferPage());
    }
}