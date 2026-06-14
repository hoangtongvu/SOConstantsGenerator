using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.ConstantFieldHandlers;

public class ArrayConstantFieldHandler : IConstantFieldHandler
{
    private IEnumerable enumerable;

    public bool CanHandle(CanHandleContext canHandleContext)
    {
        var fieldInfo = canHandleContext.FieldInfo;

        if (fieldInfo.Value is IEnumerable enumerable && fieldInfo.Type.IsArray)
        {
            this.enumerable = enumerable;
            return true;
        }

        return false;
    }

    public void HandleInLineGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var elementType = fieldInfo.Type.GetElementType();
        var elementConverterType = handleContext.ConverterTypes?[0];

        GetSourceAndDestTypes(elementConverterType, out _, out var destElementType);
        destElementType ??= elementType;

        writer.WriteLine($"public static readonly {GetCSharpFullName(destElementType)}[] {fieldInfo.Name} = new {GetCSharpFullName(destElementType)}[]");
        writer.WriteLine("{");
        writer.Indent();

        foreach (var element in enumerable)
        {
            writer.WriteLine($"{ToCodeLiteral(element, elementType, elementConverterType)},");
        }

        writer.Unindent();
        writer.WriteLine("};");
    }
}