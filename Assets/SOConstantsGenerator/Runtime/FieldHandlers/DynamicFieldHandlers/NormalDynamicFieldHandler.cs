using SOConstantsGenerator.FieldHandlers.Common;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class NormalDynamicFieldHandler : IDynamicFieldHandler
{
    public bool CanHandle(CanHandleContext canHandleContext)
    {
        return true;
    }

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var converterTypes = handleContext.ConverterTypes;
        var converterType = converterTypes?[0];

        if (!TryGetSourceAndDestTypes(converterType, out _, out var destType))
            destType = fieldInfo.Type;

        writer.WriteLine($"public static {GetCSharpFullName(destType)} {fieldInfo.Name};");
    }

    public void HandleAssignmentGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var converterTypes = handleContext.ConverterTypes;
        var converterType = converterTypes?[0];

        writer.Write($"{fieldInfo.Name} = ");

        writer.WriteLineNoIndent(converterType == null
            ? $"so.{fieldInfo.Name};"
            : $"new {GetCSharpFullName(converterType)}().Convert(so.{fieldInfo.Name});"
        );
    }
}