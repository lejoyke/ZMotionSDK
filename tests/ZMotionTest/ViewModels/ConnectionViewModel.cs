using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 连接管理ViewModel
/// </summary>
public partial class ConnectionViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;

    public ConnectionViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;

        // 设置默认值
        IpAddress = "192.168.0.11";
        Timeout = 5000;

        IsConnected = _zMotionManager.IsConnected;
    }

    #region 属性
    [ObservableProperty]
    private string ipAddress = "192.168.0.11";

    [ObservableProperty]
    private uint timeout = 500;

    [ObservableProperty]
    private bool isConnected;

    [ObservableProperty]
    private string connectionStatus = "未连接";

    [ObservableProperty]
    private string connectedIp = "--";

    [ObservableProperty]
    private string connectionTime = "--";

    [ObservableProperty]
    private DateTime? connectedDateTime;

    public Brush ConnectionStatusColor => _zMotionManager.IsConnected ? Brushes.Green : Brushes.Red;
    #endregion

    #region 命令
    [RelayCommand]
    private void Connect()
    {
        try
        {
            _zMotionManager.Connect(IpAddress, Timeout);

            IsConnected = _zMotionManager.IsConnected;
            ConnectionStatus = "已连接";
            ConnectedIp = IpAddress;
            ConnectedDateTime = DateTime.Now;
            
            // 更新主窗口连接状态
            MainWindow.Instance?.ViewModel?.UpdateConnectionStatus(true);
            MainWindow.Instance?.ViewModel?.UpdateStatus($"已连接到 {IpAddress}");
        }
        catch (Exception ex)
        {
            IsConnected = false;
            ConnectionStatus = "连接失败";
            
            // 更新主窗口连接状态
            MainWindow.Instance?.ViewModel?.UpdateConnectionStatus(false);
            MainWindow.Instance?.ViewModel?.UpdateStatus($"连接失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void Disconnect()
    {
        try
        {
            _zMotionManager.Disconnect();
            
            IsConnected = _zMotionManager.IsConnected;
            ConnectionStatus = "未连接";
            ConnectedIp = "--";
            ConnectedDateTime = null;
            
            // 更新主窗口连接状态
            MainWindow.Instance?.ViewModel?.UpdateConnectionStatus(false);
            MainWindow.Instance?.ViewModel?.UpdateStatus("已断开连接");
        }
        catch (Exception ex)
        {
            // 更新主窗口连接状态
            MainWindow.Instance?.ViewModel?.UpdateConnectionStatus(false);
            MainWindow.Instance?.ViewModel?.UpdateStatus($"断开失败: {ex.Message}");
        }
    }
    #endregion
} 