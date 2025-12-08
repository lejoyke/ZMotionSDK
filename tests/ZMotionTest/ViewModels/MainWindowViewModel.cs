
using System;
using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 主窗口ViewModel
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;

    public MainWindowViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;
        
        // 启动时间更新定时器
        StartTimeUpdater();
        
        // 初始化连接状态
        UpdateConnectionStatus(false);
    }

    #region 属性
    
    [ObservableProperty]
    private string connectionStatus = "未连接";
    
    [ObservableProperty]
    private PackIconKind connectionIconKind = PackIconKind.CloseNetworkOutline;
    
    [ObservableProperty]
    private Brush connectionStatusColor = Brushes.Red;
    
    [ObservableProperty]
    private Brush connectionIconColor = Brushes.Red;
    
    [ObservableProperty]
    private string statusText = "就绪";
    
    [ObservableProperty]
    private string timeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    #endregion
    
    #region 方法
    
    /// <summary>
    /// 更新连接状态
    /// </summary>
    /// <param name="isConnected">是否已连接</param>
    public void UpdateConnectionStatus(bool isConnected)
    {
        if (isConnected)
        {
            ConnectionStatus = "已连接";
            ConnectionIconKind = PackIconKind.Connection;
            ConnectionStatusColor = Brushes.Green;
            ConnectionIconColor = Brushes.Green;
        }
        else
        {
            ConnectionStatus = "未连接";
            ConnectionIconKind = PackIconKind.CloseNetworkOutline;
            ConnectionStatusColor = Brushes.Red;
            ConnectionIconColor = Brushes.Red;
        }
    }
    
    /// <summary>
    /// 更新状态文本
    /// </summary>
    /// <param name="status">状态文本</param>
    public void UpdateStatus(string status)
    {
        StatusText = status;
    }
    
    /// <summary>
    /// 启动时间更新定时器
    /// </summary>
    private void StartTimeUpdater()
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += (s, e) => TimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        timer.Start();
    }
    
    #endregion
} 