using BLState.AttributeUtils;
using BLState.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Text;

namespace BLState.Maker
{
    internal static class BLValueMaker
    {
        internal static string CreateBLValueProperties(TypeDeclarationSyntax typeDeclaration, SemanticModel semanticModel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var field in typeDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
            {
                var valueAttributeSyntaxList = field.AttributeLists.FirstOrDefault(x =>
                    x.HasAttribute(BLTemplates.ValueAttributeName));

                if (valueAttributeSyntaxList != null)
                {
                    string customPropertyName = GetBValueCustomNameOrNull(valueAttributeSyntaxList, semanticModel);
                    CreateBLValueProperty(stringBuilder, field, customPropertyName);
                }
            }

            return stringBuilder.ToString();
        }

        private static string GetBValueCustomNameOrNull(AttributeListSyntax attributeListSyntax, SemanticModel semanticModel)
        {
            var attributeSyntax = attributeListSyntax.Attributes.First(attribute =>
                        attribute.IsAttribute(BLTemplates.ValueAttributeName));

            string customPropertyName = null;
            var properyNameArgument = attributeSyntax.ArgumentList?.Arguments[0];
            if (properyNameArgument != null)
            {
                customPropertyName = semanticModel.GetConstantValue(properyNameArgument.Expression).ToString();
            }

            return customPropertyName;
        }

        private static void CreateBLValueProperty(StringBuilder stringBuilder, FieldDeclarationSyntax field, string customPropertyName = null)
        {
            foreach (var variable in field.Declaration.Variables)
            {
                string fieldName = variable.Identifier.ToString();

                stringBuilder.Append(BLTemplates.MakeBLProperty(
                    fieldName: fieldName,
                    propertyName: customPropertyName ?? TransformPropertyName(fieldName),
                    propertyType: field.Declaration.Type.ToString()));
            }
        }

        private static string TransformPropertyName(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != char.ToUpper(input[i]))
                {
                    var beginningCharacters = input.Substring(0, i).Where(c => c != '_').ToArray();
                    return string.Concat(beginningCharacters) + char.ToUpper(input[i]) + input.Substring(i + 1);
                }
            }

            throw new Exception($"BLValue {input} is not a valid field name. It must contain atleast one lowercase character");
        }

    }
}
