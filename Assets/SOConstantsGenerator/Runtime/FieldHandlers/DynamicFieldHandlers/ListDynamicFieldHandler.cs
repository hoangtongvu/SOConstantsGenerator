using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class ListDynamicFieldHandler : IDynamicFieldHandler
{
    private IEnumerable enumerable;
    private System.Type[] genericArguments;

    public bool CanHandle(CanHandleContext canHandleContext)
    {
        var fieldInfo = canHandleContext.FieldInfo;
        var args = fieldInfo.Type.GetGenericArguments();

        if (fieldInfo.Value is IEnumerable enumerable &&
            args.Length == 1)
        {
            this.enumerable = enumerable;
            this.genericArguments = args;
            return true;
        }

        return false;
    }

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var elementType = this.genericArguments[0];
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
            ? $"so.{fieldInfo.Name}.ToArray();"
            : $"SOConstantsGenerator.ITypeConverter.Convert(so.{fieldInfo.Name}.ToArray(), new {GetCSharpFullName(elementConverterType)}());"
        );
    }
}