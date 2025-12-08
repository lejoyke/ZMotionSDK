using System.Runtime.InteropServices;
using System.Text;

/********************************** ZMC系列控制器  ************************************************
**--------------文件信息--------------------------------------------------------------------------------
**文件名: zmcaux.h
**创建人: 郑孝洋
**时间: 20130621
**描述: ZMCDLL 辅助函数

**------------修订历史记录----------------------------------------------------------------------------
		
** 修改人: zxy
** 版  本: 1.1
** 日　期: 2014.5.11
** 描　述: ZMC_ExecuteNoAck 替换为 ZMC_Execute
		  
			
** 修改人: zxy
** 版  本: 1.3
** 日　期: 2014.7.21
** 描　述: ZMC_Execute ZMC_DirectCommand 替换为ZAux_Execute ZAux_DirectCommand
			  
增加 ZAux_SetParam  ZAux_GetParam  ZAux_Direct_SetParam  ZAux_Direct_GetParam
				
增加 ZAux_WriteUFile  ZAux_ReadUFile
				  
** 修改人: wy
** 版  本: 1.5
** 日　期: 2016.6.6
** 描　述: 对所有BASIC指令进行封装，整合ZMC库到AUX库


  ** 修改人: wy
** 版  本: 2.1
** 日　期: 2018.8.24
** 描  述：添加PCI链接函数
**		   对所有BASIC指令运动指令进行封装，封装轴列表到函数
**		   增加部分总线指令
**		   增加部分MOVE_PARA指令
		   增加位置比较输出指令
**------------------------------------------------------------------------------------------------------
********************************************************************************************************/

namespace ZMotionSDK
{
    public class zmcaux
    {
        /// <summary>
        /// Execute在线命令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="pszCommand">字符串命令</param>
        /// <param name="psResponse">返回的字符串</param>
        /// <param name="uiResponseLength">返回的字符长度</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Execute", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Execute(IntPtr handle, string pszCommand, StringBuilder psResponse, UInt32 uiResponseLength);

        /// <summary>
        /// DirectCommand在线命令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="pszCommand">命令字符串</param>
        /// <param name="psResponse">返回的字符串</param>
        /// <param name="uiResponseLength">返回的字符长度</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_DirectCommand", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_DirectCommand(IntPtr handle, string pszCommand, StringBuilder psResponse, UInt32 uiResponseLength);

        /// <summary>
        /// 命令跟踪设置
        /// </summary>
        /// <param name="bifTofile">命令获取类型 0 关闭  1-只输出错误命令  2-只输出运动与设置命令  3输出全部命令</param>
        /// <param name="pFilePathName">文件路径</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SetTraceFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SetTraceFile(int bifTofile, string pFilePathName);

        /// <summary>
        /// 控制器串口链接
        /// </summary>
        /// <param name="comid">串口号</param>
        /// <param name="phandle">句柄指针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_OpenCom", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_OpenCom(UInt32 comid, out IntPtr phandle);

        /// <summary>
        /// 搜索串口连接控制器
        /// </summary>
        /// <param name="uimincomidfind">最小串口号</param>
        /// <param name="uimaxcomidfind">最大串口号</param>
        /// <param name="pcomid">有效COM口列表</param>
        /// <param name="uims">连接时间</param>
        /// <param name="phandle">句柄指针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SearchAndOpenCom", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SearchAndOpenCom(UInt32 uimincomidfind, UInt32 uimaxcomidfind, ref uint pcomid, UInt32 uims, out IntPtr phandle);

        /// <summary>
        /// 设置COM口连接参数
        /// </summary>
        /// <param name="dwBaudRate">波特率</param>
        /// <param name="dwByteSize">数据位</param>
        /// <param name="dwParity">校验位</param>
        /// <param name="dwStopBits">停止位</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SetComDefaultBaud", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SetComDefaultBaud(UInt32 dwbaudRate, UInt32 dwByteSize, UInt32 dwParity, UInt32 dwStopBits);

        /// <summary>
        /// 设置控制器IP地址
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ipaddress">IP地址</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SetIp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SetIp(IntPtr handle, string ipaddress);

        /// <summary>
        /// 以太网方式连接控制器
        /// </summary>
        /// <param name="ipaddr">IP地址</param>
        /// <param name="phandle">句柄指针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_OpenEth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_OpenEth(string ipaddr, out IntPtr phandle);

        /// <summary>
        /// 快速搜索IP地址
        /// </summary>
        /// <param name="ipaddrlist">Ip地址列表指针</param>
        /// <param name="addrbufflength">搜索IP地址最大长度</param>
        /// <param name="uims">搜索最大时间</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SearchEthlist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SearchEthlist(StringBuilder ipaddrlist, UInt32 addrbufflength, UInt32 uims);

        /// <summary>
        /// 搜索控制器
        /// </summary>
        /// <param name="ipaddress">IP地址</param>
        /// <param name="uims">最大搜索时间</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SearchEth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SearchEth(string ipaddress, UInt32 uims);

        /// <summary>
        /// 关闭控制器链接
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Close", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Close(IntPtr handle);

        /// <summary>
        /// 暂停继续运行BAS项目
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Resume", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Resume(IntPtr handle);

        /// <summary>
        /// 暂停控制器中BAS程序
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Pause", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Pause(IntPtr handle);

        /// <summary>
        /// 单个BAS文件生成ZAR并且下载到控制器运行
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Filename">BAS文件路径</param>
        /// <param name="run_mode">RAM-ROM  0-RAM  1-ROM</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BasDown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BasDown(IntPtr handle, string Filename, UInt32 run_mode);

        /// <summary>
        /// ZPJ文件下载
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ZpjName">ZpjName ZPJ文件路径</param>
        /// <param name="ZarName">ZarName Zar文件路径</param>
        /// <param name="pPass">pPass 软件密码, 绑定APP_PASS  没有密码时pPass = NULL 或 ""</param>
        /// <param name="uid">uid 绑定控制器唯一ID， 0-不绑定</param>
        /// <param name="run_mode">下载到RAM-ROM  0-RAM  1-ROM</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_ZpjDown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_ZpjDown(IntPtr handle, string ZpjName, string ZarName, string pPass, UInt32 uid, UInt32 run_mode);

        /// <summary>
        /// 读取输入信号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">输入口编号</param>
        /// <param name="piValue">输入口状态</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetIn(IntPtr handle, int ionum, ref UInt32 piValue);

        /// <summary>
        /// 打开输出口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">输出口编号</param>
        /// <param name="iValue">输出口状态值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetOp(IntPtr handle, int ionum, UInt32 iValue);

        /// <summary>
        /// 读取输出口状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">输出口编号</param>
        /// <param name="piValue">输出口状态值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetOp(IntPtr handle, int ionum, ref UInt32 piValue);

        /// <summary>
        /// 读取模拟量输入值
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">模拟量输入编号</param>
        /// <param name="pfValue">返回的模拟量值 4系列以下0-4095</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAD(IntPtr handle, int ionum, ref float pfValue);

        /// <summary>
        /// 设置模拟量输出
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">模拟量输出编号</param>
        /// <param name="fValue">设定的模拟量值4系列以下0-4095</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetDA(IntPtr handle, int ionum, float fValue);

        /// <summary>
        /// 读取模拟输出口值
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">模拟量输出编号</param>
        /// <param name="pfValue">反馈模拟量值4系列以下0-4095</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetDA(IntPtr handle, int ionum, ref float pfValue);

        /// <summary>
        /// 设置输入口反转
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">输入口编号</param>
        /// <param name="bifInvert">反转状态设置值 0/1</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetInvertIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetInvertIn(IntPtr handle, int ionum, int bifInvert);

        /// <summary>
        /// 读取输入口反转状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">输入口编号</param>
        /// <param name="piValue">反转状态返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInvertIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetInvertIn(IntPtr handle, int ionum, ref int piValue);

        /// <summary>
        /// 设置pwm频率
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">PWM编号口</param>
        /// <param name="fValue">频率值设定值 硬件PWM1M 软PWM 2K</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetPwmFreq", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetPwmFreq(IntPtr handle, int ionum, float fValue);

        /// <summary>
        /// 读取pwm频率
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">PWM编号口</param>
        /// <param name="pfValue">频率返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetPwmFreq", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetPwmFreq(IntPtr handle, int ionum, ref float pfValue);

        /// <summary>
        /// 设置pwm占空比
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">PWM编号口</param>
        /// <param name="fValue">占空比设置值	0-1  0表示关闭PWM口</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetPwmDuty", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetPwmDuty(IntPtr handle, int ionum, float fValue);

        /// <summary>
        /// 读取pwm占空比
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionum">PWM编号口</param>
        /// <param name="pfValue">占空比反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetPwmDuty", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetPwmDuty(IntPtr handle, int ionum, ref float pfValue);

        /// <summary>
        /// 快速读取多个输入状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionumfirst">输入口起始编号</param>
        /// <param name="ionumend">输入口结束编号</param>
        /// <param name="pValueList">输入口状态返回值 按位存储</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetModbusIn(IntPtr handle, int ionumfirst, int ionumend, byte[] pValueList);

        /// <summary>
        /// 快速读取多个输出口状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ionumfirst">输出口起始编号</param>
        /// <param name="ionumend">输出口结束编号</param>
        /// <param name="pValueList">输出口状态返回值 按位存储</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusOut", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetModbusOut(IntPtr handle, int ionumfirst, int ionumend, byte[] pValueList);

        /// <summary>
        ///  快速读取多个轴的命令位置
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">轴数量</param>
        /// <param name="pValueList">命令位置反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusDpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetModbusDpos(IntPtr handle, int imaxaxises, float[] pValueList);

        /// <summary>
        /// 快速读取多个轴的反馈位置
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">轴数量</param>
        /// <param name="pValueList">反馈位置反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusMpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetModbusMpos(IntPtr handle, int imaxaxises, float[] pValueList);

        /// <summary>
        /// 快速读取多个轴的规划运行速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">轴数量</param>
        /// <param name="pValueList">规划速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetModbusCurSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetModbusCurSpeed(IntPtr handle, int imaxaxises, float[] pValueList);

        /// <summary>
        /// 通用的轴参数修改命令 
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="sParam">轴参数名称字符串 "DPOS"...</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fset">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetParam(IntPtr handle, string sParam, int iaxis, float fset);

        /// <summary>
        /// 通用的参数读取命令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="sParam">轴参数名称字符串 "DPOS"...</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetParam(IntPtr handle, string sParam, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴加速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">加速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAccel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetAccel(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴加速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">加速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAccel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAccel(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取叠加轴轴号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">被叠加轴号</param>
        /// <param name="piValue">叠加轴号反馈值 -1表示未被叠加</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAddax", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAddax(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴报警信号输入口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">输入口编号设定值 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAlmIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetAlmIn(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴报警信号输入口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">输入口编号返回值 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAlmIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAlmIn(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">轴类型设置值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetAtype(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">轴类型反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAtype(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">轴状态反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAxisStatus(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴地址
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">轴地址设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAxisAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetAxisAddress(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴地址
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">轴地址反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAxisAddress(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置总线轴使能
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">使能状态设定值 0-关闭 1- 打开</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetAxisEnable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetAxisEnable(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴使能状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">轴使能状态反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisEnable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAxisEnable(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置同步运动链接速率
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">同步连接速率设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetClutchRate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetClutchRate(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取同步运动链接速率
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">同步连接速率反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetClutchRate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetClutchRate(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置锁存触发的结束坐标范围点
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">范围设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetCloseWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetCloseWin(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取锁存触发的结束坐标范围点
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">范围反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetCloseWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetCloseWin(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置拐角减速模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">拐角减速模式设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetCornerMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetCornerMode(IntPtr handle, int iaxis, int pfValue);

        /// <summary>
        /// 读取拐角减速模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">拐角模式反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetCornerMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetCornerMode(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置回零爬行速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">回零慢速设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetCreep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetCreep(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取回零爬行速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">爬行速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetCreep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetCreep(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴原点信号输入口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">原点信号输入口编号设定值 -1为无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDatumIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetDatumIn(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴原点信号输入口编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">输入口编号返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDatumIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetDatumIn(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴减速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">减速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDecel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetDecel(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴减速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">减速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDecel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetDecel(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置拐角减速 起始减速角度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">起始减速角度设定值 弧度制</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDecelAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetDecelAngle(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取拐角减速 起始减速角度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">起始减速角度反馈值 弧度制</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDecelAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetDecelAngle(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴命令位置坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">命令位置设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetDpos(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴命令位置坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">命令位置反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetDpos(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴内部编码器值  （总线绝对值伺服时为绝对值位置）
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">编码器位置返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEncoder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetEncoder(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴当前运动的最终位置
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">返回的最终位置坐标</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEndMove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetEndMove(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取当前和缓冲中运动的最终位置，可以用于相对绝对转换
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">最终位置坐标反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEndMoveBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetEndMoveBuffer(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置SP运动的结束速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">结束速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetEndMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetEndMoveSpeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取SP运动的结束速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">结束速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetEndMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetEndMoveSpeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置错误标记，和AXISSTATUS做与运算来决定哪些错误需要关闭WDOG。
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">标志设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetErrormask", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetErrormask(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取错误标记，和AXISSTATUS做与运算来决定哪些错误需要关闭WDOG。
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">错误标志返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetErrormask", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetErrormask(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置快速JOG输入口编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">快速JOG输入点编号设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFastJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFastJog(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取快速JOG输入口编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">JOG输入口编号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFastJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFastJog(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴异常快速减速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">异常减速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFastDec", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFastDec(IntPtr handle, int iaxis, float iValue);

        /// <summary>
        /// 读取轴异常快速减速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">异常减速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFastDec", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFastDec(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴随动误差
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">轴随动误差反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFe(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴最大允许的随动误差值
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">随动误差设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFeLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFeLimit(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴最大允许的随动误差值
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">允许随动误差反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFeLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFeLimit(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴报警时随动误差值
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">随动误差设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFRange", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFRange(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴报警时的随动误差值
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">报警随动误差反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFeRange", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFeRange(IntPtr handle, int iaxis, ref float fValue);

        /// <summary>
        /// 设置轴保持输入口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">保持输入口编号设定值 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFholdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFholdIn(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴保持输入口编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">输入口编号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFholdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFholdIn(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴保持速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">保持速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFhspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFhspeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴保持速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">保持速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFhspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFhspeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴SP运动的运行速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">运行速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetForceSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetForceSpeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴SP运动的运行速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">运行速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetForceSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetForceSpeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴正向软限位
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">限位设定值 取消时设置一个较大值即可</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFsLimit(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴正向软限位
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">限位反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFsLimit(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴小圆限速最小半径
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">最小半径设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFullSpRadius", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFullSpRadius(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴小圆限速最小半径
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">最小半径反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFullSpRadius", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFullSpRadius(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴正向硬限位输入口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">硬限位输入口编号设定值 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFwdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFwdIn(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴正向硬限位输入口编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">硬限位输入口编号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFwdIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFwdIn(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴正向JOG输入口
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">JOG输入口编号 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetFwdJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetFwdJog(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴正向JOG输入口编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">JOG输入口反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetFwdJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetFwdJog(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴运动完成状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">运动状态反馈值 0-运动中 -1 停止</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetIfIdle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetIfIdle(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴脉冲输出模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">脉冲模式设定值 0-3脉冲+方向 4-7双脉冲</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetInvertStep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetInvertStep(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴脉冲输出模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">脉冲模式反馈值 0-3脉冲+方向 4-7双脉冲</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInvertStep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetInvertStep(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴插补运动时是否参与速度计算 缺省参与（1）。此参数只对直线和螺旋的第三个轴起作用
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">模式设定值 0-不参数 1-参与</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetInterpFactor", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetInterpFactor(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴插补运动时是否参与速度计算 缺省参与（1）。此参数只对直线和螺旋的第三个轴起作用
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">模式反馈值 0-不参数 1-参与</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInterpFactor", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetInterpFactor(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴JOG运动时速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetJogSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetJogSpeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴JOG运动时速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetJogSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetJogSpeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴链接运动的参考轴号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">参考主轴轴号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetLinkax", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetLinkax(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴当前除了当前运动是否还有缓冲运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">状态反馈值  -1 没有剩余函数 0-还有剩余运动</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetLoaded", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetLoaded(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴起始速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">起始速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetLspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetLspeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴起始速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">起始速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetLspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetLspeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴回零反找等待时间
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">反转等待时间设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetHomeWait", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetHomeWait(IntPtr handle, int iaxis, int fValue);

        /// <summary>
        /// 读取轴回零反找等待时间
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">反找等待时间反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetHomeWait", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetHomeWait(IntPtr handle, int iaxis, ref int pfValue);

        /// <summary>
        /// 读取编码轴锁存触发MAKR状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">锁存触发状态反馈值 -1-锁存触发 0-未触发</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMark", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMark(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取编码轴锁存触发MAKRb状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">锁存触发状态反馈值 -1-锁存触发 0-未触发</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMarkB", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMarkB(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴脉冲输出最高频率
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">最高脉冲频率设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMaxSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetMaxSpeed(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴脉冲输出最高频率
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">最高脉冲频率反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMaxSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMaxSpeed(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴连续插补模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">连续插补开关设定值 0-关闭连续插补 1-打开连续插补</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMerge", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetMerge(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴连续插补模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">连续插补开关反馈值 0-关闭连续插补 1-打开连续插补</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMerge", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMerge(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴当前被缓冲起来的运动个数
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">运动缓冲数量反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMovesBuffered", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMovesBuffered(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴当前正在运动指令的MOVE_MARK标号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">当前MARK标号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMoveCurmark", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMoveCurmark(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴运动MOVE_MARK标号 每当有运动进入轴运动缓冲时MARK自动+1
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">MARK设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMovemark", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetMovemark(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 设置编码轴反馈位置坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">反馈位置设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetMpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetMpos(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取编码轴反馈位置坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">反馈位置坐标反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMpos(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取编码轴反馈速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">轴反馈速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMspeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMspeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴当前运动指令类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">运动类型反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetMtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetMtype(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴正在运动指令后面的第一条指令类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">运动类型反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetNtype", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetNtype(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴坐标偏移
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">偏移设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOffpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetOffpos(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴坐标偏移
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">坐标偏移反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOffpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetOffpos(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置编码轴锁存触发的结束坐标范围点
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">结束坐标范围设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOpenWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetOpenWin(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取编码轴锁存触发的结束坐标范围点
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">结束坐标反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOpenWin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetOpenWin(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取编码轴锁存MAKR触发的反馈位置坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">锁存位置坐标反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRegPos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRegPos(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取编码轴锁存MAKRB触发的反馈位置坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">锁存位置坐标反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRegPosB", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRegPosB(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴当前运动还未完成的距离
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">未完成距离反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRemain", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRemain(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴剩余的直线缓冲
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">剩余执行缓冲反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRemain_LineBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRemain_LineBuffer(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取轴剩余运动缓冲 按最复杂的空间圆弧来计算
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">剩余缓冲反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRemain_Buffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRemain_Buffer(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴坐标循环范围值 根据REP_OPTION设置来自动循环轴DPOS和MPOS坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">坐标循环范围设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRepDist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetRepDist(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴坐标循环范围值 根据REP_OPTION设置来自动循环轴DPOS和MPOS坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">坐标循环范围反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRepDist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRepDist(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴坐标循环模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">循环模式设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRepOption", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetRepOption(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴坐标循环模式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">循环模式反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRepOption", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRepOption(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴负向硬件限位开关对应的输入点编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">负限位输入口编号设定值 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRevIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetRevIn(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴负向硬件限位开关对应的输入点编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">负限位输入口编号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRevIn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRevIn(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴负向JOG输入对应的输入点编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iValue">负向JOG输入口编号设定值 -1表示无效</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRevJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetRevJog(IntPtr handle, int iaxis, int iValue);

        /// <summary>
        /// 读取轴负向JOG输入对应的输入点编号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">负向JOG输入口编号反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRevJog", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRevJog(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 设置轴负向软限位坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">负向软限位设定值 设置一个较大的值时认为取消限位</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetRsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetRsLimit(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴负向软限位坐标
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">负向软限位反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetRsLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetRsLimit(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴速度，单位为units/s，当多轴运动时，作为插补运动的速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetSpeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetSpeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴S曲线加减速。 0-梯形加减速
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">S曲线平滑时间设定值 MS</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetSramp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetSramp(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴S曲线加减速
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">S曲线平滑时间反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetSramp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetSramp(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴SP运动的起始速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">SP运动起始速度设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetStartMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetStartMoveSpeed(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴SP运动的起始速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">SP运动起始速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetStartMoveSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetStartMoveSpeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴拐角减速 停止减速角度 
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">停止角度设定值 弧度制</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetStopAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetStopAngle(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴拐角减速 停止减速角度 
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">停止角度反馈值 弧度制</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetStopAngle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetStopAngle(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴拐角减速 倒角半径
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">倒角半径设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetZsmooth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetZsmooth(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴拐角减速 倒角半径
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">倒角半径反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetZsmooth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetZsmooth(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 设置轴脉冲当量
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">脉冲当量设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetUnits", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetUnits(IntPtr handle, int iaxis, float fValue);

        /// <summary>
        /// 读取轴脉冲当量
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">脉冲当量反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetUnits", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetUnits(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴当前运动和缓冲运动还未完成的距离
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">剩余距离反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVectorBuffered", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetVectorBuffered(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 读取轴当前运行的规划速度
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfValue">运行速度反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVpSpeed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetVpSpeed(IntPtr handle, int iaxis, ref float pfValue);

        /// <summary>
        /// 全局变量/参数浮点类型读取命令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="pname">全局变量字符串名称/或者指定轴号的轴参数名称DPOS(0)</param>
        /// <param name="pfValue">参数反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVariablef", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetVariablef(IntPtr handle, string pname, ref float pfValue);

        /// <summary>
        /// 全局变量/参数整数类型读取命令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="pname">全局变量字符串名称/或者指定轴号的轴参数名称DPOS(0)</param>
        /// <param name="piValue">参数反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVariableInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetVariableInt(IntPtr handle, string pname, ref int piValue);

        /// <summary>
        /// 选定运动轴列表 BASE的一个轴作为插补主轴
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Base", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Base(IntPtr handle, int imaxaxises, int[] piAxislist);

        /// <summary>
        /// 设置轴当前坐标，不建议使用，可以直接调用SETDPOS达到同样效果
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfDpos">坐标位置设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Defpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Defpos(IntPtr handle, int iaxis, float pfDpos);

        /// <summary>
        /// 多轴相对直线插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDposlist">运动距离列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Move(IntPtr handle, int imaxaxises, int[] piAxislist, float[] pfDposlist);

        /// <summary>
        /// 多轴相对直线插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDposlist">运动距离列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSp(IntPtr handle, int imaxaxises, int[] piAxislist, float[] pfDposlist);

        /// <summary>
        /// 多轴绝对直线插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDposlist">运动位置列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveAbs(IntPtr handle, int imaxaxises, int[] piAxislist, float[] pfDposlist);

        /// <summary>
        /// 多轴绝对直线插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDposlist">运动位置列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveAbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float[] pfDposlist);

        /// <summary>
        /// 运动中修改结束位置，单轴指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfDisance">运动位置</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveModify", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveModify(IntPtr handle, int iaxis, float pfDisance);

        /// <summary>
        /// 运动中修改结束位置，单轴指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="pfDisance">运动位置</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveModify2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveModify2(IntPtr handle, int imaxaxises, int[] iaxis, float[] pfDisance);

        /// <summary>
        /// 相对圆心定圆弧插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点坐标 相对与起始点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对与起始点</param>
        /// <param name="fcenter1">第一个轴圆心坐标，相对与起始点</param>
        /// <param name="fcenter2">第二个轴圆心坐标，相对与起始点</param>
        /// <param name="idirection">圆弧方向 0-逆时针，1-顺时针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCirc(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);

        /// <summary>
        /// 相对圆心定圆弧插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点坐标 相对与起始点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对与起始点</param>
        /// <param name="fcenter1">第一个轴圆心坐标，相对与起始点</param>
        /// <param name="fcenter2">第二个轴圆心坐标，相对与起始点</param>
        /// <param name="idirection">圆弧方向 0-逆时针，1-顺时针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCircSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCircSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);

        /// <summary>
        /// 绝对圆心圆弧插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <param name="fcenter1">第一个轴圆心绝对坐标</param>
        /// <param name="fcenter2">第二个轴圆心绝对坐标</param>
        /// <param name="idirection">圆弧方向 0-逆时针，1-顺时针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCircAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCircAbs(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);

        /// <summary>
        /// 绝对圆心圆弧插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <param name="fcenter1">第一个轴圆心绝对坐标</param>
        /// <param name="fcenter2">第二个轴圆心绝对坐标</param>
        /// <param name="idirection">圆弧方向 0-逆时针，1-顺时针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCircAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCircAbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection);

        /// <summary>
        /// 相对3点定圆弧插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fmid2">第二个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fend1">第一个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对于圆弧起点</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCirc2(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2);

        /// <summary>
        /// 绝对3点定圆弧插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点绝对坐标</param>
        /// <param name="fmid2">第二个轴中间点绝对坐标</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2Abs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCirc2Abs(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2);

        /// <summary>
        /// 相对3点定圆弧插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fmid2">第二个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fend1">第一个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对于圆弧起点</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2Sp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCirc2Sp(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2);

        /// <summary>
        /// 绝对3点定圆弧插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点绝对坐标</param>
        /// <param name="fmid2">第二个轴中间点绝对坐标</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCirc2AbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCirc2AbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2);

        /// <summary>
        /// 相对3轴圆心螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fcenter1">第一个轴圆心坐标 相对于圆弧起点</param>
        /// <param name="fcenter2">第二个轴圆心坐标 相对于圆弧起点</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fDistance3">第三螺旋轴运动距离</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelical(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

        /// <summary>
        /// 绝对3轴圆心螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <param name="fcenter1">第一个轴圆心绝对坐标</param>
        /// <param name="fcenter2">第二个轴圆心绝对坐标</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fDistance3">第三螺旋轴结束点绝对坐标</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelicalAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelicalAbs(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

        /// <summary>
        /// 相对3轴圆心螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fcenter1">第一个轴圆心坐标 相对于圆弧起点</param>
        /// <param name="fcenter2">第二个轴圆心坐标 相对于圆弧起点</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fDistance3">第三螺旋轴运动距离</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelicalSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelicalSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

        /// <summary>
        /// 绝对3轴圆心螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <param name="fcenter1">第一个轴圆心绝对坐标</param>
        /// <param name="fcenter2">第二个轴圆心绝对坐标</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fDistance3">第三螺旋轴结束点绝对坐标</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelicalAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelicalAbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fDistance3, int imode);

        /// <summary>
        /// 相对3轴 3点定螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fmid2">第二个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fend1">第一个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fDistance3">第三个轴运动相对距离</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelical2(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

        /// <summary>
        /// 绝对3轴 3点定螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点绝对坐标</param>
        /// <param name="fmid2">第二个轴中间点绝对坐标</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <param name="fDistance3">第三个轴运动结束点绝对坐标</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2Abs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelical2Abs(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

        /// <summary>
        /// 相对3轴 3点定螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fmid2">第二个轴中间点坐标 相对于圆弧起点</param>
        /// <param name="fend1">第一个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fend2">第二个轴结束点坐标 相对于圆弧起点</param>
        /// <param name="fDistance3">第三个轴运动相对距离</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2Sp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelical2Sp(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

        /// <summary>
        /// 绝对3轴 3点定螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fmid1">第一个轴中间点绝对坐标</param>
        /// <param name="fmid2">第二个轴中间点绝对坐标</param>
        /// <param name="fend1">第一个轴结束点绝对坐标</param>
        /// <param name="fend2">第二个轴结束点绝对坐标</param>
        /// <param name="fDistance3">第三个轴运动结束点绝对坐标</param>
        /// <param name="imode">第三轴的速度计算:0(缺省)第三轴参与速度计算。1第三轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MHelical2AbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MHelical2AbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fmid1, float fmid2, float fend1, float fend2, float fDistance3, int imode);

        /// <summary>
        /// 相对椭圆插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴运动坐标，相对于起始点</param>
        /// <param name="fend2">终点第二个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter1">中心第一个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter2">中心第二个轴运动坐标，相对于起始点</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipse", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipse(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

        /// <summary>
        /// 绝对椭圆插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴绝对坐标</param>
        /// <param name="fend2">终点第二个轴绝对坐标</param>
        /// <param name="fcenter1">中心第一个轴绝对坐标</param>
        /// <param name="fcenter2">中心第二个轴绝对坐标</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseAbs(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

        /// <summary>
        /// 相对椭圆插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴运动坐标，相对于起始点</param>
        /// <param name="fend2">终点第二个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter1">中心第一个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter2">中心第二个轴运动坐标，相对于起始点</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

        /// <summary>
        /// 绝对椭圆插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴绝对坐标</param>
        /// <param name="fend2">终点第二个轴绝对坐标</param>
        /// <param name="fcenter1">中心第一个轴绝对坐标</param>
        /// <param name="fcenter2">中心第二个轴绝对坐标</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseAbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis);

        /// <summary>
        /// 相对 椭圆 + 螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴运动坐标，相对于起始点</param>
        /// <param name="fend2">终点第二个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter1">中心第一个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter2">中心第二个轴运动坐标，相对于起始点</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <param name="fDistance3">第三个轴的运动相对距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseHelical(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);

        /// <summary>
        /// 绝对椭圆 + 螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴绝对坐标</param>
        /// <param name="fend2">终点第二个轴绝对坐标</param>
        /// <param name="fcenter1">中心第一个轴绝对坐标</param>
        /// <param name="fcenter2">中心第二个轴绝对坐标</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <param name="fDistance3">第三个轴的终点绝对坐标</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelicalAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseHelicalAbs(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);

        /// <summary>
        /// 相对 椭圆 + 螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴运动坐标，相对于起始点</param>
        /// <param name="fend2">终点第二个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter1">中心第一个轴运动坐标，相对于起始点</param>
        /// <param name="fcenter2">中心第二个轴运动坐标，相对于起始点</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <param name="fDistance3">第三个轴的运动相对距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelicalSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseHelicalSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);

        /// <summary>
        /// 绝对椭圆 + 螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">终点第一个轴绝对坐标</param>
        /// <param name="fend2">终点第二个轴绝对坐标</param>
        /// <param name="fcenter1">中心第一个轴绝对坐标</param>
        /// <param name="fcenter2">中心第二个轴绝对坐标</param>
        /// <param name="idirection">运动方向 0-逆时针，1-顺时针</param>
        /// <param name="fADis">第一轴的椭圆半径，半长轴或者半短轴都可</param>
        /// <param name="fBDis">第二轴的椭圆半径，半长轴或者半短轴都可，AB相等时自动为圆弧或螺旋</param>
        /// <param name="fDistance3">第三个轴的终点绝对坐标</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MEclipseHelicalAbsSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MEclipseHelicalAbsSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fcenter1, float fcenter2, int idirection, float fADis, float fBDis, float fDistance3);

        /// <summary>
        /// 空间圆弧 + 螺旋插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第1个轴运动终点距离	相对与起点</param>
        /// <param name="fend2">第2个轴运动终点距离	相对与起点</param>
        /// <param name="fend3">第3个轴运动终点距离	相对与起点</param>
        /// <param name="fcenter1">第1个轴中间点/圆心 运动距离参数	相对与起点</param>
        /// <param name="fcenter2">第2个轴中间点/圆心 运动距离参数	相对与起点</param>
        /// <param name="fcenter3">第3个轴中间点/圆心 运动距离参数	相对与起点</param>
        /// <param name="imode">运动模式 0-三点定圆弧 1-圆心定最小的圆弧 2-3点整圆运动 3-圆心画整圆运动</param>
        /// <param name="fcenter4">第4个轴螺旋运动相对距离</param>
        /// <param name="fcenter5">第5个轴螺旋运动相对距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MSpherical", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MSpherical(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fend3, float fcenter1, float fcenter2, float fcenter3, int imode, float fcenter4, float fcenter5);

        /// <summary>
        /// 空间圆弧 + 螺旋插补SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="fend1">第1个轴运动终点距离	相对与起点</param>
        /// <param name="fend2">第2个轴运动终点距离	相对与起点</param>
        /// <param name="fend3">第3个轴运动终点距离	相对与起点</param>
        /// <param name="fcenter1">第1个轴中间点/圆心 运动距离参数	相对与起点</param>
        /// <param name="fcenter2">第2个轴中间点/圆心 运动距离参数	相对与起点</param>
        /// <param name="fcenter3">第3个轴中间点/圆心 运动距离参数	相对与起点</param>
        /// <param name="imode">运动模式 0-三点定圆弧 1-圆心定最小的圆弧 2-3点整圆运动 3-圆心画整圆运动</param>
        /// <param name="fcenter4">第4个轴螺旋运动相对距离</param>
        /// <param name="fcenter5">第5个轴螺旋运动相对距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MSphericalSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MSphericalSp(IntPtr handle, int imaxaxises, int[] piAxislist, float fend1, float fend2, float fend3, float fcenter1, float fcenter2, float fcenter3, int imode, float fcenter4, float fcenter5);

        /// <summary>
        /// 相对渐开线圆弧插补运动，当起始半径0直接扩散时从0角度开始
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="centre1">第1轴圆心的相对距离</param>
        /// <param name="centre2">第2轴圆心的相对距离</param>
        /// <param name="circles">要旋转的圈数，可以为小数圈，负数表示顺时针.</param>
        /// <param name="pitch">每圈的扩散距离，可以为负。</param>
        /// <param name="distance3">第3轴螺旋的功能，指定第3轴的相对距离，此轴不参与速度计算</param>
        /// <param name="distance4">第4轴螺旋的功能，指定第4轴的相对距离，此轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSpiral", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSpiral(IntPtr handle, int imaxaxises, int[] piAxislist, float centre1, float centre2, float circles, float pitch, float distance3, float distance4);

        /// <summary>
        /// 相对渐开线圆弧插补SP运动，当起始半径0直接扩散时从0角度开始
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="centre1">第1轴圆心的相对距离</param>
        /// <param name="centre2">第2轴圆心的相对距离</param>
        /// <param name="circles">要旋转的圈数，可以为小数圈，负数表示顺时针.</param>
        /// <param name="pitch">每圈的扩散距离，可以为负。</param>
        /// <param name="distance3">第3轴螺旋的功能，指定第3轴的相对距离，此轴不参与速度计算</param>
        /// <param name="distance4">第4轴螺旋的功能，指定第4轴的相对距离，此轴不参与速度计算</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSpiralSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSpiralSp(IntPtr handle, int imaxaxises, int[] piAxislist, float centre1, float centre2, float circles, float pitch, float distance3, float distance4);

        /// <summary>
        /// 空间直线运动，根据下一个直线运动的绝对坐标在拐角自动插入圆弧，加入圆弧后会使得运动的终点与直线的终点不一致，拐角过大时不会插入圆弧，当距离不够时会自动减小半径
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="end1">第1个轴运动绝对坐标</param>
        /// <param name="end2">第2个轴运动绝对坐标</param>
        /// <param name="end3">第3个轴运动绝对坐标</param>
        /// <param name="next1">第1个轴下一个直线运动绝对坐标</param>
        /// <param name="next2">第2个轴下一个直线运动绝对坐标</param>
        /// <param name="next3">第3个轴下一个直线运动绝对坐标</param>
        /// <param name="radius">插入圆弧的半径，当过大的时候自动缩小。</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSmooth", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSmooth(IntPtr handle, int imaxaxises, int[] piAxislist, float end1, float end2, float end3, float next1, float next2, float next3, float radius);

        /// <summary>
        /// 空间直线SP运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="end1">第1个轴运动绝对坐标</param>
        /// <param name="end2">第2个轴运动绝对坐标</param>
        /// <param name="end3">第3个轴运动绝对坐标</param>
        /// <param name="next1">第1个轴下一个直线运动绝对坐标</param>
        /// <param name="next2">第2个轴下一个直线运动绝对坐标</param>
        /// <param name="next3">第3个轴下一个直线运动绝对坐标</param>
        /// <param name="radius">插入圆弧的半径，当过大的时候自动缩小。</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSmoothSp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSmoothSp(IntPtr handle, int imaxaxises, int[] piAxislist, float end1, float end2, float end3, float next1, float next2, float next3, float radius);

        /// <summary>
        /// 运动暂停
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">模式 0（缺省） 暂停当前运动 1 在当前运动完成后正准备执行下一条运动指令时暂停 2 在当前运动完成后正准备执行下一条运动指令时，并且两条指令的MARK标识不一样时暂停</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MovePause", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MovePause(IntPtr handle, int iaxis, int imode);

        /// <summary>
        /// 恢复暂停运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveResume", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveResume(IntPtr handle, int iaxis);

        /// <summary>
        /// 运动末尾位置增加速度限制，用于强制拐角减速
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="limitspeed">限制速度值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveLimit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveLimit(IntPtr handle, int iaxis, float limitspeed);

        /// <summary>
        /// 运动缓冲中插入输出指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="ioutnum">输出口编号</param>
        /// <param name="ivalue">输出口状态 0/1</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveOp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveOp(IntPtr handle, int iaxis, int ioutnum, int ivalue);

        /// <summary>
        /// 运动缓冲中插入连续输出口指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="ioutnumfirst">输出口起始编号</param>
        /// <param name="ioutnumend">输出口结束编号</param>
        /// <param name="ivalue">输出状态值 按位存储</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveOpMulti", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveOpMulti(IntPtr handle, int iaxis, int ioutnumfirst, int ioutnumend, int ivalue);

        /// <summary>
        /// 运动缓冲中插入输出脉冲波指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="ioutnum">输出口编号</param>
        /// <param name="ivalue">输出口状态 0/1</param>
        /// <param name="iofftimems">状态反转时间</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveOp2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveOp2(IntPtr handle, int iaxis, int ioutnum, int ivalue, int iofftimems);

        /// <summary>
        /// 运动缓冲中插入模拟量输出指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="ioutnum">模拟量输出口编号</param>
        /// <param name="fvalue">模拟量值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveAout", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveAout(IntPtr handle, int iaxis, int ioutnum, float fvalue);

        /// <summary>
        /// 运动缓冲中插入延时指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="itimems">延时时间MS</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveDelay", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveDelay(IntPtr handle, int iaxis, int itimems);

        /// <summary>
        /// 旋转台直线插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="tablenum">存储旋转台参数的table编号</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="pfDisancelist">距离列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveTurnabs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveTurnabs(IntPtr handle, int tablenum, int imaxaxises, int[] piAxislist, float[] pfDisancelist);

        /// <summary>
        /// 旋转台圆弧+螺插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="tablenum">存储旋转参数的table编号</param>
        /// <param name="refpos1">第一个轴参考点，绝对位置</param>
        /// <param name="refpos2">第二个轴参考点，绝对位置</param>
        /// <param name="mode">1-参考点是当前点前面，2-参考点是结束点后面，3-参考点在中间，采用三点定圆的方式</param>
        /// <param name="end1">第一个轴结束点，绝对位置</param>
        /// <param name="end2">第二个轴结束点，绝对位置</param>
        /// <param name="imaxaxises">参与运动轴数量</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDisancelist">螺旋轴距离列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_McircTurnabs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_McircTurnabs(IntPtr handle, int tablenum, float refpos1, float refpos2, int mode, float end1, float end2, int imaxaxises, int[] piAxislist, float[] pfDisancelist);

        /// <summary>
        /// 电子凸轮 同步运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="istartpoint">起始点TABLE编号</param>
        /// <param name="iendpoint">结束点TABLE编号</param>
        /// <param name="ftablemulti">位置比例，一般设为脉冲当量值</param>
        /// <param name="fDistance">参考运动的距离，用来计算总运动时间</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Cam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Cam(IntPtr handle, int iaxis, int istartpoint, int iendpoint, float ftablemulti, float fDistance);

        /// <summary>
        /// 电子凸轮 同步运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="istartpoint">起始点TABLE编号</param>
        /// <param name="iendpoint">结束点TABLE编号</param>
        /// <param name="ftablemulti">位置比例，一般设为脉冲当量值</param>
        /// <param name="fDistance">参考运动的距离</param>
        /// <param name="ilinkaxis">参考主轴轴号</param>
        /// <param name="ioption">参考轴的连接方式</param>
        /// <param name="flinkstartpos">ioption条件中距离参数</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Cambox", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Cambox(IntPtr handle, int iaxis, int istartpoint, int iendpoint, float ftablemulti, float fDistance, int ilinkaxis, int ioption, float flinkstartpos);

        /// <summary>
        /// 飞剪追剪MOVELINK 同步运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">参与运动的轴号(跟随轴)</param>
        /// <param name="fDistance">同步过程跟随轴运动距离</param>
        /// <param name="fLinkDis">同步过程参考轴(主轴)运动绝对距离</param>
        /// <param name="fLinkAcc">跟随轴加速阶段，参考轴移动的绝对距离</param>
        /// <param name="fLinkDec">跟随轴减速阶段，参考轴移动的绝对距离</param>
        /// <param name="iLinkaxis">参考轴的轴号</param>
        /// <param name="ioption">连接模式选项</param>
        /// <param name="flinkstartpos">连接模式选项中运动距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Movelink", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Movelink(IntPtr handle, int iaxis, float fDistance, float fLinkDis, float fLinkAcc, float fLinkDec, int iLinkaxis, int ioption, float flinkstartpos);

        /// <summary>
        /// 飞剪追剪MOVESLINK 同步运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">参与运动的轴号(跟随轴)</param>
        /// <param name="fDistance">同步过程跟随轴运动距离</param>
        /// <param name="fLinkDis">同步过程参考轴(主轴)运动绝对距离</param>
        /// <param name="startsp">启动时跟随轴和参考轴的速度比例，units/units单位，负数表示跟随轴负向运动</param>
        /// <param name="endsp">结束时跟随轴和参考轴的速度比例，units/units单位, 负数表示跟随轴负向运动</param>
        /// <param name="iLinkaxis">参考轴的轴号</param>
        /// <param name="ioption">连接模式选项</param>
        /// <param name="flinkstartpos">连接模式选项中运动距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Moveslink", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Moveslink(IntPtr handle, int iaxis, float fDistance, float fLinkDis, float startsp, float endsp, int iLinkaxis, int ioption, float flinkstartpos);

        /// <summary>
        /// 电子齿轮 同步运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ratio">比率，可正可负，注意是脉冲个数的比例</param>
        /// <param name="link_axis">连接轴的轴号，手轮时为编码器轴</param>
        /// <param name="move_axis">随动轴号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Connect(IntPtr handle, float ratio, int link_axis, int move_axis);

        /// <summary>
        /// 电子齿轮矢量同步 同步运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ratio">比例，注意是脉冲个数的比例</param>
        /// <param name="link_axis">连接轴的轴号，手轮时为编码器轴</param>
        /// <param name="move_axis">随动轴号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connpath", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Connpath(IntPtr handle, float ratio, int link_axis, int move_axis);

        /// <summary>
        /// 位置锁存指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">锁存模式</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Regist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Regist(IntPtr handle, int iaxis, int imode);

        /// <summary>
        /// 设置编码轴输出齿轮比 缺省(1,1)
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="mpos_count">分子，不要超过65535</param>
        /// <param name="input_count">分母，不要超过65535</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_EncoderRatio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_EncoderRatio(IntPtr handle, int iaxis, int mpos_count, int input_count);

        /// <summary>
        /// 设置脉冲输出齿轮比 ，缺省(1,1)
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="mpos_count">分子，1-65535</param>
        /// <param name="input_count">分母，1-65535</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_StepRatio", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_StepRatio(IntPtr handle, int iaxis, int mpos_count, int input_count);

        /// <summary>
        /// 停止所有轴运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imode">停止模式  0（缺省）取消当前运动 1-取消缓冲的运动 2-取消当前运动和缓冲运动 3-立即中断脉冲发送 </param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Rapidstop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Rapidstop(IntPtr handle, int imode);

        /// <summary>
        /// 多轴运动停止
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxises">轴数量</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="imode">停止模式  0（缺省）取消当前运动 1-取消缓冲的运动 2-取消当前运动和缓冲运动 3-立即中断脉冲发送</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_CancelAxisList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_CancelAxisList(IntPtr handle, int imaxaxises, int[] piAxislist, int imode);

        /// <summary>
        /// 机械手逆解映射指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Jogmaxaxises">关节轴数量</param>
        /// <param name="JogAxislist">关节轴列表</param>
        /// <param name="frame">机械手类型</param>
        /// <param name="tablenum">机械手参数TABLE起始编号</param>
        /// <param name="Virmaxaxises">关联虚拟轴个数</param>
        /// <param name="VirAxislist">虚拟轴列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connframe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Connframe(IntPtr handle, int Jogmaxaxises, int[] JogAxislist, int frame, int tablenum, int Virmaxaxises, int[] VirAxislist);

        /// <summary>
        /// 机械手正解映射指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Virmaxaxises">关联虚拟轴个数</param>
        /// <param name="VirAxislist">虚拟轴列表</param>
        /// <param name="frame">机械手类型</param>
        /// <param name="tablenum">机械手参数TABLE起始编号</param>
        /// <param name="Jogmaxaxises">关节轴数量</param>
        /// <param name="JogAxislist">关节轴列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Connreframe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Connreframe(IntPtr handle, int Virmaxaxises, int[] VirAxislist, int frame, int tablenum, int Jogmaxaxises, int[] JogAxislist);

        /// <summary>
        /// 轴叠加运动指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">被叠加轴</param>
        /// <param name="iaddaxis">叠加轴</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Single_Addax", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Single_Addax(IntPtr handle, int iaxis, int iaddaxis);

        /// <summary>
        /// 单轴运动停止
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">停止模式  0（缺省）取消当前运动 1-取消缓冲的运动 2-取消当前运动和缓冲运动 3-立即中断脉冲发送</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Single_Cancel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Single_Cancel(IntPtr handle, int iaxis, int imode);

        /// <summary>
        /// 单轴连续运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="idir">运动方向 1正向 -1负向</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Single_Vmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Single_Vmove(IntPtr handle, int iaxis, int idir);

        /// <summary>
        /// 控制器方式回零
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">回零模式</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Single_Datum", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Single_Datum(IntPtr handle, int iaxis, int imode);

        /// <summary>
        /// 控制器方式回零
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">回零模式</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetHomeStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetHomeStatus(IntPtr handle, int iaxis, ref UInt32 homestatus);

        /// <summary>
        /// 单轴相对运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fdistance">相对运动距离</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Single_Move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Single_Move(IntPtr handle, int iaxis, float fdistance);

        /// <summary>
        /// 单轴绝对运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fdistance">运动绝对位置坐标</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Single_MoveAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Single_MoveAbs(IntPtr handle, int iaxis, float fdistance);

        /// <summary>
        /// 设置VR寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="vrstartnum">VR起始编号</param>
        /// <param name="numes">写入的数量</param>
        /// <param name="pfValue">写入的数据列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetVrf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetVrf(IntPtr handle, int vrstartnum, int numes, float[] pfValue);

        /// <summary>
        /// 读取VR寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="vrstartnum">读取的VR起始地址</param>
        /// <param name="numes">读取的数量</param>
        /// <param name="pfValue">VR反馈值数据</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVrf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetVrf(IntPtr handle, int vrstartnum, int numes, float[] pfValue);

        /// <summary>
        /// 读取VR_INT寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="vrstartnum">读取的VR起始地址</param>
        /// <param name="numes">读取的数量</param>
        /// <param name="piValue">VRINT反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetVrInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetVrInt(IntPtr handle, int vrstartnum, int numes, int[] piValue);

        /// <summary>
        /// 设置TABLE寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="tabstart">写入的TABLE起始编号</param>
        /// <param name="numes">写入的数量</param>
        /// <param name="pfValue">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetTable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetTable(IntPtr handle, int vrstartnum, int numes, float[] pfValue);

        /// <summary>
        /// 读取TABLE寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="tabstart">读取TABLE起始地址</param>
        /// <param name="numes">读取的数量</param>
        /// <param name="pfValue">读取的反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetTable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetTable(IntPtr handle, int tabstart, int numes, float[] pfValue);

        /// <summary>
        /// 字符串转为float数据
        /// </summary>
        /// <param name="pstringin">数据的字符串</param>
        /// <param name="inumes">转换数据个数</param>
        /// <param name="pfvlaue">转换的数据反馈</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_TransStringtoFloat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_TransStringtoFloat(string pstringin, int inumes, float[] pfValue);

        /// <summary>
        /// 字符串转为INT数据
        /// </summary>
        /// <param name="pstringin">数据的字符串</param>
        /// <param name="inumes">转换数据个数</param>
        /// <param name="pfvlaue">转换的数据反馈</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_TransStringtoInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_TransStringtoInt(string pstringin, int inumes, int[] pfValue);

        /// <summary>
        /// 把float格式的变量列表存储到文件， 与控制器的U盘文件格式一致
        /// </summary>
        /// <param name="sFilename">文件绝对路径</param>
        /// <param name="pVarlist">写入的数据列表</param>
        /// <param name="inum">数据的长度</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_WriteUFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_WriteUFile(string sFilename, float[] pVarlist, int inum);

        /// <summary>
        /// 读取float格式的变量列表， 与控制器的U盘文件格式一致.
        /// </summary>
        /// <param name="sFilename">文件绝对路径</param>
        /// <param name="pVarlist">读取的数据列表</param>
        /// <param name="pinum">读取数据的长度</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_ReadUFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_ReadUFile(string sFilename, float[] pVarlist, ref int inum);

        /// <summary>
        /// 设置MODBUS_BIT位寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pdata">设置的位状态 按位存储</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set0x", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Set0x(IntPtr handle, UInt16 start, UInt16 inum, byte[] pdata);

        /// <summary>
        /// 读取MOBUS_BIT位寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pdata">pdata 返回的位状态  按位存储</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get0x", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Get0x(IntPtr handle, UInt16 start, UInt16 inum, byte[] pdata);

        /// <summary>
        /// 设置MODBUS_REG字寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pdata">REG寄存器值设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set4x", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Set4x(IntPtr handle, UInt16 start, UInt16 inum, UInt16[] pfdata);

        /// <summary>
        /// 读取MODBUS_REG字寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pdata">反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get4x", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Get4x(IntPtr handle, UInt16 start, UInt16 inum, UInt16[] pfdata);

        /// <summary>
        /// 读取MODBUS_IEEE寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pfdata">反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get4x_Float", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Get4x_Float(IntPtr handle, UInt16 start, UInt16 inum, float[] pfdata);

        /// <summary>
        /// 设置MODBUS_IEEE寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pfdata">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set4x_Float", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Set4x_Float(IntPtr handle, UInt16 start, UInt16 inum, float[] pfdata);

        /// <summary>
        /// 读取MODBUS_LONG寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pfdata">反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get4x_Long", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Get4x_Long(IntPtr handle, UInt16 start, UInt16 inum, Int32[] pfdata);

        /// <summary>
        /// 设置MODBUS_LONG寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">数量</param>
        /// <param name="pfdata">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set4x_Long", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Set4x_Long(IntPtr handle, UInt16 start, UInt16 inum, Int32[] pfdata);

        /// <summary>
        /// 读取MODBUS_STRING寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">字节数量</param>
        /// <param name="pidata">反馈字符串</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Get4x_String", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Get4x_String(IntPtr handle, UInt16 start, UInt16 inum, StringBuilder pfdata);

        /// <summary>
        /// 设置MODBUS_STRING寄存器
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="start">起始编号</param>
        /// <param name="inum">字节数量</param>
        /// <param name="pidata">设定字符串</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Modbus_Set4x_String", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Modbus_Set4x_String(IntPtr handle, UInt16 start, UInt16 inum, string pfdata);

        /// <summary>
        /// 写控制器flash块, float格式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="uiflashid">flash块号</param>
        /// <param name="uinumes">变量数量</param>
        /// <param name="pfvlue">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_FlashWritef", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_FlashWritef(IntPtr handle, UInt16 uiflashid, UInt32 uinumes, float[] pfvlue);

        /// <summary>
        /// 读取控制器flash块, float格式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="uiflashid">flash块号</param>
        /// <param name="uibuffnum">读取变量数量</param>
        /// <param name="pfvlue">反馈值</param>
        /// <param name="uireadfnum">读取变量数量反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_FlashReadf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_FlashReadf(IntPtr handle, UInt16 uiflashid, UInt32 uibuffnum, float[] pfvlue, ref UInt32 puinumesread);


        /*****************************************************************************************************2018-08-24 V2.1函数添加***************************************************************************************/
        /// <summary>
        /// 示波器触发函数 150723以后固件版本支持
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Trigger", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Trigger(IntPtr handle);

        /// <summary>
        /// 运动缓冲中插入参数修改
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="paraname">参数名称字符串</param>
        /// <param name="iaxis">修改轴号</param>
        /// <param name="fvalue">参数设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MovePara", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MovePara(IntPtr handle, UInt32 base_axis, string paraname, UInt32 iaxis, float fvalue);

        /// <summary>
        /// 运动缓冲中插入PWM指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="pwm_num">PWM口编号</param>
        /// <param name="pwm_duty">占空比设定值</param>
        /// <param name="pwm_freq">频率设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MovePwm", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MovePwm(IntPtr handle, UInt32 base_axis, UInt32 pwm_num, float pwm_duty, float pwm_freq);

        /// <summary>
        /// 运动缓冲中插入同步其他轴的运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="iaxis">同步轴号</param>
        /// <param name="fdist">相对运动距离</param>
        /// <param name="ifsp">是否使用SP运动0/1, 缺省不使用</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSynmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSynmove(IntPtr handle, UInt32 base_axis, UInt32 iaxis, float fdist, UInt32 ifsp);

        /// <summary>
        /// 运动缓冲中插入触发其他轴的运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="iaxis">触发轴轴号</param>
        /// <param name="fdist">相对运动距离</param>
        /// <param name="ifsp">是否使用SP运动0/1, 缺省不使用</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveASynmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveASynmove(IntPtr handle, UInt32 base_axis, UInt32 iaxis, float fdist, UInt32 ifsp);

        /// <summary>
        /// 运动缓冲中插入修改TABLE寄存器指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="table_num">TABLE编号</param>
        /// <param name="fvalue">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveTable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveTable(IntPtr handle, UInt32 base_axis, UInt32 table_num, float fvalue);

        /// <summary>
        /// 运动缓冲中插入等待调节指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="paraname">参数名字符串 DPOS MPOS IN AIN VPSPEED MSPEED MODBUS_REG MODBUS_IEEE MODBUS_BIT NVRAM VECT_BUFFED  REMAIN </param>
        /// <param name="inum">参数编号或轴号</param>
        /// <param name="Cmp_mode">比较条件 1-大于等于   0-等于  -1小于等于 对IN等BIT类型参数无效</param>
        /// <param name="fvalue">比较值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveWait", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveWait(IntPtr handle, UInt32 base_axis, string paraname, int inum, int Cmp_mode, float fvalue);

        /// <summary>
        ///  运动缓冲中插入TASK任务
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">主轴轴号</param>
        /// <param name="tasknum">任务编号</param>
        /// <param name="labelname">BAS程序中全局函数名或者标号名称</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveTask", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveTask(IntPtr handle, UInt32 base_axis, UInt32 tasknum, string labelname);

        /// <summary>
        /// 软件位置比较输出
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="num">比较器编号</param>
        /// <param name="enable">比较器使能</param>
        /// <param name="axisnum">比较的轴号</param>
        /// <param name="outnum">输出口编号</param>
        /// <param name="outstate">输出状态</param>
        /// <param name="setpos">比较起始位置</param>
        /// <param name="resetpos">输出复位位置</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Pswitch", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Pswitch(IntPtr handle, int num, int enable, int axisnum, int outnum, int outstate, float setpos, float resetpos);

        /// <summary>
        /// 硬件位置比较输出 4系列产品脉冲轴与编码器轴支持硬件比较输出
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Axisnum">轴号</param>
        /// <param name="Mode">模式	 mode 1-启动比较器, 2- 停止并删除没完成的比较点</param>
        /// <param name="Direction">方向  0-坐标负向,  1- 坐标正向</param>
        /// <param name="Reserve">预留</param>
        /// <param name="Tablestart">第一个比较点坐标所在TABLE编号</param>
        /// <param name="Tableend">最后一个比较点坐标所在TABLE编号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_HwPswitch", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_HwPswitch(IntPtr handle, int Axisnum, int Mode, int Direction, int Reserve, int Tablestart, int Tableend);

        /// <summary>
        /// 硬件位置比较输出剩余缓冲获取 4系列产品脉冲轴与编码器轴支持硬件比较输出
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="axisnum">轴号</param>
        /// <param name="buff">剩余缓冲反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetHwPswitchBuff", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetHwPswitchBuff(IntPtr handle, int axisnum, ref int buff);

        /// <summary>
        /// 硬件定时器用于硬件比较输出后一段时间后还原电平 4系列产品支持
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="mode">模式	0停止,2-启动</param>
        /// <param name="cyclonetime">周期时间 us单位</param>
        /// <param name="optime">有效时间 us单位</param>
        /// <param name="reptimes">重复次数</param>
        /// <param name="opstate">输出缺省状态</param>
        /// <param name="opnum">输出口编号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_HwTimer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_HwTimer(IntPtr handle, int mode, int cyclonetime, int optime, int reptimes, int opstate, int opnum);

        /// <summary>
        /// 读取轴停止运动原因
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">停止原因反馈值 参考AXISSTATUS</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAxisStopReason", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAxisStopReason(IntPtr handle, int iaxis, ref int piValue);

        /// <summary>
        /// 读取多轴轴参数
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="sParam">参数名称字符串</param>
        /// <param name="imaxaxis">读取总轴数</param>
        /// <param name="pfValue">参数值反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAllAxisPara", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAllAxisPara(IntPtr handle, string sParam, int imaxaxis, float[] pfValue);

        /// <summary>
        /// 读取多轴基本参数状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imaxaxis">读取总轴数</param>
        /// <param name="IdleStatus">轴停止状态反馈值 0-运动中 -1停止中 </param>
        /// <param name="DposStatus">轴命令位置反馈值</param>
        /// <param name="MposStatus">轴反馈位置反馈值</param>
        /// <param name="AxisStatus">轴状态</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetAllAxisInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetAllAxisInfo(IntPtr handle, int imaxaxis, int[] IdleStatus, float[] DposStatus, float[] MposStatus, int[] AxisStatus);

        /// <summary>
        /// 设置BASIC自定义全局数组
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="arrayname">全局数组名称</param>
        /// <param name="arraystart">数组起始元素</param>
        /// <param name="numes">设置数量</param>
        /// <param name="pfValue">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetUserArray", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetUserArray(IntPtr handle, string arrayname, int arraystart, int numes, float[] pfValue);

        /// <summary>
        /// 读取BASIC自定义全局数组
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="arrayname">全局数组名称</param>
        /// <param name="arraystart">数组起始元素</param>
        /// <param name="numes">读取数量</param>
        /// <param name="pfValue">反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetUserArray", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetUserArray(IntPtr handle, string arrayname, int arraystart, int numes, float[] pfValue);

        /// <summary>
        /// 设置BASIC自定义变量
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="varname">变量名称字符串</param>
        /// <param name="pfValue">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetUserVar", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetUserVar(IntPtr handle, string varname, float pfValue);

        /// <summary>
        /// 读取BASIC自定义变量
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="varname">变量名称字符串</param>
        /// <param name="pfValue">反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetUserVar", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetUserVar(IntPtr handle, string varname, ref float pfValue);

        /// <summary>
        /// 读取PCI控制卡个数
        /// </summary>
        /// <returns>卡数量</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetMaxPciCards", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetMaxPciCards();

        /// <summary>
        /// PCI卡建立链接
        /// </summary>
        /// <param name="cardnum">PCI卡号</param>
        /// <param name="phandle">句柄指针</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_OpenPci", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_OpenPci(UInt32 cardnum, out IntPtr phandle);

        /// <summary>
        /// 获取控制器卡信息
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="SoftType">控制器型号类型反馈值</param>
        /// <param name="SoftVersion">控制器软件版本（固件版本)反馈值</param>
        /// <param name="ControllerId">控制器唯一ID反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetControllerInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetControllerInfo(IntPtr handle, StringBuilder SoftType, StringBuilder SoftVersion, StringBuilder ControllerId);

        /// <summary>
        /// 读取总线控制器卡连接节点数量
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="slot">槽位号缺省</param>
        /// <param name="piValue">扫描成功节点数量反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetNodeNum", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetNodeNum(IntPtr handle, int slot, ref int piValue);

        /// <summary>
        /// 读取节点上的信息
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="slot">槽位号缺省0</param>
        /// <param name="node">节点编号</param>
        /// <param name="sel">信息编号	0-厂商编号1-设备编号 2-设备版本 3-别名 10-IN个数 11-OUT个数</param>
        /// <param name="piValue">信息反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetNodeInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetNodeInfo(IntPtr handle, UInt32 slot, UInt32 node, UInt32 sel, ref int piValue);

        /// <summary>
        /// 读取总线节点通讯状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="slot">槽位号缺省0</param>
        /// <param name="node">节点编号</param>
        /// <param name="sel">节点状态反馈值 </param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetNodeStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetNodeStatus(IntPtr handle, UInt32 slot, UInt32 node, ref UInt32 nodestatus);

        /// <summary>
        /// 读取节点SDO参数信息
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="slot">槽位号缺省0</param>
        /// <param name="node">节点编号</param>
        /// <param name="index">对象字典编号（注意函数为10进制数据）</param>
        /// <param name="subindex">子编号	（注意函数为10进制数据）</param>
        /// <param name="type">数据类型  1-bool 2-int8 3-int16 4-int32 5-uint8 6-uint16 7-uint32</param>
        /// <param name="value">参数反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_SDORead", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_SDORead(IntPtr handle, UInt32 slot, UInt32 node, UInt32 index, UInt32 subindex, UInt32 type, ref Int32 value);

        /// <summary>
        /// 写节点SDO参数信息
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="slot">槽位号缺省0</param>
        /// <param name="node">节点编号</param>
        /// <param name="index">对象字典编号（注意函数为10进制数据）</param>
        /// <param name="subindex">子编号	（注意函数为10进制数据）</param>
        /// <param name="type">数据类型  1-bool 2-int8 3-int16 4-int32 5-uint8 6-uint16 7-uint32</param>
        /// <param name="value">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_SDOWrite", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_SDOWrite(IntPtr handle, UInt32 slot, UInt32 node, UInt32 index, UInt32 subindex, UInt32 type, Int32 value);

        /// <summary>
        /// 读取Rtex驱动器参数
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="ipara">参数分类*256 + 参数编号  Pr7.11-ipara = 7*256+11</param>
        /// <param name="value">参数反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_RtexRead", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_RtexRead(IntPtr handle, UInt32 iaxis, UInt32 ipara, ref float value);

        /// <summary>
        /// 设置Rtex驱动器参数信息
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="ipara">参数分类*256 + 参数编号  Pr7.11-ipara = 7*256+11</param>
        /// <param name="value">参数设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_RtexWrite", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_RtexWrite(IntPtr handle, UInt32 iaxis, UInt32 ipara, float value);

        /// <summary>
        /// 设置回零偏移距离
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">偏移距离设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_SetDatumOffpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_SetDatumOffpos(IntPtr handle, UInt32 iaxis, float fValue);

        /// <summary>
        /// 读取回零偏移距离
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">偏移距离反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetDatumOffpos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetDatumOffpos(IntPtr handle, UInt32 iaxis, ref float fValue);

        /// <summary>
        /// 总线驱动器自身回零运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="homemode">回零模式，查看驱动器手册</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_Datum", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_Datum(IntPtr handle, UInt32 iaxis, UInt32 homemode);

        /// <summary>
        /// 总线驱动器回零完成状态
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="homestatus">回零完成标志 0-回零异常 1回零成功</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetHomeStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetHomeStatus(IntPtr handle, UInt32 iaxis, ref UInt32 homestatus);

        /// <summary>
        /// 清除总线伺服轴报警
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="mode">模式 0-清除当前告警  1-清除历史告警  2-清除外部输入告警</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_DriveClear", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_DriveClear(IntPtr handle, UInt32 iaxis, UInt32 mode);

        /// <summary>
        /// 读取总线轴当前力矩	需要设置对应的DRIVE_PROFILE类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">反馈转矩值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetDriveTorque", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetDriveTorque(IntPtr handle, UInt32 iaxis, ref int piValue);

        /// <summary>
        /// 设置总线轴最大转矩  需要设置对应的DRIVE_PROFILE类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">最大转矩限制设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_SetMaxDriveTorque", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_SetMaxDriveTorque(IntPtr handle, UInt32 iaxis, int piValue);

        /// <summary>
        /// 读取总线轴最大转矩  需要设置对应的DRIVE_PROFILE类型
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="piValue">最大转矩限制反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetMaxDriveTorque", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetMaxDriveTorque(IntPtr handle, UInt32 iaxis, ref int piValue);

        /// <summary>
        /// 设置总线轴模拟量输出 力矩、速度模式下可以  总线驱动需要设置对应DRIVE_PROFILE类型 与ATYPE
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">转矩输出值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetDAC", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetDAC(IntPtr handle, UInt32 iaxis, float fValue);

        /// <summary>
        /// 读取总线轴模拟量输出  总线驱动需要设置对应DRIVE_PROFILE类型 与ATYPE
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="fValue">转矩反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetDAC", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetDAC(IntPtr handle, UInt32 iaxis, ref float fValue);

        /// <summary>
        /// 总线初始化指令  （针对Zmotion tools 工具软件配置过总线参数控制器使用有效）
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_InitBus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_InitBus(IntPtr handle);

        /// <summary>
        /// 读取总线初始化状态  （针对Zmotion tools 工具软件配置过总线参数控制器使用有效）
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="piValue">状态反馈值 0-失败 1-成功</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_GetInitStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_GetInitStatus(IntPtr handle, ref int piValue);

        /// <summary>
        /// 读取多个输入信号
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="startio">起始输入口编号</param>
        /// <param name="endio">结束输入口编号</param>
        /// <param name="piValue">输入口状态反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetInMulti", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetInMulti(IntPtr handle, int startio, int endio, Int32[] piValue);

        /// <summary>
        /// 设置在线命令的超时时间
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="timems">超时时间 MS</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SetTimeOut", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SetTimeOut(IntPtr handle, UInt32 timems);

        #region "硬件位置比较输出2   ZAux_Direct_HwPswitch2"
        /*************************************************************2017-12-14添加
    Description:    //硬件位置比较输出2 4系列产品, 20170513以上版本支持.  ZMC306E/306N支持
    Input:          //卡链接					handle
                    //模式						mode		
                    //输出口编号				Opnum		4系列 out 0-3为硬件位置比较输出
                    //第一个比较点的输出状态	Opstate		0-关闭 1打开										
                    //多功能参数				ModePara1	 
                    //多功能参数				ModePara2
                    //多功能参数				ModePara3
                    //多功能参数				ModePara4

    mode 1-启动比较器, 
            ModePara1 =  第一个比较点坐标所在TABLE编号
            ModePara2 =	 最后一个比较点坐标所在TABLE编号
            ModePara3 =  第一个点判断方向,  0-坐标负向,  1- 坐标正向,  -1-不使用方向
            ModePara4 =	 预留

    mode 2- 停止并删除没完成的比较点. 
            ModePara1 =  预留
            ModePara2 =	 预留
            ModePara3 =  预留
            ModePara4 =	 预留

    mode 3- 矢量比较方式
            ModePara1 =  第一个比较点坐标所在TABLE编号
            ModePara2 =	 最后一个比较点坐标所在TABLE编号
            ModePara3 =  预留
            ModePara4 =	 预留

    Mode=4 :矢量比较方式, 单个比较点
            ModePara1 =  比较点坐标
            ModePara2 =	 预留
            ModePara3 =  预留
            ModePara4 =	 预留

    Mode=5 :矢量比较方式, 周期脉冲模式
            ModePara1 =  比较点坐标
            ModePara2 =	 重复周期, 一个周期内比较两次, 先输出有效状态,再输出无效状态.
            ModePara3 =  周期距离, 每隔这个距离输出Opstate, 输出有效状态的距离（ModePara4）后还原为无效状态.
            ModePara4 =	 输出有效状态的距离,  (ModePara3- ModePara4) 为无效状态距离

    Mode=6 :矢量比较方式, 周期模式, 这种模式一般与HW_TIMER一起使用.
            ModePara1 =  比较点坐标
            ModePara2 =	 重复周期, 一个周期只比较一次
            ModePara3 =  周期距离, 每隔这个距离输出一次
            ModePara4 =	 预留
    Return:         //错误码
    int32 __stdcall ZAux_Direct_HwPswitch2(ZMC_HANDLE handle,int Axisnum,int Mode, int Opnum , int Opstate, float ModePara1, float ModePara2,float ModePara3,float ModePara4);
    *************************************************************/
        #endregion
        /// <summary>
        /// 硬件位置比较输出2 4系列产品, 20170513以上版本支持.  ZMC306E/306N支持
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Axisnum">轴号</param>
        /// <param name="Mode">模式</param>
        /// <param name="Opnum">输出口编号</param>
        /// <param name="Opstate">第一个比较点的输出状态</param>
        /// <param name="ModePara1">功能参数1</param>
        /// <param name="ModePara2">功能参数2</param>
        /// <param name="ModePara3">功能参数3</param>
        /// <param name="ModePara4">功能参数4</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_HwPswitch2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_HwPswitch2(IntPtr handle, int Axisnum, int Mode, int Opnum, int Opstate, float ModePara1, float ModePara2, float ModePara3, float ModePara4);

        /// <summary>
        /// 获取控制器最大规格数
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Max_VirtuAxises">最大虚拟轴数</param>
        /// <param name="Max_motor">最大电机数量</param>
        /// <param name="Max_io">最大IN,OUT,AD,DA数量</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetSysSpecification", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetSysSpecification(IntPtr handle, ref UInt16 Max_VirtuAxises, byte[] Max_motor, byte[] Max_io);

        /// <summary>
        /// 控制器主动上报回调函数
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="itypecode">上传类型码</param>
        /// <param name="idatalength">数据长度</param>
        /// <param name="pdata">上报数据指针</param> [MarshalAs(UnmanagedType.LPArray, SizeConst = 2048)]byte[] pdata
        public delegate void ZAuxCallBack(IntPtr handle, Int32 itypecode, Int32 idatalength,  StringBuilder pdata);
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SetAutoUpCallBack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SetAutoUpCallBack(IntPtr handle, ZAuxCallBack pcallback);

        /// <summary>
        /// 设置多路输出
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iofirst">IO口起始编号</param>
        /// <param name="ioend">IO口结束编号</param>
        /// <param name="istate">输出口状态</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_SetOutMulti", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_SetOutMulti(IntPtr handle, UInt16 iofirst, UInt16 ioend, UInt32[] istate);

            /// <summary>
        /// 读取多路输出
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iofirst">IO口起始编号</param>
        /// <param name="ioend">IO口结束编号</param>
        /// <param name="istate">输出口状态</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetOutMulti", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetOutMulti(IntPtr handle, UInt16 iofirst, UInt16 ioend, UInt32[] istate);


        /// <summary>
        /// 多轴相对直线插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iMoveLen">发送运行指令数量</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDposlist">运动位置列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MultiMove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MultiMove(IntPtr handle,int iMoveLen, int imaxaxises, int[] piAxislist, float[] pfDposlist);

        /// <summary>
        /// 多轴绝对直线插补运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iMoveLen">发送运行指令数量</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴列表</param>
        /// <param name="pfDposlist">运动位置列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MultiMoveAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MultiMoveAbs(IntPtr handle,int iMoveLen, int imaxaxises, int[] piAxislist, float[] pfDposlist);

        /// <summary>
        /// 机械手坐标系旋转
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号 关节轴/虚拟轴</param>
        /// <param name="pfRotatePara">平移旋转参数</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_FrameRotate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_FrameRotate(IntPtr handle, int iaxis, float[] pfRotatePara);

        /// <summary>
        /// 获取CAN扩展资源规格
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="CanNum">当前连接的CAN从站数量</param>
        /// <param name="CanId_List">当前连接的CAN从站ID列表</param>
        /// <param name="CanIn_List">节点输入点数量</param>
        /// <param name="CanOut_List">节点输出点数量</param>
        /// <param name="CanAin_List">节点AD数量</param>
        /// <param name="CanAOut_List">节点DA数量</param>
        /// <param name="CanAxis_List">节点轴数量</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetCanInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetCanInfo(IntPtr handle, ref byte CanNum, UInt16[] CanId_List, byte[] CanIn_List, byte[] CanOut_List, byte[] CanAin_List, byte[] CanAOut_List, byte[] CanAxis_List);


        /// <summary>
        /// 多条相对PT运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iMoveLen">填写的运动数量</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="pTickslist">周期列表</param>
        /// <param name="pfDisancelist">距离列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MultiMovePt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MultiMovePt(IntPtr handle, int iMoveLen, int imaxaxises, int[] piAxislist, UInt32[] pTickslist, float[] pfDisancelist);

        /// <summary>
        /// 多条绝对PT运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iMoveLen">填写的运动数量</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="pTickslist">周期列表</param>
        /// <param name="pfDisancelist">距离列表</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MultiMovePtAbs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MultiMovePtAbs(IntPtr handle, int iMoveLen, int imaxaxises, int[] piAxislist, UInt32[] pTickslist, float[] pfDisancelist);

        /// <summary>
        /// 下载ZAR文件到控制
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="Filename">Filename 文件路径</param>
        /// <param name="run_mode">下载到RAM-ROM  0-RAM  1-ROM</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_ZarDown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_ZarDown(IntPtr handle, String Filename,UInt32 run_mode);

        /// <summary>
        /// 读取RTC时间
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="RtcDate">系统日期 格式YYMMDD</param>
        /// <param name="RtcTime">系统时间 格式HHMMSS</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_GetRtcTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_GetRtcTime(IntPtr handle, StringBuilder RtcDate, StringBuilder RtcTime);

        /// <summary>
        /// 设置RTC时间
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="RtcDate">系统日期 格式YYMMDD</param>
        /// <param name="RtcTime">系统时间 格式HHMMSS</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_SetRtcTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_SetRtcTime(IntPtr handle, String RtcDate, String RtcTime);

        /// <summary>
        /// 与控制器建立链接, 可以指定连接的等待时间
        /// </summary>
        /// <param name="type">连接类型	1-COM 2-ETH 4-PCI</param>
        /// <param name="pconnectstring">连接字符串 COM口号/IP地址</param>
        /// <param name="uims">连接超时时间</param>
        /// <param name="phandle">卡链接handle</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_FastOpen", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_FastOpen(Int32 type, string pconnectstring, UInt32 uims, out IntPtr phandle);

        /// <summary>
        /// 自定义二次回零
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">回零模式</param>
        /// <param name="HighSpeed">回零高速</param>
        /// <param name="LowSpeed">回零低速</param>
        /// <param name="DatumOffset">二次回零偏移距离</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_UserDatum", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_UserDatum(IntPtr handle, Int32 iaxis, Int32 imode,float HighSpeed,float LowSpeed,float DatumOffset);

        /// <summary>
        /// 设置轴的螺距补偿，扩展轴无效。
        /// </summary>
        /// <param name="handle">句柄连接</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iEnable">是否启用补偿</param>
        /// <param name="StartPos">起始补偿MPOS位置</param>
        /// <param name="maxpoint">补偿区间总点数</param>
        /// <param name="DisOne">每个补偿点间距</param>
        /// <param name="TablNum">补偿坐标值填入TABLE系统数组起始引导地址</param>
        /// <param name="pfDisancelist">区间补偿值列表</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Pitchset", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Pitchset(IntPtr handle,Int32 iaxis,Int32 iEnable,float StartPos,UInt32 maxpoint,float DisOne ,UInt32 TablNum,float [] pfDisancelist);

        /// <summary>
        /// 设置轴的螺距双向补偿，扩展轴无效。
        /// </summary>
        /// <param name="handle">句柄连接</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="iEnable">是否启用补偿</param>
        /// <param name="StartPos">起始补偿MPOS位置</param>
        /// <param name="maxpoint">补偿区间总点数</param>
        /// <param name="DisOne">每个补偿点间距</param>
        /// <param name="TablNum">正向补偿坐标值填入TABLE系统数组起始引导地址</param>
        /// <param name="pfDisancelist">正向区间补偿值列表</param>
        /// <param name="RevTablNum">反向补偿坐标值填入TABLE系统数组起始引导地址</param>
        /// <param name="RevpfDisancelist">反向区间补偿值列表 补偿数据方向于正向方向一致</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_Pitchset2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_Pitchset2(IntPtr handle,Int32 iaxis,Int32 iEnable,float StartPos,UInt32 maxpoint,float DisOne ,UInt32 TablNum,float [] pfDisancelist,UInt32 RevTablNum,float [] RevpfDisancelist);


        /// <summary>
        /// 轴的螺距补偿状态读取，扩展轴无效。
        /// </summary>
        /// <param name="handle">句柄连接</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="IfEnable">是否启用补偿  0关闭  -1开启</param>
        /// <param name="PitchDist">补偿距离</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_GetPitchStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_GetPitchStatus(IntPtr handle,Int32 iaxis,ref Int32 IfEnable,ref float PitchDist);

        /// <summary>
        /// 多轴多段线直线连续插补 
        /// </summary>
        /// <param name="handle">句柄连接</param>
        /// <param name="imode">imode	bit0- bifabs	bit1- bifsp	是否SP	bit2- bifresume	bit3- bifmovescan 调用	</param>
        /// <param name="iMoveLen">填写的运动长度</param>
        /// <param name="imaxaxises">参与运动总轴数</param>
        /// <param name="piAxislist">轴号列表</param>
        /// <param name="pfDisancelist">距离列表  iMoveLen * imaxaxises</param>
        /// <param name="iReBuffLen"></param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MultiLineN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MultiLineN(IntPtr handle,Int32 imode,Int32 iMoveLen, Int32 imaxaxises, Int32 []piAxislist, float []pfDisancelist,ref Int32 iReBuffLen);

        /// <summary>
        /// 皮带同步跟随运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="imode">同步模式 -1结束模式  -2强制结束 0-第一个轴跟随 10-第二个轴跟随 20-第二个轴跟随  小数位-angle：皮带旋转角度</param>
        /// <param name="synctime">同步时间，ms单位，本运动在指定时间内完成，完成时BASE轴跟上皮带且保持速度一致。0表示根据运动轴的速度加速度来估计同步时间</param>
        /// <param name="syncposition">皮带轴物体被感应到时皮带轴的位置</param>
        /// <param name="syncaxis">皮带轴轴号</param>
        /// <param name="imaxaxises">参与同步从轴总数</param>
        /// <param name="piAxislist">从站轴号列表</param>
        /// <param name="pfDposlist">皮带轴物体被感应到时从轴的绝对坐标位置</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveSync", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveSync(IntPtr handle, float imode, int synctime, float syncposition, int syncaxis, int imaxaxises, int[] piAxislist, float[] pfDposlist);
    
        /// <summary>
        /// 连续位置锁存指令
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param>
        /// <param name="imode">锁存模式</param>
        /// <param name="iTabStart">连续锁存的内容存储的table位置，第一个table元素存储锁存的个数，后面存储锁存的坐标，最多保存个数= numes-1，溢出时循环写入</param>
        /// <param name="iTabNum">占用的table个数</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_CycleRegist", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_CycleRegist(IntPtr handle, int iaxis, int imode, int iTabStart, int iTabNum);

        /// <summary>
        /// 运动中取消其他轴运动
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="base_axis">插补主轴编号</param>
        /// <param name="Cancel_Axis">取消运动轴号</param>
        /// <param name="iMode">停止模式</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Direct_MoveCancel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Direct_MoveCancel(IntPtr handle, int base_axis, int Cancel_Axis, int iMode);

        /// <summary>
        /// Pdo写操作
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="inode">节点号</param>
        /// <param name="index">对象字典</param>
        /// <param name="subindex">子对象</param>
        /// <param name="type">数据类型</param>
        /// <param name="value">写入值</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_NodePdoWrite", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_NodePdoWrite(IntPtr handle, UInt32 inode, UInt32 index, UInt32 subindex, UInt32 type, Int32 value);	

        /// <summary>
        /// Pdo读操作
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="inode">节点号</param>
        /// <param name="index">对象字典</param>
        /// <param name="subindex">子对象</param>
        /// <param name="type">数据类型</param>
        /// <param name="ivalue">返回的数据值</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_NodePdoRead", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_NodePdoRead(IntPtr handle, UInt32 inode, UInt32 index, UInt32 subindex, UInt32 type, ref Int32 ivalue);

        /// <summary>
        /// 使能周期上报
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="cycleindex">上报通道号</param>
        /// <param name="fintervalms">上报间隔时间</param>
        /// <param name="psetesname">上报参数选择，</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_CycleUpEnable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_CycleUpEnable(IntPtr handle, UInt32 cycleindex, float fintervalms, string psetesname);

        /// <summary>
        /// 关闭使能周期上报
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="cycleindex">上报通道号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_CycleUpDisable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_CycleUpDisable(IntPtr handle, UInt32 cycleindex);

        /// <summary>
        /// 周期上报收到的包次数, 超过溢出. 调试使用.
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="cycleindex">上报通道号</param>
        /// <returns>上报次数</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_CycleUpGetRecvTimes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_CycleUpGetRecvTimes(IntPtr handle, UInt32 cycleindex);

        /// <summary>
        /// 强制上报一次, 在运动指令后idle可能不准确的情况下调用.
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="cycleindex">上报通道号</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_CycleUpForceOnce", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_CycleUpForceOnce(IntPtr handle, UInt32 cycleindex);

        /// <summary>
        /// 从周期上报里面读取内容double格式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="cycleindex">上报通道号</param>
        /// <param name="psetname">参数名称</param>
        /// <param name="isetindex">参数编号</param>
        /// <param name="pvalue">返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_CycleUpReadBuff", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_CycleUpReadBuff(IntPtr handle, UInt32 cycleindex, string psetesname, UInt32 isetindex,ref double pvalue);

        /// <summary>
        /// 从周期上报里面读取内容Int32格式
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="cycleindex">上报通道号</param>
        /// <param name="psetname">参数名称</param>
        /// <param name="isetindex">参数编号</param>
        /// <param name="pvalue">返回值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_CycleUpReadBuffInt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_CycleUpReadBuffInt(IntPtr handle, UInt32 cycleindex, string psetesname, UInt32 isetindex, ref Int32 pvalue);

        /// <summary>
        /// 通过轴号进行 SDO 读取
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param> 
        /// <param name="index">对象字典编号（注意函数为10进制数据）</param>
        /// <param name="subindex">子编号	（注意函数为10进制数据）</param>
        /// <param name="type">数据类型  1-bool 2-int8 3-int16 4-int32 5-uint8 6-uint16 7-uint32</param>
        /// <param name="value">参数反馈值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_SDOReadAxis", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_SDOReadAxis(IntPtr handle, UInt32 iaxis, UInt32 index, UInt32 subindex, UInt32 type, ref Int32 value);

        /// <summary>
        /// 通过轴号进行 SDO 写节
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="iaxis">轴号</param> 
        /// <param name="index">对象字典编号（注意函数为10进制数据）</param>
        /// <param name="subindex">子编号	（注意函数为10进制数据）</param>
        /// <param name="type">数据类型  1-bool 2-int8 3-int16 4-int32 5-uint8 6-uint16 7-uint32</param>
        /// <param name="value">设定值</param>
        /// <returns>错误码</returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_BusCmd_SDOWriteAxis", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_BusCmd_SDOWriteAxis(IntPtr handle, UInt32 iaxis, UInt32 index, UInt32 subindex, UInt32 type, Int32 value);

        [DllImport("zmotion.dll", EntryPoint = "ZMC_GetState", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZMC_GetState(IntPtr handle, byte[] pstate);

         /// <summary>
        /// 读取脚本输出的信息
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="pbuff">脚本空间</param>
        /// <param name="uimax">最大等待时间</param>
        /// <param name="puiread">返回的字符长度</param>
        /// <returns>错误码</returns>
        [DllImport("zmotion.dll", EntryPoint = "ZMC_ReadMessage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZMC_ReadMessage(IntPtr handle, byte[] pbuff, UInt32 uimax, ref UInt32 puiread);


        /// <summary>
        /// 读取在线命令的应答， 对没有接收应答的命令有用
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="pbuff">脚本空间</param>
        /// <param name="uimax">最大等待时间</param>
        /// <param name="puiread">返回的字符长度</param>
        /// <param name="pbifExcuteDown">是否已经执行结束</param>
        /// <returns>错误码</returns>
        [DllImport("zmotion.dll", EntryPoint = "ZMC_ExecuteGetReceive", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZMC_ExecuteGetReceive(IntPtr handle, byte[] pbuff, UInt32 uimax, ref UInt32 puiread, ref byte pbifExcuteDown);

        /// <summary>
        /// 动态下载3次文件, 一次下载的内容必须是完整的单行或多行,没有下载完成时, 后面用\n结尾, 全部下载完成时, 加\0结尾,\0会导致任务结束, 需要再启动可以再次调用ZAux_Run3FileRam
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ifile3num">3次文件编号</param>
        /// <param name="premainbyte">剩余空间字节数</param>
        /// <param name="File3Name">设置3次文件名.3次文件不会自动运行, 需调用ZAux_Run3FileRam启动</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_3FileRamDownBegin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_3FileRamDownBegin(IntPtr handle, int ifile3num, ref UInt32 premainbyte, string File3Name);

        /// <summary>
        /// 读取3次文件可写的空间.
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ifile3num">3次文件编号</param>
        /// <param name="premainbyte">剩余空间字节数</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_3FileRamGetRemainSpace", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_3FileRamGetRemainSpace(IntPtr handle, int ifile3num, ref UInt32 premainbyte);

        /// <summary>
        /// 动态下载3次文件, 一次下载的内容必须是完整的单行或多行,没有下载完成时, 后面用\n结尾, 全部下载完成时, 加\0结尾
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ifile3num">3次文件编号</param>
        /// <param name="pbuffer">下载的字符串</param>
        /// <param name="buffsize">字符串长度</param>
        /// <param name="premainbyte">剩余空间字节数</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_3FileRamDownPart", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_3FileRamDownPart(IntPtr handle, int ifile3num, StringBuilder pbuffer, UInt32 buffsize, ref UInt32 premainbyte);
        /// <summary>
        /// 运行控制器内3次文件
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ifile3num">3次文件编号</param>
        /// <param name="itasknum">运动任务号</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_Run3FileRam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_Run3FileRam(IntPtr handle, int ifile3num, int itasknum);

        /// <summary>
        /// 动态下载3次文件, 下载结束, 自动加\0, 并且调用后再次调用 ZMC_3FileRamDownPart 会返回错误
        /// </summary>
        /// <param name="handle">连接句柄</param>
        /// <param name="ifile3num">3次文件编号</param>
        /// <returns></returns>
        [DllImport("zauxdll.dll", EntryPoint = "ZAux_3FileRamDownEnd", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ZAux_3FileRamDownEnd(IntPtr handle, int ifile3num);
        }
}
