namespace ZMotionSDK.Models;

/// <summary>
/// 等待运动完成的结果
/// </summary>
public struct WaitMoveResult
{
    /// <summary>
    /// 轴号
    /// </summary>
    public int Axis { get; set; }

    /// <summary>
    /// 是否成功完成运动
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 经过的时间（毫秒）
    /// </summary>
    public double ElapsedTime { get; set; }

    public override string ToString()
    {
        return $"Axis: {Axis}, IsSuccess: {IsSuccess}, ElapsedTime: {ElapsedTime}ms";
    }
}