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

        writer.WriteLine($"public static readonly {elementType}[] {fieldInfo.Name} = new {elementType}[]");
        writer.WriteLine("{");
        writer.Indent();

        foreach (var element in enumerable)
        {
            var bytesString = BoxedStructToBytesString(elementType, element);
            writer.WriteLine($"Unsafe.As<byte, {elementType}>(ref new byte[] {{ {bytesString} }}[0]),");
        }

        writer.Unindent();
        writer.WriteLine("};");
    }
}