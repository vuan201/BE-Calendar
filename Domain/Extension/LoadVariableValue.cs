using System.Reflection;

namespace Domain.Extension;

public class LoadVariableValue
{
    public static List<string> GetAllConstValue(Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f => f.GetValue(null)?.ToString())
            .Where(value => value is not null)
            .Select(value => value!)
            .ToList();
    }
}