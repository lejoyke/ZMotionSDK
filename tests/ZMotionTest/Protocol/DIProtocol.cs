using MessageToolkit.Attributes;

namespace ZMotionTest.Protocol;

public struct DIProtocol{
    [Address(0)]
    public bool 启动按钮 { get; set; }

    [Address(1)]
    public bool 停止按钮 { get; set; }

    [Address(2)]
    public bool 急停按钮 { get; set; }

    [Address(3)]
    public bool 复位按钮 { get; set; }

    [Address(4)]
    public bool 到位传感器 { get; set; }

    [Address(5)]
    public bool 急停传感器 { get; set; }

    [Address(6)]
    public bool 复位传感器 { get; set; }
}