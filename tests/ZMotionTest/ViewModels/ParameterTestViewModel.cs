using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZMotionSDK.Helper;
using ZMotionSDK.Models;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 参数测试ViewModel - 测试GetParam和SetParam方法
/// </summary>
public partial class ParameterTestViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;

    public ParameterTestViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;

        // 初始化参数列表
        InitializeParameterLists();
        InitializeReadResults();
    }

    #region 属性

    /// <summary>
    /// 轴索引
    /// </summary>
    [ObservableProperty]
    private int axisIndex = 0;

    /// <summary>
    /// 选中的读取参数
    /// </summary>
    [ObservableProperty]
    private ParameterPreset? selectedReadParam;

    /// <summary>
    /// 选中的写入参数
    /// </summary>
    [ObservableProperty]
    private ParameterPreset? selectedWriteParam;

    /// <summary>
    /// 写入值
    /// </summary>
    [ObservableProperty]
    private float writeValue;

    /// <summary>
    /// 读取结果
    /// </summary>
    [ObservableProperty]
    private float readResult;

    /// <summary>
    /// 状态信息
    /// </summary>
    [ObservableProperty]
    private string statusInfo = "就绪";

    /// <summary>
    /// 可读取的参数列表
    /// </summary>
    public ObservableCollection<ParameterPreset> ReadParameterList { get; private set; } = new();

    /// <summary>
    /// 可写入的参数列表
    /// </summary>
    public ObservableCollection<ParameterPreset> WriteParameterList { get; private set; } = new();

    /// <summary>
    /// 参数读取结果列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ParameterReadResult> readResults = new();

    /// <summary>
    /// 常用参数预设列表
    /// </summary>
    public ObservableCollection<ParameterPreset> CommonParameters { get; private set; } = new();

    #endregion

    #region 命令

    /// <summary>
    /// 读取单个参数
    /// </summary>
    [RelayCommand]
    private void ReadParameter()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            ReadResult = _zMotionManager.ZMotion.GetParam(AxisIndex, SelectedReadParam.Parameter);
            ShowMessage($"读取成功: 轴{AxisIndex} {SelectedReadParam.Description} = {ReadResult}");
        }
        catch (Exception ex)
        {
            ShowMessage($"读取失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 写入单个参数
    /// </summary>
    [RelayCommand]
    private void WriteParameter()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            _zMotionManager.ZMotion.SetParam(AxisIndex, SelectedWriteParam.Parameter, WriteValue);
            ShowMessage($"写入成功: 轴{AxisIndex} {SelectedWriteParam.Description} = {WriteValue}");
        }
        catch (Exception ex)
        {
            ShowMessage($"写入失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 读取所有常用参数
    /// </summary>
    [RelayCommand]
    private void ReadAllCommonParameters()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            ReadResults.Clear();
            int successCount = 0;
            int failCount = 0;

            foreach (var preset in CommonParameters)
            {
                try
                {
                    var value = _zMotionManager.ZMotion.GetParam(AxisIndex, preset.Parameter);
                    ReadResults.Add(new ParameterReadResult
                    {
                        ParameterName = preset.Parameter.ToString(),
                        Description = preset.Description,
                        Value = value,
                        Status = "成功",
                        ReadTime = DateTime.Now
                    });
                    successCount++;
                }
                catch (Exception ex)
                {
                    ReadResults.Add(new ParameterReadResult
                    {
                        ParameterName = preset.Parameter.ToString(),
                        Description = preset.Description,
                        Value = 0,
                        Status = $"失败: {ex.Message}",
                        ReadTime = DateTime.Now
                    });
                    failCount++;
                }
            }

            ShowMessage($"批量读取完成: 成功 {successCount} 个，失败 {failCount} 个");
        }
        catch (Exception ex)
        {
            ShowMessage($"批量读取失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 清空读取结果
    /// </summary>
    [RelayCommand]
    private void ClearResults()
    {
        ReadResults.Clear();
        ShowMessage("已清空读取结果");
    }

    /// <summary>
    /// 应用预设参数
    /// </summary>
    [RelayCommand]
    private void ApplyPreset(ParameterPreset preset)
    {
        if (preset != null)
        {
            SelectedReadParam = preset;
            ShowMessage($"已选择参数: {preset.Description}");
        }
    }

    /// <summary>
    /// 刷新参数读取结果
    /// </summary>
    [RelayCommand]
    private void RefreshParameterResult(ParameterReadResult result)
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            if (Enum.TryParse<ReadBaiscParmName>(result.ParameterName, out var paramName))
            {
                var value = _zMotionManager.ZMotion.GetParam(AxisIndex, paramName);
                result.Value = value;
                result.Status = "成功";
                result.ReadTime = DateTime.Now;
                ShowMessage($"刷新成功: {result.ParameterName} = {value}");
            }
        }
        catch (Exception ex)
        {
            result.Status = $"失败: {ex.Message}";
            result.ReadTime = DateTime.Now;
            ShowMessage($"刷新失败: {ex.Message}");
        }
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 初始化参数列表
    /// </summary>
    private void InitializeParameterLists()
    {
        // 初始化读取参数列表
        ReadParameterList.Clear();
        var readParameterInfos = BaiscParmNameHelper.GetReadParameterInfoList();
        foreach (var paramInfo in readParameterInfos)
        {
            ReadParameterList.Add(new ParameterPreset
            {
                Parameter = paramInfo.Name,
                Description = paramInfo.DisplayText
            });
        }

        // 初始化写入参数列表
        WriteParameterList.Clear();
        var writeParameterInfos = BaiscParmNameHelper.GetWriteParameterInfoList();
        foreach (var paramInfo in writeParameterInfos)
        {
            WriteParameterList.Add(new ParameterPreset
            {
                Parameter = paramInfo.Name,
                Description = paramInfo.DisplayText
            });
        }

        CommonParameters.Clear();
        var commonParameterInfos = BaiscParmNameHelper.GetCommonParameterInfoList();
        foreach (var paramInfo in commonParameterInfos)
        {
            CommonParameters.Add(new ParameterPreset
            {
                Parameter = paramInfo.Name,
                Description = paramInfo.Description
            });
        }
    }

    /// <summary>
    /// 初始化读取结果
    /// </summary>
    private void InitializeReadResults()
    {
        ReadResults.Clear();
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    /// <param name="message">消息内容</param>
    private void ShowMessage(string message)
    {
        StatusInfo = $"[{DateTime.Now:HH:mm:ss}] {message}";
    }

    #endregion
}

/// <summary>
/// 参数预设类
/// </summary>
public class ParameterPreset
{
    public string Parameter { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 参数读取结果类
/// </summary>
public partial class ParameterReadResult : ObservableObject
{
    [ObservableProperty]
    private string parameterName = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private float value;

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private DateTime readTime;

    public string FormattedTime => ReadTime.ToString("HH:mm:ss");
}