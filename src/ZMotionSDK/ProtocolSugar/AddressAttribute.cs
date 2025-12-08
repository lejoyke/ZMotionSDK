namespace ZMotionSDK.ProtocolSugar;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AddressAttribute(int address) : Attribute
{
    /// <summary>
    /// 字节地址
    /// </summary>
    public int Address { get; } = address;
}