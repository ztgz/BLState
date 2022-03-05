namespace BLState.Templates
{
    internal static  class BLTemplates
    {
        internal const string StoreAttributeName = "BLStore";
        internal const string ValueAttributeName = "BLValue";
        internal const string GeneratedNameSpace = "BLState";

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
