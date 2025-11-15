using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Editor.Utilities;

namespace SOConstantsGenerator.Editor.FieldHandlers.ConstantFieldHandlers;

public class ListConstantFieldHandler : IConstantFieldHandler
{
    private IEnumerable enumerable;
    private System.Type[] genericArguments;

    public bool CanHandle(CanHandleInput canHandleInput)
    {
        var fieldInfo = canHandleInput.FieldInfo;
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

    public void HandleInLineGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var elementType = this.genericArguments[0];

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