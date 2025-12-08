using System.Security.Cryptography.X509Certificates;
using System.Text;
using ZMotionSDK.Models;

namespace ZMotionSDK;

public partial class ZMotion
{
    #region 缓存模式

    #region 缓存状态查询方法

    /// <summary>
    /// 读取轴当前运动的最终位置
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>结束移动标志值</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetEndPositionMove(int axis)
    {
        float value = 0;
        var result = zmcaux.ZAux_Direct_GetEndMove(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取当前和缓冲中运动的最终位置，可以用于相对绝对转换
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>缓存结束移动标志值</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetEndPositionMove_Buffer(int axis)
    {
        float value = 0;
        var result = zmcaux.ZAux_Direct_GetEndMoveBuffer(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取轴当前除了当前运动是否还有缓冲运动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>是否还有运动缓冲,true:有,false:没有</returns>
    /// <exception cref="ZMotionException"></exception>
    public bool GetLoaded(int axis)
    {
        int value = 0;
        var result = zmcaux.ZAux_Direct_GetLoaded(_handle, axis, ref value);
        CheckResult(result);
        return value == 0;
    }

    /// <summary>
    /// 读取轴当前被缓冲起来的运动个数
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>缓存移动数量</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetMovesBuffered(int axis)
    {
        int value = 0;
        var result = zmcaux.ZAux_Direct_GetMovesBuffered(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取轴当前正在运动指令的MOVE_MARK标号
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>当前移动标记值</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetMoveCurmark(int axis)
    {
        int value = 0;
        var result = zmcaux.ZAux_Direct_GetMoveCurmark(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 设置轴运动MOVE_MARK标号 每当有运动进入轴运动缓冲时MARK自动+1
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="value">移动标记值</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetMovemark(int axis, int value)
    {
        var result = zmcaux.ZAux_Direct_SetMovemark(_handle, axis, value);
        CheckResult(result);
    }

    /// <summary>
    /// 读取轴当前运动还未完成的距离
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>剩余距离值</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetRemain(int axis)
    {
        float value = 0;
        var result = zmcaux.ZAux_Direct_GetRemain(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取轴剩余的直线缓冲
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>线性缓存剩余数量</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetRemainLineBuffer(int axis)
    {
        int value = 0;
        var result = zmcaux.ZAux_Direct_GetRemain_LineBuffer(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取轴剩余运动缓冲 按最复杂的空间圆弧来计算
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>缓存剩余数量</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetRemainBuffer(int axis)
    {
        int value = 0;
        var result = zmcaux.ZAux_Direct_GetRemain_Buffer(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    /// <summary>
    /// 读取轴当前运动和缓冲运动还未完成的距离
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>向量缓存数量</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetVectorBuffered(int axis)
    {
        float value = 0;
        var result = zmcaux.ZAux_Direct_GetVectorBuffered(_handle, axis, ref value);
        CheckResult(result);
        return value;
    }

    #endregion

    #region 缓存移动控制方法

    /// <summary>
    /// 缓存模式下的输出控制移动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="outputNum">输出端口号</param>
    /// <param name="value">输出值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveOp(int axis, int outputNum, int value)
    {
        var result = zmcaux.ZAux_Direct_MoveOp(_handle, axis, outputNum, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的多输出控制移动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="outputNumFirst">起始输出端口号</param>
    /// <param name="outputNumEnd">结束输出端口号</param>
    /// <param name="value">输出值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveOpMulti(int axis, int outputNumFirst, int outputNumEnd, int value)
    {
        var result = zmcaux.ZAux_Direct_MoveOpMulti(_handle, axis, outputNumFirst, outputNumEnd, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的带时间的输出控制移动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="outputNum">输出端口号</param>
    /// <param name="value">输出值</param>
    /// <param name="offTimeMs">关闭时间(ms)</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveOp2(int axis, int outputNum, int value, int offTimeMs)
    {
        var result = zmcaux.ZAux_Direct_MoveOp2(_handle, axis, outputNum, value, offTimeMs);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的模拟输出控制移动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="outputNum">模拟输出端口号</param>
    /// <param name="value">输出值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveAout(int axis, int outputNum, float value)
    {
        var result = zmcaux.ZAux_Direct_MoveAout(_handle, axis, outputNum, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的延时移动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="timeMs">延时时间(毫秒)</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveDelay(int axis, int timeMs)
    {
        var result = zmcaux.ZAux_Direct_MoveDelay(_handle, axis, timeMs);
        CheckResult(result);
    }

    /// <summary>
    /// 运动缓冲加入一个条件判断的阻塞
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="paraName">参数名称</param>
    /// <param name="num">编号</param>
    /// <param name="cmpMode">比较模式;1:>=,0:=,-1:<=</param>
    /// <param name="value">比较值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveWait(uint baseAxis, string paraName, int num, int cmpMode, float value)
    {
        var result = zmcaux.ZAux_Direct_MoveWait(_handle, baseAxis, paraName, num, cmpMode, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的参数移动
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="paraName">参数名称</param>
    /// <param name="num">编号</param>
    /// <param name="cmpMode">比较模式;1:>=,0:=,-1:<=</param>
    /// <param name="value">比较值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveWait(uint baseAxis, WaitParaName paraName, int num, int cmpMode, float value)
    {
        var result = zmcaux.ZAux_Direct_MoveWait(_handle, baseAxis, paraName.ToString(), num, cmpMode, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的任务移动
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="taskNum">任务号</param>
    /// <param name="labelName">标签名称</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveTask(uint baseAxis, uint taskNum, string labelName)
    {
        var result = zmcaux.ZAux_Direct_MoveTask(_handle, baseAxis, taskNum, labelName);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的参数移动
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="paraName">参数名称</param>
    /// <param name="axis">轴号</param>
    /// <param name="value">参数值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MovePara(uint baseAxis, string paraName, uint axis, float value)
    {
        var result = zmcaux.ZAux_Direct_MovePara(_handle, baseAxis, paraName, axis, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的参数移动
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="paraName">参数名称</param>
    /// <param name="axis">轴号</param>
    /// <param name="value">参数值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MovePara(uint baseAxis, WriteBaiscParmName paraName, uint axis, float value)
    {
        var result = zmcaux.ZAux_Direct_MovePara(_handle, baseAxis, paraName.ToString(), axis, value);
        CheckResult(result);
    }

    /// <summary>
    /// 缓存模式下的PWM移动
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="pwmNum">PWM编号</param>
    /// <param name="pwmDuty">PWM占空比</param>
    /// <param name="pwmFreq">PWM频率</param>
    /// <exception cref="ZMotionException"></exception>
    public void MovePwm(uint baseAxis, uint pwmNum, float pwmDuty, float pwmFreq)
    {
        var result = zmcaux.ZAux_Direct_MovePwm(_handle, baseAxis, pwmNum, pwmDuty, pwmFreq);
        CheckResult(result);
    }

    /// <summary>
    /// 运动中触发其他轴运动写入缓冲，当前轴等待。
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="axis">同步轴号</param>
    /// <param name="dist">移动距离</param>
    /// <param name="fsp">同步标志</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveSynmove(uint baseAxis, uint axis, float dist, uint fsp)
    {
        var result = zmcaux.ZAux_Direct_MoveSynmove(_handle, baseAxis, axis, dist, fsp);
        CheckResult(result);
    }

    /// <summary>
    /// 运动中触发其他轴运动，缓存模式下的异步移动(当前轴不等待)
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="axis">异步轴号</param>
    /// <param name="dist">移动距离</param>
    /// <param name="fsp">异步标志</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveASynmove(uint baseAxis, uint axis, float dist, uint fsp)
    {
        var result = zmcaux.ZAux_Direct_MoveASynmove(_handle, baseAxis, axis, dist, fsp);
        CheckResult(result);
    }

    /// <summary>
    /// 运动缓冲中插入修改TABLE寄存器指令
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="tableNum">表格编号</param>
    /// <param name="value">表格值</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveTable(uint baseAxis, uint tableNum, float value)
    {
        var result = zmcaux.ZAux_Direct_MoveTable(_handle, baseAxis, tableNum, value);
        CheckResult(result);
    }

    /// <summary>
    /// 运动中取消其他轴运动
    /// </summary>
    /// <param name="baseAxis">基础轴号</param>
    /// <param name="cancelAxis">取消轴号</param>
    /// <param name="mode">取消模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveCancel(int baseAxis, int cancelAxis, int mode)
    {
        var result = zmcaux.ZAux_Direct_MoveCancel(_handle, baseAxis, cancelAxis, mode);
        CheckResult(result);
    }

    /// <summary>
    /// 运动中修改结束位置，单轴指令
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="distance">修改距离</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveModify(int axis, float distance)
    {
        var result = zmcaux.ZAux_Direct_MoveModify(_handle, axis, distance);
        CheckResult(result);
    }

    /// <summary>
    /// 运动末尾位置增加速度限制，用于强制拐角减速
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="limitSpeed">限制速度</param>
    /// <exception cref="ZMotionException"></exception>
    public void MoveLimit(int axis, float limitSpeed)
    {
        var result = zmcaux.ZAux_Direct_MoveLimit(_handle, axis, limitSpeed);
        CheckResult(result);
    }

    #endregion

    #endregion

    #region 单轴运动


    /// <summary>
    /// 等待轴运动完成
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="timeoutMs">超时时间(毫秒)，默认10秒</param>
    /// <param name="intervalMs">检查间隔时间(毫秒)，默认50毫秒</param>
    /// <param name="exceptionHandler">异常处理器，用于处理运行时异常</param>
    /// <returns>运动完成结果</returns>
    public async Task<WaitMoveResult> WaitMoveCompleteAsync(int axis, int timeoutMs = 10000, int intervalMs = 50, Action<Exception>? exceptionHandler = null)
    {
        var startTime = DateTime.Now;
        var result = new WaitMoveResult { Axis = axis, IsSuccess = false, ElapsedTime = 0 };

        using var cts = new CancellationTokenSource(timeoutMs);

        while (!cts.Token.IsCancellationRequested)
        {
            try
            {
                var isRunning = GetIsRunning(axis);
                if (!isRunning)
                {
                    result.IsSuccess = true;
                    result.ElapsedTime = (DateTime.Now - startTime).TotalMilliseconds;
                    return result;
                }

                await Task.Delay(intervalMs, cts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // 超时处理
                result.IsSuccess = false;
                result.ElapsedTime = (DateTime.Now - startTime).TotalMilliseconds;
                return result;
            }
            catch (Exception ex)
            {
                // 使用异常处理器而不是直接抛出
                exceptionHandler?.Invoke(ex);
            }
        }

        return result;
    }

    /// <summary>
    /// 单轴相对运动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="distance">距离units</param>
    /// <exception cref="ZMotionException"></exception>
    public void Move_Relative(int axis, float distance)
    {
        var result = zmcaux.ZAux_Direct_Single_Move(_handle, axis, distance);
        CheckResult(result);
    }

    /// <summary>
    /// 单轴绝对运动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="position">位置units</param>
    /// <exception cref="ZMotionException"></exception>
    public void Move_Absolute(int axis, float position)
    {
        var result = zmcaux.ZAux_Direct_Single_MoveAbs(_handle, axis, position);
        CheckResult(result);
    }

    /// <summary>
    /// 单轴连续运动
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="isForward">是否正向</param>
    /// <exception cref="ZMotionException"></exception>
    public void Jog(int axis, bool isForward)
    {
        var result = zmcaux.ZAux_Direct_Single_Vmove(_handle, axis, isForward ? 1 : -1);
        CheckResult(result);
    }

    /// <summary>
    /// 设置单轴点动速度
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="speed">速度units/s</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetJogSpeed(int axis, float speed)
    {
        var result = zmcaux.ZAux_Direct_SetJogSpeed(_handle, axis, speed);
        CheckResult(result);
    }

    /// <summary>
    /// 获取单轴点动速度
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>速度units/s</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetJogSpeed(int axis)
    {
        float speed = 0;
        var result = zmcaux.ZAux_Direct_GetJogSpeed(_handle, axis, ref speed);
        CheckResult(result);
        return speed;
    }

    #region IO控制快速点动

    #endregion

    #endregion

    #region 回零

    /// <summary>
    /// 单轴回零
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="mode">回零模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void GoHome(int axis, int mode)
    {
        var result = zmcaux.ZAux_Direct_Single_Datum(_handle, axis, mode);
        CheckResult(result);
    }

    /// <summary>
    /// 总线驱动器回零
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="mode">回零模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void GoHome_Bus(uint axis, uint mode)
    {
        var result = zmcaux.ZAux_BusCmd_Datum(_handle, axis, mode);
        CheckResult(result);
    }

    /// <summary>
    /// 获取回零状态
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>回零状态;true:回零正常;false:回零异常</returns>
    /// <exception cref="ZMotionException"></exception>
    public bool GetHomeStatus(int axis)
    {
        uint status = 0;
        var result = zmcaux.ZAux_Direct_GetHomeStatus(_handle, axis, ref status);
        CheckResult(result);
        return status == 1;
    }

    /// <summary>
    /// 获取总线驱动器回零状态
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>回零状态;true:回零正常;false:回零异常</returns>
    /// <exception cref="ZMotionException"></exception>
    public bool GetHomeStatus_Bus(uint axis)
    {
        uint status = 0;
        var result = zmcaux.ZAux_BusCmd_GetHomeStatus(_handle, axis, ref status);
        CheckResult(result);
        return status == 1;
    }

    /// <summary>
    /// 设置回零慢速
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="speed">速度</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetGoHomeCreepSpeed(int axis, float speed)
    {
        var result = zmcaux.ZAux_Direct_SetCreep(_handle, axis, speed);
        CheckResult(result);
    }

    /// <summary>
    /// 获取回零慢速
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>速度</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetGoHomeCreepSpeed(int axis)
    {
        float speed = 0;
        var result = zmcaux.ZAux_Direct_GetCreep(_handle, axis, ref speed);
        CheckResult(result);
        return speed;
    }

    /// <summary>
    /// 设置回零偏移量
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="offpos">偏移量</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetGoHomeOffpos(uint axis, float offpos)
    {
        var result = zmcaux.ZAux_BusCmd_SetDatumOffpos(_handle, axis, offpos);
        CheckResult(result);
    }

    /// <summary>
    /// 获取回零偏移量
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>偏移量</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetGoHomeOffpos(uint axis)
    {
        float offpos = 0;
        var result = zmcaux.ZAux_BusCmd_GetDatumOffpos(_handle, axis, ref offpos);
        CheckResult(result);
        return offpos;
    }

    /// <summary>
    /// 设置回零返找等待时间(默认2ms)
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="waitTime">等待时间(ms)</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetGoHomeWaitTime(int axis, int waitTime)
    {
        var result = zmcaux.ZAux_Direct_SetHomeWait(_handle, axis, waitTime);
        CheckResult(result);
    }

    /// <summary>
    /// 获取回零返找等待时间(默认2ms)
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>等待时间(ms)</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetGoHomeWaitTime(int axis)
    {
        int waitTime = 0;
        var result = zmcaux.ZAux_Direct_GetHomeWait(_handle, axis, ref waitTime);
        CheckResult(result);
        return waitTime;
    }

    #endregion

    #region 轴暂停/恢复/停止

    /// <summary>
    /// 轴暂停
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="mode">暂停模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void Pause(int axis, int mode)
    {
        var result = zmcaux.ZAux_Direct_MovePause(_handle, axis, mode);
        CheckResult(result);
    }

    /// <summary>
    /// 轴恢复
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <exception cref="ZMotionException"></exception>
    public void Resume(int axis)
    {
        var result = zmcaux.ZAux_Direct_MoveResume(_handle, axis);
        CheckResult(result);
    }

    /// <summary>
    /// 停止所有轴
    /// </summary>
    /// <param name="mode">停止模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void Stop_All(CancelMode mode = CancelMode.取消当前运动和缓冲运动)
    {
        var result = zmcaux.ZAux_Direct_Rapidstop(_handle, (int)mode);
        CheckResult(result);
    }

    /// <summary>
    /// 停止单轴
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="mode">停止模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void Stop(int axis, CancelMode mode = CancelMode.取消当前运动和缓冲运动)
    {
        var result = zmcaux.ZAux_Direct_Single_Cancel(_handle, axis, (int)mode);
        CheckResult(result);
    }

    /// <summary>
    /// 停止多个轴
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="mode">停止模式</param>
    /// <exception cref="ZMotionException"></exception>
    public void Stop_List(int[] axis, CancelMode mode = CancelMode.取消当前运动和缓冲运动)
    {
        var result = zmcaux.ZAux_Direct_CancelAxisList(_handle, axis.Length, axis, (int)mode);
        CheckResult(result);
    }
    #endregion

    #region 在线命令

    /// <summary>
    /// 执行在线命令
    /// </summary>
    /// <param name="command">命令</param>
    /// <returns>响应</returns>
    /// <exception cref="ZMotionException"></exception>
    public string Execute_Buffer(string command)
    {
        StringBuilder response = new StringBuilder(1024);
        var result = zmcaux.ZAux_Execute(_handle, command, response, 1024);
        CheckResult(result);
        return response.ToString();
    }

    /// <summary>
    /// 执行在线命令
    /// </summary>
    /// <param name="command">命令</param>
    /// <returns>响应</returns>
    /// <exception cref="ZMotionException"></exception>
    public string Execute_Direct(string command)
    {
        StringBuilder response = new StringBuilder(1024);
        var result = zmcaux.ZAux_DirectCommand(_handle, command, response, 1024);
        CheckResult(result);
        return response.ToString();
    }

    /// <summary>
    /// 触发示波器
    /// </summary>
    /// <exception cref="ZMotionException"></exception>
    public void Trigger()
    {
        var result = zmcaux.ZAux_Trigger(_handle);
        CheckResult(result);
    }
    #endregion

    #region 总线相关

    /// <summary>
    /// 获取总线节点数量
    /// </summary>
    /// <param name="slot">槽位号</param>
    /// <returns>节点数量</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetBusNodeNum(int slot)
    {
        int num = 0;
        var result = zmcaux.ZAux_BusCmd_GetNodeNum(_handle, slot, ref num);
        CheckResult(result);
        return num;
    }

    /// <summary>
    /// 清除总线驱动器报警
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="mode">清除模式,0-清除当前告警  1-清除历史告警  2-清除外部输入告警</param>
    /// <exception cref="ZMotionException"></exception>
    public void ClearAlarm_Bus(uint axis, uint mode)
    {
        var result = zmcaux.ZAux_BusCmd_DriveClear(_handle, axis, mode);
        CheckResult(result);
    }

    /// <summary>
    /// 设置总线轴使能
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="enable">使能状态;true:使能;false:失能</param>
    /// <exception cref="ZMotionException"></exception>
    public void AxisEnable_Bus(int axis, bool enable)
    {
        var result = zmcaux.ZAux_Direct_SetAxisEnable(_handle, axis, enable ? 1 : 0);
        CheckResult(result);
    }

    /// <summary>
    /// 获取总线轴使能状态
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>使能状态;true:使能;false:失能</returns>
    /// <exception cref="ZMotionException"></exception>
    public bool GetAxisEnable_Bus(int axis)
    {
        int enable = 0;
        var result = zmcaux.ZAux_Direct_GetAxisEnable(_handle, axis, ref enable);
        CheckResult(result);
        return enable == 1;
    }

    /// <summary>
    /// 初始化总线
    /// </summary>
    /// <exception cref="ZMotionException"></exception>
    public void Init_Bus()
    {
        var result = zmcaux.ZAux_BusCmd_InitBus(_handle);
        CheckResult(result);
    }

    /// <summary>
    /// 获取总线初始化状态
    /// </summary>
    /// <returns>初始化状态;true:初始化成功;false:初始化失败</returns>
    /// <exception cref="ZMotionException"></exception>
    public bool GetBusInitStatus()
    {
        int status = 0;
        var result = zmcaux.ZAux_BusCmd_GetInitStatus(_handle, ref status);
        CheckResult(result);
        return status == 1;
    }

    #endregion

    #region 电子齿轮

    /// <summary>
    /// 设置连接速率
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="rate">连接速率,单位:ratio/s,默认值 100_0000</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetClutchRate(int axis, float rate)
    {
        var result = zmcaux.ZAux_Direct_SetClutchRate(_handle, axis, rate);
        CheckResult(result);
    }

    /// <summary>
    /// 获取连接速率
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>连接速率,单位:ratio/s</returns>
    /// <exception cref="ZMotionException"></exception>
    public float GetClutchRate(int axis)
    {
        float rate = 0;
        var result = zmcaux.ZAux_Direct_GetClutchRate(_handle, axis, ref rate);
        CheckResult(result);
        return rate;
    }

    /// <summary>
    /// 设置编码轴输入齿轮比，缺省(1,1)，设置负值可切换方向
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <param name="output_count">输出齿轮比分子</param>
    /// <param name="input_count">输入齿轮比分母</param>
    /// <exception cref="ZMotionException"></exception>
    public void SetEncoderRatio(int axis, int output_count, int input_count)
    {
        var result = zmcaux.ZAux_Direct_EncoderRatio(_handle, axis, output_count, input_count);
        CheckResult(result);
    }

    /// <summary>
    /// 设置电子齿轮矢量同步
    /// </summary>
    /// <param name="link_axis">连接轴的轴号，手轮时为编码器轴</param>
    /// <param name="move_axis">随动轴号</param>
    /// <param name="ratio">比率，可正可负，注意是脉冲个数的比例</param>
    /// <exception cref="ZMotionException"></exception>
    public void Connect(int link_axis, int move_axis, float ratio)
    {
        var result = zmcaux.ZAux_Direct_Connect(_handle, ratio, link_axis, move_axis);
        CheckResult(result);
    }

    /// <summary>
    /// 设置电子齿轮矢量同步,将当前轴的目标位置与 link_axis 轴的插补矢量长度通过电子齿轮连接。
    /// </summary>
    /// <param name="link_axis">连接轴的轴号，手轮时为编码器轴</param>
    /// <param name="move_axis">随动轴号</param>
    /// <param name="ratio">比率，可正可负，注意是脉冲个数的比例</param>
    /// <exception cref="ZMotionException"></exception>
    public void Connpath(int link_axis, int move_axis, float ratio)
    {
        var result = zmcaux.ZAux_Direct_Connpath(_handle, ratio, link_axis, move_axis);
        CheckResult(result);
    }

    /// <summary>
    /// 获取轴链接运动的参考轴号
    /// </summary>
    /// <param name="axis">轴号</param>
    /// <returns>参考主轴轴号</returns>
    /// <exception cref="ZMotionException"></exception>
    public int GetLinkAxis(int axis)
    {
        int link_axis = 0;
        var result = zmcaux.ZAux_Direct_GetLinkax(_handle, axis, ref link_axis);
        CheckResult(result);
        return link_axis;
    }
    #endregion
}