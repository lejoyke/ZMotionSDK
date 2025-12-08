using System.Text;

namespace ZMotionSDK.Models;

public struct AxisStatus
{
    /// <summary>
    /// 状态值
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// 随动误差超限告警
    /// </summary>
    public bool Alarm_FollowOverLimit { get; set; }


    /// <summary>
    /// 与远程轴通讯错误
    /// </summary>
    public bool Error_Communication { get; set; }

    /// <summary>
    /// 轴告警
    /// </summary>
    public bool Alarm_Axis { get; set; }


    /// <summary>
    /// 正向硬限位
    /// </summary>
    public bool Limit_Forward { get; set; }

    /// <summary>
    /// 反向硬限位
    /// </summary>
    public bool Limit_Backward { get; set; }


    /// <summary>
    /// 找原点中
    /// </summary>
    public bool IsFindingHome { get; set; }

    /// <summary>
    /// 随动误差超限出错
    /// </summary>
    public bool Error_FollowOverLimit { get; set; }


    /// <summary>
    /// 超过正向软限位
    /// </summary>
    public bool Limit_Forward_Soft { get; set; }


    /// <summary>
    /// 超过反向软限位
    /// </summary>
    public bool Limit_Backward_Soft { get; set; }

    /// <summary>
    /// 电源异常
    /// </summary>
    public bool Error_Power { get; set; }


    /// <summary>
    /// 轴速度保护
    /// </summary>
    public bool AxisSpeedProtection { get; set; }


    /// <summary>
    /// 运动中触发特殊指令事变
    /// </summary>
    public bool Error_SpecialCommand { get; set; }

    /// <summary>
    /// 告警信号输入
    /// </summary>
    public bool Alarm_Input { get; set; }


    /// <summary>
    /// 轴进入暂停状态
    /// </summary>
    public bool IsPause { get; set; }


    /// <summary>
    /// 轴运动被取消
    /// </summary>
    public bool IsCancelled { get; set; }


    public static AxisStatus Parse(int value)
    {
        var status = new AxisStatus();
        status.Alarm_FollowOverLimit = value.GetBit(1);
        status.Error_Communication = value.GetBit(2);
        status.Alarm_Axis = value.GetBit(3);
        status.Limit_Forward = value.GetBit(4);
        status.Limit_Backward = value.GetBit(5);
        status.IsFindingHome = value.GetBit(6);

        status.Error_FollowOverLimit = value.GetBit(8);
        status.Limit_Forward_Soft = value.GetBit(9);
        status.Limit_Backward_Soft = value.GetBit(10);
        status.IsCancelled = value.GetBit(11);

        status.Error_Power = value.GetBit(18);

        status.AxisSpeedProtection = value.GetBit(20);

        status.Error_SpecialCommand = value.GetBit(21);
        status.Alarm_Input = value.GetBit(22);
        status.IsPause = value.GetBit(23);

        status.Value = value;

        return status;
    }

    public override string ToString()
    {
        if (Value == 0) return "";

        var str = new List<string>();
        if (Alarm_FollowOverLimit) str.Add("随动误差超限告警");
        if (Error_Communication) str.Add("与远程轴通讯错误");
        if (Alarm_Axis) str.Add("轴告警");
        if (Limit_Forward) str.Add("正向硬限位");
        if (Limit_Backward) str.Add("反向硬限位");
        if (IsFindingHome) str.Add("找原点中");
        if (Error_FollowOverLimit) str.Add("随动误差超限出错");
        if (Limit_Forward_Soft) str.Add("超过正向软限位");
        if (Limit_Backward_Soft) str.Add("超过反向软限位");
        if (Error_Power) str.Add("电源异常");
        if (AxisSpeedProtection) str.Add("轴速度保护");
        if (Error_SpecialCommand) str.Add("运动中触发特殊指令事变");
        if (Alarm_Input) str.Add("告警信号输入");
        if (IsPause) str.Add("轴进入暂停状态");
        if (IsCancelled) str.Add("轴运动被取消");

        return string.Join(",", str);
    }
}