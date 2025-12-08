using ZMotionSDK.ProtocolSugar;

namespace ZMotionSDK.ProtocolSugar;

public sealed class PropertyValueSetter<TProtocol>(
    IProtocolDataMapping<TProtocol> writeMapping,
    int address) where TProtocol : struct
{
    private readonly IProtocolDataMapping<TProtocol> _writeMapping = writeMapping;
    private readonly int _address = address;

    public IProtocolDataMapping<TProtocol> Value(bool value)
    {
        _writeMapping.AddData(_address, value);
        return _writeMapping;
    }
}
