using ZMotionSDK.ProtocolSugar;

namespace ZMotionTest.Protocol;

public struct DOProtocol{
    [Address(0)]
    public bool 启动 { get; set; }

    [Address(1)]
    public bool 停止 { get; set; }

    [Address(2)]
    public bool 急停 { get; set; }

    [Address(3)]
    public bool 复位 { get; set; }

    [Address(4)]
    public bool 开始运动 { get; set; }
}