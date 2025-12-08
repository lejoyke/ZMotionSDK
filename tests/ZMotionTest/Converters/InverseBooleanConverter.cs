using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ZMotionTest.Converters;

/// <summary>
/// 布尔值取反转换器
/// </summary>
public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }
}

/// <summary>
/// 布尔值转字符串转换器
/// </summary>
public class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "1" : "0";
        }
        return "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string strValue)
        {
            return strValue == "1" || strValue.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}

/// <summary>
/// 布尔值到颜色转换器
/// </summary>
public class BooleanToColorConverter : IValueConverter
{
    public static readonly BooleanToColorConverter Instance = new BooleanToColorConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? 
                new SolidColorBrush(Colors.LimeGreen) : 
                new SolidColorBrush(Colors.Gray);
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 切换按钮文本转换器
/// </summary>
public class ToggleTextConverter : IValueConverter
{
    public static readonly ToggleTextConverter Instance = new ToggleTextConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "失能轴" : "使能轴";
        }
        return "使能轴";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 