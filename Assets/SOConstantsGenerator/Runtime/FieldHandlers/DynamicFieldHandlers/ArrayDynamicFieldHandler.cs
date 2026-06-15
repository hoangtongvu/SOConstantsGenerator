using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class ArrayDynamicFieldHandler : IDynamicFieldHandler
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

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var elementType = fieldInfo.Type.GetElementType();
        var elementConverterTypes = handleContext.ConverterTypes;
        var elementConverterType = elementConverterTypes?[0];

        if (!TryGetSourceAndDestTypes(elementConverterType, out _, out var destElementType))
            destElementType = elementType;

        writer.WriteLine($"public static {GetCSharpFullName(destElementType)}[] {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var elementConverterTypes = handleContext.ConverterTypes;
        var elementConverterType = elementConverterTypes?[0];

        writer.Write($"{fieldInfo.Name} = ");

        writer.WriteLineNoIndent(elementConverterType == null
            ? $"so.{fieldInfo.Name};"
            : $"SOConstantsGenerator.ITypeConverter.Convert(so.{fieldInfo.Name}, new {GetCSharpFullName(elementConverterType)}());"
        );
    }
}