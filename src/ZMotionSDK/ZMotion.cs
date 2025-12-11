using ZMotionSDK.Models;
using ZMotionSDK.Helper;

namespace ZMotionSDK;

public partial class ZMotion
{
    private IntPtr _handle;

    #region DIDOHelper
    private readonly Dictionary<int, int> _signalIndex_Homes = [];
    private readonly Dictionary<int, int> _signalIndex_ForwardLimits = [];
    private readonly Dictionary<int, int> _signalIndex_BackwardLimits = [];

    private DIDOInverterConfig _didoInverterConfig = new();
    #endregion

    public IntPtr Handle => _handle;

    public EventHandler<string>? OnExecuteError { get; set; }

    /// <summary>
    /// DIDO反转配置
    /// </summary>
    public DIDOInverterConfig DIDOInverterConfig => _didoInverterConfig.Clone();

    /// <summary>
    /// 应用DIDO反转配置,类库内部反转
    /// </summary>
    /// <param name="config">DIDO反转配置</param>
    public void ApplyDIDOInverter(DIDOInverterConfig config)
    {
        // DI直接通过内部反转,DO通过配置反转
        _didoInverterConfig = config;
    }


    /// <summary>
    /// 写入DIDO反转配置,通过写入控制器DI反转
    /// </summary>
    /// <param name="config"></param>
    public void WriteDIDOInverterConfig(DIDOInverterConfig config)
    {
        var diInvert = config.GetDIInvertArray();
        for (int i = 0; i < diInvert.Length; i++)
        {
            SetDIInvert(i, diInvert[i]);
        }

        var homeSignalInvert = config.GetInvertArray(SignalType.HomeSignal);
        for (int i = 0; i < homeSignalInvert.Length; i++)
        {
            var index = GetDIIndex_Home(i);
            if (index >= 0)
            {
                SetDIInvert(index, homeSignalInvert[i]);
            }
        }

        var positiveLimitInvert = config.GetInvertArray(SignalType.PositiveLimit);
        for (int i = 0; i < positiveLimitInvert.Length; i++)
        {
            var index = GetDIIndex_ForwardLimit(i);
            if (index >= 0)
            {
                SetDIInvert(index, positiveLimitInvert[i]);
            }
        }

        var negativeLimitInvert = config.GetInvertArray(SignalType.NegativeLimit);
        for (int i = 0; i < negativeLimitInvert.Length; i++)
        {
            var index = GetDIIndex_BackwardLimit(i);
            if (index >= 0)
            {
                SetDIInvert(index, negativeLimitInvert[i]);
            }
        }
    }

    /// <summary>
    /// 打开运动控制器
    /// </summary>
    /// <param name="ip">运动控制器IP地址</param>
    /// <param name="timeout">超时时间</param>
    /// <exception cref="ZMotionException"></exception>
    public void Open(string ip, uint timeout)
    {
        var result = zmcaux.ZAux_FastOpen(2, ip, timeout, out IntPtr handle);
        CheckResult(result);

        this._handle = handle;
    }

    /// <summary>
    /// 打开运动控制器
    /// </summary>
    /// <param name="ip">运动控制器IP地址</param>
    /// <exception cref="ZMotionException"></exception>
    public void Open_Eth(string ip)
    {
        var result = zmcaux.ZAux_OpenEth(ip, out IntPtr handle);
        CheckResult(result);

        _handle = handle;
    }

    /// <summary>
    /// 关闭运动控制器
    /// </summary>
    /// <exception cref="ZMotionException"></exception>
    public void Close()
    {
        var result = zmcaux.ZAux_Close(_handle);
        CheckResult(result);
    }

    /// <summary>
    /// 下载程序
    /// </summary>
    /// <param name="filePath">程序文件路径</param>
    /// <param name="mode">RAM-ROM</param>
    /// <exception cref="ZMotionException"></exception>
    public void BasDown(string filePath, BasDownMode mode = BasDownMode.ROM)
    {
        var result = zmcaux.ZAux_BasDown(_handle, filePath, (uint)mode);
        CheckResult(result);
    }

    #region 硬件接口

    /// <summary>
    /// 获取数字输入
    /// </summary>
    /// <param name="index">输入索引</param>
    /// <returns>输入状态</returns>
    public bool GetDI(int index)
    {
        uint value = 0;
        var result = zmcaux.ZAux_Direct_GetIn(_handle, index, ref value);
        CheckResult(result);

        // 根据反转配置处理DI值
        return _didoInverterConfig.ProcessDIValue(index, value);
    }

    /// <summary>
    /// 获取数字输出
    /// </summary>
    /// <param name="index">输出索引</param>
    /// <returns>输出状态</returns>
    public bool GetDO(int index)
    {
        uint value = 0;
        var result = zmcaux.ZAux_Direct_GetOp(_handle, index, ref value);
        CheckResult(result);

        // 根据反转配置处理DO值
        return _didoInverterConfig.ProcessDOValue(index, value);
    }

    /// <summary>
    /// 设置数字输出
    /// </summary>
    /// <param name="index">输出索引</param>
    /// <param name="value">输出状态</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDO(int index, bool value)
    {
        // 根据反转配置处理DO值
        uint outputValue = value ? 1u : 0u;
        outputValue = _didoInverterConfig.ProcessDOValue(index, outputValue) ? 1u : 0u;

        var result = zmcaux.ZAux_Direct_SetOp(_handle, index, outputValue);
        CheckResult(result);
    }

    /// <summary>
    /// 获取数字输入
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输入状态,按位存储</returns>
    public bool[] GetDI_Multi_Modbus(int startIndex, int endIndex)
    {
        byte[] value = new byte[(endIndex - startIndex) / 8 + 1];
        var result = zmcaux.ZAux_GetModbusIn(_handle, startIndex, endIndex, value);

        CheckResult(result);

        bool[] boolResult = new bool[endIndex - startIndex + 1];
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = (value[i / 8] & (1 << (i % 8))) != 0;
        }

        // 根据反转配置处理DI值
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = _didoInverterConfig.ProcessDIValue(startIndex + i, boolResult[i]);
        }

        return boolResult;
    }

    /// <summary>
    /// 获取数字输出
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输出状态,按位存储</returns>
    public bool[] GetDO_Multi_Modbus(int startIndex, int endIndex)
    {
        byte[] value = new byte[(endIndex - startIndex) / 8 + 1];
        var result = zmcaux.ZAux_GetModbusOut(_handle, startIndex, endIndex, value);
        CheckResult(result);

        bool[] boolResult = new bool[endIndex - startIndex + 1];
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = (value[i / 8] & (1 << (i % 8))) != 0;
        }

        // 根据反转配置处理DO值
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = _didoInverterConfig.ProcessDOValue(startIndex + i, boolResult[i]);
        }
        return boolResult;
    }

    /// <summary>
    /// 获取数字输出
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输出状态,按位存储</returns>
    public bool[] GetDO_Multi(ushort startIndex, ushort endIndex)
    {
        uint[] value = new uint[(endIndex - startIndex) / 32 + 1];
        var result = zmcaux.ZAux_Direct_GetOutMulti(_handle, startIndex, endIndex, value);
        CheckResult(result);

        bool[] boolResult = new bool[endIndex - startIndex + 1];
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = (value[i / 32] & (1 << (i % 32))) != 0;
        }

        // 根据反转配置处理DO值
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = _didoInverterConfig.ProcessDOValue(startIndex + i, boolResult[i]);
        }

        return boolResult;
    }

    /// <summary>
    /// 设置数字输出
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="value">输出状态</param>
    public void SetDO_Multi(ushort startIndex, bool[] value)
    {
        bool[] invertValue = new bool[value.Length];
        for (int i = 0; i < invertValue.Length; i++)
        {
            invertValue[i] = _didoInverterConfig.ProcessDOValue(startIndex + i, value[i]);
        }

        uint[] uintValue = new uint[(value.Length - 1) / 32 + 1];
        for (int i = 0; i < uintValue.Length; i++)
        {
            uintValue[i] = 0;
            for (int j = 0; j < 32; j++)
            {
                if (i * 32 + j >= value.Length)
                    break;

                // 如果对应状态为False,则不需要设置
                if (invertValue[i * 32 + j])
                {
                    uintValue[i] = uintValue[i].SetBit(j, true);
                }
            }
        }

        var result = zmcaux.ZAux_Direct_SetOutMulti(_handle, startIndex, (ushort)(startIndex + value.Length - 1), uintValue);
        CheckResult(result);
    }

    /// <summary>
    /// 获取数字输入
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输入状态,按位存储</returns>
    public bool[] GetDI_Multi(int startIndex, int endIndex)
    {
        int[] value = new int[(endIndex - startIndex) / 32 + 1];
        var result = zmcaux.ZAux_Direct_GetInMulti(_handle, startIndex, endIndex, value);
        CheckResult(result);

        bool[] boolResult = new bool[endIndex - startIndex + 1];
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = (value[i / 32] & (1 << (i % 32))) != 0;
        }

        // 根据反转配置处理DI值
        for (int i = 0; i < boolResult.Length; i++)
        {
            boolResult[i] = _didoInverterConfig.ProcessDIValue(startIndex + i, boolResult[i]);
        }

        return boolResult;
    }

    /// <summary>
    /// 获取轴正限位输入索引
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>正限位输入索引</returns>
    public int GetDIIndex_ForwardLimit(int axis)
    {
        int index = 0;
        var result = zmcaux.ZAux_Direct_GetFwdIn(_handle, axis, ref index);
        CheckResult(result);
        if (!_signalIndex_ForwardLimits.TryAdd(axis, index))
            _signalIndex_ForwardLimits[axis] = index;
        return index;
    }

    /// <summary>
    /// 获取轴负限位输入索引
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>负限位输入索引</returns>
    public int GetDIIndex_BackwardLimit(int axis)
    {
        int index = 0;
        var result = zmcaux.ZAux_Direct_GetRevIn(_handle, axis, ref index);
        CheckResult(result);
        if (!_signalIndex_BackwardLimits.TryAdd(axis, index))
            _signalIndex_BackwardLimits[axis] = index;
        return index;
    }

    /// <summary>
    /// 获取回零信号DI索引
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <exception cref="ZMotionException"></exception>
    public int GetDIIndex_Home(int axis)
    {
        int signal = 0;
        var result = zmcaux.ZAux_Direct_GetDatumIn(_handle, axis, ref signal);
        CheckResult(result);

        if (!_signalIndex_Homes.TryAdd(axis, signal))
            _signalIndex_Homes[axis] = signal;
        return signal;
    }

    /// <summary>
    /// 获取回零信号反向
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <exception cref="ZMotionException"></exception>
    public bool GetDIInvert(int axis)
    {
        int isInvert = 0;
        var result = zmcaux.ZAux_Direct_GetInvertIn(_handle, axis, ref isInvert);
        CheckResult(result);
        return isInvert == 1;
    }

    /// <summary>
    /// 设置回零信号反向
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="isInvert">是否反向</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDIInvert(int axis, bool isInvert)
    {
        var result = zmcaux.ZAux_Direct_SetInvertIn(_handle, axis, isInvert ? 1 : 0);
        CheckResult(result);
    }

    /// <summary>
    /// 设置回零信号DI
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="signalIndex">信号索引</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDIIndex_Home(int axis, int signalIndex)
    {
        var result = zmcaux.ZAux_Direct_SetDatumIn(_handle, axis, signalIndex);
        CheckResult(result);
        if (!_signalIndex_Homes.TryAdd(axis, signalIndex))
            _signalIndex_Homes[axis] = signalIndex;
    }

    /// <summary>
    /// 设置轴正限位信号DI
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="signalIndex">信号索引</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDIIndex_ForwardLimit(int axis, int signalIndex)
    {
        var result = zmcaux.ZAux_Direct_SetFwdIn(_handle, axis, signalIndex);
        CheckResult(result);
        if (!_signalIndex_ForwardLimits.TryAdd(axis, signalIndex))
            _signalIndex_ForwardLimits[axis] = signalIndex;
    }

    /// <summary>
    /// 设置轴负限位信号DI
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="signalIndex">信号索引</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDIIndex_BackwardLimit(int axis, int signalIndex)
    {
        var result = zmcaux.ZAux_Direct_SetRevIn(_handle, axis, signalIndex);
        CheckResult(result);
        if (!_signalIndex_BackwardLimits.TryAdd(axis, signalIndex))
            _signalIndex_BackwardLimits[axis] = signalIndex;
    }

    /// <summary>
    /// 获取零点信号DI
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>回零信号DI</returns>
    public bool GetDI_Home(int axis)
    {
        if (!_signalIndex_Homes.ContainsKey(axis))
        {
            GetDIIndex_Home(axis);
        }
        return _didoInverterConfig.ProcessHomeSignalValue(axis, GetDI(_signalIndex_Homes[axis]));
    }

    /// <summary>
    /// 获取轴正限位信号DI
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>正限位信号DI</returns>
    public bool GetDI_ForwardLimit(int axis)
    {
        if (!_signalIndex_ForwardLimits.ContainsKey(axis))
        {
            GetDIIndex_ForwardLimit(axis);
        }
        return _didoInverterConfig.ProcessPositiveLimitValue(axis, GetDI(_signalIndex_ForwardLimits[axis]));
    }

    /// <summary>
    /// 获取轴负限位信号DI
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>负限位信号DI</returns>
    public bool GetDI_BackwardLimit(int axis)
    {
        if (!_signalIndex_BackwardLimits.ContainsKey(axis))
        {
            GetDIIndex_BackwardLimit(axis);
        }
        return _didoInverterConfig.ProcessNegativeLimitValue(axis, GetDI(_signalIndex_BackwardLimits[axis]));
    }
    #endregion

    #region 运动控制参数设置

    /// <summary>
    /// 设置轴类型
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="type">轴类型</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetAxisType(int axis, AxisType type)
    {
        var result = zmcaux.ZAux_Direct_SetAtype(_handle, axis, (int)type);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴类型
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>轴类型</returns>
    public AxisType GetAxisType(int axis)
    {
        int type = 0;
        var result = zmcaux.ZAux_Direct_GetAtype(_handle, axis, ref type);
        CheckResult(result);
        return (AxisType)type;
    }

    /// <summary>
    /// 设置轴脉冲当量
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="unit">脉冲当量</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetUnits(int axis, float unit)
    {
        var result = zmcaux.ZAux_Direct_SetUnits(_handle, axis, unit);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴脉冲当量
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>脉冲当量</returns>
    public float GetUnits(int axis)
    {
        float unit = 0;
        var result = zmcaux.ZAux_Direct_GetUnits(_handle, axis, ref unit);
        CheckResult(result);
        return unit;
    }

    /// <summary>
    /// 设置轴加速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="accel">加速度(units/s^2)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetAccel(int axis, float accel)
    {
        var result = zmcaux.ZAux_Direct_SetAccel(_handle, axis, accel);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴加速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>加速度(units/s^2)</returns>
    public float GetAccel(int axis)
    {
        float accel = 0;
        var result = zmcaux.ZAux_Direct_GetAccel(_handle, axis, ref accel);
        CheckResult(result);
        return accel;
    }

    /// <summary>
    /// 设置轴减速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="decel">减速度(units/s^2)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDecel(int axis, float decel)
    {
        var result = zmcaux.ZAux_Direct_SetDecel(_handle, axis, decel);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴减速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>减速度(units/s^2)</returns>
    public float GetDecel(int axis)
    {
        float decel = 0;
        var result = zmcaux.ZAux_Direct_GetDecel(_handle, axis, ref decel);
        CheckResult(result);
        return decel;
    }

    /// <summary>
    /// 设置轴速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="speed">速度(units/s)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetSpeed(int axis, float speed)
    {
        var result = zmcaux.ZAux_Direct_SetSpeed(_handle, axis, speed);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>速度(units/s)</returns>
    public float GetSpeed(int axis)
    {
        float speed = 0;
        var result = zmcaux.ZAux_Direct_GetSpeed(_handle, axis, ref speed);
        CheckResult(result);
        return speed;
    }

    /// <summary>
    /// 快速读取多个轴当前的速度。Modbus 寄存器方式,从 0 开始
    /// </summary>
    /// <param name="count">数量</param>
    /// <returns>速度</returns>
    public float[] GetSpeed_Multi_Modbus(int count)
    {
        float[] speed = new float[count];
        var result = zmcaux.ZAux_GetModbusCurSpeed(_handle, count, speed);
        CheckResult(result);
        return speed;
    }

    /// <summary>
    /// 设置轴的规划位置
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="position">规划位置(units)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetPosition_D(int axis, float position)
    {
        var result = zmcaux.ZAux_Direct_SetDpos(_handle, axis, position);
        CheckResult(result);
    }

    /// <summary>
    /// 读取轴的规划位置
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>规划位置(units)</returns>
    public float GetPosition_D(int axis)
    {
        float position = 0;
        var result = zmcaux.ZAux_Direct_GetDpos(_handle, axis, ref position);
        CheckResult(result);
        return position;
    }

    /// <summary>
    /// 快速读取多个轴当前的 DPOS。Modbus 寄存器方式,从 0 开始
    /// </summary>
    /// <param name="count">数量</param>
    /// <returns>规划位置</returns>
    public float[] GetPosition_D_Multi_Modbus(int count)
    {
        float[] position = new float[count];
        var result = zmcaux.ZAux_GetModbusDpos(_handle, count, position);
        CheckResult(result);
        return position;
    }

    /// <summary>
    /// 快速读取多个轴当前的 MPOS。Modbus 寄存器方式,从 0 开始
    /// </summary>
    /// <param name="count">数量</param>
    /// <returns>反馈位置</returns>
    public float[] GetPosition_M_Multi_Modbus(int count)
    {
        float[] position = new float[count];
        var result = zmcaux.ZAux_GetModbusMpos(_handle, count, position);
        CheckResult(result);
        return position;
    }

    /// <summary>
    /// 设置反馈位置
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="position">反馈位置(units)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetPosition_M(int axis, float position)
    {
        var result = zmcaux.ZAux_Direct_SetMpos(_handle, axis, position);
        CheckResult(result);
    }

    /// <summary>
    /// 读取反馈位置
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>反馈位置(units)</returns>
    public float GetPosition_M(int axis)
    {
        float position = 0;
        var result = zmcaux.ZAux_Direct_GetMpos(_handle, axis, ref position);
        CheckResult(result);
        return position;
    }

    /// <summary>
    /// 获取控制器接收的脉冲数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>脉冲数</returns>
    public float GetPosition_Encoder(int axis)
    {
        float encoder = 0;
        var result = zmcaux.ZAux_Direct_GetEncoder(_handle, axis, ref encoder);
        CheckResult(result);
        return encoder;
    }

    /// <summary>
    /// 设置快速减速度(急停减速度)
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="fastDec">快速减速度(units/s²)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetDecel_Fast(int axis, float fastDec)
    {
        var result = zmcaux.ZAux_Direct_SetFastDec(_handle, axis, fastDec);
        CheckResult(result);
    }

    /// <summary>
    /// 读取快速减速度(急停减速度)
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>快速减速度(units/s²)</returns>
    public float GetDecel_Fast(int axis)
    {
        float fastDec = 0;
        var result = zmcaux.ZAux_Direct_GetFastDec(_handle, axis, ref fastDec);
        CheckResult(result);
        return fastDec;
    }

    /// <summary>
    /// 设置轴起始速度(当多轴运动时,设置主轴作为插补运动的起始速度)
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="lspeed">起始速度(units/s)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetSpeed_L(int axis, float lspeed)
    {
        var result = zmcaux.ZAux_Direct_SetLspeed(_handle, axis, lspeed);
        CheckResult(result);
    }

    /// <summary>
    /// 读取轴起始速度(当多轴运动时,设置主轴作为插补运动的起始速度)
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>起始速度(units/s)</returns>
    public float GetSpeed_L(int axis)
    {
        float lspeed = 0;
        var result = zmcaux.ZAux_Direct_GetLspeed(_handle, axis, ref lspeed);
        CheckResult(result);
        return lspeed;
    }

    /// <summary>
    /// 设置S曲线设置
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="sramp">S曲线设置，0-梯形加减速</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetSramp(int axis, float sramp)
    {
        var result = zmcaux.ZAux_Direct_SetSramp(_handle, axis, sramp);
        CheckResult(result);
    }

    /// <summary>
    /// 读取S曲线设置
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>S曲线设置，0-梯形加减速</returns>
    public float GetSramp(int axis)
    {
        float sramp = 0;
        var result = zmcaux.ZAux_Direct_GetSramp(_handle, axis, ref sramp);
        CheckResult(result);
        return sramp;
    }

    /// <summary>
    /// 读取反馈速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>反馈速度(units/s)</returns>
    public float GetSpeed_M(int axis)
    {
        float mspeed = 0;
        var result = zmcaux.ZAux_Direct_GetMspeed(_handle, axis, ref mspeed);
        CheckResult(result);
        return mspeed;
    }

    /// <summary>
    /// 读取当前轴运行的规划速度
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>命令速度(units/s)</returns>
    public float GetSpeed_Vp(int axis)
    {
        float vpSpeed = 0;
        var result = zmcaux.ZAux_Direct_GetVpSpeed(_handle, axis, ref vpSpeed);
        CheckResult(result);
        return vpSpeed;
    }

    /// <summary>
    /// 读取轴是否运动结束
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>true-正在运动，false-运动结束</returns>
    public bool GetIsRunning(int axis)
    {
        int idle = 0;
        var result = zmcaux.ZAux_Direct_GetIfIdle(_handle, axis, ref idle);
        CheckResult(result);
        return idle == 0;
    }

    /// <summary>
    /// 读取轴的状态
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>轴状态</returns>
    public AxisStatus GetAxisStatus(int axis)
    {
        int status = 0;
        var result = zmcaux.ZAux_Direct_GetAxisStatus(_handle, axis, ref status);
        CheckResult(result);
        return AxisStatus.Parse(status);
    }

    /// <summary>
    /// 读取轴历史异常停止原因
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <returns>停止原因</returns>
    public AxisStatus GetAxisStopReason(int axis)
    {
        int reason = 0;
        var result = zmcaux.ZAux_Direct_GetAxisStopReason(_handle, axis, ref reason);
        CheckResult(result);
        return AxisStatus.Parse(reason);
    }

    /// <summary>
    /// 读取轴参数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="paramName">Baisc 语法参数名称</param>
    /// <returns>参数值</returns>
    public float GetParam(int axis, ReadBaiscParmName paramName)
    {
        float value = 0;
        var result = zmcaux.ZAux_Direct_GetParam(_handle, paramName.ToString(), axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取轴参数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="paramName">参数名称</param>
    /// <returns>参数值</returns>
    public float GetParam(int axis, string paramName)
    {
        float value = 0;
        var result = zmcaux.ZAux_Direct_GetParam(_handle, paramName, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 设置轴参数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="paramName">Baisc 语法参数名称</param>
    /// <param name="value">参数值</param>
    public void SetParam(int axis, WriteBaiscParmName paramName, float value)
    {
        var result = zmcaux.ZAux_Direct_SetParam(_handle, paramName.ToString(), axis, value);
        CheckResult(result);
    }

    /// <summary>
    /// 设置轴参数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="paramName">参数名称</param>
    /// <param name="value">参数值</param>
    public void SetParam(int axis, string paramName, float value)
    {
        var result = zmcaux.ZAux_Direct_SetParam(_handle, paramName, axis, value);
        CheckResult(result);
    }

    /// <summary>
    /// 读取多个轴的参数
    /// </summary>
    /// <param name="number">轴数量</param>
    /// <param name="paramName">Baisc 语法参数名称</param>
    /// <returns>参数值</returns>
    public float[] GetParam_Multi(int number, ReadBaiscParmName paramName)
    {
        float[] value = new float[number];
        var result = zmcaux.ZAux_Direct_GetAllAxisPara(_handle, paramName.ToString(), number, value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取多个轴的参数
    /// </summary>
    /// <param name="number">轴数量</param>
    /// <param name="paramName">参数名称</param>
    /// <returns>参数值</returns>
    public float[] GetParam_Multi(int number, string paramName)
    {
        float[] value = new float[number];
        var result = zmcaux.ZAux_Direct_GetAllAxisPara(_handle, paramName, number, value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取多个轴的运行状态
    /// </summary>
    /// <param name="number">轴数量</param>
    /// <returns>轴运行信息</returns>
    public AxisInfo[] GetAxisInfo(int number)
    {
        int[] isRunning = new int[number];
        float[] dPosition = new float[number];
        float[] mPosition = new float[number];
        int[] axisStatus = new int[number];
        var result = zmcaux.ZAux_Direct_GetAllAxisInfo(_handle, number, isRunning, dPosition, mPosition, axisStatus);
        CheckResult(result);

        AxisInfo[] info = new AxisInfo[number];
        for (int i = 0; i < number; i++)
        {
            info[i].IsRunning = isRunning[i] != -1;
            info[i].DPosition = dPosition[i];
            info[i].MPosition = mPosition[i];
            info[i].AxisStatus = AxisStatus.Parse(axisStatus[i]);
        }
        return info;
    }
    #endregion

    #region 安全机制

    /// <summary>
    /// 设置安全极限
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="limit">极限值</param>
    /// <param name="isForward">是否正向</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetLimit(int axis, float limit, bool isForward)
    {
        int result = 0;
        if (isForward)
        {
            result = zmcaux.ZAux_Direct_SetFsLimit(_handle, axis, limit);
        }
        else
        {
            result = zmcaux.ZAux_Direct_SetRsLimit(_handle, axis, limit);
        }
        CheckResult(result);
    }

    /// <summary>
    /// 获取安全极限
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="isForward">是否正向</param>
    /// <returns>极限值</returns>
    public float GetLimit(int axis, bool isForward)
    {
        float limit = 0;
        int result = 0;
        if (isForward)
        {
            result = zmcaux.ZAux_Direct_GetFsLimit(_handle, axis, ref limit);
        }
        else
        {
            result = zmcaux.ZAux_Direct_GetRsLimit(_handle, axis, ref limit);
        }
        CheckResult(0);
        return limit;
    }
    #endregion

    public void CheckResult(int result)
    {
        if (result != 0)
        {
            OnExecuteError?.Invoke(this, $"Error Code: {result}; {ErrorCode.Parse(result)}");
            throw new ZMotionException(result);
        }
    }
}