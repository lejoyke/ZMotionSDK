using System.Linq;
using ZMotionSDK.Models;

namespace ZMotionSDK.Helper;

/// <summary>
/// Basic参数名称帮助类
/// 提供获取枚举值和对应中文注释的功能
/// </summary>
public static class BaiscParmNameHelper
{
    /// <summary>
    /// 参数信息类
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// 枚举值名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 中文描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 格式化显示文本 (名称 - 描述)
        /// </summary>
        public string DisplayText => $"{Name} - {Description}";

        /// <summary>
        /// 格式化显示文本 (描述 ({名称}))
        /// </summary>
        public string DisplayTextReverse => $"{Description} ({Name})";
    }

    /// <summary>
    /// 获取可读参数的信息列表
    /// </summary>
    /// <returns>参数信息列表</returns>
    public static List<ParameterInfo> GetReadParameterInfoList()
    {
        return GetParameterInfoList<ReadBaiscParmName>();
    }

    /// <summary>
    /// 获取可写参数的信息列表
    /// </summary>
    /// <returns>参数信息列表</returns>
    public static List<ParameterInfo> GetWriteParameterInfoList()
    {
        return GetParameterInfoList<WriteBaiscParmName>();
    }

    /// <summary>
    /// 获取可读参数的字符串列表 (格式: 枚举值 - 中文描述)
    /// </summary>
    /// <returns>字符串列表</returns>
    public static List<string> GetReadParameterStringList()
    {
        return GetReadParameterInfoList().Select(p => p.DisplayText).ToList();
    }

    /// <summary>
    /// 获取可写参数的字符串列表 (格式: 枚举值 - 中文描述)
    /// </summary>
    /// <returns>字符串列表</returns>
    public static List<string> GetWriteParameterStringList()
    {
        return GetWriteParameterInfoList().Select(p => p.DisplayText).ToList();
    }

    /// <summary>
    /// 获取可读参数的字符串列表 (格式: 中文描述 (枚举值))
    /// </summary>
    /// <returns>字符串列表</returns>
    public static List<string> GetReadParameterStringListReverse()
    {
        return GetReadParameterInfoList().Select(p => p.DisplayTextReverse).ToList();
    }

    /// <summary>
    /// 获取可写参数的字符串列表 (格式: 中文描述 (枚举值))
    /// </summary>
    /// <returns>字符串列表</returns>
    public static List<string> GetWriteParameterStringListReverse()
    {
        return GetWriteParameterInfoList().Select(p => p.DisplayTextReverse).ToList();
    }

    /// <summary>
    /// 根据可读参数枚举值获取中文描述（无反射，高性能）
    /// </summary>
    /// <param name="paramName">参数枚举值</param>
    /// <returns>中文描述</returns>
    public static string GetReadParameterDescription(ReadBaiscParmName paramName)
    {
        return GetEnumDescription(paramName);
    }

    /// <summary>
    /// 根据可写参数枚举值获取中文描述（无反射，高性能）
    /// </summary>
    /// <param name="paramName">参数枚举值</param>
    /// <returns>中文描述</returns>
    public static string GetWriteParameterDescription(WriteBaiscParmName paramName)
    {
        return GetEnumDescription(paramName);
    }

    /// <summary>
    /// 根据参数名称获取可读参数的描述
    /// </summary>
    /// <param name="paramName">参数名称</param>
    /// <returns>中文描述，如果未找到返回参数名称本身</returns>
    public static string GetReadParameterDescriptionByName(string paramName)
    {
        if (Enum.TryParse<ReadBaiscParmName>(paramName, out var enumValue))
        {
            return GetReadParameterDescription(enumValue);
        }
        return paramName;
    }

    /// <summary>
    /// 根据参数名称获取可写参数的描述
    /// </summary>
    /// <param name="paramName">参数名称</param>
    /// <returns>中文描述，如果未找到返回参数名称本身</returns>
    public static string GetWriteParameterDescriptionByName(string paramName)
    {
        if (Enum.TryParse<WriteBaiscParmName>(paramName, out var enumValue))
        {
            return GetWriteParameterDescription(enumValue);
        }
        return paramName;
    }

    /// <summary>
    /// 获取常用参数的信息列表（用于快速选择）
    /// </summary>
    /// <returns>常用参数信息列表</returns>
    public static List<ParameterInfo> GetCommonParameterInfoList()
    {
        var commonParams = new[]
        {
            ReadBaiscParmName.Dpos,
            ReadBaiscParmName.Mpos,
            ReadBaiscParmName.Speed,
            ReadBaiscParmName.Accel,
            ReadBaiscParmName.Decel,
            ReadBaiscParmName.Units,
            ReadBaiscParmName.Atype,
            ReadBaiscParmName.Idle,
            ReadBaiscParmName.Axisstatus,
            ReadBaiscParmName.Vp_Speed,
            ReadBaiscParmName.Mspeed,
            ReadBaiscParmName.Lspeed,
            ReadBaiscParmName.Creep,
            ReadBaiscParmName.Sramp,
            ReadBaiscParmName.Fastdec,
            ReadBaiscParmName.Fs_Limit,
            ReadBaiscParmName.Rs_Limit,
            ReadBaiscParmName.Endmove,
            ReadBaiscParmName.Encoder,
            ReadBaiscParmName.Axis_StopReason
        };

        return commonParams.Select(param => new ParameterInfo
        {
            Name = param.ToString(),
            Description = GetReadParameterDescription(param),
            Value = (int)param
        }).ToList();
    }

    /// <summary>
    /// 获取指定枚举类型的参数信息列表
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>参数信息列表</returns>
    private static List<ParameterInfo> GetParameterInfoList<T>() where T : Enum
    {
        var parameterInfoList = new List<ParameterInfo>();
        var enumValues = Enum.GetValues(typeof(T)).Cast<T>();

        foreach (var enumValue in enumValues)
        {
            parameterInfoList.Add(new ParameterInfo
            {
                Name = enumValue.ToString(),
                Description = GetEnumDescription(enumValue),
                Value = Convert.ToInt32(enumValue)
            });
        }

        return parameterInfoList.OrderBy(p => p.Name).ToList();
    }

    /// <summary>
    /// 获取可读参数枚举值的描述信息
    /// </summary>
    /// <param name="enumValue">枚举值</param>
    /// <returns>描述信息</returns>
    private static string GetEnumDescription(ReadBaiscParmName enumValue)
    {
        return enumValue switch
        {
            ReadBaiscParmName.Atype => "轴类型",
            ReadBaiscParmName.Units => "脉冲当量",
            ReadBaiscParmName.Accel => "加速度",
            ReadBaiscParmName.Decel => "减速度",
            ReadBaiscParmName.Speed => "运行速度",
            ReadBaiscParmName.Creep => "接近速度",
            ReadBaiscParmName.Lspeed => "起始速度",
            ReadBaiscParmName.Merge => "连续插补开关",
            ReadBaiscParmName.Sramp => "加减速曲线时间设置",
            ReadBaiscParmName.Dpos => "轴指令规划位置",
            ReadBaiscParmName.Mpos => "编码器反馈位置",
            ReadBaiscParmName.Endmove => "当前运动目标位置",
            ReadBaiscParmName.Fs_Limit => "正向软限位设置",
            ReadBaiscParmName.Rs_Limit => "反向软限位设置",
            ReadBaiscParmName.Datum_In => "映射原点输入",
            ReadBaiscParmName.Fwd_In => "映射正限位输入",
            ReadBaiscParmName.Rev_In => "映射反限位输入",
            ReadBaiscParmName.Idle => "运动状态",
            ReadBaiscParmName.Loaded => "缓冲是否为空状态位",
            ReadBaiscParmName.Mspeed => "实际编码反馈速度",
            ReadBaiscParmName.Mtype => "当前运动类型",
            ReadBaiscParmName.Ntype => "当前运动类型",
            ReadBaiscParmName.Remain => "当前运动剩余距离",
            ReadBaiscParmName.Vp_Speed => "当前运行速度",
            ReadBaiscParmName.Axisstatus => "轴状态",
            ReadBaiscParmName.Move_Mark => "运动标号",
            ReadBaiscParmName.Move_Curmark => "当前运动标号",
            ReadBaiscParmName.Vector_Buffered => "当前运动缓冲数",
            ReadBaiscParmName.Axis_StopReason => "轴停止原因",
            ReadBaiscParmName.Moves_Buffered => "当前运动缓冲数",
            ReadBaiscParmName.Axis_Address => "轴地址",
            ReadBaiscParmName.Axis_Enable => "轴使能",
            ReadBaiscParmName.Force_Speed => "sp运动速度",
            ReadBaiscParmName.Startmove_Speed => "sp运动开始速度",
            ReadBaiscParmName.Endmove_Speed => "sp运动结束速度",
            ReadBaiscParmName.Fastdec => "快减减速度",
            ReadBaiscParmName.Addax_Axis => "叠加轴号",
            ReadBaiscParmName.Link_Axis => "连接轴号",
            ReadBaiscParmName.Corner_Mode => "拐角模式",
            ReadBaiscParmName.Decel_Angle => "拐角减速开始",
            ReadBaiscParmName.Stop_Angle => "拐角减速结束",
            ReadBaiscParmName.Full_Sp_Radius => "限速半径",
            ReadBaiscParmName.Splimit_Radius => "限速值",
            ReadBaiscParmName.Zsmooth => "倒角半径",
            ReadBaiscParmName.Vector_Moved => "当前运动的距离",
            ReadBaiscParmName.Endmove_Buffer => "缓冲最终位置",
            ReadBaiscParmName.Homewait => "回零反找延时",
            ReadBaiscParmName.Fast_Jog => "映射点动输入",
            ReadBaiscParmName.Fwd_Jog => "映射正向 Jog 输入",
            ReadBaiscParmName.Rev_Jog => "反向运动Jog输入",
            ReadBaiscParmName.Jogspeed => "映射 Jog 速度",
            ReadBaiscParmName.Fhold_in => "映射保持输入",
            ReadBaiscParmName.Fhspeed => "保持速度",
            ReadBaiscParmName.Encoder => "编码器原始值",
            ReadBaiscParmName.Encoder_Status => "编码器状态",
            ReadBaiscParmName.PP_Step => "编码器内部比例",
            ReadBaiscParmName.Reg_Import => "锁存输入映射",
            ReadBaiscParmName.Mark => "锁存触发",
            ReadBaiscParmName.MarkB => "锁存2触发",
            ReadBaiscParmName.MarkC => "锁存3触发",
            ReadBaiscParmName.MarkD => "锁存4触发",
            ReadBaiscParmName.Reg_Pos => "锁存位置",
            ReadBaiscParmName.Reg_PosB => "锁存2位置",
            ReadBaiscParmName.Reg_PosC => "锁存3位置",
            ReadBaiscParmName.Reg_PosD => "锁存4位置",
            ReadBaiscParmName.Alm_In => "映射报警输入",
            ReadBaiscParmName.Rep_Option => "坐标循环模式",
            ReadBaiscParmName.Rep_Dist => "坐标循环位置",
            ReadBaiscParmName.Invert_Step => "脉冲模式设置",
            ReadBaiscParmName.Max_Speed => "脉冲频率限制",
            ReadBaiscParmName.Axis_Zset => "精准输出设置",
            ReadBaiscParmName.Dac => "反馈精确度控制",
            ReadBaiscParmName.ErrorMask => "错误时操作",
            _ => enumValue.ToString()
        };
    }

    /// <summary>
    /// 获取可写参数枚举值的描述信息
    /// </summary>
    /// <param name="enumValue">枚举值</param>
    /// <returns>描述信息</returns>
    private static string GetEnumDescription(WriteBaiscParmName enumValue)
    {
        return enumValue switch
        {
            WriteBaiscParmName.Atype => "轴类型",
            WriteBaiscParmName.Units => "脉冲当量",
            WriteBaiscParmName.Accel => "加速度",
            WriteBaiscParmName.Decel => "减速度",
            WriteBaiscParmName.Speed => "运行速度",
            WriteBaiscParmName.Creep => "接近速度",
            WriteBaiscParmName.Lspeed => "起始速度",
            WriteBaiscParmName.Merge => "连续插补开关",
            WriteBaiscParmName.Sramp => "加减速曲线时间设置",
            WriteBaiscParmName.Dpos => "轴指令规划位置",
            WriteBaiscParmName.Mpos => "编码器反馈位置",
            WriteBaiscParmName.Fs_Limit => "正向软限位设置",
            WriteBaiscParmName.Rs_Limit => "反向软限位设置",
            WriteBaiscParmName.Datum_In => "映射原点输入",
            WriteBaiscParmName.Fwd_In => "映射正限位输入",
            WriteBaiscParmName.Rev_In => "映射反限位输入",
            WriteBaiscParmName.Move_Mark => "运动标号",
            WriteBaiscParmName.Axis_Address => "轴地址",
            WriteBaiscParmName.Axis_Enable => "轴使能",
            WriteBaiscParmName.Force_Speed => "sp运动速度",
            WriteBaiscParmName.Startmove_Speed => "sp运动开始速度",
            WriteBaiscParmName.Endmove_Speed => "sp运动结束速度",
            WriteBaiscParmName.Fastdec => "快减减速度",
            WriteBaiscParmName.Corner_Mode => "拐角模式",
            WriteBaiscParmName.Decel_Angle => "拐角减速开始",
            WriteBaiscParmName.Stop_Angle => "拐角减速结束",
            WriteBaiscParmName.Full_Sp_Radius => "限速半径",
            WriteBaiscParmName.Splimit_Radius => "限速值",
            WriteBaiscParmName.Zsmooth => "倒角半径",
            WriteBaiscParmName.Vector_Moved => "当前运动的距离",
            WriteBaiscParmName.Homewait => "回零反找延时",
            WriteBaiscParmName.Fast_Jog => "映射点动输入",
            WriteBaiscParmName.Fwd_Jog => "映射正向 Jog 输入",
            WriteBaiscParmName.Rev_Jog => "反向运动Jog输入",
            WriteBaiscParmName.Jogspeed => "映射 Jog 速度",
            WriteBaiscParmName.Fhold_in => "映射保持输入",
            WriteBaiscParmName.Fhspeed => "保持速度",
            WriteBaiscParmName.PP_Step => "编码器内部比例",
            WriteBaiscParmName.Reg_Import => "锁存输入映射",
            WriteBaiscParmName.Alm_In => "映射报警输入",
            WriteBaiscParmName.Rep_Option => "坐标循环模式",
            WriteBaiscParmName.Rep_Dist => "坐标循环位置",
            WriteBaiscParmName.Invert_Step => "脉冲模式设置",
            WriteBaiscParmName.Max_Speed => "脉冲频率限制",
            WriteBaiscParmName.Axis_Zset => "精准输出设置",
            WriteBaiscParmName.Dac => "反馈精确度控制",
            WriteBaiscParmName.ErrorMask => "错误时操作",
            _ => enumValue.ToString()
        };
    }

    /// <summary>
    /// 通用枚举描述获取方法（保持向后兼容）
    /// </summary>
    /// <param name="enumValue">枚举值</param>
    /// <returns>描述信息</returns>
    private static string GetEnumDescription(Enum enumValue)
    {
        return enumValue switch
        {
            ReadBaiscParmName readParam => GetEnumDescription(readParam),
            WriteBaiscParmName writeParam => GetEnumDescription(writeParam),
            _ => enumValue.ToString()
        };
    }
} 