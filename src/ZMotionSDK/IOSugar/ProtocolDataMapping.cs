using MessageToolkit;
using MessageToolkit.Abstractions;
using MessageToolkit.Models;

namespace ZMotionSDK;

public class ProtocolDataMapping<TProtocol> : NativeDataMapping<TProtocol, bool>
    where TProtocol : struct
{
    private readonly Action<IEnumerable<WriteFrame<bool>>> _commitAction;

    internal ProtocolDataMapping(IProtocolSchema<TProtocol> Schema, Action<IEnumerable<WriteFrame<bool>>> commitAction) : base(Schema)
    {
        _commitAction = commitAction;
    }

    /// <summary>
    /// 提交所有映射的数据到 ZMotion
    /// </summary>
    public void Commit()
    {
        var frames = BuildOptimized();
        _commitAction(frames);
    }
}