# ZMotionIOClient 使用指南

## 类名变更
- **旧类名**: `MessageBuilder<TDI, TDO>`
- **新类名**: `ZMotionIOClient<TDI, TDO>`

## 新增特性

### 1. 更清晰的命名
`ZMotionIOClient` 更准确地表达了这是一个基于 ZMotion 的 IO 协议客户端。

### 2. 数据缓存属性
新增了两个属性用于缓存上次读取的数据：

```csharp
public TDIProtocol? LastDIData { get; private set; }  // 上次读取的 DI 数据
public TDOProtocol? LastDOData { get; private set; }  // 上次读取的 DO 数据
```

### 3. 使用方式

#### 初始化（使用对象初始化器）
```csharp
var client = new ZMotionIOClient<DIProtocol, DOProtocol>
{
    ZMotion = zmotion  // required 属性，必须在初始化时设置
};
```

#### 读取并缓存数据
```csharp
// 读取 DI，同时缓存到 LastDIData
var diData = client.ReadDI();

// 读取 DO，同时缓存到 LastDOData
var doData = client.ReadDO();
```

#### 使用缓存数据（无需重新读取）
```csharp
// 适用于对实时性要求不高的场景
if (client.LastDIData.HasValue)
{
    var cachedDI = client.LastDIData.Value;
    // 使用缓存数据，避免频繁通信
}
```

#### 典型使用场景
```csharp
// UI 更新场景 - 定时读取并显示
Timer timer = new Timer(_ =>
{
    var diData = client.ReadDI();  // 自动缓存
    
    // 在 UI 线程使用缓存数据
    Dispatcher.Invoke(() =>
    {
        if (client.LastDIData.HasValue)
        {
            UpdateUI(client.LastDIData.Value);
        }
    });
}, null, 0, 100);

// 其他地方可以快速访问最新数据而无需通信
void OnButtonClick()
{
    if (client.LastDIData.HasValue)
    {
        ProcessData(client.LastDIData.Value);  // 使用缓存
    }
}
```

## 完整 API

### 属性
- `IProtocolSchema<TDIProtocol> DISchema` - DI 协议模式
- `IProtocolSchema<TDOProtocol> DOSchema` - DO 协议模式
- `TDIProtocol? LastDIData` - 上次读取的 DI 数据缓存
- `TDOProtocol? LastDOData` - 上次读取的 DO 数据缓存
- `ZMotion ZMotion` - ZMotion 实例（required）

### 方法
- `int GetDIAddress(Expression<Func<TDI, bool>>)` - 获取 DI 字段地址
- `int GetDOAddress(Expression<Func<TDO, bool>>)` - 获取 DO 字段地址
- `ProtocolDataMapping<TDO> CreateWriteMapping()` - 创建批量写入映射
- `bool ReadDI(Expression<Func<TDI, bool>>)` - 读取单个 DI 点位
- `bool ReadDO(Expression<Func<TDO, bool>>)` - 读取单个 DO 点位
- `TDIProtocol ReadDI()` - 读取完整 DI 协议（并缓存）
- `TDOProtocol ReadDO()` - 读取完整 DO 协议（并缓存）
- `bool[] ReadDIData()` - 读取原始 DI 数据数组
- `bool[] ReadDOData()` - 读取原始 DO 数据数组
- `void Write(Expression<Func<TDO, bool>>, bool)` - 写入单个 DO 点位
- `void Write(TDOProtocol)` - 写入完整 DO 协议
- `void Write(bool[])` - 写入原始 DO 数据数组
- `void Write(int address, bool value)` - 写入指定地址
- `bool ReadDI(int address)` - 读取指定 DI 地址
- `bool ReadDO(int address)` - 读取指定 DO 地址

## 优势

1. **减少通信开销** - 缓存数据可重复使用，无需频繁读取
2. **提高响应速度** - UI 刷新等场景可直接使用缓存
3. **代码更清晰** - 类名和职责更明确
4. **类型安全** - 使用可空类型表示缓存状态

## 注意事项

⚠️ **缓存数据的实时性**
- `LastDIData` 和 `LastDOData` 只在调用 `ReadDI()` 或 `ReadDO()` 时更新
- 如果需要实时数据，请调用读取方法，不要直接使用缓存
- 适合UI显示、日志记录等对实时性要求不高的场景
- 不适合安全关键的控制逻辑

## 迁移指南

### 从 MessageBuilder 迁移

**旧代码**:
```csharp
var builder = new MessageBuilder<DIProtocol, DOProtocol>();
builder.ZMotion = zmotion;
```

**新代码**:
```csharp
var client = new ZMotionIOClient<DIProtocol, DOProtocol>
{
    ZMotion = zmotion
};
```

其他 API 保持不变，只需替换类名即可。
