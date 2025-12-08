using ZMotionSDK.Models;

namespace ZMotionSDK.EventArgs;

public class MessageFrameArg : System.EventArgs
{
    public ZMotionHeartBeatMessage Message { get; set; }

    public MessageFrameArg(ZMotionHeartBeatMessage message)
    {
        Message = message;
    }
}