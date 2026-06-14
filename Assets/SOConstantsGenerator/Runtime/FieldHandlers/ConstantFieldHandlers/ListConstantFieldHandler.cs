using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.ConstantFieldHandlers;

public class ListConstantFieldHandler : IConstantFieldHandler
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

    public void HandleInLineGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;
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