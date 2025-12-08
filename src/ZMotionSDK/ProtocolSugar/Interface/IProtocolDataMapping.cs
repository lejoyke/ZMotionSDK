using System.Linq.Expressions;

namespace ZMotionSDK.ProtocolSugar;

public interface IProtocolDataMapping
{
    /// <summary>
    /// 向映射表中添加数据
    /// </summary>
    /// <param name="address"></param>
    /// <param name="data"></param>
    void AddData(int address, bool data);

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <returns></returns>
    void Commit();
}

public interface IProtocolDataMapping<TProtocol> : IProtocolDataMapping where TProtocol : struct
{
    /// <summary>
    /// Set the value of the property
    /// </summary>
    /// <param name="propertyExpression"></param>
    /// <returns></returns>
    PropertyValueSetter<TProtocol> Property(Expression<Func<TProtocol, bool>> propertyExpression);


    PropertyValueSetter<TProtocol> Property(int address);
}