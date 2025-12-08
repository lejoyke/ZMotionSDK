using System.Collections.ObjectModel;
using System.Windows;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZMotionSDK;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 运动缓存测试ViewModel
/// </summary>
public partial class MotionBufferViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;
    private System.Windows.Threading.DispatcherTimer? _statusTimer;
    private StringBuilder _logBuilder = new();

    public MotionBufferViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;

        // 初始化轴选择列表
        InitializeAxisList();

        // 初始化状态定时器
        InitializeStatusTimer();

        // 初始化缓存队列
        BufferQueue = new ObservableCollection<BufferQueueItem>();

        // 刷新状态
        RefreshBufferStatus();
    }

    #region 属性

    [ObservableProperty]
    private ObservableCollection<string> axisList = new();

    [ObservableProperty]
    private int selectedAxisIndex;

    // 缓存状态属性
    [ObservableProperty]
    private int movesBuffered;

    [ObservableProperty]
    private bool hasBufferMovement;

    [ObservableProperty]
    private int currentMoveMark;

    [ObservableProperty]
    private int remainBuffer;

    [ObservableProperty]
    private int remainLineBuffer;

    [ObservableProperty]
    private float vectorBuffered;

    // 实时状态属性
    [ObservableProperty]
    private bool isAxisRunning;

    [ObservableProperty]
    private bool isAxisEnabled;

    [ObservableProperty]
    private float currentDPosition;

    [ObservableProperty]
    private float currentMPosition;

    [ObservableProperty]
    private float currentSpeed;

    [ObservableProperty]
    private float remainDistance;

    // 运动参数
    [ObservableProperty]
    private float moveDistance1 = 10;

    [ObservableProperty]
    private float moveDistance2 = 20;

    [ObservableProperty]
    private float moveDistance3 = 15;

    [ObservableProperty]
    private int moveMark = 1;

    // IO控制参数
    [ObservableProperty]
    private int outputPort = 0;

    [ObservableProperty]
    private int outputState = 1;

    [ObservableProperty]
    private int delayTime = 1000;

    // 自定义命令
    [ObservableProperty]
    private string customBufferCommand = "";

    // 日志和状态
    [ObservableProperty]
    private string operationLog = "";

    [ObservableProperty]
    private ObservableCollection<BufferQueueItem> bufferQueue;

    #endregion

    #region 命令

    [RelayCommand]
    private void RefreshBufferStatus()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                AddLog("警告: 控制器未连接");
                return;
            }

            var zMotion = _zMotionManager.ZMotion;

            // 获取缓存状态
            MovesBuffered = zMotion.GetMovesBuffered(SelectedAxisIndex);
            HasBufferMovement = zMotion.GetLoaded(SelectedAxisIndex);
            CurrentMoveMark = zMotion.GetMoveCurmark(SelectedAxisIndex);
            RemainBuffer = zMotion.GetRemainBuffer(SelectedAxisIndex);
            RemainLineBuffer = zMotion.GetRemainLineBuffer(SelectedAxisIndex);
            VectorBuffered = zMotion.GetVectorBuffered(SelectedAxisIndex);

            // 更新实时状态
            UpdateRealtimeStatus();

            AddLog("缓存状态刷新成功");
        }
        catch (Exception ex)
        {
            AddLog($"刷新缓存状态失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ClearBuffer()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            
            // 停止轴运动并清空缓存
            zMotion.Stop(SelectedAxisIndex, ZMotionSDK.Models.CancelMode.取消当前运动和缓冲运动); // 模式2：取消当前运动并取消缓存运动

            AddLog($"轴 {SelectedAxisIndex} 缓存已清空");
            
            // 刷新状态
            RefreshBufferStatus();
        }
        catch (Exception ex)
        {
            AddLog($"清空缓存失败: {ex.Message}");
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

            var zMotion = _zMotionManager.ZMotion;
            zMotion.Pause(SelectedAxisIndex, 0);

            AddLog($"轴 {SelectedAxisIndex} 已暂停");
        }
        catch (Exception ex)
        {
            AddLog($"暂停轴失败: {ex.Message}");
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

            var zMotion = _zMotionManager.ZMotion;
            zMotion.Resume(SelectedAxisIndex);

            AddLog($"轴 {SelectedAxisIndex} 已恢复");
        }
        catch (Exception ex)
        {
            AddLog($"恢复轴失败: {ex.Message}");
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

            var zMotion = _zMotionManager.ZMotion;
            zMotion.Stop(SelectedAxisIndex, ZMotionSDK.Models.CancelMode.取消当前运动和缓冲运动);

            AddLog($"轴 {SelectedAxisIndex} 已停止");
        }
        catch (Exception ex)
        {
            AddLog($"停止轴失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddSingleMove()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            zMotion.Move_Relative(SelectedAxisIndex, MoveDistance1);

            AddBufferQueueItem("相对移动", $"轴 {SelectedAxisIndex} 移动距离 {MoveDistance1}");
            AddLog($"添加相对移动: {MoveDistance1}");
            
            // 刷新状态
            RefreshBufferStatus();
        }
        catch (Exception ex)
        {
            AddLog($"添加移动失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddSequenceMove()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            
            // 添加多个连续移动到缓存
            zMotion.Move_Relative(SelectedAxisIndex, MoveDistance1);
            zMotion.Move_Relative(SelectedAxisIndex, MoveDistance2);
            zMotion.Move_Relative(SelectedAxisIndex, MoveDistance3);

            AddBufferQueueItem("序列移动", $"轴 {SelectedAxisIndex} 序列移动: {MoveDistance1}, {MoveDistance2}, {MoveDistance3}");
            AddLog($"添加序列移动: {MoveDistance1}, {MoveDistance2}, {MoveDistance3}");
            
            // 刷新状态
            RefreshBufferStatus();
        }
        catch (Exception ex)
        {
            AddLog($"添加序列移动失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void SetMoveMark()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            zMotion.SetMovemark(SelectedAxisIndex, MoveMark);

            AddBufferQueueItem("设置标记", $"轴 {SelectedAxisIndex} 移动标记设置为 {MoveMark}");
            AddLog($"设置移动标记: {MoveMark}");
        }
        catch (Exception ex)
        {
            AddLog($"设置移动标记失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddMoveOp()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            zMotion.MoveOp(SelectedAxisIndex, OutputPort, OutputState);

            AddBufferQueueItem("缓存IO", $"轴 {SelectedAxisIndex} 输出 {OutputPort} 设置为 {OutputState}");
            AddLog($"添加缓存IO控制: 端口 {OutputPort} = {OutputState}");
            
            // 刷新状态
            RefreshBufferStatus();
        }
        catch (Exception ex)
        {
            AddLog($"添加IO控制失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddMoveDelay()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            zMotion.MoveDelay(SelectedAxisIndex, DelayTime);

            AddBufferQueueItem("缓存延时", $"轴 {SelectedAxisIndex} 延时 {DelayTime}ms");
            AddLog($"添加缓存延时: {DelayTime}ms");
            
            // 刷新状态
            RefreshBufferStatus();
        }
        catch (Exception ex)
        {
            AddLog($"添加延时失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ExecuteCustomCommand()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                MessageBox.Show("请先连接控制器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CustomBufferCommand))
            {
                MessageBox.Show("请输入要执行的命令", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var zMotion = _zMotionManager.ZMotion;
            var response = zMotion.Execute_Buffer(CustomBufferCommand);

            AddBufferQueueItem("自定义命令", CustomBufferCommand);
            AddLog($"执行自定义命令: {CustomBufferCommand}");
            if (!string.IsNullOrEmpty(response))
            {
                AddLog($"命令响应: {response}");
            }
            
            // 刷新状态
            RefreshBufferStatus();
        }
        catch (Exception ex)
        {
            AddLog($"执行自定义命令失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ClearQueueLog()
    {
        BufferQueue.Clear();
        AddLog("缓存队列日志已清空");
    }

    [RelayCommand]
    private void ClearLog()
    {
        _logBuilder.Clear();
        OperationLog = "";
        AddLog("日志已清空");
    }

    #endregion

    #region 私有方法

    private void InitializeAxisList()
    {
        // 初始化轴列表（0-31）
        for (int i = 0; i <= 31; i++)
        {
            AxisList.Add($"轴 {i}");
        }
        
        SelectedAxisIndex = 0;
    }

    private void InitializeStatusTimer()
    {
        _statusTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500) // 500ms更新一次
        };
        _statusTimer.Tick += UpdateRealtimeStatus;
        _statusTimer.Start();
    }

    private void UpdateRealtimeStatus(object? sender = null, EventArgs? e = null)
    {
        try
        {
            if (!_zMotionManager.IsConnected) return;

            var zMotion = _zMotionManager.ZMotion;

            // 更新运行状态
            IsAxisRunning = zMotion.GetIsRunning(SelectedAxisIndex);
            IsAxisEnabled = zMotion.GetParam(SelectedAxisIndex, "AXIS_ENABLE") == 1;

            // 更新位置和速度信息
            CurrentDPosition = zMotion.GetPosition_D(SelectedAxisIndex);
            CurrentMPosition = zMotion.GetPosition_M(SelectedAxisIndex);
            CurrentSpeed = zMotion.GetSpeed_Vp(SelectedAxisIndex);
            RemainDistance = zMotion.GetRemain(SelectedAxisIndex);

            // 定期刷新缓存状态
            if (sender != null) // 来自定时器调用
            {
                MovesBuffered = zMotion.GetMovesBuffered(SelectedAxisIndex);
                VectorBuffered = zMotion.GetVectorBuffered(SelectedAxisIndex);
            }
        }
        catch
        {
            // 静默处理异常，避免定时器更新时的错误干扰用户操作
        }
    }

    private void AddLog(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        _logBuilder.AppendLine($"[{timestamp}] {message}");
        
        // 保持日志在合理长度内
        if (_logBuilder.Length > 10000)
        {
            var text = _logBuilder.ToString();
            var lines = text.Split('\n');
            _logBuilder.Clear();
            for (int i = Math.Max(0, lines.Length - 100); i < lines.Length; i++)
            {
                _logBuilder.AppendLine(lines[i]);
            }
        }
        
        OperationLog = _logBuilder.ToString();
    }

    private void AddBufferQueueItem(string commandType, string description)
    {
        BufferQueue.Insert(0, new BufferQueueItem
        {
            CommandType = commandType,
            Description = description,
            Timestamp = DateTime.Now
        });

        // 保持队列在合理长度内
        while (BufferQueue.Count > 50)
        {
            BufferQueue.RemoveAt(BufferQueue.Count - 1);
        }
    }

    #endregion
}

/// <summary>
/// 缓存队列项
/// </summary>
public class BufferQueueItem
{
    public string CommandType { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Timestamp { get; set; }
} 