namespace ZMotionSDK.Helper
{
    /// <summary>
    /// 信号类型枚举
    /// </summary>
    public enum SignalType
    {
        /// <summary>数字输入</summary>
        DI,
        /// <summary>数字输出</summary>
        DO,
        /// <summary>原点信号</summary>
        HomeSignal,
        /// <summary>正向限位信号</summary>
        PositiveLimit,
        /// <summary>反向限位信号</summary>
        NegativeLimit
    }

    /// <summary>
    /// DIDO信号反转配置帮助类
    /// 统一管理数字输入输出信号和轴信号的反转配置
    /// </summary>
    public class DIDOInverterConfig
    {
        private readonly Dictionary<SignalType, HashSet<int>> _invertedSignals = new();

        /// <summary>
        /// 信号类型对应的最大数量配置
        /// </summary>
        private readonly Dictionary<SignalType, int> _maxCounts = new();

        /// <summary>
        /// 最大DI数量
        /// </summary>
        public int MaxDICount => _maxCounts[SignalType.DI];

        /// <summary>
        /// 最大DO数量
        /// </summary>
        public int MaxDOCount => _maxCounts[SignalType.DO];

        /// <summary>
        /// 最大轴数量
        /// </summary>
        public int MaxAxisCount => _maxCounts[SignalType.HomeSignal];

        /// <summary>
        /// 初始化DIDO反转配置
        /// </summary>
        /// <param name="maxDICount">最大DI数量</param>
        /// <param name="maxDOCount">最大DO数量</param>
        /// <param name="maxAxisCount">最大轴数量</param>
        public DIDOInverterConfig(int maxDICount = 32, int maxDOCount = 32, int maxAxisCount = 16)
        {
            // 初始化最大数量配置
            _maxCounts[SignalType.DI] = maxDICount;
            _maxCounts[SignalType.DO] = maxDOCount;
            _maxCounts[SignalType.HomeSignal] = maxAxisCount;
            _maxCounts[SignalType.PositiveLimit] = maxAxisCount;
            _maxCounts[SignalType.NegativeLimit] = maxAxisCount;

            // 初始化反转信号集合
            foreach (SignalType signalType in Enum.GetValues<SignalType>())
            {
                _invertedSignals[signalType] = new HashSet<int>();
            }
        }

        #region 通用配置方法

        /// <summary>
        /// 设置指定信号类型的反转索引
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <param name="indexes">需要反转的索引数组</param>
        public void SetInverted(SignalType signalType, params int[] indexes)
        {
            var maxCount = _maxCounts[signalType];
            var invertedSet = _invertedSignals[signalType];

            invertedSet.Clear();
            foreach (var index in indexes.Where(i => i >= 0 && i < maxCount))
            {
                invertedSet.Add(index);
            }
        }

        /// <summary>
        /// 设置指定信号类型的所有索引反转
        /// </summary>
        /// <param name="signalType">信号类型</param>
        public void SetAllInverted(SignalType signalType)
        {
            var maxCount = _maxCounts[signalType];
            var invertedSet = _invertedSignals[signalType];

            invertedSet.Clear();
            for (int i = 0; i < maxCount; i++)
            {
                invertedSet.Add(i);
            }
        }

        /// <summary>
        /// 添加需要反转的索引
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <param name="index">索引</param>
        public void AddInverted(SignalType signalType, int index)
        {
            var maxCount = _maxCounts[signalType];
            if (index >= 0 && index < maxCount)
            {
                _invertedSignals[signalType].Add(index);
            }
        }

        /// <summary>
        /// 移除需要反转的索引
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <param name="index">索引</param>
        public void RemoveInverted(SignalType signalType, int index)
        {
            _invertedSignals[signalType].Remove(index);
        }

        /// <summary>
        /// 检查指定索引是否需要反转
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <param name="index">索引</param>
        /// <returns>是否需要反转</returns>
        public bool IsInverted(SignalType signalType, int index)
        {
            return _invertedSignals[signalType].Contains(index);
        }

        /// <summary>
        /// 获取指定信号类型的反转配置数组
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <returns>反转配置数组，索引对应信号编号，值表示是否反转</returns>
        public bool[] GetInvertArray(SignalType signalType)
        {
            var maxCount = _maxCounts[signalType];
            var result = new bool[maxCount];
            foreach (var index in _invertedSignals[signalType])
            {
                result[index] = true;
            }
            return result;
        }

        /// <summary>
        /// 获取指定信号类型的反转索引数组
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <returns>反转索引数组</returns>
        public int[] GetInvertedIndexes(SignalType signalType)
        {
            return _invertedSignals[signalType].OrderBy(x => x).ToArray();
        }

        /// <summary>
        /// 清除指定信号类型的反转配置
        /// </summary>
        /// <param name="signalType">信号类型</param>
        public void Clear(SignalType signalType)
        {
            _invertedSignals[signalType].Clear();
        }

        /// <summary>
        /// 清除所有反转配置
        /// </summary>
        public void ClearAll()
        {
            foreach (var signalSet in _invertedSignals.Values)
            {
                signalSet.Clear();
            }
        }

        #endregion

        #region 便捷方法 - DI/DO

        /// <summary>
        /// 设置需要反转的DI索引
        /// </summary>
        public void SetInvertedDI(params int[] indexes) => SetInverted(SignalType.DI, indexes);

        /// <summary>
        /// 设置需要反转的DO索引
        /// </summary>
        public void SetInvertedDO(params int[] indexes) => SetInverted(SignalType.DO, indexes);

        /// <summary>
        /// 设置所有DI反转
        /// </summary>
        public void SetAllInvertedDI() => SetAllInverted(SignalType.DI);

        /// <summary>
        /// 设置所有DO反转
        /// </summary>
        public void SetAllInvertedDO() => SetAllInverted(SignalType.DO);

        /// <summary>
        /// 检查指定DI索引是否需要反转
        /// </summary>
        public bool IsDIInverted(int index) => IsInverted(SignalType.DI, index);

        /// <summary>
        /// 检查指定DO索引是否需要反转
        /// </summary>
        public bool IsDOInverted(int index) => IsInverted(SignalType.DO, index);

        #endregion

        #region 便捷方法 - 轴信号

        /// <summary>
        /// 设置需要反转原点信号的轴索引
        /// </summary>
        public void SetInvertedHomeSignal(params int[] axisIndexes) => SetInverted(SignalType.HomeSignal, axisIndexes);

        /// <summary>
        /// 设置需要反转正向限位信号的轴索引
        /// </summary>
        public void SetInvertedPositiveLimit(params int[] axisIndexes) => SetInverted(SignalType.PositiveLimit, axisIndexes);

        /// <summary>
        /// 设置需要反转反向限位信号的轴索引
        /// </summary>
        public void SetInvertedNegativeLimit(params int[] axisIndexes) => SetInverted(SignalType.NegativeLimit, axisIndexes);

        /// <summary>
        /// 检查指定轴的原点信号是否需要反转
        /// </summary>
        public bool IsHomeSignalInverted(int axisIndex) => IsInverted(SignalType.HomeSignal, axisIndex);

        /// <summary>
        /// 检查指定轴的正向限位信号是否需要反转
        /// </summary>
        public bool IsPositiveLimitInverted(int axisIndex) => IsInverted(SignalType.PositiveLimit, axisIndex);

        /// <summary>
        /// 检查指定轴的反向限位信号是否需要反转
        /// </summary>
        public bool IsNegativeLimitInverted(int axisIndex) => IsInverted(SignalType.NegativeLimit, axisIndex);

        #endregion

        #region 信号值处理

        /// <summary>
        /// 根据反转配置处理信号值
        /// </summary>
        /// <param name="signalType">信号类型</param>
        /// <param name="index">信号索引</param>
        /// <param name="value">原始信号值</param>
        /// <returns>处理后的信号值</returns>
        public bool ProcessSignalValue(SignalType signalType, int index, bool value)
        {
            return IsInverted(signalType, index) ? !value : value;
        }

        /// <summary>
        /// 根据反转配置处理DI值
        /// </summary>
        public bool ProcessDIValue(int index, bool value) => ProcessSignalValue(SignalType.DI, index, value);

        /// <summary>
        /// 根据反转配置处理DI值
        /// </summary>
        public bool ProcessDIValue(int index, uint value) => ProcessDIValue(index, value != 0);

        /// <summary>
        /// 根据反转配置处理DO值
        /// </summary>
        public bool ProcessDOValue(int index, bool value) => ProcessSignalValue(SignalType.DO, index, value);

        /// <summary>
        /// 根据反转配置处理DO值
        /// </summary>
        public bool ProcessDOValue(int index, uint value) => ProcessDOValue(index, value != 0);

        /// <summary>
        /// 根据反转配置处理原点信号值
        /// </summary>
        public bool ProcessHomeSignalValue(int axisIndex, bool value) => ProcessSignalValue(SignalType.HomeSignal, axisIndex, value);

        /// <summary>
        /// 根据反转配置处理正向限位信号值
        /// </summary>
        public bool ProcessPositiveLimitValue(int axisIndex, bool value) => ProcessSignalValue(SignalType.PositiveLimit, axisIndex, value);

        /// <summary>
        /// 根据反转配置处理反向限位信号值
        /// </summary>
        public bool ProcessNegativeLimitValue(int axisIndex, bool value) => ProcessSignalValue(SignalType.NegativeLimit, axisIndex, value);

        #endregion

        #region 兼容性方法 - 保持向后兼容

        /// <summary>
        /// 获取DI反转配置数组
        /// </summary>
        public bool[] GetDIInvertArray() => GetInvertArray(SignalType.DI);

        /// <summary>
        /// 获取DO反转配置数组
        /// </summary>
        public bool[] GetDOInvertArray() => GetInvertArray(SignalType.DO);

        /// <summary>
        /// 获取已配置的反转DI索引列表
        /// </summary>
        public int[] GetInvertedDIIndexes() => GetInvertedIndexes(SignalType.DI);

        /// <summary>
        /// 获取已配置的反转DO索引列表
        /// </summary>
        public int[] GetInvertedDOIndexes() => GetInvertedIndexes(SignalType.DO);

        /// <summary>
        /// 获取需要反转原点信号的轴索引数组
        /// </summary>
        public int[] GetInvertedHomeSignalAxisIndexes() => GetInvertedIndexes(SignalType.HomeSignal);

        /// <summary>
        /// 获取需要反转正向限位信号的轴索引数组
        /// </summary>
        public int[] GetInvertedPositiveLimitAxisIndexes() => GetInvertedIndexes(SignalType.PositiveLimit);

        /// <summary>
        /// 获取需要反转反向限位信号的轴索引数组
        /// </summary>
        public int[] GetInvertedNegativeLimitAxisIndexes() => GetInvertedIndexes(SignalType.NegativeLimit);

        #endregion

        public DIDOInverterConfig Clone()
        {
            var cloned = new DIDOInverterConfig(MaxDICount, MaxDOCount, MaxAxisCount);

            // 复制所有信号类型的反转配置
            foreach (var kvp in _invertedSignals)
            {
                cloned.SetInverted(kvp.Key, [.. kvp.Value]);
            }

            return cloned;
        }
    }
}