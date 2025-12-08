namespace ZMotionSDK
{
    public static class BitConverter
    {
        public static bool GetBit(this int value, int index)
        {
            return (value & (1 << index)) != 0;
        }

        public static bool GetBit(this ushort value, int index)
        {
            return (value & (1 << index)) != 0;
        }

        public static bool GetBit(this short value, int index)
        {
            return (value & (1 << index)) != 0;
        }

        public static int SetBit(this int value, int index, bool flag)
        {
            if (flag)
            {
                return value | (1 << index);
            }
            else
            {
                return value & ~(1 << index);
            }
        }

        public static ushort SetBit(this ushort value, int index, bool flag)
        {
            if (flag)
            {
                return (ushort)(value | (1 << index));
            }
            else
            {
                return (ushort)(value & ~(1 << index));
            }
        }

        public static short SetBit(this short value, int index, bool flag)
        {
            if (flag)
            {
                return (short)(value | (1 << index));
            }
            else
            {
                return (short)(value & ~(1 << index));
            }
        }

        public static uint SetBit(this uint value, int index, bool flag)
        {
            if (flag)
            {
                return (uint)(value | (1 << index));
            }
            else
            {
                return (uint)(value & ~(1 << index));
            }
        }
    }
}
