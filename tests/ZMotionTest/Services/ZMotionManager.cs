using ZMotionSDK;
using ZMotionSDK.Models;
using ZMotionSDK.Helper;

namespace ZMotionTest.Services;

/// <summary>
/// ZMotion管理器，用于管理全局的连接状态和设备实例
/// </summary>
public sealed class ZMotionManager
{
    private static readonly Lazy<ZMotionManager> _instance = new(() => new ZMotionManager());
    private readonly ZMotion _zMotion;

    private ZMotionManager()
    {
        _zMotion = new ZMotion();
    }

    public static ZMotionManager Instance => _instance.Value;

    /// <summary>
    /// ZMotion实例
    /// </summary>
    public ZMotion ZMotion => _zMotion;

    /// <summary>
    /// 连接状态
    /// </summary>
    public bool IsConnected => _zMotion.Handle != IntPtr.Zero;

    /// <summary>
    /// 连接设备
    /// </summary>
    public void Connect(string ipAddress, uint timeout)
    {
        _zMotion.Open(ipAddress, timeout);
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        if (IsConnected)
        {
            _zMotion.Close();
        }
    }

    /// <summary>
    /// 应用DIDO反转配置
    /// </summary>
    /// <param name="config">DIDO反转配置</param>
    public void ApplyDIDOInverter(DIDOInverterConfig config)
    {
        _zMotion.ApplyDIDOInverter(config);
    }

    /// <summary>
    /// 获取轴状态
    /// </summary>
    public AxisStatus GetAxisStatus(int axisIndex)
    {
        return _zMotion.GetAxisStatus(axisIndex);
    }

    /// <summary>
    /// 获取轴信息数组
    /// </summary>
    public AxisInfo[] GetAxisInfoArray(int axisCount)
    {
        return _zMotion.GetAxisInfo(axisCount);
    }

    /// <summary>
    /// 获取规划位置
    /// </summary>
    public float GetDpos(int axisIndex)
    {
        return _zMotion.GetPosition_D(axisIndex);
    }

    /// <summary>
    /// 获取反馈位置
    /// </summary>
    public float GetMpos(int axisIndex)
    {
        return _zMotion.GetPosition_M(axisIndex);
    }

    /// <summary>
    /// 获取轴运行状态
    /// </summary>
    public bool GetIsRunning(int axisIndex)
    {
        return _zMotion.GetIsRunning(axisIndex);
    }

    /// <summary>
    /// 获取轴速度
    /// </summary>
    public float GetSpeed(int axisIndex)
    {
        return _zMotion.GetSpeed(axisIndex);
    }

    #region 参数读写方法
    /// <summary>
    /// 读取轴参数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="paramName">参数名称</param>
    /// <returns>参数值</returns>
    public float GetParam(int axis, ReadBaiscParmName paramName)
    {
        return _zMotion.GetParam(axis, paramName);
    }

    /// <summary>
    /// 设置轴参数
    /// </summary>
    /// <param name="axis">轴索引</param>
    /// <param name="paramName">参数名称</param>
    /// <param name="value">参数值</param>
    public void SetParam(int axis, WriteBaiscParmName paramName, float value)
    {
        _zMotion.SetParam(axis, paramName, value);
    }

    /// <summary>
    /// 读取多个轴的参数
    /// </summary>
    /// <param name="number">轴数量</param>
    /// <param name="paramName">参数名称</param>
    /// <returns>参数值数组</returns>
    public float[] GetParam_Multi(int number, ReadBaiscParmName paramName)
    {
        return _zMotion.GetParam_Multi(number, paramName);
    }
    #endregion

    #region 硬件接口 - 单个IO
    /// <summary>
    /// 获取数字输入
    /// </summary>
    public bool GetDI(int index)
    {
        return _zMotion.GetDI(index);
    }

    /// <summary>
    /// 获取数字输出
    /// </summary>
    public bool GetDO(int index)
    {
        return _zMotion.GetDO(index);
    }

    /// <summary>
    /// 设置数字输出
    /// </summary>
    public void SetDO(int index, bool value)
    {
        _zMotion.SetDO(index, value);
    }
    #endregion

    #region 硬件接口 - Modbus方式IO
    /// <summary>
    /// 获取数字输入 - Modbus方式
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输入状态,按位存储</returns>
    public bool[] GetDI_Modbus(int startIndex, int endIndex)
    {
        return _zMotion.GetDI_Multi_Modbus(startIndex, endIndex);
    }

    /// <summary>
    /// 获取数字输出 - Modbus方式
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输出状态,按位存储</returns>
    public bool[] GetDO_Modbus(int startIndex, int endIndex)
    {
        return _zMotion.GetDO_Multi_Modbus(startIndex, endIndex);
    }
    #endregion

    #region 硬件接口 - 多个IO
    /// <summary>
    /// 获取多个数字输入
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输入状态,按位存储</returns>
    public bool[] GetDI_Multi(ushort startIndex, ushort endIndex)
    {
        return _zMotion.GetDI_Multi(startIndex, endIndex);
    }

    /// <summary>
    /// 获取多个数字输出
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <returns>输出状态,按位存储</returns>
    public bool[] GetDO_Multi(ushort startIndex, ushort endIndex)
    {
        return _zMotion.GetDO_Multi(startIndex, endIndex);
    }

    /// <summary>
    /// 设置多个数字输出
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">结束索引</param>
    /// <param name="value">输出状态</param>
    public void SetDO_Multi(ushort startIndex,  bool[] value)
    {
        _zMotion.SetDO_Multi(startIndex, value);
    }
    #endregion
} 