namespace ZMotionSDK;

public class ZMotionException(string message) : Exception(message)
{
    public ZMotionException(int errorCode) : this(ErrorCode.Parse(errorCode))
    {
    }
}