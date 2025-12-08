namespace ZMotionSDK.Models;

public class ZMotionHeartBeatMessage
{
    public AxisMotionState[] AxisMotionStates { get; set; } = [];

    public bool[] DIData { get; set; } = [];

    public bool[] DOData { get; set; } = [];
}