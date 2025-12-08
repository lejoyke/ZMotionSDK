namespace ZMotionSDK.Models;

public enum AxisType : int{
    虚拟轴 = 0,

    脉冲方向方式的步进和伺服 = 1,

    模拟信号控制方式的伺服 = 2,

    正交编码器 = 3,

    脉冲方向输出_正交编码器输入 = 4,

    脉冲方向输出_脉冲方向编码器输入 = 5,

    /// <summary>
    /// 可用于手轮输入
    /// </summary>
    脉冲方向方式的编码器 = 6,

    EtherCAT_周期位置模式 = 65,

    EtherCAT_周期速度模式 = 66,

    EtherCAT_周期力矩模式 = 67,
}