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
        internal static string CreateBLStore(
            IEnumerable<UsingDirectiveSyntax> usingDirectiveSyntax, 
            TypeDeclarationSyntax storeTypeDeclaration, 
            SemanticModel semanticModel,
            string classNamespace)
        {
            GuardClassMustBePartial(storeTypeDeclaration);

            var sourceBuilder = new StringBuilder();
            AddUsingDirectives(ref sourceBuilder, usingDirectiveSyntax);
            
            // Add the class to the source builder
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

        private static void GuardClassMustBePartial(TypeDeclarationSyntax storeTypeDeclaration)
        {
            if (storeTypeDeclaration.Modifiers.All(modifer => modifer.ToString() != "partial"))
                throw new Exception($"Store {storeTypeDeclaration.Identifier.ToString()} must be marked as partial");
        }

        const string SystemNameSpace = "System";
        private static void AddUsingDirectives(ref StringBuilder sourceBuilder, IEnumerable<UsingDirectiveSyntax> usingDirectiveSyntax)
        {
            sourceBuilder.AppendLine($"using {SystemNameSpace};");
            sourceBuilder.AppendLine($"using {BLTemplates.GeneratedNameSpace};");

            var filtredUsingsStatements = usingDirectiveSyntax.Where(usingDirective => usingDirective.Name.ToString() != SystemNameSpace);
            foreach (var usingDirective in filtredUsingsStatements)
                sourceBuilder.AppendLine($"using {usingDirective.Name.ToString()};");
        }
    }
}
