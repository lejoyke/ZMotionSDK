using System.Linq.Expressions;

namespace ZMotionSDK.ProtocolSugar;

public interface IMessageBuilder
{
    public ZMotion? ZMotion { get; set; }

    /// <summary>
    /// 写入对应的地址和数据
    /// </summary>
    /// <param name="datas">地址和数据的映射表</param>
    /// <returns></returns>
    void Commit(Dictionary<int, bool> datas);
    
    /// <summary>
    /// 写入协议中的某个值
    /// </summary>
    /// <param name="address"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    void Write(int address, bool value);


    /// <summary>
    /// 读取DI地址的某个值
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    bool ReadDI(int address);

    /// <summary>
    /// 读取DO地址的某个值
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    bool ReadDO(int address);
}

public interface IMessageBuilder<TDIProtocol,TDOProtocol> : IMessageBuilder where TDIProtocol : struct where TDOProtocol : struct
{
    public IProtocolConfiguration<TDIProtocol> DIConfiguration { get; }

    public IProtocolConfiguration<TDOProtocol> DOConfiguration { get; }
    
    public int GetDIAddress(Expression<Func<TDIProtocol, bool>> propertyExpression);
    
    public int GetDOAddress(Expression<Func<TDOProtocol, bool>> propertyExpression);

    /// <summary>
    /// 创建写入映射表
    /// </summary>
    /// <returns></returns>
    IProtocolDataMapping<TDOProtocol> CreateWriteMapping();

    /// <summary>
    /// 单独读取DI协议中某段数据
    /// </summary>
    /// <returns></returns>
    bool ReadDI(Expression<Func<TDIProtocol, bool>> propertyExpression);

    /// <summary>
    /// 单独读取DO协议中某段数据
    /// </summary>
    /// <returns></returns>
    bool ReadDO(Expression<Func<TDOProtocol, bool>> propertyExpression);

    /// <summary>
    /// 单独写入DO协议中某段数据
    /// </summary>
    /// <returns></returns>
    void Write(Expression<Func<TDOProtocol, bool>> propertyExpression, bool Value);


    /// <summary>
    /// 读取DI协议
    /// </summary>
    /// <returns></returns>
    TDIProtocol ReadDI();

    /// <summary>
    /// 读取DI协议数据
    /// </summary>
    /// <returns></returns>
    bool[] ReadDIData();

    /// <summary>
    /// 读取DO协议
    /// </summary>
    /// <returns></returns>
    TDOProtocol ReadDO();

    /// <summary>
    /// 读取DO协议数据
    /// </summary>
    /// <returns></returns>
    bool[] ReadDOData();

    /// <summary>
    /// 写入DO协议
    /// </summary>
    /// <param name="protocol"></param>
    /// <returns></returns>
    void Write(TDOProtocol protocol);
}
