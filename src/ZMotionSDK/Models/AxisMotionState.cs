namespace ZMotionSDK.Models;

public struct AxisMotionState
{
    /// <summary>
    /// 当前位置
    /// </summary>
    public float CurrentPosition { get; set; }

    /// <summary>
    /// 规划位置
    /// </summary>
    public float PlanPosition { get; set; }

    /// <summary>
    /// 当前速度
    /// </summary>
    public float CurrentSpeed { get; set; }

    /// <summary>
    /// 规划速度
    /// </summary>
    public float PlanSpeed { get; set; }

    /// <summary>
    /// 剩余距离
    /// </summary>
    public float RemainingDistance { get; set; }

    /// <summary>
    /// 最终位置
    /// </summary>
    public float FinalPosition { get; set; }


    /// <summary>
    /// 运动缓冲数
    /// </summary>
    public float MovesBuffered { get; set; }

    /// <summary>
    /// 编码器原始值
    /// </summary>
    public float Encoder { get; set; }

    /// <summary>
    /// 轴停止原因
    /// </summary>
    public AxisStatus StopReason { get; set; }

    /// <summary>
    /// 是否运行
    /// </summary>
    public bool IsRunning { get; set; }

    /// <summary>
    /// 轴状态
    /// </summary>
    public AxisStatus Status { get; set; }
}
