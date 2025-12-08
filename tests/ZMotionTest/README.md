# ZMotionTest 测试应用说明

## 项目概述

ZMotionTest是ZMotionSDK的WPF测试应用程序，用于测试和验证ZMotion运动控制器的各项功能。

## 主要功能模块

### 1. 连接管理 (ConnectionPage)
- 支持TCP/IP网络连接
- 串口连接
- 连接状态监控
- 自动重连功能

### 2. IO控制 (IOControlPage) 
- 数字输入读取和监控
- 数字输出控制
- 实时IO状态显示
- IO测试功能

### 3. 轴运动控制 (AxisControlPage) - **全面优化**

#### 🆕 新增功能
- **轴使能控制**: 动态使能/失能轴操作
- **报警管理**: 清除轴报警功能
- **在线命令执行**: 支持缓冲和直接命令执行
- **总线功能**: 总线初始化、节点管理
- **高级回零设置**: 完整的回零参数配置

#### 🎨 界面优化
- **三栏布局**: 左侧控制、中间运动、右侧监控
- **状态指示灯**: 直观的Chip组件显示各种状态
- **实时监控**: 美观的卡片式位置和速度显示
- **颜色编码**: 状态指示采用直观的颜色系统

#### 📊 状态监控增强
- **运行状态**: 实时显示轴运行状态
- **使能状态**: 轴使能/失能状态监控
- **报警状态**: 轴报警状态及清除
- **限位状态**: 正向/反向限位开关状态
- **伺服状态**: 伺服驱动器就绪状态
- **详细状态**: 加速、减速、匀速等运动状态

#### 🔧 控制功能完善
- **基本运动**: 相对/绝对移动、点动控制
- **轴参数**: 速度、加速度、减速度设置
- **安全限制**: 软限位和快速减速设置
- **回零控制**: 多模式回零和高级参数设置
- **总线管理**: 总线初始化和节点信息获取

### 4. 轴状态监控 (AxisMonitorPage)
- 多轴状态同时监控
- 位置、速度实时显示
- 轴状态历史记录

## 🎯 最新改进亮点

### 界面布局优化
- 采用现代化的三栏布局设计
- 功能区域明确分离，操作更加直观
- 响应式设计，适应不同屏幕尺寸

### 状态显示升级
- 使用Material Design Chip组件显示状态
- 颜色编码状态（绿色=正常，灰色=异常）
- 卡片式数据展示，信息层次清晰

### 功能覆盖完整
- 覆盖ZMotion.Motion.cs中所有主要API
- 支持完整的轴控制生命周期
- 从基础设置到高级功能全面覆盖

### 用户体验提升
- 实时状态更新（100ms刷新间隔）
- 友好的错误提示和操作反馈
- 直观的图标和颜色指示

## 技术架构

### MVVM模式
- **View**: WPF用户界面
- **ViewModel**: 业务逻辑和数据绑定
- **Model**: 数据模型（AxisInfo, AxisStatus等）
- **Service**: ZMotionManager统一管理控制器连接

### 依赖技术
- **.NET 8.0**: 现代化的.NET框架
- **WPF**: Windows桌面应用UI框架  
- **Material Design in XAML**: 现代化UI组件库
- **CommunityToolkit.Mvvm**: MVVM框架支持

## 使用指南

### 启动应用
1. 确保ZMotion控制器已连接
2. 启动ZMotionTest应用
3. 在连接页面配置并连接控制器

### 轴控制操作流程
1. **连接设备**: 在连接页面建立通信
2. **选择轴**: 在轴控制页面选择要操作的轴
3. **使能轴**: 点击使能按钮激活轴
4. **设置参数**: 配置速度、加速度等运动参数
5. **执行运动**: 进行相对/绝对移动或点动操作
6. **监控状态**: 实时查看轴的运行状态和位置信息

### 高级功能
- **在线命令**: 直接发送控制器命令进行调试
- **总线管理**: 管理EtherCAT等总线设备
- **回零设置**: 配置复杂的回零参数和模式

## 开发说明

### 项目结构
```
ZMotionTest/
├── Pages/              # 页面视图
├── ViewModels/         # 视图模型
├── Models/            # 数据模型  
├── Services/          # 服务层
├── Converters/        # 数据转换器
└── README.md          # 项目说明
```

### 扩展开发
- 遵循MVVM模式添加新功能
- 使用Material Design组件保持界面一致性
- 通过ZMotionManager统一访问控制器API
- 添加适当的错误处理和用户提示

## 注意事项

1. **安全操作**: 在进行轴运动前确保设备安全
2. **参数校验**: 输入参数前请确认合理性
3. **状态监控**: 密切关注轴状态指示，及时处理异常
4. **总线操作**: 总线相关功能需要相应硬件支持

## 版本历史

### v2.0 - 轴控制界面全面优化
- 重新设计轴控制界面布局
- 新增轴使能、报警清除功能
- 增强状态监控和显示效果
- 添加在线命令和总线功能
- 完善回零高级设置

### v1.0 - 基础功能实现
- 基本连接管理
- IO控制功能
- 简单轴运动控制
- 基础状态监控

## 📁 项目结构

```
Test/ZMotionTest/
├── Pages/                      # 页面文件
│   ├── ConnectionPage.xaml     # 连接管理页面
│   ├── AxisMonitorPage.xaml    # 轴状态监控页面  
│   ├── AxisControlPage.xaml    # 轴运动控制页面
│   └── IOControlPage.xaml      # IO控制页面
├── ViewModels/                 # 视图模型
│   ├── ConnectionViewModel.cs  # 连接管理ViewModel
│   ├── AxisMonitorViewModel.cs # 轴监控ViewModel
│   ├── AxisControlViewModel.cs # 轴控制ViewModel
│   ├── IOControlViewModel.cs   # IO控制ViewModel
│   └── MainWindowViewModel.cs  # 主窗口ViewModel
├── Services/                   # 服务层
│   └── ZMotionManager.cs      # ZMotion设备管理器
├── Converters/                 # 值转换器
│   └── InverseBooleanConverter.cs # 布尔值反转转换器
├── Models/                     # 数据模型
│   ├── AxisViewModel.cs       # 轴信息模型
│   └── IOViewModel.cs         # IO信息模型
├── MainWindow.xaml            # 主窗口
└── App.xaml                   # 应用程序入口
```

## 🎨 技术特性

### UI框架
- **WPF (.NET 8.0)**: 现代桌面应用框架
- **Material Design 5.2.1**: Google Material Design设计语言
- **MVVM模式**: 完整的Model-View-ViewModel架构

### 核心组件
- **CommunityToolkit.Mvvm**: 现代MVVM框架
- **ZMotionSDK**: 运动控制器SDK集成
- **导航架构**: 基于Frame的页面导航系统

## 🔧 功能模块

### 1. 连接管理 (ConnectionPage)
- IP地址配置
- 设备连接/断开
- 实时连接状态显示
- 连接状态指示器

### 2. 轴状态监控 (AxisMonitorPage)
- 轴数量动态配置
- 实时轴状态数据表格
- 自动刷新机制
- 状态格式化显示

### 3. 轴运动控制 (AxisControlPage)
- 轴参数设置(速度、加速度等)
- 相对运动控制 (`Move_Relative`)
- 绝对运动控制 (`Move_Absolute`)
- 点动控制 (`Jog`)
- 回零功能 (`GoHome`)
- 紧急停止 (`Stop`)

### 4. IO控制 (IOControlPage)
- DI/DO数量配置
- 数字输入状态监控
- 数字输出控制
- 实时IO状态表格

### 5. IO控制测试 (最新更新)

#### 5.1 单个IO测试
**功能说明**: 测试基本的单个数字输入输出功能
- **数字输入(DI)测试**
  - 支持自定义DI数量
  - 实时读取单个DI状态
  - 表格形式显示DI索引、状态和描述
  - 一键刷新所有DI状态

- **数字输出(DO)测试**
  - 支持自定义DO数量
  - 实时读取单个DO状态
  - 表格形式显示DO索引、状态和描述
  - 支持在表格中直接修改DO输出状态
  - 一键刷新所有DO状态

#### 5.2 Modbus IO测试
**功能说明**: 测试Modbus协议方式的批量IO读取
- **配置参数**
  - 起始索引设置
  - 结束索引设置

- **数字输入测试**
  - 使用`GetDI_Modbus(startIndex, endIndex)`方法
  - 返回字节数组形式的DI状态
  - 显示十六进制、二进制位详细信息

- **数字输出测试**
  - 使用`GetDO_Modbus(startIndex, endIndex)`方法
  - 返回字节数组形式的DO状态
  - 显示十六进制、二进制位详细信息

#### 5.3 多个IO测试
**功能说明**: 测试高效的批量IO操作
- **配置参数**
  - 起始索引设置(ushort类型)
  - 结束索引设置(ushort类型)

- **多个数字输入测试**
  - 使用`GetDI_Multi(startIndex, endIndex)`方法
  - 返回int[]数组形式的DI状态
  - 显示十六进制、十进制、二进制位详细信息

- **多个数字输出测试**
  - 使用`GetDO_Multi(startIndex, endIndex)`方法读取当前状态
  - 使用`SetDO_Multi(startIndex, endIndex, values)`方法批量设置
  - 支持十进制和十六进制(0x前缀)输入格式
  - 提供数据表格显示，按32位为一组进行管理
  - 支持从表格生成输入值字符串

## 🏗️ 架构设计

### 服务层模式
```csharp
// 单例设备管理器
public class ZMotionManager
{
    public static ZMotionManager Instance { get; }
    public ZMotion ZMotion { get; }
    public bool IsConnected { get; }
}
```

### MVVM实现
- **RelayCommand**: 使用CommunityToolkit的命令绑定
- **ObservableProperty**: 自动属性变更通知
- **ObservableCollection**: 动态数据集合绑定

### 导航系统
- 主窗口容器模式
- Frame内容区域
- 侧边导航菜单
- 页面状态管理

## 🛠️ 最近修复的问题

### Material Design主题配置
- ✅ 修复了 `MaterialDesign2.Defaults.xaml` 资源路径
- ✅ 更新到Material Design 5.2.1最新版本
- ✅ 修复了命名空间拼写错误

### 代码质量优化
- ✅ 清理了所有未使用的异常变量警告
- ✅ 删除了重复和无用的代码
- ✅ 实现了真实的运动控制API调用
- ✅ 添加了缺失的转换器类

### API集成完善
- ✅ 替换了注释掉的API调用为实际方法
- ✅ 修复了 `AxisStatus` 模型属性问题  
- ✅ 优化了异常处理机制

## 🚀 使用方法

### 运行应用程序
```bash
cd Test/ZMotionTest
dotnet run
```

### 编译项目
```bash
dotnet build Test/ZMotionTest/ZMotionTest.csproj
```

## 📋 依赖包

| 包名 | 版本 | 用途 |
|------|------|------|
| MaterialDesignThemes | 5.2.1 | UI主题框架 |
| MaterialDesignColors | 5.2.1 | 颜色主题 |
| CommunityToolkit.Mvvm | 8.4.0 | MVVM框架 |
| System.ComponentModel.Annotations | 5.0.0 | 数据注解 |

## 🎯 开发建议

### 新增功能
1. **错误日志**: 添加详细的操作日志记录
2. **参数保存**: 实现设置的持久化存储
3. **多语言支持**: 国际化界面文本
4. **主题切换**: 支持深色/浅色主题切换

### 代码规范
- 遵循C# 编码约定
- 使用async/await进行异步操作
- 实现适当的错误处理和用户反馈
- 保持MVVM模式的清晰分离

## 📄 许可证

本项目遵循与ZMotionSDK相同的许可证。

## 🔗 相关链接

- [ZMotionSDK主项目](../../README.md)
- [Material Design文档](https://material.io/design)
- [CommunityToolkit.Mvvm文档](https://docs.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)

---

**状态**: ✅ 已修复所有编译错误，应用程序可正常运行 