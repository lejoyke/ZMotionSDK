using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace ZMotionSDK.ProtocolSugar;

public sealed class ProtocolConfiguration<TProtocol> : IProtocolConfiguration<TProtocol> where TProtocol : struct
{
    public int StartAddress { get; private set; }
    public int Size { get; private set; }
    public FrozenDictionary<string, int> AddressMapping { get; private set; } = FrozenDictionary<string, int>.Empty;

    public ProtocolConfiguration()
    {
        StartAddress = int.MaxValue;
        Size = 0;

        AddressMapping = BuildAddressMapping();
    }

    public int GetAddress(string memberName)
    {
        if (AddressMapping.TryGetValue(memberName, out var address))
        {
            return address;
        }
        throw new ArgumentException($"Field '{memberName}' not found in address mapping.");
    }

    public int GetAddress(Expression<Func<TProtocol, bool>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            var fieldName = memberExpression.Member.Name;
            return GetAddress(fieldName);
        }
        throw new ArgumentException("Invalid expression. Expected a member expression.");
    }

    [MemberNotNull(nameof(AddressMapping))]
    private FrozenDictionary<string, int> BuildAddressMapping()
    {
        var maxAddress = 0;
        var minAddress = int.MaxValue;
        var mapping = new Dictionary<string, int>();

        // 获取字段
        var fields = typeof(TProtocol).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<AddressAttribute>();
            if (attribute == null) continue;

            if (attribute.Address > maxAddress)
            {
                maxAddress = attribute.Address;
            }
            
            if (attribute.Address < minAddress)
            {
                minAddress = attribute.Address;
            }

            mapping.Add(field.Name, attribute.Address);
        }

        // 获取属性
        var properties = typeof(TProtocol).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<AddressAttribute>();
            if (attribute == null) continue;

            if (attribute.Address > maxAddress)
            {
                maxAddress = attribute.Address;
            }
            
            if (attribute.Address < minAddress)
            {
                minAddress = attribute.Address;
            }

            mapping.Add(property.Name, attribute.Address);
        }

        // 设置起始地址和计算总大小
        if (mapping.Count > 0)
        {
            StartAddress = minAddress;
            Size = maxAddress - minAddress + 1;
        }
        else
        {
            StartAddress = 0;
            Size = 0;
        }

        return mapping.ToFrozenDictionary();
    }
}
