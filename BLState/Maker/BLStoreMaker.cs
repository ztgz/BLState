using BLState.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLState.Maker
{
    internal static class BLStoreMaker
    {
        internal static string CreateBLStore(TypeDeclarationSyntax storeTypeDeclaration, SemanticModel semanticModel ,string classNamespace)
        {
            if (storeTypeDeclaration.Modifiers.All(modifer => modifer.ToString() != "partial"))
                throw new Exception($"Store {storeTypeDeclaration.Identifier.ToString()} must be marked as partial");

            var sourceBuilder = new StringBuilder("using System;\r\n");
            sourceBuilder.AppendLine($"using {BLTemplates.GeneratedNameSpace};");

            var modifiersText = string.Join(" ", storeTypeDeclaration.Modifiers);
            sourceBuilder.Append($@"
namespace {classNamespace}
{{
    {modifiersText} class {storeTypeDeclaration.Identifier.ToString()} : BLStoreBase
    {{
{BLValueMaker.CreateBLValueProperties(storeTypeDeclaration, semanticModel)}
    }}
}}
");
            return sourceBuilder.ToString();
        }
    }
}
