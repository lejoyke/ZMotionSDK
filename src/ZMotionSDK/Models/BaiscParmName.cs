namespace ZMotionSDK.Models;

/// <summary>
/// 可读取的Basic参数名称
/// </summary>
public enum ReadBaiscParmName
{   
    /// <summary>
    /// 轴类型
    /// </summary>
    Atype,
    
    /// <summary>
    /// 脉冲当量
    /// </summary>
    Units,
    
    /// <summary>
    /// 加速度
    /// </summary>
    Accel,
    
    /// <summary>
    /// 减速度
    /// </summary>
    Decel,
    
    /// <summary>
    /// 运行速度
    /// </summary>
    Speed,
    
    /// <summary>
    /// 爬行速度(回零低速)
    /// </summary>
    Creep,
    
    /// <summary>
    /// 起始速度
    /// </summary>
    Lspeed,
    
    /// <summary>
    /// 连续插补开关
    /// </summary>
    Merge,
    
    /// <summary>
    /// 加减速曲线时间设置
    /// </summary>
    Sramp,
    
    /// <summary>
    /// 轴指令规划位置
    /// </summary>
    Dpos,
    
    /// <summary>
    /// 编码器反馈位置
    /// </summary>
    Mpos,
    
    /// <summary>
    /// 当前运动目标位置
    /// </summary>
    Endmove,

    /// <summary>
    /// 正向软限位设置
    /// </summary>
    Fs_Limit,

    /// <summary>
    /// 反向软限位设置
    /// </summary>
    Rs_Limit,
    
    /// <summary>
    /// 映射原点输入
    /// </summary>
    Datum_In,
    
    /// <summary>
    /// 映射正限位输入
    /// </summary>
    Fwd_In,
    
    /// <summary>
    /// 映射反限位输入
    /// </summary>
    Rev_In,

    /// <summary>
    /// 运动状态
    /// </summary>
    Idle,
    
    /// <summary>
    /// 缓冲是否为空状态位
    /// </summary>
    Loaded,

    /// <summary>
    /// 实际编码反馈速度
    /// </summary>
    Mspeed,
    
    /// <summary>
    /// 当前运动类型
    /// </summary>
    Mtype,
    
    /// <summary>
    /// 当前运动类型
    /// </summary>
    Ntype,
    
    /// <summary>
    /// 当前运动剩余距离
    /// </summary>
    Remain,
    
    /// <summary>
    /// 当前规划运行速度
    /// </summary>
    Vp_Speed,
    
    /// <summary>
    /// 轴状态
    /// </summary>
    Axisstatus,

    /// <summary>
    /// 运动标号
    /// </summary>
    Move_Mark,

    /// <summary>
    /// 当前运动标号
    /// </summary>
    Move_Curmark,

    /// <summary>
    /// 当前运动缓冲数
    /// </summary>
    Vector_Buffered,
    
    /// <summary>
    /// 轴停止原因
    /// </summary>
    Axis_StopReason,
    
    /// <summary>
    /// 当前运动缓冲数
    /// </summary>
    Moves_Buffered,
    
    /// <summary>
    /// 轴地址
    /// </summary>
    Axis_Address,

    /// <summary>
    /// 轴使能
    /// </summary>
    Axis_Enable,
    
    /// <summary>
    /// sp运动速度
    /// </summary>
    Force_Speed,
    
    /// <summary>
    /// sp运动开始速度
    /// </summary>
    Startmove_Speed,
    
    /// <summary>
    /// sp运动结束速度
    /// </summary>
    Endmove_Speed,
    
    /// <summary>
    /// 快减减速度
    /// </summary>
    Fastdec,

    /// <summary>
    /// 叠加轴号
    /// </summary>
    Addax_Axis,

    /// <summary>
    /// 连接轴号
    /// </summary>
    Link_Axis,
    
    /// <summary>
    /// 拐角模式
    /// </summary>
    Corner_Mode,
    
    /// <summary>
    /// 拐角减速开始
    /// </summary>
    Decel_Angle,
    
    /// <summary>
    /// 拐角减速结束
    /// </summary>
    Stop_Angle,
    
    /// <summary>
    /// 限速半径
    /// </summary>
    Full_Sp_Radius,
    
    /// <summary>
    /// 限速值
    /// </summary>
    Splimit_Radius,
    
    /// <summary>
    /// 倒角半径
    /// </summary>
    Zsmooth,
    
    /// <summary>
    /// 当前运动的距离
    /// </summary>
    Vector_Moved,
    
    /// <summary>
    /// 缓冲最终位置
    /// </summary>
    Endmove_Buffer,
    
    /// <summary>
    /// 回零反找延时
    /// </summary>
    Homewait,
    
    /// <summary>
    /// 映射点动输入
    /// </summary>
    Fast_Jog,

    /// <summary>
    /// 映射正向 Jog 输入
    /// </summary>
    Fwd_Jog,
    
    /// <summary>
    /// 反向运动Jog输入
    /// </summary>
    Rev_Jog,
    
    /// <summary>
    /// 映射 Jog 速度
    /// </summary>
    Jogspeed,
    
    /// <summary>
    /// 映射保持输入
    /// </summary>
    Fhold_in,

    /// <summary>
    /// 保持速度
    /// </summary>
    Fhspeed,
    
    /// <summary>
    /// 编码器原始值
    /// </summary>
    Encoder,
    
    /// <summary>
    /// 编码器状态
    /// </summary>
    Encoder_Status,

    /// <summary>
    /// 编码器内部比例
    /// </summary>
    PP_Step,
    
    /// <summary>
    /// 锁存输入映射
    /// </summary>
    Reg_Import,
    
    /// <summary>
    /// 锁存触发
    /// </summary>
    Mark,
    
    /// <summary>
    /// 锁存2触发
    /// </summary>
    MarkB,
    
    /// <summary>
    /// 锁存3触发
    /// </summary>
    MarkC,
    
    /// <summary>
    /// 锁存4触发
    /// </summary>
    MarkD,
    
    /// <summary>
    /// 锁存位置
    /// </summary>
    Reg_Pos,
    
    /// <summary>
    /// 锁存2位置
    /// </summary>
    Reg_PosB,
    
    /// <summary>
    /// 锁存3位置
    /// </summary>
    Reg_PosC,

    /// <summary>
    /// 锁存4位置
    /// </summary>
    Reg_PosD,

    /// <summary>
    /// 映射报警输入
    /// </summary>
    Alm_In,
    
    /// <summary>
    /// 坐标循环模式
    /// </summary>
    Rep_Option,
    
    /// <summary>
    /// 坐标循环位置
    /// </summary>
    Rep_Dist,
    
    /// <summary>
    /// 脉冲模式设置
    /// </summary>
    Invert_Step,
    
    /// <summary>
    /// 脉冲频率限制
    /// </summary>
    Max_Speed,
    
    /// <summary>
    /// 精准输出设置
    /// </summary>
    Axis_Zset,
    
    /// <summary>
    /// 反馈精确度控制
    /// </summary>
    Dac,
    
    /// <summary>
    /// 错误时操作
    /// </summary>
    ErrorMask,
}


/// <summary>
/// 可写入的Basic参数名称
/// </summary>
public enum WriteBaiscParmName
{
    /// <summary>
    /// 轴类型
    /// </summary>
    Atype,
    
    /// <summary>
    /// 脉冲当量
    /// </summary>
    Units,
    
    /// <summary>
    /// 加速度
    /// </summary>
    Accel,
    
    /// <summary>
    /// 减速度
    /// </summary>
    Decel,
    
    /// <summary>
    /// 运行速度
    /// </summary>
    Speed,
    
    /// <summary>
    /// 接近速度
    /// </summary>
    Creep,
    
    /// <summary>
    /// 起始速度
    /// </summary>
    Lspeed,
    
    /// <summary>
    /// 连续插补开关
    /// </summary>
    Merge,
    
    /// <summary>
    /// 加减速曲线时间设置
    /// </summary>
    Sramp,
    
    /// <summary>
    /// 轴指令规划位置
    /// </summary>
    Dpos,
    
    /// <summary>
    /// 编码器反馈位置
    /// </summary>
    Mpos,

    /// <summary>
    /// 正向软限位设置
    /// </summary>
    Fs_Limit,

    /// <summary>
    /// 反向软限位设置
    /// </summary>
    Rs_Limit,
    
    /// <summary>
    /// 映射原点输入
    /// </summary>
    Datum_In,
    
    /// <summary>
    /// 映射正限位输入
    /// </summary>
    Fwd_In,
    
    /// <summary>
    /// 映射反限位输入
    /// </summary>
    Rev_In,

    /// <summary>
    /// 运动标号
    /// </summary>
    Move_Mark,
    
    /// <summary>
    /// 轴地址
    /// </summary>
    Axis_Address,

    /// <summary>
    /// 轴使能
    /// </summary>
    Axis_Enable,
    
    /// <summary>
    /// sp运动速度
    /// </summary>
    Force_Speed,
    
    /// <summary>
    /// sp运动开始速度
    /// </summary>
    Startmove_Speed,
    
    /// <summary>
    /// sp运动结束速度
    /// </summary>
    Endmove_Speed,
    
    /// <summary>
    /// 快减减速度
    /// </summary>
    Fastdec,
    
    /// <summary>
    /// 拐角模式
    /// </summary>
    Corner_Mode,
    
    /// <summary>
    /// 拐角减速开始
    /// </summary>
    Decel_Angle,
    
    /// <summary>
    /// 拐角减速结束
    /// </summary>
    Stop_Angle,
    
    /// <summary>
    /// 限速半径
    /// </summary>
    Full_Sp_Radius,
    
    /// <summary>
    /// 限速值
    /// </summary>
    Splimit_Radius,
    
    /// <summary>
    /// 倒角半径
    /// </summary>
    Zsmooth,
    
    /// <summary>
    /// 当前运动的距离
    /// </summary>
    Vector_Moved,
    
    /// <summary>
    /// 回零反找延时
    /// </summary>
    Homewait,
    
    /// <summary>
    /// 映射点动输入
    /// </summary>
    Fast_Jog,

    /// <summary>
    /// 映射正向 Jog 输入
    /// </summary>
    Fwd_Jog,
    
    /// <summary>
    /// 反向运动Jog输入
    /// </summary>
    Rev_Jog,
    
    /// <summary>
    /// 映射 Jog 速度
    /// </summary>
    Jogspeed,
    
    /// <summary>
    /// 映射保持输入
    /// </summary>
    Fhold_in,

    /// <summary>
    /// 保持速度
    /// </summary>
    Fhspeed,

    /// <summary>
    /// 编码器内部比例
    /// </summary>
    PP_Step,
    
    /// <summary>
    /// 锁存输入映射
    /// </summary>
    Reg_Import,

    /// <summary>
    /// 映射报警输入
    /// </summary>
    Alm_In,
    
    /// <summary>
    /// 坐标循环模式
    /// </summary>
    Rep_Option,
    
    /// <summary>
    /// 坐标循环位置
    /// </summary>
    Rep_Dist,
    
    /// <summary>
    /// 脉冲模式设置
    /// </summary>
    Invert_Step,
    
    /// <summary>
    /// 脉冲频率限制
    /// </summary>
    Max_Speed,
    
    /// <summary>
    /// 精准输出设置
    /// </summary>
    Axis_Zset,
    
    /// <summary>
    /// 反馈精确度控制
    /// </summary>
    Dac,
    
    /// <summary>
    /// 错误时操作
    /// </summary>
    ErrorMask,
}