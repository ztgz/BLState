using BLState.Templates;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace BLState.AttributeUtils
{
    internal static class AttributeComparer
    {
        internal static bool HasAttributeListWithAttribute(this TypeDeclarationSyntax typeDeclarationSyntax, string attributeName)
        {
            return typeDeclarationSyntax.AttributeLists
                .Any(attributeList => attributeList.HasAttribute(attributeName));
        }

        internal static bool HasAttributeListWithAttribute(this FieldDeclarationSyntax fieldDeclartionSyntax, string attributeName)
        {
            return fieldDeclartionSyntax.AttributeLists
                .Any(attributeList => attributeList.HasAttribute(attributeName));
        }

        internal static bool HasAttribute(this AttributeListSyntax attributeListSyntax, string attributeName)
        {
            return attributeListSyntax.Attributes
                .Any(attribute => attribute.IsAttribute(attributeName));
        }

        internal static bool IsAttribute(this AttributeSyntax attribute, string attributeName)
        {
            var trimmed = attribute.Name.ToString().Trim();

            return trimmed.Equals(attributeName)
                || trimmed.Equals($"{BLTemplates.GeneratedNameSpace}.{attributeName}");

        }
    }
}
