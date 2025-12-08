using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZMotionTest.Models;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 轴监控ViewModel
/// </summary>
public partial class AxisMonitorViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;
    private readonly DispatcherTimer _monitorTimer;

    public AxisMonitorViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;
        
        // 初始化监控定时器
        _monitorTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _monitorTimer.Tick += MonitorTimer_Tick;

        // 设置默认值
        AxisCount = 8;
        RefreshInterval = 100;
    }

    #region 属性
    [ObservableProperty]
    private int axisCount = 8;

    [ObservableProperty]
    private bool isMonitoring;

    [ObservableProperty]
    private int refreshInterval = 100;

    [ObservableProperty]
    private ObservableCollection<AxisViewModel> axisStatusList = new();
    #endregion

    #region 命令
    [RelayCommand]
    private void StartMonitor()
    {
        try
        {
            // 初始化轴状态列表
            AxisStatusList.Clear();
            for (int i = 0; i < AxisCount; i++)
            {
                AxisStatusList.Add(new AxisViewModel
                {
                    AxisIndex = i,
                    Description = $"轴{i}"
                });
            }

            IsMonitoring = true;
            _monitorTimer.Start();
        }
        catch (Exception)
        {
            // 可以添加错误消息显示
        }
    }

    [RelayCommand]
    private void StopMonitor()
    {
        IsMonitoring = false;
        _monitorTimer.Stop();
    }

    [RelayCommand]
    private void RefreshOnce()
    {
        RefreshAxisStatus();
    }
    #endregion

    #region 私有方法
    private void MonitorTimer_Tick(object? sender, EventArgs e)
    {
        RefreshAxisStatus();
    }

    private void RefreshAxisStatus()
    {
        if (!_zMotionManager.IsConnected || AxisStatusList.Count == 0)
            return;

        try
        {
            foreach (var axis in AxisStatusList)
            {
                // 获取轴状态信息
                axis.IsRunning = _zMotionManager.GetIsRunning(axis.AxisIndex);
                axis.DPosition = _zMotionManager.GetDpos(axis.AxisIndex);
                axis.MPosition = _zMotionManager.GetMpos(axis.AxisIndex);
                axis.Speed = _zMotionManager.GetSpeed(axis.AxisIndex);
                
                // 获取轴状态文本
                var status = _zMotionManager.GetAxisStatus(axis.AxisIndex);
                axis.StatusText = FormatAxisStatus(status);
            }
        }
        catch (Exception)
        {
            // 可以添加错误处理
        }
    }

    private string FormatAxisStatus(ZMotionSDK.Models.AxisStatus status)
    {
        // 根据实际的轴状态结构来格式化状态文本
        var statusParts = new List<string>();
        
        if (status.IsFindingHome)
            statusParts.Add("寻找原点");
        if (status.IsPause)
            statusParts.Add("暂停");
        if (status.Limit_Forward)
            statusParts.Add("正向限位");
        if (status.Limit_Backward)
            statusParts.Add("反向限位");
        if (status.Alarm_Axis)
            statusParts.Add("轴告警");
        
        return statusParts.Count > 0 ? string.Join(", ", statusParts) : "正常";
    }

    partial void OnRefreshIntervalChanged(int value)
    {
        if (_monitorTimer != null)
        {
            _monitorTimer.Interval = TimeSpan.FromMilliseconds(value);
        }
    }
    #endregion
} 