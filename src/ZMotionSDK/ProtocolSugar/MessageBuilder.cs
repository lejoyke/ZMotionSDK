using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;

namespace ZMotionSDK.ProtocolSugar;

public class MessageBuilder<TDIProtocol, TDOProtocol> : IMessageBuilder<TDIProtocol, TDOProtocol> where TDIProtocol : struct where TDOProtocol : struct
{
    public ZMotion? ZMotion { get; set; }

    public IProtocolConfiguration<TDIProtocol> DIConfiguration { get; private set; }
    public IProtocolConfiguration<TDOProtocol> DOConfiguration { get; private set; }

    // 反射缓存，提高性能
    private static readonly ConcurrentDictionary<string, PropertyInfo?> _diPropertyCache = new();
    private static readonly ConcurrentDictionary<string, FieldInfo?> _diFieldCache = new();
    private static readonly ConcurrentDictionary<string, PropertyInfo?> _doPropertyCache = new();
    private static readonly ConcurrentDictionary<string, FieldInfo?> _doFieldCache = new();

    public MessageBuilder()
    {
        DIConfiguration = new ProtocolConfiguration<TDIProtocol>();
        DOConfiguration = new ProtocolConfiguration<TDOProtocol>();
    }
    
    public int GetDIAddress(Expression<Func<TDIProtocol, bool>> propertyExpression)
    {
       return  DIConfiguration.GetAddress(propertyExpression);
    }

    public int GetDOAddress(Expression<Func<TDOProtocol, bool>> propertyExpression)
    {
        return DOConfiguration.GetAddress(propertyExpression);
    }

    public IProtocolDataMapping<TDOProtocol> CreateWriteMapping()
    {
        return new ProtocolDataMapping<TDOProtocol>(DOConfiguration, this);
    }

    public bool ReadDI(Expression<Func<TDIProtocol, bool>> propertyExpression)
    {
        CheckZMotion();

        var address = DIConfiguration.GetAddress(propertyExpression);
        return ZMotion.GetDI(address);
    }

    public bool ReadDO(Expression<Func<TDOProtocol, bool>> propertyExpression)
    {
        CheckZMotion();

        var address = DOConfiguration.GetAddress(propertyExpression);
        return ZMotion.GetDO(address);
    }

    public void Write(Expression<Func<TDOProtocol, bool>> propertyExpression, bool Value)
    {
        CheckZMotion();

        var address = DOConfiguration.GetAddress(propertyExpression);
        ZMotion.SetDO(address, Value);
    }

    /// <summary>
    /// 读取数字输入协议数据
    /// </summary>
    /// <returns>包含所有DI状态的协议实例</returns>
    /// <exception cref="InvalidOperationException">当ZMotion未初始化时抛出</exception>
    public virtual TDIProtocol ReadDI()
    {
        CheckZMotion();

        var datas = ZMotion.GetDI_Multi(DIConfiguration.StartAddress, DIConfiguration.StartAddress + DIConfiguration.Size - 1);
        
        // 装箱为object以确保反射操作生效（解决struct值类型问题）
        object protocolObj = Activator.CreateInstance<TDIProtocol>();
        
        // 遍历地址映射，根据地址设置对应的字段/属性值
        foreach (var mapping in DIConfiguration.AddressMapping)
        {
            var memberName = mapping.Key;
            var address = mapping.Value;
            var dataIndex = address - DIConfiguration.StartAddress;
            
            // 检查数组边界
            if (dataIndex < 0 || dataIndex >= datas.Length)
            {
                continue;
            }
            
            var value = datas[dataIndex];
            
            // 使用缓存获取PropertyInfo
            var property = _diPropertyCache.GetOrAdd($"{typeof(TDIProtocol).FullName}.{memberName}", 
                _ => typeof(TDIProtocol).GetProperty(memberName));
            
            if (property != null && property.CanWrite && property.PropertyType == typeof(bool))
            {
                property.SetValue(protocolObj, value);
                continue; // 如果属性设置成功，跳过字段设置
            }
            
            // 如果没有属性或属性不可写，尝试设置字段
            var field = _diFieldCache.GetOrAdd($"{typeof(TDIProtocol).FullName}.{memberName}", 
                _ => typeof(TDIProtocol).GetField(memberName));
            
            if (field != null && !field.IsInitOnly && field.FieldType == typeof(bool))
            {
                field.SetValue(protocolObj, value);
            }
        }
        
        // 拆箱返回修改后的struct
        return (TDIProtocol)protocolObj;
    }

    public bool[] ReadDIData()
    {
        CheckZMotion();
        return ZMotion.GetDI_Multi(DIConfiguration.StartAddress, DIConfiguration.StartAddress + DIConfiguration.Size - 1);
    }

    /// <summary>
    /// 读取数字输出协议数据
    /// </summary>
    /// <returns>包含所有DO状态的协议实例</returns>
    /// <exception cref="InvalidOperationException">当ZMotion未初始化时抛出</exception>
    public virtual TDOProtocol ReadDO()
    {
        CheckZMotion();

        var datas = ZMotion.GetDO_Multi((ushort)DOConfiguration.StartAddress, (ushort)(DOConfiguration.StartAddress + DOConfiguration.Size - 1));
        
        // 装箱为object以确保反射操作生效（解决struct值类型问题）
        object protocolObj = Activator.CreateInstance<TDOProtocol>();
        
        // 遍历地址映射，根据地址设置对应的字段/属性值
        foreach (var mapping in DOConfiguration.AddressMapping)
        {
            var memberName = mapping.Key;
            var address = mapping.Value;
            var dataIndex = address - DOConfiguration.StartAddress;
            
            // 检查数组边界
            if (dataIndex < 0 || dataIndex >= datas.Length)
            {
                continue;
            }
            
            var value = datas[dataIndex];
            
            // 使用缓存获取PropertyInfo
            var property = _doPropertyCache.GetOrAdd($"{typeof(TDOProtocol).FullName}.{memberName}", 
                _ => typeof(TDOProtocol).GetProperty(memberName));
            
            if (property != null && property.CanWrite && property.PropertyType == typeof(bool))
            {
                property.SetValue(protocolObj, value);
                continue; // 如果属性设置成功，跳过字段设置
            }
            
            // 如果没有属性或属性不可写，尝试设置字段
            var field = _doFieldCache.GetOrAdd($"{typeof(TDOProtocol).FullName}.{memberName}", 
                _ => typeof(TDOProtocol).GetField(memberName));
            
            if (field != null && !field.IsInitOnly && field.FieldType == typeof(bool))
            {
                field.SetValue(protocolObj, value);
            }
        }
        
        // 拆箱返回修改后的struct
        return (TDOProtocol)protocolObj;
    }

    public bool[] ReadDOData()
    {
        CheckZMotion();
        return ZMotion.GetDO_Multi((ushort)DOConfiguration.StartAddress, (ushort)(DOConfiguration.StartAddress + DOConfiguration.Size - 1));
    }

    /// <summary>
    /// 将协议数据写入数字输出
    /// </summary>
    /// <param name="protocol">要写入的DO协议数据</param>
    /// <exception cref="InvalidOperationException">当ZMotion未初始化时抛出</exception>
    public virtual void Write(TDOProtocol protocol)
    {
        CheckZMotion();

        var values = new bool[DOConfiguration.Size];
        
        // 遍历配置的地址映射
        foreach (var mapping in DOConfiguration.AddressMapping)
        {
            var memberName = mapping.Key;
            var address = mapping.Value;
            var dataIndex = address - DOConfiguration.StartAddress;
            
            // 检查数组边界
            if (dataIndex < 0 || dataIndex >= values.Length)
            {
                continue;
            }
            
            bool value = false;
            
            // 使用缓存获取PropertyInfo并尝试读取属性值
            var property = _doPropertyCache.GetOrAdd($"{typeof(TDOProtocol).FullName}.{memberName}", 
                _ => typeof(TDOProtocol).GetProperty(memberName));
            
            if (property != null && property.CanRead && property.PropertyType == typeof(bool))
            {
                value = (bool)(property.GetValue(protocol) ?? false);
            }
            else
            {
                // 如果没有属性或属性不可读，尝试读取字段值
                var field = _doFieldCache.GetOrAdd($"{typeof(TDOProtocol).FullName}.{memberName}", 
                    _ => typeof(TDOProtocol).GetField(memberName));
                
                if (field != null && field.FieldType == typeof(bool))
                {
                    value = (bool)(field.GetValue(protocol) ?? false);
                }
            }
            
            values[dataIndex] = value;
        }
        
        // 批量写入DO
        ZMotion.SetDO_Multi((ushort)DOConfiguration.StartAddress, values);
    }

    public void Write(bool[] values)
    {
        CheckZMotion();
        ZMotion.SetDO_Multi((ushort)DOConfiguration.StartAddress, values);
    }

    public void Write(int address, bool value)
    {
        CheckZMotion();
        ZMotion.SetDO(address, value);
    }

    public bool ReadDI(int address)
    {
        CheckZMotion();
        return ZMotion.GetDI(address);
    }

    public bool ReadDO(int address)
    {
        CheckZMotion();
        return ZMotion.GetDO(address);
    }

    public void Commit(Dictionary<int, bool> datas)
    {
        CheckZMotion();
        
        foreach (var data in datas)
        {
            ZMotion.SetDO(data.Key, data.Value);
        }
    }

    [MemberNotNull(nameof(ZMotion))]
    private void CheckZMotion()
    {
        if (ZMotion == null)
        {
            throw new InvalidOperationException("ZMotion is not set. Please initialize ZMotion before using the MessageBuilder.");
        }
    }
}