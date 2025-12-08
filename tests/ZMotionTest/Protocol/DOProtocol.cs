using ZMotionSDK.ProtocolSugar;

namespace ZMotionTest.Protocol;

public struct DOProtocol{
    [Address(0)]
    public bool 启动;

    [Address(1)]
    public bool 停止;

    [Address(2)]
    public bool 急停;

    [Address(3)]
    public bool 复位;

    [Address(4)]
    public bool 开始运动;
}