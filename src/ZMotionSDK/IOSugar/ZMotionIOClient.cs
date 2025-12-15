using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MessageToolkit;
using MessageToolkit.Abstractions;
using MessageToolkit.Models;

namespace ZMotionSDK;

/// <summary>
/// ZMotion IO 协议客户端，基于 ZMotion 提供类型安全的 DI/DO 操作
/// </summary>
public class ZMotionIOClient<TDIProtocol, TDOProtocol>
    where TDIProtocol : struct
    where TDOProtocol : struct
{
    #region 字段和属性

    private readonly INativeFrameBuilder<TDIProtocol, bool> _diBuilder;
    private readonly INativeFrameBuilder<TDOProtocol, bool> _doBuilder;

    /// <summary>
    /// ZMotion 实例
    /// </summary>
    public required ZMotion ZMotion { get; init; }

    public bool[] LastRawDIData { get; private set; }

    public bool[] LastRawDOData { get; private set; }

    /// <summary>
    /// 上次读取的 DI 协议数据（缓存）
    /// </summary>
    public TDIProtocol? LastDIData { get; private set; }

    /// <summary>
    /// 上次读取的 DO 协议数据（缓存）
    /// </summary>
    public TDOProtocol? LastDOData { get; private set; }

    /// <summary>
    /// DI 协议模式
    /// </summary>
    public IProtocolSchema<TDIProtocol> DISchema => _diBuilder.Schema;

    /// <summary>
    /// DO 协议模式
    /// </summary>
    public IProtocolSchema<TDOProtocol> DOSchema => _doBuilder.Schema;

    #endregion

    #region 构造函数

    public ZMotionIOClient()
    {
        _diBuilder = FrameBuilderFactory.CreateNative<TDIProtocol, bool>();
        _doBuilder = FrameBuilderFactory.CreateNative<TDOProtocol, bool>();

        // 检查是否不存在地址为0的属性
        if (!_diBuilder.Schema.Properties.Any(p => p.Value.Address == 0))
        {
            throw new InvalidOperationException("TDIProtocol does not contain any properties mapped to address 0, which is required.");
        }
        if (!_doBuilder.Schema.Properties.Any(p => p.Value.Address == 0))
        {
            throw new InvalidOperationException("TDOProtocol does not contain any properties mapped to address 0, which is required.");
        }

        LastRawDIData = new bool[DISchema.TotalSize];
        LastRawDOData = new bool[DOSchema.TotalSize];
    }

    #endregion

    #region 地址和映射

    /// <summary>
    /// 获取 DI 协议字段的地址
    /// </summary>
    public int GetDIAddress(Expression<Func<TDIProtocol, bool>> propertyExpression)
    {
        return _diBuilder.Schema.GetAddress(propertyExpression);
    }

    /// <summary>
    /// 获取 DO 协议字段的地址
    /// </summary>
    public int GetDOAddress(Expression<Func<TDOProtocol, bool>> propertyExpression)
    {
        return _doBuilder.Schema.GetAddress(propertyExpression);
    }

    /// <summary>
    /// 创建批量写入映射
    /// </summary>
    public ProtocolDataMapping<TDOProtocol> CreateWriteMapping()
    {
        return new ProtocolDataMapping<TDOProtocol>(_doBuilder.Schema, Commit);
    }

    #endregion

    #region 单个点位操作（通过表达式）

    /// <summary>
    /// 读取单个 DI 点位
    /// </summary>
    public bool ReadDI(Expression<Func<TDIProtocol, bool>> propertyExpression)
    {
        CheckZMotion();
        var address = GetDIAddress(propertyExpression);
        var value = ZMotion.GetDI(address);
        LastRawDIData[address] = value;
        return value;
    }

    public bool ReadDI(int address)
    {
        CheckZMotion();
        var value = ZMotion.GetDI(address);
        LastRawDIData[address] = value;
        return value;
    }

    /// <summary>
    /// 读取单个 DO 点位
    /// </summary>
    public bool ReadDO(Expression<Func<TDOProtocol, bool>> propertyExpression)
    {
        CheckZMotion();
        var address = GetDOAddress(propertyExpression);
        var value = ZMotion.GetDO(address);
        LastRawDOData[address] = value;
        return value;
    }

    public bool ReadDO(int address)
    {
        CheckZMotion();
        var value = ZMotion.GetDO(address);
        LastRawDOData[address] = value;
        return value;
    }

    /// <summary>
    /// 写入单个 DO 点位
    /// </summary>
    public void Write(Expression<Func<TDOProtocol, bool>> propertyExpression, bool value)
    {
        CheckZMotion();
        var address = GetDOAddress(propertyExpression);
        ZMotion.SetDO(address, value);
    }

    /// <summary>
    /// 写入指定地址的 DO 点位
    /// </summary>
    public void Write(int address, bool value)
    {
        CheckZMotion();
        ZMotion.SetDO(address, value);
    }

    #endregion

    #region 完整协议数据操作

    /// <summary>
    /// 读取完整的 DI 协议数据
    /// </summary>
    public TDIProtocol ReadDI()
    {
        CheckZMotion();
        var startAddress = DISchema.StartAddress;
        var endAddress = startAddress + DISchema.TotalSize - 1;
        var data = ZMotion.GetDI_Multi(startAddress, endAddress);
        var result = _diBuilder.Codec.Decode(data);
        LastRawDIData = data;
        LastDIData = result; // 缓存数据
        return result;
    }

    /// <summary>
    /// 读取完整的 DO 协议数据
    /// </summary>
    public TDOProtocol ReadDO()
    {
        CheckZMotion();
        var startAddress = (ushort)DOSchema.StartAddress;
        var endAddress = (ushort)(DOSchema.StartAddress + DOSchema.TotalSize - 1);
        var data = ZMotion.GetDO_Multi(startAddress, endAddress);
        var result = _doBuilder.Codec.Decode(data);
        LastRawDOData = data;
        LastDOData = result; // 缓存数据
        return result;
    }

    /// <summary>
    /// 写入完整的 DO 协议数据
    /// </summary>
    public void Write(TDOProtocol protocol)
    {
        CheckZMotion();
        var values = _doBuilder.Codec.Encode(protocol);
        var startAddress = (ushort)DOSchema.StartAddress;
        ZMotion.SetDO_Multi(startAddress, values);
    }

    #endregion

    #region 原始数据操作

    /// <summary>
    /// 读取原始 DI 数据数组
    /// </summary>
    public bool[] ReadDIData()
    {
        CheckZMotion();
        var startAddress = DISchema.StartAddress;
        var endAddress = startAddress + DISchema.TotalSize - 1;
        var data = ZMotion.GetDI_Multi(startAddress, endAddress);
        LastRawDIData = data;
        return data;
    }

    /// <summary>
    /// 读取原始 DO 数据数组
    /// </summary>
    public bool[] ReadDOData()
    {
        CheckZMotion();
        var startAddress = (ushort)DOSchema.StartAddress;
        var endAddress = (ushort)(DOSchema.StartAddress + DOSchema.TotalSize - 1);
        var data = ZMotion.GetDO_Multi(startAddress, endAddress);
        LastRawDOData = data;
        return data;
    }

    /// <summary>
    /// 写入原始 DO 数据数组
    /// </summary>
    public void Write(bool[] values)
    {
        CheckZMotion();
        var startAddress = (ushort)DOSchema.StartAddress;
        ZMotion.SetDO_Multi(startAddress, values);
    }

    #endregion

    #region 内部辅助方法

    /// <summary>
    /// 批量提交 DO 数据写入
    /// </summary>
    public void Commit(Dictionary<int, bool> datas)
    {
        CheckZMotion();
        foreach (var (address, value) in datas)
        {
            ZMotion.SetDO(address, value);
        }
    }

    public void Commit(IEnumerable<WriteFrame<bool>> frames)
    {
        CheckZMotion();
        foreach (var frame in frames)
        {
            ZMotion.SetDO_Multi((ushort)frame.StartAddress, frame.Data);
        }
    }

    [MemberNotNull(nameof(ZMotion))]
    private void CheckZMotion()
    {
        if (ZMotion == null)
        {
            throw new InvalidOperationException("ZMotion instance is not initialized. Please set the ZMotion property.");
        }
    }

    #endregion
}