namespace ZMotionSDK.Models
{
    public struct AxisSignal
    {
        public bool HomeSignal { get; set; }

        public bool PositiveLimitSignal { get; set; }

        public bool NegativeLimitSignal { get; set; }

        public bool RunningSignal { get; set; }

        public bool AlarmSignal { get; set; }

        public bool EnableSignal { get; set; }
    }
}
