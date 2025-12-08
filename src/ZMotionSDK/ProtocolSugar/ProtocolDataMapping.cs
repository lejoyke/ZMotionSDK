using System.Linq.Expressions;

namespace ZMotionSDK.ProtocolSugar;

public class ProtocolDataMapping<TProtocol>(IProtocolConfiguration<TProtocol> configuration
,IMessageBuilder messageBuilder) : IProtocolDataMapping<TProtocol> where TProtocol : struct
{
    private readonly IProtocolConfiguration<TProtocol> _configuration = configuration;
    private readonly IMessageBuilder _messageBuilder = messageBuilder;
    private readonly Dictionary<int, bool> _datas = [];

    public void AddData(int address, bool data)
    {
        _datas.Add(address, data);
    }

    public void Commit()
    {
        _messageBuilder.Commit(_datas);
    }

    public PropertyValueSetter<TProtocol> Property(Expression<Func<TProtocol, bool>> propertyExpression)
    {
        var address = _configuration.GetAddress(propertyExpression);

        return new PropertyValueSetter<TProtocol>(this, address);
    }

    public PropertyValueSetter<TProtocol> Property(int address)
    {
        return new PropertyValueSetter<TProtocol>(this, address);
    }
}