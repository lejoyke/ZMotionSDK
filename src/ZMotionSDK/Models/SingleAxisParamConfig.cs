namespace ZMotionSDK.Models;

public class StageSpeedParam
{
    /// <summary>
    /// 阶段名称
    /// </summary>
    public string Name { get; set; } = "Stage";

    /// <summary>
    /// 阶段索引
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// 速度
    /// </summary>
    public float Speed { get; set; } = 5;

    /// <summary>
    /// 加速度
    /// </summary>
    public float Acceleration { get; set; } = 5;

    /// <summary>
    /// 减速度
    /// </summary>
    public float Deceleration { get; set; } = 5;

    /// <summary>
    /// 平滑系数
    /// </summary>
    public float SmoothingFactor { get; set; }

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

public class SingleAxisParamConfig
{
    /// <summary>
    /// 轴名称
    /// </summary>
    public string AxisName { get; set; } = "Axis";

    /// <summary>
    /// 轴号
    /// </summary>
    public int AxisNumber { get; set; }

    /// <summary>
    /// 根据索引获取阶段速度参数
    /// </summary>
    public StageSpeedParam this[int index] => StageSpeedParams[index];

    /// <summary>
    /// 根据名称获取阶段速度参数
    /// </summary>
    public StageSpeedParam this[string name] => GetStageSpeedParam(name);

    /// <summary>
    /// 阶段速度参数
    /// </summary>
    public StageSpeedParam[] StageSpeedParams { get; set; }

    /// <summary>
    /// 指令当量
    /// </summary>
    public float Units { get; set; } = 1;

    /// <summary>
    /// 正向软限位位置
    /// </summary>
    public float PositiveLimit { get; set; } = 20000_0000;

    /// <summary>
    /// 负向软限位位置
    /// </summary>
    public float NegativeLimit { get; set; } = -20000_0000;

    /// <summary>
    /// 急停减速度
    /// </summary>
    public float EmergencyDeceleration { get; set; } = 1000000;

    /// <summary>
    /// 爬行速度
    /// </summary>
    public float CreepSpeed { get; set; } = 10;

    /// <summary>
    /// 回零模式
    /// </summary>
    public uint GoHomeMode { get; set; } = 1;

    public SingleAxisParamConfig()
    {
        StageSpeedParams = [
            new() { Name = "Normal" },
            new() {Name = "Jog"},
            new() { Name = "GoHome",Index =2}];
    }

    public AxisParam ToAxisParam()
    {
        return new AxisParam()
        {
            Speed = StageSpeedParams[0].Speed,
            Acceleration = StageSpeedParams[0].Acceleration,
            Deceleration = StageSpeedParams[0].Deceleration,
            SmoothingFactor = StageSpeedParams[0].SmoothingFactor,
            Units = Units,
            NegativeLimit = NegativeLimit,
            PositiveLimit = PositiveLimit,
            EmergencyDeceleration = EmergencyDeceleration,
            CreepSpeed = CreepSpeed,
        };
    }

    public StageSpeedParam GetStageSpeedParam(string name)
    {
        //如果不存在则报错
        var speedParam = StageSpeedParams.FirstOrDefault(x => x.Name == name);
        return speedParam is null ? throw new Exception($"阶段速度参数{name}不存在") : speedParam;
    }

    public StageSpeedParam GetGoHomeSpeedParam()
    {
        return StageSpeedParams.FirstOrDefault(x => x.Name == "GOHome") ?? StageSpeedParams.Last();
    }

    public StageSpeedParam GetJogSpeedParam()
    {
        return StageSpeedParams.FirstOrDefault(x => x.Name == "Jog") ?? throw new Exception("没有Jog阶段速度参数");
    }
}

