using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZMotionSDK;
using ZMotionSDK.Models;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 轴控制ViewModel
/// </summary>
public partial class AxisControlViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;
    private System.Windows.Threading.DispatcherTimer? _statusTimer;

    public AxisControlViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;

        // 初始化轴选择列表
        InitializeAxisList();

        // 初始化状态定时器
        InitializeStatusTimer(); 
        GetAxisInfo();
    }

    #region 属性
    [ObservableProperty]
    private ObservableCollection<string> axisList = new();

    [ObservableProperty]
    private ObservableCollection<AxisTypeOption> axisTypes = new();

    [ObservableProperty]
    private int selectedAxisIndex;

    [ObservableProperty]
    private AxisType selectedAxisType = AxisType.脉冲方向方式的步进和伺服;

    // 运动参数
    [ObservableProperty]
    private float speed = 10;

    [ObservableProperty]
    private float accel = 100;

    [ObservableProperty]
    private float decel = 100;

    [ObservableProperty]
    private float units = 1;

    [ObservableProperty]
    private float lSpeed = 1;

    [ObservableProperty]
    private float sramp = 0;

    [ObservableProperty]
    private float fastDecel = 200;

    [ObservableProperty]
    private float jogSpeed = 5;

    // 运动控制
    [ObservableProperty]
    private float moveDistance = 10;

    [ObservableProperty]
    private float movePosition = 0;

    // 安全限制
    [ObservableProperty]
    private float forwardLimit = 1000;

    [ObservableProperty]
    private float reverseLimit = -1000;

    // 状态监控
    [ObservableProperty]
    private bool isAxisRunning;

    [ObservableProperty]
    private bool isAxisEnabled;

    [ObservableProperty]
    private bool hasAxisAlarm;

    [ObservableProperty]
    private float currentDPosition;

    [ObservableProperty]
    private float currentMPosition;

    [ObservableProperty]
    private float currentEncoder;

    [ObservableProperty]
    private float currentMSpeed;

    [ObservableProperty]
    private float currentVpSpeed;

    [ObservableProperty]
    private string axisStatusText = "未知";

    [ObservableProperty]
    private string stopReasonText = "无";

    [ObservableProperty]
    private string axisStatusDetails = "";

    // 回零参数
    [ObservableProperty]
    private int homeMode = 0;

    [ObservableProperty]
    private int homeSignalDI = 0;

    [ObservableProperty]
    private bool homeSignalInvert = false;

    [ObservableProperty]
    private bool homeStatus = false;

    [ObservableProperty]
    private float homeCreepSpeed = 1;

    [ObservableProperty]
    private float homeOffset = 0;

    [ObservableProperty]
    private int homeWaitTime = 2;

    // 在线命令
    [ObservableProperty]
    private string onlineCommand = "";

    [ObservableProperty]
    private string commandResponse = "";

    // 总线相关
    [ObservableProperty]
    private int busSlot = 0;

    [ObservableProperty]
    private int busNodeCount = 0;

    [ObservableProperty]
    private bool busInitStatus = false;

    [ObservableProperty]
    private bool isForwardLimitActive;

    [ObservableProperty]
    private bool isReverseLimitActive;

    [ObservableProperty]
    private bool isAlarmActive;

    [ObservableProperty]
    private bool isServoReady;
    #endregion

    #region 命令

    #region 轴参数设置
    [RelayCommand]
    private void GetAxisInfo()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;

            // 获取当前参数
            Speed = zMotion.GetSpeed(SelectedAxisIndex);
            Accel = zMotion.GetAccel(SelectedAxisIndex);
            Decel = zMotion.GetDecel(SelectedAxisIndex);
            Units = zMotion.GetUnits(SelectedAxisIndex);
            LSpeed = zMotion.GetSpeed_L(SelectedAxisIndex);
            Sramp = zMotion.GetSramp(SelectedAxisIndex);
            FastDecel = zMotion.GetDecel_Fast(SelectedAxisIndex);
            JogSpeed = zMotion.GetJogSpeed(SelectedAxisIndex);
            SelectedAxisType = zMotion.GetAxisType(SelectedAxisIndex);

            // 获取安全限制
            ForwardLimit = zMotion.GetLimit(SelectedAxisIndex, true);
            ReverseLimit = zMotion.GetLimit(SelectedAxisIndex, false);

            // 获取回零参数
            HomeSignalDI = zMotion.GetDIIndex_Home(SelectedAxisIndex);
            HomeSignalInvert = zMotion.GetDIInvert(SelectedAxisIndex);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"获取轴信息失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void SetAxisParams()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;

            // 设置轴类型
            zMotion.SetAxisType(SelectedAxisIndex, SelectedAxisType);

            // 设置运动参数
            zMotion.SetSpeed(SelectedAxisIndex, Speed);
            zMotion.SetAccel(SelectedAxisIndex, Accel);
            zMotion.SetDecel(SelectedAxisIndex, Decel);
            zMotion.SetUnits(SelectedAxisIndex, Units);
            zMotion.SetSpeed_L(SelectedAxisIndex, LSpeed);
            zMotion.SetSramp(SelectedAxisIndex, Sramp);
            zMotion.SetDecel_Fast(SelectedAxisIndex, FastDecel);
            zMotion.SetJogSpeed(SelectedAxisIndex, JogSpeed);

            MessageBox.Show("轴参数设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"设置轴参数失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void SetSafetyLimits()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            zMotion.SetLimit(SelectedAxisIndex, ForwardLimit, true);
            zMotion.SetLimit(SelectedAxisIndex, ReverseLimit, false);
            zMotion.SetDecel_Fast(SelectedAxisIndex, FastDecel);

            MessageBox.Show("安全限制设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"设置安全限制失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region 轴使能与报警
    [RelayCommand]
    private void ToggleAxisEnable()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            bool newState = !IsAxisEnabled;
            zMotion.AxisEnable_Bus(SelectedAxisIndex, newState);
            
            MessageBox.Show($"轴{SelectedAxisIndex} {(newState ? "使能" : "失能")}成功", "提示", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"切换轴使能状态失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ClearAxisAlarm()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            // 清除当前告警
            zMotion.ClearAlarm_Bus((uint)SelectedAxisIndex, 0);
            // 清除历史告警
            //zMotion.ClearAlarm_Bus((uint)SelectedAxisIndex, 1);
            
            MessageBox.Show($"轴{SelectedAxisIndex}报警清除成功", "提示", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"清除轴报警失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region 运动控制
    [RelayCommand]
    private void MoveRel()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Move_Relative(SelectedAxisIndex, MoveDistance);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"相对运动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void MoveAbs()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Move_Absolute(SelectedAxisIndex, MovePosition);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"绝对运动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void JogPos()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Jog(SelectedAxisIndex, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"正向点动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void JogNeg()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Jog(SelectedAxisIndex, false);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"反向点动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Home()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.GoHome(SelectedAxisIndex, HomeMode);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"回零失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void StopAxis()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Stop(SelectedAxisIndex, 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"停止轴失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void EmergencyStop()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Stop_All();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"急停失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void PauseAxis()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Pause(SelectedAxisIndex, 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"暂停轴失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void ResumeAxis()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Resume(SelectedAxisIndex);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"恢复轴失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region 位置设置
    [RelayCommand]
    private void SetDPosition()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.SetPosition_D(SelectedAxisIndex, CurrentDPosition);
            MessageBox.Show("规划位置设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"设置规划位置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void SetMPosition()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.SetPosition_M(SelectedAxisIndex, CurrentMPosition);
            MessageBox.Show("反馈位置设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"设置反馈位置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region 回零设置
    [RelayCommand]
    private void SetHomeParams()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            zMotion.SetDIIndex_Home(SelectedAxisIndex, HomeSignalDI);
            zMotion.SetDIInvert(SelectedAxisIndex, HomeSignalInvert);
            zMotion.SetGoHomeCreepSpeed(SelectedAxisIndex, HomeCreepSpeed);
            zMotion.SetGoHomeOffpos((uint)SelectedAxisIndex, HomeOffset);
            zMotion.SetGoHomeWaitTime(SelectedAxisIndex, HomeWaitTime);

            MessageBox.Show("回零参数设置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"设置回零参数失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void GetHomeParams()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            HomeSignalDI = zMotion.GetDIIndex_Home(SelectedAxisIndex);
            HomeSignalInvert = zMotion.GetDIInvert(SelectedAxisIndex);
            HomeCreepSpeed = zMotion.GetGoHomeCreepSpeed(SelectedAxisIndex);
            HomeOffset = zMotion.GetGoHomeOffpos((uint)SelectedAxisIndex);
            HomeWaitTime = zMotion.GetGoHomeWaitTime(SelectedAxisIndex);

            MessageBox.Show("回零参数获取成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"获取回零参数失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region 在线命令
    [RelayCommand]
    private void ExecuteBufferCommand()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var response = _zMotionManager.ZMotion.Execute_Buffer(OnlineCommand);
            CommandResponse = response;
        }
        catch (Exception ex)
        {
            CommandResponse = $"缓冲命令执行失败: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ExecuteDirectCommand()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            OnlineCommand = "?*max";
            var response = _zMotionManager.ZMotion.Execute_Direct(OnlineCommand);
            CommandResponse = response;
        }
        catch (Exception ex)
        {
            CommandResponse = $"直接命令执行失败: {ex.Message}";
        }
    }

    [RelayCommand]
    private void TriggerScope()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Trigger();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"触发示波器失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region 总线相关
    [RelayCommand]
    private void InitBus()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _zMotionManager.ZMotion.Init_Bus();
            MessageBox.Show("总线初始化成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"总线初始化失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void GetBusInfo()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            BusNodeCount = zMotion.GetBusNodeNum(BusSlot);
            BusInitStatus = zMotion.GetBusInitStatus();

            MessageBox.Show($"槽位{BusSlot}节点数量: {BusNodeCount}\n总线初始化状态: {(BusInitStatus ? "成功" : "失败")}", 
                "总线信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"获取总线信息失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #endregion

    #region 私有方法
    private void InitializeAxisList()
    {
        AxisList.Clear();
        for (int i = 0; i < 16; i++)
        {
            AxisList.Add($"轴{i}");
        }
        SelectedAxisIndex = 0;
        AxisTypes.Clear();
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.虚拟轴));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.脉冲方向方式的步进和伺服));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.模拟信号控制方式的伺服));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.正交编码器));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.脉冲方向输出_正交编码器输入));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.脉冲方向输出_脉冲方向编码器输入));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.脉冲方向方式的编码器));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.EtherCAT_周期位置模式));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.EtherCAT_周期速度模式));
        AxisTypes.Add(AxisTypeOption.FromAxisType(AxisType.EtherCAT_周期力矩模式));
    }

    private void InitializeStatusTimer()
    {
        _statusTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _statusTimer.Tick += UpdateAxisStatus;
        _statusTimer.Start();
    }

    private void UpdateAxisStatus(object? sender, EventArgs e)
    {
        try
        {
            if (!_zMotionManager.IsConnected) return;

            var zMotion = _zMotionManager.ZMotion;

            // 更新轴状态
            IsAxisRunning = zMotion.GetIsRunning(SelectedAxisIndex);
            CurrentDPosition = zMotion.GetPosition_D(SelectedAxisIndex);
            CurrentMPosition = zMotion.GetPosition_M(SelectedAxisIndex);
            CurrentEncoder = zMotion.GetPosition_Encoder(SelectedAxisIndex);
            CurrentMSpeed = zMotion.GetSpeed_M(SelectedAxisIndex);
            CurrentVpSpeed = zMotion.GetSpeed_Vp(SelectedAxisIndex);

            // 更新轴状态文本
            var axisStatus = zMotion.GetAxisStatus(SelectedAxisIndex);
            AxisStatusText = axisStatus.ToString();

            var stopReason = zMotion.GetAxisStopReason(SelectedAxisIndex);
            StopReasonText = stopReason.ToString();

            // 更新使能状态
            IsAxisEnabled = zMotion.GetAxisEnable_Bus(SelectedAxisIndex);

            // 更新回零状态
            HomeStatus = zMotion.GetHomeStatus(SelectedAxisIndex);

            // 更新限位状态
            UpdateLimitStatus(axisStatus);
        }
        catch
        {
            // 忽略状态更新错误
        }
    }


    private void UpdateLimitStatus(AxisStatus axisStatus)
    {
        // 这里假设轴状态位定义，实际应根据控制器文档调整
        IsForwardLimitActive = axisStatus.Limit_Forward;
        IsReverseLimitActive = axisStatus.Limit_Backward;
        IsAlarmActive = axisStatus.Alarm_Axis;
            
        HasAxisAlarm = IsAlarmActive;
    }
    #endregion

    public class AxisTypeOption
    {
        public AxisType Type { get; set; }
        public string Name { get; set; } = string.Empty;

        public static AxisTypeOption FromAxisType(AxisType type)
        {
            return new AxisTypeOption { Type = type, Name = type.ToString() };
        }
    }
}