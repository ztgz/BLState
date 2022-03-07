namespace BLState;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class BLStoreAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class BLValueAttribute : Attribute
{
    public string? PropertyName { get; set; }
}
