using System.Text;
using ZMotionSDK.Models;

namespace ZMotionSDK;

public partial class ZMotion
{
    /// <summary>
    /// 获取轴参数
    /// </summary>
    /// <param name="axisCount">轴数</param>
    /// <returns>轴参数</returns>
    public AxisParam[] GetAxisParam(int axisCount)
    {
        var axisParams = new AxisParam[axisCount];

        var speeds = GetParam_Multi(axisCount, ReadBaiscParmName.Speed);
        var accelerations = GetParam_Multi(axisCount, ReadBaiscParmName.Accel);
        var decelerations = GetParam_Multi(axisCount, ReadBaiscParmName.Decel);
        var smoothingFactors = GetParam_Multi(axisCount, ReadBaiscParmName.Sramp);
        var units = GetParam_Multi(axisCount, ReadBaiscParmName.Units);
        var positiveLimits = GetParam_Multi(axisCount, ReadBaiscParmName.Fs_Limit);
        var negativeLimits = GetParam_Multi(axisCount, ReadBaiscParmName.Rs_Limit);
        var emergencyDecelerations = GetParam_Multi(axisCount, ReadBaiscParmName.Fastdec);
        var creepSpeeds = GetParam_Multi(axisCount, ReadBaiscParmName.Creep);


        for (int i = 0; i < axisCount; i++)
        {
            axisParams[i] = new AxisParam
            {
                Speed = speeds[i],
                Acceleration = accelerations[i],
                Deceleration = decelerations[i],
                SmoothingFactor = smoothingFactors[i],

                Units = units[i],
                NegativeLimit = negativeLimits[i],
                PositiveLimit = positiveLimits[i],
                EmergencyDeceleration = emergencyDecelerations[i],
                CreepSpeed = creepSpeeds[i]
            };
        }

        return axisParams;
    }

    /// <summary>
    /// 获取轴运动状态
    /// </summary>
    /// <param name="axisCount">轴数</param>
    /// <returns>轴运动状态</returns>
    public AxisMotionState[] GetAxisMotionState(int axisCount)
    {
        var axisMotionStates = new AxisMotionState[axisCount];

        var axisInfos = GetAxisInfo(axisCount);
        var currentSpeeds = GetParam_Multi(axisCount, ReadBaiscParmName.Mspeed);
        var remainingDistances = GetParam_Multi(axisCount, ReadBaiscParmName.Remain);
        var planSpeeds = GetParam_Multi(axisCount, ReadBaiscParmName.Vp_Speed);
        var finalPositions = GetParam_Multi(axisCount, ReadBaiscParmName.Endmove_Buffer);
        var movesBuffered = GetParam_Multi(axisCount, ReadBaiscParmName.Moves_Buffered);
        var stopReasons = GetParam_Multi(axisCount, ReadBaiscParmName.Axis_StopReason);
        var statuses = GetParam_Multi(axisCount, ReadBaiscParmName.Axisstatus);
        var encoders = GetParam_Multi(axisCount, ReadBaiscParmName.Encoder);

        for (int i = 0; i < axisCount; i++)
        {
            axisMotionStates[i] = new AxisMotionState
            {
                CurrentPosition = axisInfos[i].MPosition,
                PlanPosition = axisInfos[i].DPosition,
                RemainingDistance = remainingDistances[i],
                FinalPosition = finalPositions[i],
                CurrentSpeed = currentSpeeds[i],
                PlanSpeed = planSpeeds[i],
                IsRunning = axisInfos[i].IsRunning,
                MovesBuffered = movesBuffered[i],

                Status = AxisStatus.Parse((int)statuses[i]),
                StopReason = AxisStatus.Parse((int)stopReasons[i]),
                Encoder = encoders[i],
            };
        }

        return axisMotionStates;
    }

    /// <summary>
    /// 获取轴信号
    /// </summary>
    /// <param name="axisIndex">轴号</param>
    /// <returns>轴信号</returns>
    public AxisSignal GetAxisSignal(int axisIndex)
    {
        var axisSignal = new AxisSignal
        {
            AlarmSignal = GetAxisStatus(axisIndex).Value != 0,

            HomeSignal = GetDI_Home(axisIndex),
            PositiveLimitSignal = GetDI_ForwardLimit(axisIndex),
            NegativeLimitSignal = GetDI_BackwardLimit(axisIndex),

            RunningSignal = GetIsRunning(axisIndex),
            EnableSignal = GetAxisEnable_Bus(axisIndex)
        };

        return axisSignal;
    }

    /// <summary>
    /// 设置轴参数
    /// </summary>
    /// <param name="axisIndex">轴号</param>
    /// <param name="axisParam">轴参数</param>
    public void SetAxisParam(int axisIndex, AxisParam axisParam)
    {
        SetSpeed(axisIndex, axisParam.Speed);
        SetAccel(axisIndex, axisParam.Acceleration);
        SetDecel(axisIndex, axisParam.Deceleration);
        SetSramp(axisIndex, axisParam.SmoothingFactor);

        SetUnits(axisIndex, axisParam.Units);

        SetLimit(axisIndex, axisParam.NegativeLimit, false);
        SetLimit(axisIndex, axisParam.PositiveLimit, true);
        SetDecel_Fast(axisIndex, axisParam.EmergencyDeceleration);
        SetGoHomeCreepSpeed(axisIndex, axisParam.CreepSpeed);
    }

    /// <summary>
    /// 将速度参数修改加入缓存队列
    /// </summary>
    /// <param name="axisIndex">轴号</param>
    /// <param name="speed"></param>
    /// <param name="acceleration"></param>
    /// <param name="deceleration"></param>
    /// <param name="smoothingFactor"></param>
    public void Move_SetSpeedPara(int axisIndex, float speed, float acceleration, float deceleration, float smoothingFactor)
    {
        var axis = (uint)axisIndex;
        MovePara(axis, WriteBaiscParmName.Speed, axis, speed);
        MovePara(axis, WriteBaiscParmName.Accel, axis, acceleration);
        MovePara(axis, WriteBaiscParmName.Decel, axis, deceleration);
        MovePara(axis, WriteBaiscParmName.Sramp, axis, smoothingFactor);
    }

    /// <summary>
    /// 将速度参数修改加入缓存队列
    /// </summary>
    /// <param name="axisIndex">轴号</param>
    /// <param name="speedPara">轴速度参数</param>
    public void Move_SetSpeedPara(int axisIndex, AixsSpeedParam speedPara)
    {
        Move_SetSpeedPara(axisIndex, speedPara.Speed, speedPara.Acceleration, speedPara.Deceleration, speedPara.SmoothingFactor);
    }

    /// <summary>
    /// 立即执行速度参数修改
    /// </summary>
    /// <param name="axisIndex">轴号</param>
    /// <param name="speed">速度</param>
    /// <param name="acceleration">加速度</param>
    /// <param name="deceleration">减速度</param>
    /// <param name="smoothingFactor">平滑因子</param>
    public void SetSpeedPara(int axisIndex, float speed, float acceleration, float deceleration, float smoothingFactor)
    {
        SetSpeed(axisIndex, speed);
        SetAccel(axisIndex, acceleration);
        SetDecel(axisIndex, deceleration);
        SetSramp(axisIndex, smoothingFactor);
    }

    /// <summary>
    /// 立即执行速度参数修改
    /// </summary>
    /// <param name="axisIndex">轴号</param>
    /// <param name="speedPara">速度参数</param>
    public void SetSpeedPara(int axisIndex, AixsSpeedParam speedPara)
    {
        SetSpeed(axisIndex, speedPara.Speed);
        SetAccel(axisIndex, speedPara.Acceleration);
        SetDecel(axisIndex, speedPara.Deceleration);
        SetSramp(axisIndex, speedPara.SmoothingFactor);
    }

    #region 闭环控制参数

    /// <summary>
    /// 设置轴编码器闭环功能
    /// </summary>
    /// <param name="iaxis">轴号</param>
    /// <param name="IsON">是否打开编码器的全闭环功能 0:关闭 1：打开</param>
    public void SetEncoderServo(int iaxis, int IsON)
    {
        var cmdbuff = $"ENCODER_SERVO({iaxis}) = {IsON}";
        var cmdbuffAck = new StringBuilder(1024);

        //调用命令执行函数
        var result = zmcaux.ZAux_DirectCommand(_handle, cmdbuff, cmdbuffAck, 2048);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴编码器全闭环功能开关情况
    /// </summary>
    /// <param name="iaxis">轴号</param>
    /// <returns>是否打开编码器的全闭环功能</returns>
    public bool GetEncoderServo(int iaxis)
    {
        var cmdbuff = $"?ENCODER_SERVO({iaxis})";
        var cmdbuffAck = new StringBuilder(1024);

        //调用命令执行函数
        var result = zmcaux.ZAux_Execute(_handle, cmdbuff, cmdbuffAck, 2048);
        CheckResult(result);

        //解析返回的字符串
        if (cmdbuffAck.Length == 0)
        {
            throw new ZMotionException("返回字符串为空");
        }

        return int.Parse(cmdbuffAck.ToString()) != 0;
    }

    /// <summary>
    /// 轴全闭环开关
    /// </summary>
    /// <param name="iaxis">轴号</param>
    /// <param name="IsON">是否打轴的全闭环功能 0:关闭 1：打开</param>
    public void SetServo(int iaxis, int IsON)
    {
        var cmdbuff = $"SERVO({iaxis}) = {IsON}";
        var cmdbuffAck = new StringBuilder(1024);

        //调用命令执行函数
        var result = zmcaux.ZAux_DirectCommand(_handle, cmdbuff, cmdbuffAck, 2048);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴全闭环功能开关情况
    /// </summary>
    /// <param name="iaxis">轴号</param>
    /// <returns>轴全闭环功能开关情况</returns>
    public bool GetServo(int iaxis)
    {
        var cmdbuff = $"?SERVO({iaxis})";
        var cmdbuffAck = new StringBuilder(1024);

        var result = zmcaux.ZAux_Execute(_handle, cmdbuff, cmdbuffAck, 2048);
        CheckResult(result);

        if (cmdbuffAck.Length == 0)
        {
            throw new ZMotionException("返回字符串为空");
        }

        return int.Parse(cmdbuffAck.ToString()) != 0;
    }
    #endregion
}
