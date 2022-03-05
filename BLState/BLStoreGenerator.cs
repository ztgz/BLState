using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLState.AttributeUtils;
using BLState.Templates;
using static BLState.Models.BLGeneratorModels;
using BLState.Maker;
using Microsoft.CodeAnalysis.Text;

namespace BLState
{
    [Generator]
    public class BLStoreGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            List<BLStoreModel> bLStoreModels = new List<BLStoreModel>();
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
                foreach (var typeDeclaration in GetStoreTypeDeclarations(syntaxTree))
                {
                    string classNamespace = semanticModel.GetDeclaredSymbol(typeDeclaration)
                        .ContainingNamespace?.ToDisplayString();
                    string className = typeDeclaration.Identifier.ToString();
                    bLStoreModels.Add(new BLStoreModel(className, classNamespace));
                    
                    // Generate store files
                    var storeClass = BLStoreMaker.CreateBLStore(typeDeclaration, semanticModel, classNamespace);
                    context.AddSource($"{className}.g"
                        , SourceText.From(storeClass, Encoding.UTF8));
                }
            }

            var diClass = BLDIMaker.CreateDIClass(bLStoreModels);
            context.AddSource("BStoreInit.g", SourceText.From(diClass, Encoding.UTF8));
        }

        List<TypeDeclarationSyntax> GetStoreTypeDeclarations(SyntaxTree syntaxTree)
        {
            return syntaxTree
                .GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>()
                .Where(x => x.HasAttributeListWithAttribute(BLTemplates.StoreAttributeName))
                .ToList();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No need for code
        }
    }
}
