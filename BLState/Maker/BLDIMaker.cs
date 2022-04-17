using BLState.Templates;
using System.Collections.Generic;
using System.Linq;
using static BLState.Models.BLGeneratorModels;

namespace BLState.Maker
{
    internal static class BLDIMaker
    {
        internal static string CreateDIClass(List<BLStoreModel> storeModels)
        {
            var storeNamespaces = storeModels.Select(s => $"using {s.Namespace};").Distinct();
            var storeContent = storeModels.Select(s => $"            services.AddScoped<{s.Name}>();");

            return $@"{string.Join("\n\r", storeNamespaces)}
using Microsoft.Extensions.DependencyInjection;

namespace {BLTemplates.GeneratedNameSpace}
{{
    public static class BLStoreSetup
    {{
        public static IServiceCollection AddBLStore(this IServiceCollection services)
        {{
{string.Join("\n\r", storeContent)}

            return services;
        }}
    }}
}}
";
        }
    }
}
