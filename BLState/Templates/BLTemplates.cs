using System;
using System.Collections.Generic;
using System.Text;

namespace BLState.Templates
{
    internal static  class BLTemplates
    {
        internal const string StoreAttributeName = "BLStore";
        internal const string ValueAttributeName = "BLValue";
        internal const string GeneratedNameSpace = "BLState";

        internal const string BLAttributeClass = @"
namespace BLState
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BLStoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class BLValueAttribute : Attribute 
    { 
        public string PropertyName { get; set; }
    }
}";

        internal const string BLStoreBaseClass = @"
namespace BLState
{
    public abstract class BLStoreBase
    {        
        private event Action? onChange = null;
        public void Subscribe(Action onStoreUpdate)
        {
            onChange += onStoreUpdate;
        }

        public void Unsubscribe(Action onStoreUpdate)
        {
            try
            {
                onChange -= onStoreUpdate;
            }
            catch { }
        }

        protected void InvokeUpdates()
        {
            try
            {
                onChange?.Invoke();
            }
            catch { }
        }
    }
}";


        internal static string MakeBLProperty(string fieldName, string propertyName, string propertyType) =>
$@"
        public {propertyType} {propertyName}
        {{
            get => {fieldName};
            set
            {{
                {fieldName} = value;
                InvokeUpdates();
            }}
        }}
";
    }
}
