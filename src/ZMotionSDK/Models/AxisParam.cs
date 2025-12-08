namespace ZMotionSDK.Models;

public struct AxisParam
{
    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// 加速度
    /// </summary>
    public float Acceleration { get; set; }

    /// <summary>
    /// 减速度
    /// </summary>
    public float Deceleration { get; set; }

    /// <summary>
    /// 平滑系数
    /// </summary>
    public float SmoothingFactor { get; set; }

    /// <summary>
    /// 指令当量
    /// </summary>
    public float Units { get; set; }

    /// <summary>
    /// 爬行速度
    /// </summary>
    public float CreepSpeed { get; set; }

    /// <summary>
    /// 正向软限位位置
    /// </summary>
    public float PositiveLimit { get; set; }

    /// <summary>
    /// 负向软限位位置
    /// </summary>
    public float NegativeLimit { get; set; }

    /// <summary>
    /// 急停减速度
    /// </summary>
    public float EmergencyDeceleration { get; set; }

    public AixsSpeedParam ToAixsSpeedParam()
    {
        return new AixsSpeedParam
        {
            Speed = Speed,
            Acceleration = Acceleration,
            Deceleration = Deceleration,
            SmoothingFactor = SmoothingFactor,
        };
    }
}


public struct AixsSpeedParam
{
    public float Speed { get; set; }

    public float Acceleration { get; set; }

    public float Deceleration { get; set; }

    public float SmoothingFactor { get; set; }
}