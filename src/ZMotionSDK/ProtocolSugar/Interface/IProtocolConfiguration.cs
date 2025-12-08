using System.Collections.Frozen;
using System.Linq.Expressions;

namespace ZMotionSDK.ProtocolSugar;

public interface IProtocolConfiguration<TProtocol> where TProtocol : struct
{
    /// <summary>
    /// 协议起始地址
    /// </summary>
    int StartAddress { get; }

    /// <summary>
    /// 协议数据大小
    /// </summary>
    int Size { get; }

    /// <summary>
    /// 地址映射表(字段名/属性名 -> 地址)
    /// </summary>
    FrozenDictionary<string, int> AddressMapping { get; }

    /// <summary>
    /// 获取字段的地址
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public int GetAddress(string memberName);

    /// <summary>
    /// 获取字段的地址
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public int GetAddress(Expression<Func<TProtocol, bool>> expression);
}
