using SOConstantsGenerator.Common;
using SOConstantsGenerator.FieldHandlers.Common;
using System;
using System.Linq;
using System.Reflection;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.ConstantFieldHandlers;

public class NormalConstantFieldHandler : IConstantFieldHandler
{
    public bool CanHandle(CanHandleContext canHandleContext)
    {
        return true;
    }

    public void HandleInLineGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
        var converterTypes = handleContext.ConverterTypes;
        var converterType = converterTypes?[0];

        if (CanBeUnmanagedConst(fieldInfo.Type))
        {
            // Handle constants
            writer.WriteLine($"public const {GetCSharpFullName(fieldInfo.Type)} {fieldInfo.Name} = {FormatValue(fieldInfo.Value)};");
        }
        else
        {
            // Handle static readonly
            if (converterType == null)
            {
                writer.WriteLine($"public static readonly {GetCSharpFullName((fieldInfo.Type))} {fieldInfo.Name} =");
            }
            else
            {
                GetSourceAndDestTypes(converterType, out _, out var destType);
                writer.WriteLine($"public static readonly {GetCSharpFullName(destType)} {fieldInfo.Name} =");
            }

            writer.Indent();
            writer.WriteLine($"{ToCodeLiteral(fieldInfo.Value, fieldInfo.Type, converterType)};");
            writer.Unindent();
        }
    }
}