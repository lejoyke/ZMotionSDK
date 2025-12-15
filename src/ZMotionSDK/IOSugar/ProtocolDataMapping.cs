using MessageToolkit;
using MessageToolkit.Abstractions;
using MessageToolkit.Models;
using System.Linq.Expressions;

namespace ZMotionSDK;

public class ProtocolDataMapping<TProtocol> where TProtocol : struct
{
    private readonly NativeDataMapping<TProtocol, bool> _baseMapping;
    private readonly IProtocolSchema<TProtocol> _schema;
    private readonly Action<IEnumerable<WriteFrame<bool>>> _commitAction;

    public ProtocolDataMapping(IProtocolSchema<TProtocol> Schema, Action<IEnumerable<WriteFrame<bool>>> commitAction)
    {
        _baseMapping = new NativeDataMapping<TProtocol, bool>(Schema);
        _schema = Schema;
        _commitAction = commitAction;
    }

    public ProtocolDataMapping<TProtocol> Write(Expression<Func<TProtocol, bool>> fieldSelector, bool value)
    {
        _baseMapping.Write(fieldSelector, value);
        return this;
    }

    public ProtocolDataMapping<TProtocol> Write(ushort address, bool value)
    {
        _baseMapping.Write(address, value);
        return this;
    }

    public PropertyValueSetterBool<TProtocol> Property(ushort address)
    {
        return new PropertyValueSetterBool<TProtocol>(this, address);
    }

    public PropertyValueSetterBool<TProtocol> Property(Expression<Func<TProtocol, bool>> fieldSelector)
    {
        ushort address = _schema.GetAddress(fieldSelector);
        return new PropertyValueSetterBool<TProtocol>(this, address);
    }

    /// <summary>
    /// 提交所有映射的数据到 ZMotion
    /// </summary>
    public void Commit()
    {
        var frames = _baseMapping.BuildOptimized();
        _commitAction(frames);
    }
}

public sealed class PropertyValueSetterBool<TProtocol> where TProtocol : struct
{
    private readonly ProtocolDataMapping<TProtocol> _writeMapping;
    private readonly ushort _address;

    public PropertyValueSetterBool(ProtocolDataMapping<TProtocol> writeMapping, ushort address)
    {
        _writeMapping = writeMapping;
        _address = address;
    }

    public ProtocolDataMapping<TProtocol> Value(bool value)
    {
        _writeMapping.Write(_address, value);
        return _writeMapping;
    }
}