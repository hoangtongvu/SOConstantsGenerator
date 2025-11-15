using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Editor.Utilities;

namespace SOConstantsGenerator.Editor.FieldHandlers.ConstantFieldHandlers;

public class ArrayConstantFieldHandler : IConstantFieldHandler
{
    private IEnumerable enumerable;

    public bool CanHandle(CanHandleInput canHandleInput)
    {
        var fieldInfo = canHandleInput.FieldInfo;

        if (fieldInfo.Value is IEnumerable enumerable && fieldInfo.Type.IsArray)
        {
            this.enumerable = enumerable;
            return true;
        }

        return false;
    }

    public void HandleInLineGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
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