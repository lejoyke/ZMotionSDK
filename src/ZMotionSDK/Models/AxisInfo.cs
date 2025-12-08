namespace ZMotionSDK.Models;

public struct AxisInfo{

    /// <summary>
    /// 轴是否运行
    /// </summary>
    public bool IsRunning{get;set;}
    
    /// <summary>
    /// 轴的规划位置
    /// </summary>
    public float DPosition{get;set;}   

    /// <summary>
    /// 轴的反馈位置
    /// </summary>
    public float MPosition{get;set;}

    /// <summary>
    /// 轴的状态
    /// </summary>
    public AxisStatus AxisStatus{get;set;}
}