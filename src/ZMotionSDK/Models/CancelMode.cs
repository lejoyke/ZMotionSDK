namespace ZMotionSDK.Models;

public enum CancelMode : int
{
    取消当前运动 = 0,
    取消缓冲的运动 = 1,
    取消当前运动和缓冲运动 = 2,
    立即中断脉冲发送 = 3,
}