using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPFProject.Share.Model;
using ZMotionSDK.Helper;
using ZMotionSDK.ProtocolSugar;
using ZMotionTest.Models;
using ZMotionTest.Protocol;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// IO控制ViewModel - 测试所有硬件接口方法
/// </summary>
public partial class IOControlViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;

    public IOControlViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;

        // 设置默认值
        DICount = 16;
        DOCount = 16;
        ModbusStartIndex = 0;
        ModbusEndIndex = 15;
        MultiStartIndex = 0;
        MultiEndIndex = 31;

        // 初始化IO列表
        InitializeIOLists();
        InitializeBitStatusLists();
        InitializeInverterConfigLists();

        WriteBitCommand = new RelayCommand<DataStatusModel<int>>(WriteBit);

        _protocolBuilder = new MessageBuilder<DIProtocol, DOProtocol>();
        _protocolBuilder.ZMotion = _zMotionManager.ZMotion;

        InitDataStatus();

        _timer = new Timer((state) => UpdateStatus(), null, 0, 300);
    }

    #region 单个IO属性

    [ObservableProperty] private int dICount = 16;

    [ObservableProperty] private int dOCount = 16;

    [ObservableProperty] private ObservableCollection<IOViewModel> dIList = new();

    [ObservableProperty] private ObservableCollection<IOViewModel> dOList = new();

    #endregion

    #region Modbus IO属性

    [ObservableProperty] private int modbusStartIndex = 0;

    [ObservableProperty] private int modbusEndIndex = 15;

    [ObservableProperty] private ObservableCollection<BitStatusViewModel> modbusDIBits = new();

    [ObservableProperty] private ObservableCollection<BitStatusViewModel> modbusDOBits = new();

    [ObservableProperty] private string modbusStatusInfo = "";

    #endregion

    #region 多个IO属性

    [ObservableProperty] private ushort multiStartIndex = 0;

    [ObservableProperty] private ushort multiEndIndex = 31;

    [ObservableProperty] private ObservableCollection<BitStatusViewModel> multiDIBits = new();

    [ObservableProperty] private ObservableCollection<EditableBitStatusViewModel> multiDOBits = new();

    #endregion

    #region DIDO反转配置属性

    [ObservableProperty] private ObservableCollection<InverterConfigViewModel> dIInverterConfigList = new();

    [ObservableProperty] private ObservableCollection<InverterConfigViewModel> dOInverterConfigList = new();

    #endregion

    #region 协议配置属性

    public ObservableCollection<DataStatusModel<int>> ReadDataStatuses { get; set; } = [];
    public ObservableCollection<DataStatusModel<int>> WriteDataStatuses { get; set; } = [];

    private readonly ICommand WriteBitCommand;

    private readonly MessageBuilder<DIProtocol, DOProtocol> _protocolBuilder;

    private Timer _timer;

    // 协议对象属性
    [ObservableProperty] private DIProtocol currentDIProtocol;

    [ObservableProperty] private DOProtocol currentDOProtocol;

    // 表达式测试结果
    [ObservableProperty] private string expressionTestResult = "";

    private void InitDataStatus()
    {
        var addressMapping = _protocolBuilder.DIConfiguration.AddressMapping.OrderBy(n => n.Value);

        foreach (var item in addressMapping)
        {
            ReadDataStatuses.Add(new DataStatusModel<int>()
                { Name = item.Key, Address = item.Value, Visibility = false });
        }

        addressMapping = _protocolBuilder.DOConfiguration.AddressMapping.OrderBy(n => n.Value);

        foreach (var item in addressMapping)
        {
            WriteDataStatuses.Add(new DataStatusModel<int>()
                { Name = item.Key, Address = item.Value, Visibility = true, Command = WriteBitCommand });
        }
    }

    private void UpdateStatus()
    {
        var diData = _protocolBuilder.ReadDIData();
        var doData = _protocolBuilder.ReadDOData();
        Application.Current.Dispatcher.Invoke((Delegate)(() =>
        {
            foreach (var item in ReadDataStatuses)
            {
                item.Status = diData[item.Address];
            }

            foreach (var item in WriteDataStatuses)
            {
                item.Status = doData[item.Address];
            }
        }));
    }

    private void WriteBit(DataStatusModel<int>? arg)
    {
        _protocolBuilder.Write(arg!.Address, !arg.Status);
    }

    #endregion

    #region 协议高级方法命令

    [RelayCommand]
    private void ReadCompleteProtocols()
    {
        try
        {
            CurrentDIProtocol = _protocolBuilder.ReadDI();
            CurrentDOProtocol = _protocolBuilder.ReadDO();

            ShowMessage("协议读取完成");
        }
        catch (Exception ex)
        {
            ShowMessage($"协议读取失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void WriteCompleteProtocol()
    {
        try
        {
            var doProtocol = new DOProtocol()
            {
                急停 = true,
                开始运动 = true
            };

            _protocolBuilder.Write(doProtocol);
            ShowMessage("协议写入完成");
        }
        catch (Exception ex)
        {
            ShowMessage($"协议写入失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void TestExpressionRead()
    {
        try
        {
            // 测试表达式读取
            var 启动按钮状态 = _protocolBuilder.ReadDI(x => x.启动按钮);
            var 停止按钮状态 = _protocolBuilder.ReadDI(x => x.停止按钮);
            var 启动输出状态 = _protocolBuilder.ReadDO(x => x.启动);
            var 停止输出状态 = _protocolBuilder.ReadDO(x => x.停止);

            ExpressionTestResult = $"启动按钮: {启动按钮状态}, 停止按钮: {停止按钮状态}, 启动输出: {启动输出状态}, 停止输出: {停止输出状态}";
            ShowMessage("表达式读取完成");
        }
        catch (Exception ex)
        {
            ShowMessage($"表达式读取失败: {ex.Message}");
            ExpressionTestResult = $"读取失败: {ex.Message}";
        }
    }

    [RelayCommand]
    private void TestExpressionWrite()
    {
        try
        {
            // 测试表达式写入
            _protocolBuilder.Write(x => x.启动, true);
            _protocolBuilder.Write(x => x.停止, false);
            _protocolBuilder.Write(x => x.急停, false);

            ShowMessage("表达式写入完成");
        }
        catch (Exception ex)
        {
            ShowMessage($"表达式写入失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void CreateWriteMapping()
    {
        try
        {
            var mapping = _protocolBuilder.CreateWriteMapping();

            // 设置一些示例值
            mapping.Property(x => x.启动).Value(true);
            mapping.Property(x => x.停止).Value(false);
            mapping.Property(x => x.急停).Value(false);
            mapping.Property(x => x.复位).Value(true);
            mapping.Property(x => x.开始运动).Value(true);

            // 提交映射
            mapping.Commit();

            ShowMessage("写入映射创建并提交完成");
        }
        catch (Exception ex)
        {
            ShowMessage($"写入映射失败: {ex.Message}");
        }
    }

    #endregion

    #region 单个IO命令

    [RelayCommand]
    private void RefreshDI()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            int activeCount = 0;
            foreach (var di in DIList)
            {
                di.IsActive = _zMotionManager.GetDI(di.Index);
                if (di.IsActive) activeCount++;
            }

            ShowMessage($"DI刷新完成，{activeCount}/{DICount} 个激活");
        }
        catch (Exception ex)
        {
            ShowMessage($"DI刷新失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void RefreshDO()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            int activeCount = 0;
            foreach (var do_ in DOList)
            {
                do_.IsActive = _zMotionManager.GetDO(do_.Index);
                if (do_.IsActive) activeCount++;
            }

            ShowMessage($"DO刷新完成，{activeCount}/{DOCount} 个激活");
        }
        catch (Exception ex)
        {
            ShowMessage($"DO刷新失败: {ex.Message}");
        }
    }

    #endregion

    #region Modbus IO命令

    [RelayCommand]
    private void GetModbusDI()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            var result = _zMotionManager.GetDI_Modbus(ModbusStartIndex, ModbusEndIndex);
            UpdateBitStatus(result, ModbusDIBits, ModbusStartIndex);

            int activeCount = ModbusDIBits.Count(b => b.IsActive);
            ModbusStatusInfo = $"读取完成 | 激活: {activeCount}/{ModbusDIBits.Count}";
            ShowMessage($"Modbus DI 读取完成: {activeCount} 个激活");
        }
        catch (Exception ex)
        {
            ShowMessage($"Modbus DI 读取失败: {ex.Message}");
            ModbusStatusInfo = $"读取失败: {ex.Message}";
        }
    }

    [RelayCommand]
    private void GetModbusDO()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            var result = _zMotionManager.GetDO_Modbus(ModbusStartIndex, ModbusEndIndex);
            UpdateBitStatus(result, ModbusDOBits, ModbusStartIndex);

            int activeCount = ModbusDOBits.Count(b => b.IsActive);
            ModbusStatusInfo = $"读取完成 | 激活: {activeCount}/{ModbusDOBits.Count}";
            ShowMessage($"Modbus DO 读取完成: {activeCount} 个激活");
        }
        catch (Exception ex)
        {
            ShowMessage($"Modbus DO 读取失败: {ex.Message}");
            ModbusStatusInfo = $"读取失败: {ex.Message}";
        }
    }

    #endregion

    #region 多个IO命令

    [RelayCommand]
    private void GetMultiDI()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            var result = _zMotionManager.GetDI_Multi(MultiStartIndex, MultiEndIndex);
            UpdateBitStatus(result, MultiDIBits, MultiStartIndex);

            int activeCount = MultiDIBits.Count(b => b.IsActive);
            ShowMessage($"多个 DI 读取完成: {activeCount} 个激活");
        }
        catch (Exception ex)
        {
            ShowMessage($"多个 DI 读取失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void GetMultiDO()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            var result = _zMotionManager.GetDO_Multi(MultiStartIndex, MultiEndIndex);
            UpdateBitStatus(result, MultiDOBits, MultiStartIndex);

            int activeCount = MultiDOBits.Count(b => b.IsActive);
        }
        catch (Exception ex)
        {
            ShowMessage($"多个 DO 读取失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ApplyMultiDOFromBits()
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                ShowMessage("设备未连接");
                return;
            }

            // 从位状态生成bool数组
            var values = GenerateBoolValuesFromBits();
            _zMotionManager.SetDO_Multi(MultiStartIndex, values);

            int activeCount = MultiDOBits.Count(b => b.IsActive);
        }
        catch (Exception ex)
        {
            ShowMessage($"多个 DO 应用失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void SetAllMultiDO()
    {
        foreach (var bit in MultiDOBits)
        {
            bit.IsActive = true;
        }

        ShowMessage("已设置所有DO为激活状态");
    }

    [RelayCommand]
    private void ClearAllMultiDO()
    {
        foreach (var bit in MultiDOBits)
        {
            bit.IsActive = false;
        }

        ShowMessage("已清除所有DO状态");
    }

    [RelayCommand]
    private void InvertAllMultiDO()
    {
        foreach (var bit in MultiDOBits)
        {
            bit.IsActive = !bit.IsActive;
        }

        ShowMessage("已翻转所有DO状态");
    }

    [RelayCommand]
    private void SetOddMultiDO()
    {
        for (int i = 0; i < MultiDOBits.Count; i++)
        {
            MultiDOBits[i].IsActive = (i % 2 == 1);
        }

        ShowMessage("已设置奇数位DO为激活状态");
    }

    [RelayCommand]
    private void SetEvenMultiDO()
    {
        for (int i = 0; i < MultiDOBits.Count; i++)
        {
            MultiDOBits[i].IsActive = (i % 2 == 0);
        }

        ShowMessage("已设置偶数位DO为激活状态");
    }

    [RelayCommand]
    private void ToggleDOBit(EditableBitStatusViewModel bit)
    {
        if (bit != null)
        {
            bit.IsActive = !bit.IsActive;
        }
    }

    #endregion

    #region DIDO反转配置命令

    [RelayCommand]
    private void ApplyDIDOInverter()
    {
        try
        {
            var config = new DIDOInverterConfig();

            var invertedDI = DIInverterConfigList.Where(i => i.IsInverted).Select(i => i.Index).ToArray();
            config.SetInvertedDI(invertedDI);

            var invertedDO = DOInverterConfigList.Where(i => i.IsInverted).Select(i => i.Index).ToArray();
            config.SetInvertedDO(invertedDO);

            _zMotionManager.ApplyDIDOInverter(config);

            ShowMessage("DIDO反转配置已应用");
            // 刷新IO状态以反映最新配置
            RefreshDI();
            RefreshDO();
        }
        catch (Exception ex)
        {
            ShowMessage($"DIDO反转配置应用失败: {ex.Message}");
        }
    }

    [RelayCommand]
    private void SelectAllDIInverter(bool select)
    {
        foreach (var item in DIInverterConfigList)
        {
            item.IsInverted = select;
        }
    }

    [RelayCommand]
    private void SelectAllDOInverter(bool select)
    {
        foreach (var item in DOInverterConfigList)
        {
            item.IsInverted = select;
        }
    }

    #endregion

    #region 私有方法

    private void InitializeIOLists()
    {
        DIList.Clear();
        DOList.Clear();
        for (int i = 0; i < DICount; i++)
        {
            DIList.Add(new IOViewModel
            {
                Index = i,
                Description = $"数字输入{i}"
            });
        }

        for (int i = 0; i < DOCount; i++)
        {
            var doItem = new IOViewModel
            {
                Index = i,
                Description = $"数字输出{i}",
                ValueChangedCallback = OnDOValueChanged
            };
            DOList.Add(doItem);
        }
    }

    private void InitializeBitStatusLists()
    {
        ModbusDIBits.Clear();
        ModbusDOBits.Clear();
        for (int i = ModbusStartIndex; i <= ModbusEndIndex; i++)
        {
            ModbusDIBits.Add(new BitStatusViewModel { Index = i });
            ModbusDOBits.Add(new BitStatusViewModel { Index = i });
        }

        MultiDIBits.Clear();
        MultiDOBits.Clear();
        for (int i = MultiStartIndex; i <= MultiEndIndex; i++)
        {
            MultiDIBits.Add(new BitStatusViewModel { Index = i });
            MultiDOBits.Add(new EditableBitStatusViewModel { Index = i });
        }
    }

    private void InitializeInverterConfigLists()
    {
        DIInverterConfigList.Clear();
        DOInverterConfigList.Clear();

        for (int i = 0; i < 64; i++) // 默认配置64个
        {
            DIInverterConfigList.Add(new InverterConfigViewModel { Index = i, Description = $"DI {i}" });
            DOInverterConfigList.Add(new InverterConfigViewModel { Index = i, Description = $"DO {i}" });
        }
    }

    private void UpdateBitStatus(bool[] data, ICollection<BitStatusViewModel> bitList, int startIndex)
    {
        for (int i = 0; i < data.Length; i++)
        {
            int index = startIndex + i;
            var bit = bitList.FirstOrDefault(b => b.Index == index);
            if (bit != null)
            {
                bit.IsActive = data[i];
            }
        }
    }

    private void UpdateBitStatus(bool[] data, ICollection<EditableBitStatusViewModel> bitList, int startIndex)
    {
        for (int i = 0; i < data.Length; i++)
        {
            int index = startIndex + i;
            var bit = bitList.FirstOrDefault(b => b.Index == index);
            if (bit != null)
            {
                bit.IsActive = data[i];
            }
        }
    }

    private bool[] GenerateBoolValuesFromBits()
    {
        var values = new bool[MultiDOBits.Count];
        for (int i = 0; i < MultiDOBits.Count; i++)
        {
            values[i] = MultiDOBits[i].IsActive;
        }

        return values;
    }

    private void OnDOValueChanged(int index, bool value)
    {
        try
        {
            if (!_zMotionManager.IsConnected)
            {
                //ShowMessage("设备未连接");
                return;
            }

            _zMotionManager.SetDO(index, value);
        }
        catch (Exception ex)
        {
            ShowMessage($"DO {index} 设置失败: {ex.Message}");
        }
    }

    private void ShowMessage(string message)
    {
        // 实际项目中应使用更完善的消息提示机制
        //System.Diagnostics.Debug.WriteLine($"[IOControlViewModel] {message}");
        MessageBox.Show(message, "信息", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    partial void OnDICountChanged(int value)
    {
        InitializeIOLists();
    }

    partial void OnDOCountChanged(int value)
    {
        InitializeIOLists();
    }

    partial void OnModbusStartIndexChanged(int value)
    {
        InitializeBitStatusLists();
    }

    partial void OnModbusEndIndexChanged(int value)
    {
        InitializeBitStatusLists();
    }

    partial void OnMultiStartIndexChanged(ushort value)
    {
        InitializeBitStatusLists();
    }

    partial void OnMultiEndIndexChanged(ushort value)
    {
        InitializeBitStatusLists();
    }

    #endregion
}

/// <summary>
/// 位状态ViewModel - 用于可视化显示位状态
/// </summary>
public partial class BitStatusViewModel : ObservableObject
{
    [ObservableProperty] private int index;

    [ObservableProperty] private bool isActive;

    public string DisplayText => $"{Index}";
    public string StatusText => IsActive ? "ON" : "OFF";
}

/// <summary>
/// 可编辑位状态ViewModel - 用于DO位状态的交互编辑
/// </summary>
public partial class EditableBitStatusViewModel : ObservableObject
{
    [ObservableProperty] private int index;

    [ObservableProperty] private bool isActive;

    public string DisplayText => $"{Index}";
    public string StatusText => IsActive ? "ON" : "OFF";
}